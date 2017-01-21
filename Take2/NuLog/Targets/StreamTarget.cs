/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.LogEvents;
using System.IO;
using System.Text;
using NuLog.Configuration;
using System;

namespace NuLog.Targets
{
    /// <summary>
    /// A target for writing log events to a stream.
    /// </summary>
    public class StreamTarget : TargetBase
    {
        private const int streamWriterBuffer = 4096;

        private ILayout layout;

        private Stream stream;

        private StreamWriter streamWriter;

        private bool closeOnDispose;

        public StreamTarget()
        {
            // Nothing to do
        }

        /// <summary>
        /// Builds a new instance of this stream target.
        /// </summary>
        /// <param name="layout">        
        /// The layout to use to format log events as they're written to the stream.
        /// </param>
        /// <param name="stream">        The stream to write to.</param>
        /// <param name="closeOnDispose">
        /// True will tell the target to close the stream when the target is disposed.
        /// </param>
        public StreamTarget(string name, ILayout layout, Stream stream, bool closeOnDispose)
        {
            this.Name = name;

            this.layout = layout;

            this.stream = stream;

            this.streamWriter = new StreamWriter(stream, Encoding.Default, streamWriterBuffer, !closeOnDispose);

            this.closeOnDispose = closeOnDispose;
        }

        public override void Configure(TargetConfig config)
        {
            TryGetProperty(config, "layout", out this.layout);

            TryGetProperty(config, "stream", out this.stream);

            TryGetProperty(config, "closeOnDispose", out this.closeOnDispose);

            BuildNewStreamWriter();
        }

        public override void Write(LogEvent logEvent)
        {
            var message = this.layout.Format(logEvent);
            this.streamWriter.Write(message);
            this.streamWriter.Flush();
        }

        public override void Dispose()
        {
            if (this.closeOnDispose)
            {
                this.stream.Close();
                this.stream.Dispose();
            }

            this.streamWriter.Dispose();
        }

        #region Internals

        private void BuildNewStreamWriter()
        {
            if (this.stream != null)
            {
                if (this.streamWriter != null)
                {
                    this.streamWriter.Dispose();
                }

                this.streamWriter = new StreamWriter(this.stream, Encoding.Default, streamWriterBuffer, !closeOnDispose);
            }
        }

        #endregion
    }
}