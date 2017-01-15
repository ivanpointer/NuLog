/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

namespace NuLog.Samples.Samples.S1_3_TagGroups
{
    /// <summary>
    /// A sample showing how tag groups work.  The narration of this sample can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/1.3-Tag-Groups
    /// </summary>
    public class TagGroupsSample : SampleBase
    {
        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public TagGroupsSample(string section, string sample) : base(section, sample) { }

        #endregion Sample Wiring

        // Execute our sample
        public override void ExecuteSample()
        {
            // Initialize here because the samples are constructed only once
            //  We want to be running on the configuration for this sample
            var factory = new LoggerFactory("Samples/S1_3_TagGroups/NuLog.json");
            var logger = factory.Logger();

            logger.LogNow("I like oranges!", "orange");
            logger.LogNow("I really like peaches!", "peach");
            logger.LogNow("I like tomatoes!", "tomato");
            logger.LogNow("I like fruit!", "fruit");
            logger.LogNow("I don't like mushrooms!", "mushroom");

            logger.LogNow("Debug message!", "debug");
            logger.LogNow("Info message!", "info");
            logger.LogNow("Fatal message!", "fatal");
        }
    }
}