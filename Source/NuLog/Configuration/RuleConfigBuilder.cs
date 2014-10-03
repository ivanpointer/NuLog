using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Configuration
{
    public class RuleConfigBuilder
    {
        public ICollection<string> Include { get; set; }
        public bool StrictInclude { get; set; }
        public ICollection<string> Exclude { get; set; }
        public ICollection<string> WriteTo { get; set; }
        public bool Final { get; set; }

        private RuleConfigBuilder()
        {
            Include = new List<string>();
            StrictInclude = false;
            Exclude = new List<string>();
            WriteTo = new List<string>();
            Final = false;
        }

        public static RuleConfigBuilder CreateRuleConfig()
        {
            return new RuleConfigBuilder();
        }

        public RuleConfigBuilder AddInclude(params string[] includes)
        {
            foreach (var include in includes)
                Include.Add(include);
            return this;
        }

        public RuleConfigBuilder AddExclude(params string[] excludes)
        {
            foreach (var exclude in excludes)
                Exclude.Add(exclude);
            return this;
        }

        public RuleConfigBuilder AddWriteTo(params string[] writeTos)
        {
            foreach (var writeTo in writeTos)
                WriteTo.Add(writeTo);
            return this;
        }

        public RuleConfigBuilder SetStrictInclude(bool strictInclude)
        {
            StrictInclude = strictInclude;
            return this;
        }

        public RuleConfigBuilder SetFinal(bool final)
        {
            Final = final;
            return this;
        }

        public RuleConfig Build()
        {
            return new RuleConfig
            {
                Include = this.Include,
                StrictInclude = this.StrictInclude,
                Exclude = this.Exclude,
                WriteTo = this.WriteTo,
                Final = this.Final
            };
        }

    }
}
