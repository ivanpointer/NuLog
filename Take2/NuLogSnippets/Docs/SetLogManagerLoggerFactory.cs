/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog;
using System;
using System.Collections.Generic;

namespace NuLogSnippets.Docs
{
    public class SetLogManagerLoggerFactory
    {
        public void SetupMyApplication()
        {
            LogManager.SetFactory(new MyCustomLoggerFactory());
        }
    }

    internal class MyCustomLoggerFactory : ILoggerFactory
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ILogger GetLogger(IMetaDataProvider metaDataProvider, IEnumerable<string> defaultTags)
        {
            throw new NotImplementedException();
        }
    }
}