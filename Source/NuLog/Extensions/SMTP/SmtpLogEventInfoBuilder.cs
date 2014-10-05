using NuLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Extensions.SMTP
{
    public class SmtpLogEventInfoBuilder
    {
        protected LogEvent LogEventInfo { get; set; }
        protected string Subject { get; set; }
        protected ICollection<SmtpAttachment> Attachments { get; set; }
        protected IDictionary<string, string> Headers { get; set; }
        protected ICollection<string> To { get; set; }
        protected ICollection<string> CC { get; set; }
        protected ICollection<string> BCC { get; set; }

        private SmtpLogEventInfoBuilder()
        {
            LogEventInfo = new LogEvent();
            Attachments = new List<SmtpAttachment>();
            Headers = new Dictionary<string, string>();

            To = new List<string>();
            CC = new List<string>();
            BCC = new List<string>();
        }

        public static SmtpLogEventInfoBuilder Create()
        {
            return new SmtpLogEventInfoBuilder();
        }

        public static SmtpLogEventInfoBuilder Create(LogEvent logEventInfo)
        {
            return new SmtpLogEventInfoBuilder
            {
                LogEventInfo = logEventInfo
            };
        }

        public static SmtpLogEventInfoBuilder Create(string message, params string[] tags)
        {
            return new SmtpLogEventInfoBuilder
            {
                LogEventInfo = new LogEvent
                {
                    Message = message,
                    Tags = tags
                }
            };
        }

        public static SmtpLogEventInfoBuilder Create(string message, IDictionary<string, object> metaData, params string[] tags)
        {
            return new SmtpLogEventInfoBuilder
            {
                LogEventInfo = new LogEvent
                {
                    Message = message,
                    MetaData = metaData,
                    Tags = tags
                }
            };
        }

        public static SmtpLogEventInfoBuilder Create(string message, Exception exception, params string[] tags)
        {
            return new SmtpLogEventInfoBuilder
            {
                LogEventInfo = new LogEvent
                {
                    Message = message,
                    Exception = exception,
                    Tags = tags
                }
            };
        }

        public static SmtpLogEventInfoBuilder Create(string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            return new SmtpLogEventInfoBuilder
            {
                LogEventInfo = new LogEvent
                {
                    Message = message,
                    Exception = exception,
                    MetaData = metaData,
                    Tags = tags
                }
            };
        }

        public SmtpLogEventInfoBuilder SetSubject(string subject)
        {
            Subject = subject;
            return this;
        }

        public SmtpLogEventInfoBuilder AddToAddress(string toAddress)
        {
            To.Add(toAddress);
            return this;
        }

        public SmtpLogEventInfoBuilder AddCCAddress(string ccAddress)
        {
            CC.Add(ccAddress);
            return this;
        }

        public SmtpLogEventInfoBuilder AddBCCAdress(string bccAddress)
        {
            BCC.Add(bccAddress);
            return this;
        }

        public SmtpLogEventInfoBuilder AddAttachment(string fileName)
        {
            Attachments.Add(new SmtpAttachment
            {
                FileName = fileName
            });
            return this;
        }

        public SmtpLogEventInfoBuilder AddAttachment(byte[] data, string name)
        {
            Attachments.Add(new SmtpAttachment
            {
                Data = data,
                Name = name
            });
            return this;
        }

        public SmtpLogEventInfoBuilder AddAttachment(SmtpAttachment attachment)
        {
            Attachments.Add(attachment);
            return this;
        }

        public SmtpLogEventInfoBuilder SetHeader(string name, string value)
        {
            if (String.IsNullOrEmpty(name) == false)
                Headers[name] = value;
            return this;
        }

        public LogEvent Build()
        {
            if (LogEventInfo.MetaData == null)
                LogEventInfo.MetaData = new Dictionary<string, object>();

            if (String.IsNullOrEmpty(Subject))
                LogEventInfo.MetaData[SmtpTarget.MetaSubject] = Subject;

            if (Attachments.Count > 0)
                LogEventInfo.MetaData[SmtpTarget.MetaAttachments] = Attachments;

            if (Headers.Count > 0)
                LogEventInfo.MetaData[SmtpTarget.MetaHeaders] = Headers;

            if (To.Count > 0)
                LogEventInfo.MetaData[SmtpTarget.MetaTo] = To;

            if (CC.Count > 0)
                LogEventInfo.MetaData[SmtpTarget.MetaCC] = CC;

            if (BCC.Count > 0)
                LogEventInfo.MetaData[SmtpTarget.MetaBCC] = BCC;

            return LogEventInfo;
        }

    }
}
