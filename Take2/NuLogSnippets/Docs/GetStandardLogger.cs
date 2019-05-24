/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

// use NuLog
using NuLog;

namespace NuLogSnippets.Docs {

    public class GetStandardLogger {

        // Get the logger from the log manager
        private static readonly ILogger _logger = LogManager.GetLogger();

        public void DoSomething() {
            // Log something
            _logger.Log("Hello, world!  I'll be dispatched later.");
            _logger.LogNow("Hello, world! I'll be dispatched immediately.");
        }
    }
}