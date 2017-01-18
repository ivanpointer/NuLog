/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Dispatchers;
using NuLog.Dispatchers.TagRouters;
using NuLog.Layouts.Standard;
using NuLog.Layouts.Standard.LayoutParsers;
using NuLog.Layouts.Standard.PropertyParsers;
using NuLog.Loggers;
using NuLog.TagRouters;
using NuLog.TagRouters.RuleProcessors;
using NuLog.TagRouters.TagGroupProcessors;
using NuLog.Targets;
using System.Collections.Generic;

namespace NuLog.Console.Tests
{
    /// <summary>
    /// A console application for testing performance and memory. Helps figure out where performance
    /// and memory tuning is needed. Don't depend on this project! It is a "throw away" project, just
    /// a playground for toying around with NuLog.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            // Yeah, we need a factory...  Good thing it's on the books!
            var layout = GetLayout("${DateLogged:'{0:MM/dd/yyyy hh:mm:ss}'} - ${Message}\r\n");
            var target = new StreamTarget("console", layout, System.Console.OpenStandardOutput(), false);
            var dispatcher = GetDispatcher(new ITarget[] { target });
            var normalizer = new StandardTagNormalizer();
            var logger = new StandardLogger(dispatcher, normalizer, null, new string[] { "program" });

            logger.Log("Hello, World!");

            System.Console.ReadLine();
        }

        private static IDispatcher GetDispatcher(IEnumerable<ITarget> targets)
        {
            var tagRouter = GetTagRouter();
            var dispatcher = new StandardDispatcher(targets, tagRouter);
            return dispatcher;
        }

        private static ITagRouter GetTagRouter()
        {
            var tagGroupProcessor = new StandardTagGroupProcessor(new TagGroup[0]);

            var ruleProcessor = new StandardRuleProcessor(new Rule[] {
                new Rule
                {
                    Include = new string[] { "*" },
                    Targets = new string[] { "console" }
                }
            }, tagGroupProcessor);

            var tagRouter = new StandardTagRouter(ruleProcessor);

            return tagRouter;
        }

        private static ILayout GetLayout(string format)
        {
            var layoutParser = new StandardLayoutParser();
            var layoutParms = layoutParser.Parse(format);
            var propertyParser = new StandardPropertyParser();
            var layout = new StandardLayout(layoutParms, propertyParser);
            return layout;
        }
    }
}