using NuLog.Configuration.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Configuration.Targets
{
    public class SmtpTargetConfigBuilder
    {
        protected SmtpTargetConfig Config { get; set; }

        private SmtpTargetConfigBuilder()
        {
            Config = new SmtpTargetConfig();
        }

        public static SmtpTargetConfigBuilder Create()
        {
            return new SmtpTargetConfigBuilder();
        }

        public SmtpTargetConfigBuilder SetName(string name)
        {
            Config.Name = name;
            return this;
        }

        public SmtpTargetConfigBuilder SetSynchronous(bool synchronous)
        {
            Config.Synchronous = synchronous;
            return this;
        }

        public SmtpTargetConfigBuilder SetHost(string host)
        {
            Config.Host = host;
            return this;
        }

        public SmtpTargetConfigBuilder SetPort(int port)
        {
            Config.Port = port;
            return this;
        }

        public SmtpTargetConfigBuilder SetUserName(string userName)
        {
            Config.UserName = userName;
            return this;
        }

        public SmtpTargetConfigBuilder SetPassword(string password)
        {
            Config.Password = password;
            return this;
        }

        public SmtpTargetConfigBuilder SetEnableSSL(bool enableSSL)
        {
            Config.EnableSSL = enableSSL;
            return this;
        }

        public SmtpTargetConfigBuilder SetFromAddress(string fromAddress)
        {
            Config.FromAddress = fromAddress;
            return this;
        }

        public SmtpTargetConfigBuilder AddReplyTo(string reployTo)
        {
            if (Config.ReplyTo == null)
                Config.ReplyTo = new List<string>();

            Config.ReplyTo.Add(reployTo);

            return this;
        }

        public SmtpTargetConfigBuilder AddTo(string to)
        {
            if (Config.To == null)
                Config.To = new List<string>();

            Config.To.Add(to);

            return this;
        }

        public SmtpTargetConfigBuilder AddCC(string cc)
        {
            if (Config.CC == null)
                Config.CC = new List<string>();

            Config.CC.Add(cc);

            return this;
        }

        public SmtpTargetConfigBuilder AddBCC(string bcc)
        {
            if (Config.BCC == null)
                Config.BCC = new List<string>();

            Config.BCC.Add(bcc);

            return this;
        }

        public SmtpTargetConfigBuilder SetSubject(string subject)
        {
            Config.Subject = subject;

            return this;
        }

        public SmtpTargetConfigBuilder SetSubjectLayout(LayoutConfig subjectLayout)
        {
            Config.SubjectLayout = subjectLayout;

            return this;
        }

        public SmtpTargetConfigBuilder SetBodyLayout(LayoutConfig bodyLayout)
        {
            Config.BodyLayout = bodyLayout;

            return this;
        }

        public SmtpTargetConfigBuilder SetBodyFile(string bodyFile)
        {
            Config.BodyFile = bodyFile;

            return this;
        }

        public SmtpTargetConfigBuilder SetIsBodyHtml(bool isBodyHtml)
        {
            Config.IsBodyHtml = isBodyHtml;

            return this;
        }

        public SmtpTargetConfigBuilder AddHeader(string name, string value)
        {
            if (Config.Headers == null)
                Config.Headers = new Dictionary<string, string>();

            Config.Headers[name] = value;

            return this;
        }

        public SmtpTargetConfig Build()
        {
            return Config;
        }
    }
}
