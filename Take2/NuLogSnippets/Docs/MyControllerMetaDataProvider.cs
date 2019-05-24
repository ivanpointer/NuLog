/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog;
using System.Collections.Generic;
using System.Web.Mvc;

namespace NuLogSnippets.Docs {

    public class MyControllerMetaDataProvider : IMetaDataProvider {
        private readonly Controller myController;

        public MyControllerMetaDataProvider(Controller controller) {
            myController = controller;
        }

        public IDictionary<string, object> ProvideMetaData() {
            var request = myController.Request;
            return new Dictionary<string, object>
            {
                { "UserHostAddress", request.UserHostAddress },
                { "URL", request.Url }
            };
        }
    }
}