/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog;
using NuLog.Configuration;
using NuLog.LogEvents;
using System;
using System.Diagnostics;

namespace NuLogSnippets.Docs.CustomTargets {

    // start_snippet
    public class HelloWorldTarget : ITarget {
        public string Name { get; set; }

        public void Configure(TargetConfig config) {
            // Nothing to do
        }

        public void Write(LogEvent logEvent) {
            Debug.WriteLine(logEvent.Message);
        }

        #region IDisposable Support

        protected virtual void Dispose(bool disposing) {
            // Nothing to do
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            Dispose(true);

            // Tell the GC that we've got it
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }

    // end_snippet
}