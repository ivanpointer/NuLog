using NuLog.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
