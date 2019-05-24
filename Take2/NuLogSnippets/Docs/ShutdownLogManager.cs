/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog;

namespace NuLogSnippets.Docs {

    public class ShutdownLogManager {

        public void DoStuff() {
            // ...

            LogManager.Shutdown();

            // ...
        }
    }
}