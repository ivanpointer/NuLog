/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using NuLog.LogEvents;
using System;

namespace NuLog.CLI.Benchmarking {

    /// <summary>
    /// A dummy target for performance testing - we need to see the raw results of the engine, not
    /// the individual target.
    /// </summary>
    public class DummyTarget : ITarget {
        public string Name { get; set; }

        public void Configure(TargetConfig config) {
            // noop
        }

        public void Write(LogEvent logEvent) {
            // noop
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // noop
                }

                // noop

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}