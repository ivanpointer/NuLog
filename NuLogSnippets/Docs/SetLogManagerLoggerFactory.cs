/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog;
using System;
using System.Collections.Generic;

namespace NuLogSnippets.Docs {

    // start_snippet
    public class SetLogManagerLoggerFactory {

        public void SetupMyApplication() {
            LogManager.SetFactory(new MyCustomLoggerFactory());
        }
    }

    // end_snippet

    internal class MyCustomLoggerFactory : ILoggerFactory {

        public ILogger GetLogger(IMetaDataProvider metaDataProvider, IEnumerable<string> defaultTags) {
            throw new NotImplementedException();
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // Noop
                }

                // Noop

                disposedValue = true;
            }
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