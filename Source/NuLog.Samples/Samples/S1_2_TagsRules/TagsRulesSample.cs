/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/25/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using System;
namespace NuLog.Samples.Samples.S1_2_TagsRules
{
    /// <summary>
    /// A simple sample illustrating how the targets, tags and rules all play together to determine
    ///   which targets log events are dispatched to.  The narration of this sample can be found at:
    ///   https://github.com/ivanpointer/NuLog/wiki/1.2-Tags-and-Rules
    /// </summary>
    public class TagsRulesSample : SampleBase
    {
        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public TagsRulesSample(string section, string sample) : base(section, sample) { }

        #endregion

        // Logging Example
        public override void ExecuteSample(Arguments args)
        {
            ExecuteTagsAndRules();

            PauseSample();

            ExecuteExceptionExample();
        }

        public void ExecuteTagsAndRules()
        {
            // Initialize here because the samples are constructed only once
            //  We want to be running on the configuration for this sample
            LoggerFactory.Initialize("Samples/S1_2_TagsRules/NuLog.json");
            LoggerBase logger = LoggerFactory.GetLogger();

            // Exercise our rules
            logger.LogNow("I will go to trace");
            logger.LogNow("I will go to both console and trace", "mytag");

            logger.LogNow("I will only go to the console because of final", "consoleonly");
            logger.LogNow("I will only go to the console because I am excluded from the trace", "mytag", "notrace");

            // Testing out default tags:
            logger = LoggerFactory.GetLogger(defaultTags: "consoleonly");
            logger.LogNow("I will go to console only, because of a default tag");

            // Clean up for when we run again
            logger.DefaultTags.Remove("consoleonly");
        }

        public void ExecuteExceptionExample()
        {
            // Initialize here because the samples are constructed only once
            //  We want to be running on the configuration for this sample
            LoggerFactory.Initialize("Samples/S1_2_TagsRules/ExceptionNuLog.json");
            LoggerBase logger = LoggerFactory.GetLogger();

            // Normal log message
            logger.LogNow("No exception, all is well here");

            // Exception log message
            try
            {
                throw new Exception("I am an error!");
            }
            catch(Exception cause)
            {
                logger.LogNow("I caught an exception!", cause);
            }
        }
    }
}
