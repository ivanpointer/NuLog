using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Configuration.Targets
{
    public enum RolloverPolicy
    {
        None,
        Size,
        Day
    }

    public class TextFileTargetConfig : LayoutTargetConfig
    {
        public bool Append { get; set; }
        public string FileName { get; set; }
        public string OldFileNamePattern { get; set; }
        public RolloverPolicy RolloverPolicy { get; set; }
        public long RolloverTrigger { get; set; }
        public int OldFileLimit { get; set; }
        public bool CompressOldFiles { get; set; }
        public int CompressionLevel { get; set; }
        public string CompressionPassword { get; set; }

        public TextFileTargetConfig()
            : base()
        {
            Defaults();
        }

        public TextFileTargetConfig(JToken jToken)
            : base(jToken)
        {
            Defaults();

            if (jToken != null)
            {
                Append = GetValue<bool>(jToken, "append", Append);
                FileName = GetValue<string>(jToken, "fileName", FileName);
                OldFileNamePattern = GetValue<string>(jToken, "oldFileNamePattern", OldFileNamePattern);

                var rolloverPolicyToken = jToken["rolloverPolicy"];
                if (rolloverPolicyToken != null)
                {
                    var rolloverPolicyName = rolloverPolicyToken.Value<string>();
                    RolloverPolicy = (RolloverPolicy)Enum.Parse(typeof(RolloverPolicy), rolloverPolicyName);
                }

                if (RolloverPolicy == Targets.RolloverPolicy.Size)
                {
                    var triggerString = GetValue<string>(jToken, "rolloverTrigger", "1MB");
                    triggerString = triggerString.ToUpper();
                    long triggerLong = Convert.ToInt64(triggerString.Substring(0, triggerString.Length - 2));

                    if (triggerString.EndsWith("GB"))
                    {
                        RolloverTrigger = triggerLong * 1024 * 1024 * 1024;
                    }
                    else if (triggerString.EndsWith("MB"))
                    {
                        RolloverTrigger = triggerLong * 1024 * 1024;
                    }
                    else if (triggerString.EndsWith("KB"))
                    {
                        RolloverTrigger = triggerLong * 1024;
                    }
                    else
                    {
                        RolloverTrigger = triggerLong;
                    }
                }
                else
                {
                    RolloverTrigger = 0L;
                }

                OldFileLimit = GetValue<int>(jToken, "oldFileLimit", OldFileLimit);
                CompressOldFiles = GetValue<bool>(jToken, "compressOldFiles", CompressOldFiles);
                CompressionLevel = GetValue<int>(jToken, "compressionLevel", CompressionLevel);
                CompressionPassword = GetValue<string>(jToken, "compressionPassword", CompressionPassword);
            }
        }

        private void Defaults()
        {
            Append = true;
            FileName = "log.log";
            OldFileNamePattern = "log{0:.yyyy.MM.dd.hh.mm.ss}.log";
            RolloverPolicy = RolloverPolicy.None;
            RolloverTrigger = -1;
            OldFileLimit = -1;
            CompressOldFiles = false;
            CompressionLevel = 3;
            CompressionPassword = null;
        }

        #region Helpers

        private T GetValue<T>(JToken jToken, string name, T defVal)
        {
            var child = jToken[name];
            if (child != null)
                return child.Value<T>();
            return defVal;
        }

        #endregion
    }
}
