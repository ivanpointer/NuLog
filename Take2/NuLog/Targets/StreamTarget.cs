/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.LogEvents;
using System.IO;
using System.Text;

namespace NuLog.Targets
{
    /// <summary>
    /// A target for writing log events to a stream.
    /// </summary>
    public class StreamTarget : ITarget
    {
        public string Name { get; set; }

        private readonly ILayout layout;

        private readonly Stream stream;

        private readonly StreamWriter streamWriter;

        private readonly bool closeOnDispose;

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

            this.streamWriter = new StreamWriter(stream, Encoding.Default, 4096, !closeOnDispose);

            this.closeOnDispose = closeOnDispose;
        }

        public void Write(LogEvent logEvent)
        {
            var message = this.layout.Format(logEvent);
            this.streamWriter.Write(message);
            this.streamWriter.Flush();
        }

        public void Dispose()
        {
            if (this.closeOnDispose)
            {
                this.stream.Close();
                this.stream.Dispose();
            }

            this.streamWriter.Dispose();
        }
    }
}