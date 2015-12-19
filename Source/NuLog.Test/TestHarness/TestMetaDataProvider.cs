using NuLog.MetaData;
using System.Collections.Generic;

namespace NuLog.Test.TestHarness
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