using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.MetaData
{
    public interface IMetaDataProvider
    {
        IDictionary<string, object> ProvideMetaData();
    }
}
