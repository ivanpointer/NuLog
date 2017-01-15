/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.MetaData;
using System.Collections.Generic;

namespace NuLog.Tests.Scaffolding
{
    public class TestMetaDataProvider : IMetaDataProvider
    {
        private IDictionary<string, object> MetaData { get; set; }

        public TestMetaDataProvider()
        {
            MetaData = new Dictionary<string, object>();
            MetaData["Hello"] = "World";
            MetaData["Addtl"] = "Data";
        }

        public IDictionary<string, object> ProvideMetaData()
        {
            return MetaData;
        }
    }
}