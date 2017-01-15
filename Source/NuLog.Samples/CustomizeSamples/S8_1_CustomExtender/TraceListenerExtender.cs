/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using NuLog.Configuration.Extenders;
using NuLog.Dispatch;
using NuLog.Extenders;
using System;
using System.Diagnostics;
using System.Text;

namespace NuLog.Samples.CustomizeSamples.S8_1_CustomExtender
{
    /// <summary>
    /// A sample illustrating how to create an extender for the NuLog framework.
    /// For the narration of this example, see the article:
    /// https://github.com/ivanpointer/NuLog/wiki/8.1-Creating-the-Trace-Listener-Extender
    /// </summary>
    public class TraceListenerExtender : ExtenderBase
    {
        /// <summary>
        /// This is an internal class that extends the basic TraceListener
        ///  This is what will actually do the listening for trace
        /// </summary>
        internal class InternalTraceListener : TraceListener
        {
            #region Constants/Members/Messages

            // Our constants, including a write lock to help keep our writing clean
            private static readonly object WriteLock = new object();

            private static readonly char[] Split = Environment.NewLine.ToCharArray();

            // Our members, including a logger
            private static readonly LoggerBase _logger = LoggerFactory.GetLogger();

            private string[] _tags;
            private StringBuilder _stringBuilder;

            #endregion Constants/Members/Messages

            /// <summary>
            /// The only constructor.  Configures this trace listener based on
            /// the configuration in the provided TraceListenerExtender
            /// </summary>
            /// <param name="extender">The TraceListenerExtender that this TraceListener belongs to, and from which to load the configuration</param>
            public InternalTraceListener(TraceListenerExtender extender)
                : base()
            {
                // Prepare the String Builder
                _stringBuilder = new StringBuilder();

                // We want to get the tags from the config
                if (extender != null && extender.TraceListenerConfig != null)
                    _tags = extender.TraceListenerConfig.TraceTags;
            }

            /// <summary>
            /// Writes a single message.  If it has no newline characters, the contents are stored
            /// in a string buffer until a WriteLine is called, or a Write with a message that
            /// contains newlines, at which point, the message will be flushed to log
            /// </summary>
            /// <param name="message">The message to flush to log</param>
            public override void Write(string message)
            {
                // Make sure that we have a message
                if (string.IsNullOrEmpty(message) == false)
                    lock (WriteLock)
                    {
                        // If the message has no newline, just queue it up
                        if (!message.Contains(Environment.NewLine))
                        {
                            _stringBuilder.Append(message);
                        }
                        // The message contains at least one newline, split it up
                        else
                        {
                            // Setup
                            bool endsNewline = message.EndsWith(Environment.NewLine);
                            var parts = message.Split(Split);

                            // Append, and write the first part
                            //  unless it is the only part and we have no newline at the end
                            _stringBuilder.Append(parts[0]);
                            if (parts.Length > 1 || endsNewline)
                            {
                                WriteLine(_stringBuilder.ToString());
                                _stringBuilder.Clear();
                            }

                            // Write out each of the child parts if there are more, queuing the last bit if
                            //  it isn't followed by a newline
                            if (parts.Length > 1)
                            {
                                for (int index = 1; index < (endsNewline ? parts.Length : parts.Length - 1); index++)
                                    WriteLine(parts[index]);

                                if (endsNewline == false)
                                    _stringBuilder.Append(parts[parts.Length - 1]);
                            }
                        }
                    }
            }

            /// <summary>
            /// Flushes the buffer and writes the message to log
            /// </summary>
            /// <param name="message">The message to write to log</param>
            public override void WriteLine(string message)
            {
                // Ignore this request if the message is empty
                if (string.IsNullOrEmpty(message) == false)
                    lock (WriteLock)
                    {
                        // Write the contents of the buffer out, followed by the message
                        _logger.LogNow(new LogEvent
                        {
                            Message = string.Format("{0}{1}", _stringBuilder.ToString(), message),
                            Tags = _tags,
                            // Help prevent feedback loops:
                            Silent = true
                        });
                        _stringBuilder.Clear();
                    }
            }

            /// <summary>
            /// Flushes the contents of the buffer out to log
            /// </summary>
            public override void Flush()
            {
                lock (WriteLock)
                {
                    // Write out the contents of the buffer
                    WriteLine(_stringBuilder.ToString());
                    _stringBuilder.Clear();
                }
            }
        }

        // This TraceListenerExtender's TraceListener
        private TraceListener _traceListener;

        /// <summary>
        /// The configuration for this TraceListenerExtender
        /// </summary>
        public TraceListenerConfig TraceListenerConfig { get; set; }

        /// <summary>
        /// Initializes this extender with the provided configuration
        /// </summary>
        /// <param name="extenderConfig">The configuration for this specific extender</param>
        /// <param name="loggingConfig">The whole configuration for the framework</param>
        public override void Initialize(ExtenderConfig extenderConfig, LoggingConfig loggingConfig)
        {
            // Let the base configure itself out
            base.Initialize(extenderConfig, loggingConfig);

            // Configure this extender, using the TraceListenerConfig
            if (extenderConfig != null)
            {
                TraceListenerConfig = extenderConfig is TraceListenerConfig
                    ? (TraceListenerConfig)extenderConfig
                    : new TraceListenerConfig(extenderConfig.Config);
            }
        }

        /// <summary>
        /// Starts this trace listener extender.  Starts a trace listener that will
        /// route trace messages through the framework as log events
        /// </summary>
        /// <param name="dispatcher">The dispatcher that has been initialized for the framework.  This TraceListenerExtender doesn't use the dispatcher.</param>
        public override void Startup(LogEventDispatcher dispatcher)
        {
            _traceListener = new InternalTraceListener(this);
            Trace.Listeners.Add(_traceListener);
        }

        /// <summary>
        /// Shuts down this instance.  Will flush the buffer of the trace listener and remove it from the trace listeners list.
        /// </summary>
        /// <param name="timeout">The amount of time for this to shutdown.  Ignored.</param>
        /// <param name="stopwatch">A stopwatch.  Ignored.</param>
        /// <returns>A boolean indicating whether or not this shutdown successfully.  This always returns true.</returns>
        public override bool Shutdown(int timeout = DefaultShutdownTimeout, Stopwatch stopwatch = null)
        {
            // If we have a configured trace listener, flush it and remove it from the list
            //  of listeners.
            if (_traceListener != null)
            {
                _traceListener.Flush();
                _traceListener.Close();
                _traceListener.Dispose();
                Trace.Listeners.Remove(_traceListener);
                _traceListener = null;
            }
            return base.Shutdown();
        }
    }
}