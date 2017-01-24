/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

namespace NuLog.CLI.PerfTune
{
    /// <summary>
    /// A "throwaway" project for performance tuning NuLog.
    /// </summary>
    public class Program
    {
        private static readonly ILogger _logger = LogManager.GetLogger();

        private static void Main(string[] args)
        {
            for (var lp = 0; lp < 5000; lp++)
            {
                _logger.Log("Stress message " + lp);
            }
        }
    }
}