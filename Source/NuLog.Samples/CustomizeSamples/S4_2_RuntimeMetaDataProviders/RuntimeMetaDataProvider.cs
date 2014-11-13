/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/12/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.MetaData;
using System.Collections.Generic;

namespace NuLog.Samples.CustomizeSamples.S4_2_RuntimeMetaDataProviders
{
    /// <summary>
    /// An example runtime meta data provider.  The narration for this
    /// can be found in the article:
    /// https://github.com/ivanpointer/NuLog/wiki/4.2-Runtime-Meta-Data-Providers
    /// </summary>
    public class RuntimeMetaDataProvider : IMetaDataProvider
    {
        // A public constant to help with identifying the meta data we are after
        public const string CountMeta = "SampleCount";

        // A variable to represent internal state
        private int _myCount;

        // Simple constructor
        public RuntimeMetaDataProvider()
        {
            _myCount = 0;
        }

        // Function for providing the meta data
        public IDictionary<string, object> ProvideMetaData()
        {
            // Note that we may have duplicate numbers come out of this
            //  if this is used by multiple threads at once.  The ++
            //  operator here causes a check-then-act situation that
            //  may cause a thread to be misdirected at the variables
            //  current state.

            // Return our state meta data
            return new Dictionary<string, object>()
            {
                { CountMeta, ++_myCount }
            };
        }
    }
}

