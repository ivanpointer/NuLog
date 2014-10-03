using Newtonsoft.Json.Linq;
using NuLog.Configuration.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Configuration.Targets
{
    public class SmtpTargetConfig : TargetConfig
    {
        private const string DefaultSubjectLayout = "${Subject}";
        private const string DefaultBodyLayout = "${DateTime:'{0:MM/dd/yyyy hh:mm:ss.fff}'} | ${Thread.ManagedThreadId:'{0,4}'} | ${Tags} | ${Message}${?Exception:'\r\n{0}'}\r\n";

        // Server Information
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool EnableSSL { get; set; }

        // Message Information
        public string FromAddress { get; set; }
        public ICollection<string> ReplyTo { get; set; }
        public ICollection<string> To { get; set; }
        public ICollection<string> CC { get; set; }
        public ICollection<string> BCC { get; set; }
        public string Subject { get; set; }
        public LayoutConfig SubjectLayout { get; set; }
        public LayoutConfig BodyLayout { get; set; }
        public string BodyFile { get; set; }
        public bool IsBodyHtml { get; set; }
        public IDictionary<string, string> Headers { get; set; }

        public SmtpTargetConfig(JToken jToken = null)
        {
            Defaults();

            if (jToken != null)
            {
                Host = GetValue<string>(jToken, "host", Host);
                Port = GetValue<int>(jToken, "port", Port);
                UserName = GetValue<string>(jToken, "userName", UserName);
                Password = GetValue<string>(jToken, "password", Password);
                EnableSSL = GetValue<bool>(jToken, "enableSSL", EnableSSL);
                FromAddress = GetValue<string>(jToken, "fromAddress", FromAddress);
                Subject = GetValue<string>(jToken, "subject", Subject);

                var replyToToken = jToken["replyTo"];
                if (replyToToken != null && replyToToken.Type == JTokenType.Array)
                    ReplyTo = replyToToken.Values<string>().ToList();

                var toToken = jToken["to"];
                if (toToken != null && toToken.Type == JTokenType.Array)
                    To = toToken.Values<string>().ToList();

                var ccToken = jToken["cc"];
                if (ccToken != null && toToken.Type == JTokenType.Array)
                    CC = ccToken.Values<string>().ToList();

                var bccToken = jToken["bcc"];
                if (bccToken != null && bccToken.Type == JTokenType.Array)
                    BCC = bccToken.Values<string>().ToList();

                var subjectToken = jToken["subjectLayout"];
                SubjectLayout = subjectToken != null
                    ? new LayoutConfig(subjectToken)
                    : new LayoutConfig(DefaultSubjectLayout);

                var bodyToken = jToken["bodyLayout"];
                BodyLayout = bodyToken != null
                    ? new LayoutConfig(bodyToken)
                    : new LayoutConfig(DefaultBodyLayout);

                var headersToken = jToken["headers"];
                if (headersToken != null)
                {
                    var headers = headersToken.Children();
                    Headers = new Dictionary<string, string>();
                    JToken nameToken;
                    string name;

                    JToken valueToken;
                    string value;
                    foreach (var header in headers)
                    {
                        nameToken = header["name"];
                        name = nameToken != null
                            ? nameToken.Value<string>()
                            : null;

                        valueToken = header["token"];
                        value = valueToken != null
                            ? valueToken.Value<string>()
                            : null;

                        if (String.IsNullOrEmpty(name) == false)
                            Headers[name] = value;
                    }
                }

                BodyFile = GetValue<string>(jToken, "bodyFile", BodyFile);
                IsBodyHtml = GetValue<bool>(jToken, "isBodyHtml", IsBodyHtml);
            }
        }

        private void Defaults()
        {
            Host = "localhost";
            Port = 25; // Default SMTP
            UserName = null;
            Password = null;
            EnableSSL = false;
            FromAddress = null;
            ReplyTo = null;
            To = null;
            CC = null;
            BCC = null;
            Subject = "Message from Logging System";
            SubjectLayout = null;
            BodyLayout = null;
            BodyFile = null;
            IsBodyHtml = false;
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
