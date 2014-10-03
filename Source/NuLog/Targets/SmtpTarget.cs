using NuLog.Configuration.Layouts;
using NuLog.Configuration.Targets;
using NuLog.Targets.Layouts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Targets
{
    public class SmtpTarget : TargetBase
    {
        public const string MetaSubject = "EmailSubject";
        public const string MetaHeaders = "EmailHeaders";
        public const string MetaAttachments = "EmailAttachments";
        public const string MetaTo = "EmailTo";
        public const string MetaCC = "EmailCC";
        public const string MetaBCC = "EmailBCC";

        private static readonly object _configLock = new object();
        private SmtpTargetConfig Config { get; set; }

        private ILayout SubjectLayout { get; set; }
        private ILayout BodyLayout { get; set; }

        private MailAddress FromAddress { get; set; }
        private ICollection<MailAddress> ReplyTo { get; set; }
        private ICollection<MailAddress> To { get; set; }
        private ICollection<MailAddress> CC { get; set; }
        private ICollection<MailAddress> BCC { get; set; }

        public SmtpTarget()
        {
            ReplyTo = new List<MailAddress>();
            To = new List<MailAddress>();
            CC = new List<MailAddress>();
            BCC = new List<MailAddress>();
        }

        public override void Initialize(TargetConfig targetConfig, bool? synchronous = null)
        {
            lock (_configLock)
            {
                if (targetConfig != null)
                {
                    base.Initialize(targetConfig, synchronous);

                    // Check target config type here then get it into a SmtpTargetConfig
                    if (typeof(SmtpTargetConfig).IsAssignableFrom(targetConfig.GetType()))
                    {
                        Config = (SmtpTargetConfig)targetConfig;
                    }
                    else
                    {
                        Config = new SmtpTargetConfig(targetConfig.Config);
                    }

                    SubjectLayout = LayoutFactory.BuildLayout(Config.SubjectLayout);

                    // Make sure body layout is setup correctly
                    if (Config.BodyLayout == null)
                        Config.BodyLayout = new LayoutConfig();
                    if (String.IsNullOrEmpty(Config.BodyFile) == false && File.Exists(Config.BodyFile))
                        Config.BodyLayout.Format = File.ReadAllText(Config.BodyFile);
                    BodyLayout = LayoutFactory.BuildLayout(Config.BodyLayout);

                    FromAddress = new MailAddress(Config.FromAddress);
                    if (Config.ReplyTo != null)
                        foreach (var replyToAddress in Config.ReplyTo)
                            ReplyTo.Add(new MailAddress(replyToAddress));

                    if (Config.To != null)
                        foreach (var toAddress in Config.To)
                            To.Add(new MailAddress(toAddress));

                    if (Config.CC != null)
                        foreach (var ccAddress in Config.CC)
                            CC.Add(new MailAddress(ccAddress));

                    if (Config.BCC != null)
                        foreach (var bccAddress in Config.BCC)
                            BCC.Add(new MailAddress(bccAddress));
                }
            }
        }

        public override void NotifyNewConfig(TargetConfig targetConfig)
        {
            base.NotifyNewConfig(targetConfig);

            lock (_configLock)
            {
                FromAddress = null;
                ReplyTo.Clear();
                To.Clear();
                CC.Clear();
                BCC.Clear();

                Initialize(targetConfig);
            }
        }

        public override void Log(LogEvent logEvent)
        {
            lock (_configLock)
            {
                using (var smtpClient = new SmtpClient(Config.Host, Config.Port))
                using (var mailMessage = new MailMessage())
                {
                    smtpClient.EnableSsl = Config.EnableSSL;

                    if (!String.IsNullOrEmpty(Config.UserName) && !String.IsNullOrEmpty(Config.Password))
                        smtpClient.Credentials = new NetworkCredential(Config.UserName, Config.Password);

                    mailMessage.From = FromAddress;

                    foreach (var replyAddress in ReplyTo)
                        mailMessage.ReplyToList.Add(replyAddress);

                    foreach (var toAddress in To)
                        mailMessage.To.Add(toAddress);
                    if (logEvent.MetaData != null && logEvent.MetaData.ContainsKey(MetaTo))
                    {
                        var emailToRaw = logEvent.MetaData[MetaTo];
                        if (emailToRaw != null && typeof(ICollection<string>).IsAssignableFrom(emailToRaw.GetType()))
                        {
                            var emailTo = (ICollection<string>)emailToRaw;
                            foreach (var address in emailTo)
                                mailMessage.To.Add(new MailAddress(address));
                        }
                    }

                    foreach (var ccAddress in CC)
                        mailMessage.CC.Add(ccAddress);
                    if (logEvent.MetaData != null && logEvent.MetaData.ContainsKey(MetaCC))
                    {
                        var emailCCRaw = logEvent.MetaData[MetaCC];
                        if (emailCCRaw != null && typeof(ICollection<string>).IsAssignableFrom(emailCCRaw.GetType()))
                        {
                            var emailCC = (ICollection<string>)emailCCRaw;
                            foreach (var address in emailCC)
                                mailMessage.CC.Add(new MailAddress(address));
                        }
                    }

                    foreach (var bccAddress in BCC)
                        mailMessage.Bcc.Add(bccAddress);
                    if (logEvent.MetaData != null && logEvent.MetaData.ContainsKey(MetaBCC))
                    {
                        var emailBCCRaw = logEvent.MetaData[MetaBCC];
                        if (emailBCCRaw != null && typeof(ICollection<string>).IsAssignableFrom(emailBCCRaw.GetType()))
                        {
                            var emailBCC = (ICollection<string>)emailBCCRaw;
                            foreach (var address in emailBCC)
                                mailMessage.Bcc.Add(new MailAddress(address));
                        }
                    }

                    mailMessage.IsBodyHtml = Config.IsBodyHtml;

                    if (logEvent.MetaData == null)
                    {
                        logEvent.MetaData = new Dictionary<string, object>();
                        logEvent.MetaData[MetaSubject] = Config.Subject;
                    }
                    else if (!logEvent.MetaData.ContainsKey(MetaSubject))
                    {
                        logEvent.MetaData[MetaSubject] = Config.Subject;
                    }

                    mailMessage.Subject = SubjectLayout.FormatLogEvent(logEvent);
                    mailMessage.Body = BodyLayout.FormatLogEvent(logEvent);

                    if (Config.Headers != null && Config.Headers.Count > 0)
                        foreach (var name in Config.Headers.Keys)
                            mailMessage.Headers.Add(name, Config.Headers[name]);

                    if (logEvent.MetaData != null && logEvent.MetaData.ContainsKey(MetaHeaders))
                    {
                        var emailHeadersRaw = logEvent.MetaData[MetaHeaders];
                        if (emailHeadersRaw != null && typeof(IDictionary<string, string>).IsAssignableFrom(emailHeadersRaw.GetType()))
                        {
                            var emailHeaders = (IDictionary<string, string>)emailHeadersRaw;
                            foreach (var name in emailHeaders.Keys)
                                mailMessage.Headers.Add(name, emailHeaders[name]);
                        }
                    }

                    var memoryStreams = new List<MemoryStream>();
                    try
                    {
                        if (logEvent.MetaData != null && logEvent.MetaData.ContainsKey(MetaAttachments))
                        {
                            object emailAttachmentsObj = logEvent.MetaData[MetaAttachments];
                            if (typeof(ICollection<SmtpAttachment>).IsAssignableFrom(emailAttachmentsObj.GetType()))
                            {
                                var emailAttachments = (ICollection<SmtpAttachment>)emailAttachmentsObj;
                                MemoryStream newMemoryStream;
                                foreach (var emailAttachment in emailAttachments)
                                {
                                    if (emailAttachment.Data != null)
                                    {
                                        newMemoryStream = new MemoryStream(emailAttachment.Data);
                                        memoryStreams.Add(newMemoryStream);
                                        mailMessage.Attachments.Add(new Attachment(newMemoryStream, emailAttachment.Name));
                                    }
                                    else if (String.IsNullOrEmpty(emailAttachment.FileName) == false && File.Exists(emailAttachment.FileName))
                                    {
                                        mailMessage.Attachments.Add(new Attachment(emailAttachment.FileName));
                                    }
                                }
                            }
                        }

                        smtpClient.Send(mailMessage);
                    }
                    finally
                    {
                        foreach (var memoryStream in memoryStreams)
                        {
                            try
                            {
                                memoryStream.Close();
                                memoryStream.Dispose();
                            }
                            catch (Exception e)
                            {
                                Trace.WriteLine(e);
                            }
                        }
                    }
                }
            }
        }
    }
}
