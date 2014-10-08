/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/7/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

namespace NuLog.Configuration
{
    /// <summary>
    /// A helper class to help with building rule configurations at runtime
    /// </summary>
    public class RuleConfigBuilder
    {
        // The RuleConfig being built
        private RuleConfig RuleConfig { get; set; }

        // Private constructor, we want "CreateRuleConfig" to be the first call
        private RuleConfigBuilder()
        {
            RuleConfig = new RuleConfig();
        }

        /// <summary>
        /// Creates a new RuleConfigBuilder instance
        /// </summary>
        /// <returns>A new RuleConfigBuilder instance</returns>
        public static RuleConfigBuilder CreateRuleConfig()
        {
            return new RuleConfigBuilder();
        }

        /// <summary>
        /// Adds one or more tags to the include list
        /// </summary>
        /// <param name="includes">The tags to add to the include list</param>
        /// <returns>This RuleConfigBuilder</returns>
        public RuleConfigBuilder AddInclude(params string[] includes)
        {
            foreach (var include in includes)
                RuleConfig.Include.Add(include);
            return this;
        }

        /// <summary>
        /// Adds one or more tags to the exclude list
        /// </summary>
        /// <param name="excludes">The tags to add to the exclude list</param>
        /// <returns>This RuleConfigBuilder</returns>
        public RuleConfigBuilder AddExclude(params string[] excludes)
        {
            foreach (var exclude in excludes)
                RuleConfig.Exclude.Add(exclude);
            return this;
        }

        /// <summary>
        /// Adds one or more names of targets to the  write-to list
        /// </summary>
        /// <param name="writeTos">The names of the targets to add to the list</param>
        /// <returns>This RuleConfigBuilder</returns>
        public RuleConfigBuilder AddWriteTo(params string[] writeTos)
        {
            foreach (var writeTo in writeTos)
                RuleConfig.WriteTo.Add(writeTo);
            return this;
        }

        /// <summary>
        /// Sets the "strict include" flag
        /// </summary>
        /// <param name="strictInclude">The "strict include" flag to set</param>
        /// <returns>This RuleConfigBuilder</returns>
        public RuleConfigBuilder SetStrictInclude(bool strictInclude)
        {
            RuleConfig.StrictInclude = strictInclude;
            return this;
        }

        /// <summary>
        /// Sets the "final" flag
        /// </summary>
        /// <param name="final">The "final" flag to set</param>
        /// <returns>This RuleConfigBuilder</returns>
        public RuleConfigBuilder SetFinal(bool final)
        {
            RuleConfig.Final = final;
            return this;
        }

        /// <summary>
        /// Returns the built RuleConfig
        /// </summary>
        /// <returns>The built RuleConfig</returns>
        public RuleConfig Build()
        {
            return RuleConfig;
        }

    }
}
