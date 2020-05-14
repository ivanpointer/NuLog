/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using System.Collections.Generic;

namespace NuLog.Tests {

    public class TargetConfigBuilder {
        private readonly TargetConfig targetConfig;

        private TargetConfigBuilder() {
            targetConfig = new TargetConfig {
                Properties = new Dictionary<string, object>()
            };
        }

        public static TargetConfigBuilder Start() {
            return new TargetConfigBuilder();
        }

        public TargetConfigBuilder Add(string key, string value) {
            targetConfig.Properties.Add(key, value);
            return this;
        }

        public TargetConfig Build() {
            return this.targetConfig;
        }
    }
}