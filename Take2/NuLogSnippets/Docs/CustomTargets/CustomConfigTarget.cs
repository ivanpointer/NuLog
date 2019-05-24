/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog;
using NuLog.Configuration;
using NuLog.LogEvents;
using System;

namespace NuLogSnippets.Docs.CustomTargets {

    public class CustomConfigTarget : ITarget {
        public string MyCustomProperty { get; set; }

        public string Name { get; set; }

        // start_snippet
        public void Configure(TargetConfig config) {
            this.MyCustomProperty = Convert.ToString(config.Properties["myCustomProperty"]);
        }

        // end_snippet

        public void Write(LogEvent logEvent) {
            throw new NotImplementedException();
        }

        #region IDisposable Support

        protected virtual void Dispose(bool disposing) {
            // Nothing to do
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);

            // Tell the GC that we've got it
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}