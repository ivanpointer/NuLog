﻿/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog;
using System;
using System.Collections.Generic;

namespace NuLogSnippets.Docs
{
    // start_snippet
    public class SetLogManagerLoggerFactory
    {
        public void SetupMyApplication()
        {
            LogManager.SetFactory(new MyCustomLoggerFactory());
        }
    }

    // end_snippet

    internal class MyCustomLoggerFactory : ILoggerFactory
    {
        public ILogger GetLogger(IMetaDataProvider metaDataProvider, IEnumerable<string> defaultTags)
        {
            throw new NotImplementedException();
        }

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            // Nothing to do
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion IDisposable Support
    }
}