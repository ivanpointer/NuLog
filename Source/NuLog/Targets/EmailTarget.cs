/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/8/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */
using NuLog.Configuration.Layouts;
using NuLog.Configuration.Targets;
using NuLog.Dispatch;
using NuLog.Layouts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace NuLog.Targets
{
    /// <summary>
    /// Sends log events via email message
    /// </summary>
    public class EmailTarget : TargetBase
    {

        #region Constants

        public const string ParseEmailAddressFailureMessage = "Failed to parse email address \"{0}\"";
        public const string AttachmentNotFoundMessage = "Cannot attach \"{0}\", not found";
        public const string FailedToLoadBodyFileMessage = "Failed to load body layout file \"{0}\" because of exception {1}";

        public const string MetaSubject = "EmailSubject";
        public const string MetaHeaders = "EmailHeaders";
        public const string MetaAttachments = "EmailAttachments";
        public const string MetaTo = "EmailTo";
        public const string MetaCC = "EmailCC";
        public const string MetaBCC = "EmailBCC";

        #endregion

        #region Members, Constructors and Initialization

        // Configuration
        private static readonly object _configLock = new object();
        private EmailTargetConfig Config { get; set; }

        // Layouts
        private ILayout SubjectLayout { get; set; }
        private ILayout BodyLayout { get; set; }

        // Email Information (Defaults, can be overridden with Meta Data)
        private MailAddress FromAddress { get; set; }
        private ICollection<MailAddress> ReplyTo { get; set; }
        private ICollection<MailAddress> To { get; set; }
        private ICollection<MailAddress> CC { get; set; }
        private ICollection<MailAddress> BCC { get; set; }

        /// <summary>
        /// Builds an empty, unconfigured email target
        /// </summary>
        public EmailTarget()
        {
            ReplyTo = new List<MailAddress>();
            To = new List<MailAddress>();
            CC = new List<MailAddress>();
            BCC = new List<MailAddress>();
        }

        /// <summary>
        /// Initializes/Configures this email target
        /// </summary>
        /// <param name="targetConfig">The target config to use to configure this email target</param>
        /// <param name="dispatcher">The dispatcher this target is associated to</param>
        /// <param name="synchronous">An optional override to the synchronous flag in the target config</param>
        public override void Initialize(TargetConfig targetConfig, LogEventDispatcher dispatcher = null, bool? synchronous = null)
        {
            // Wire up all the default goodies
            base.Initialize(targetConfig, dispatcher, synchronous);

            lock (_configLock)
            {
                // Reset our "Default" settings to "zero"
                FromAddress = null;
                ReplyTo.Clear();
                To.Clear();
                CC.Clear();
                BCC.Clear();

                if (targetConfig != null)
                {
                    // Check target config type here then get it into a SmtpTargetConfig
                    if (typeof(EmailTargetConfig).IsAssignableFrom(targetConfig.GetType()))
                    {
                        Config = (EmailTargetConfig)targetConfig;
                    }
                    else
                    {
                        Config = new EmailTargetConfig(targetConfig.Config);
                    }

                    // Build out the subject layout
                    SubjectLayout = LayoutFactory.BuildLayout(Config.SubjectLayout);

                    // Make sure body layout is setup correctly
                    //  If the body file is specified, load a StandardLayout with the contents of the file as the format
                    //  Otherwise, use the layout factory to build our layout
                    if (String.IsNullOrEmpty(Config.BodyFile))
                    {
                        BodyLayout = LayoutFactory.BuildLayout(Config.BodyLayout);
                    }
                    else
                    {
                        try
                        {
                            string fileContent = File.ReadAllText(Config.BodyFile);
                            BodyLayout = new StandardLayout(fileContent);
                        }
                        catch(Exception cause)
                        {
                            Trace.WriteLine(String.Format(FailedToLoadBodyFileMessage, Config.BodyFile, cause));
                            BodyLayout = new StandardLayout();
                        }
                    }

                    // Parse out the addresses
                    FromAddress = new MailAddress(Config.FromAddress);

                    // Parse the other email address lists
                    AddAddresses(ReplyTo, Config.ReplyTo);
                    AddAddresses(To, Config.To);
                    AddAddresses(CC, Config.CC);
                    AddAddresses(BCC, Config.BCC);
                }
            }
        }

        #endregion

        #region Logging

        /// <summary>
        /// Sends the given log event as an email using the configuration set by <see cref="Initialize" />
        /// </summary>
        /// <param name="logEvent">The log event to send as an email</param>
        public override void Log(LogEvent logEvent)
        {
            lock (_configLock)
            {
                using (var mailMessage = new MailMessage())
                {
                    // Setup the addressing
                    mailMessage.From = FromAddress;
                    SetAddresses(mailMessage.To, logEvent.MetaData, MetaTo, To);
                    SetAddresses(mailMessage.CC, logEvent.MetaData, MetaCC, CC);
                    SetAddresses(mailMessage.Bcc, logEvent.MetaData, MetaBCC, BCC);
                    foreach (var replyAddress in ReplyTo)
                        mailMessage.ReplyToList.Add(replyAddress);

                    // Setup the subject
                    SetSubject(logEvent, mailMessage);
                    
                    // Setup the body
                    SetMessageBody(logEvent, mailMessage);

                    // Setup the headers
                    SetMessageHeaders(logEvent, mailMessage);

                    var memoryStreams = new List<MemoryStream>();
                    try
                    {
                        // Setup the attachments
                        SetAttachments(logEvent, mailMessage, memoryStreams);

                        // Send the message
                        using (var smtpClient = new SmtpClient(Config.Host, Config.Port))
                        {
                            smtpClient.EnableSsl = Config.EnableSSL;

                            if (!String.IsNullOrEmpty(Config.UserName) && !String.IsNullOrEmpty(Config.Password))
                                smtpClient.Credentials = new NetworkCredential(Config.UserName, Config.Password);

                            smtpClient.Send(mailMessage);
                        }
                    }
                    finally
                    {
                        // Make sure that the memory streams get closed out
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

        // Sets the address to the dest collection from the meta data, if found, or using the default otherwise
        private void SetAddresses(MailAddressCollection destCollection, IDictionary<string, object> metaData, string metaDataKey, ICollection<MailAddress> defaultSourceCollection)
        {
            // Addresses are setup to be overriden by the meta data (as opposed to supplemented)
            //  If addresses are provided in the meta data, the addresses in the meta data are used exclusively
            //   otherwise, the addresses provided in the configuration are used

            if(metaData != null && metaData.ContainsKey(metaDataKey))
            {
                var metaListRaw = metaData[metaDataKey];
                if (metaListRaw != null && typeof(ICollection<string>).IsAssignableFrom(metaListRaw.GetType()))
                {
                    var emailList = (ICollection<string>)metaListRaw;
                    AddAddresses(destCollection, emailList);
                }
            }
            else
            {
                foreach (var address in defaultSourceCollection)
                    destCollection.Add(address);
            }
        }

        // Set the subject to the mail message from the log event
        private void SetSubject(LogEvent logEvent, MailMessage mailMessage)
        {
            // The subject is stored in the metadata.  If the subject is not already
            //  in the meta data, we put it in here from the configuration.  In other
            //  words, the subject from the configuration is used by default, and
            //  can be overriden in the meta data.

            // A layout is used to format the subject

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
        }

        // Set the message body to the mail message from the log event
        private void SetMessageBody(LogEvent logEvent, MailMessage mailMessage)
        {
            // The message of the log event is used as the body of the email
            // A layout is used to format the message for the body of the email

            mailMessage.IsBodyHtml = Config.IsBodyHtml;
            mailMessage.Body = BodyLayout.FormatLogEvent(logEvent);
        }

        // Set the message headers to the mail message from the log event
        private void SetMessageHeaders(LogEvent logEvent, MailMessage mailMessage)
        {
            //   Headers are handled supplementally here.  Headers defined in both the config
            // and the meta data are used.  However, if there is a collision between
            // the configuration and the meta data, the header in the meta data trumps.

            if (Config.Headers != null && Config.Headers.Count > 0)
                foreach (var headerName in Config.Headers.Keys)
                    mailMessage.Headers.Add(headerName, Config.Headers[headerName]);

            if (logEvent.MetaData != null && logEvent.MetaData.ContainsKey(MetaHeaders))
            {
                var emailHeadersRaw = logEvent.MetaData[MetaHeaders];
                if (emailHeadersRaw != null && typeof(IDictionary<string, string>).IsAssignableFrom(emailHeadersRaw.GetType()))
                {
                    var emailHeaders = (IDictionary<string, string>)emailHeadersRaw;
                    foreach (var headerName in emailHeaders.Keys)
                        mailMessage.Headers.Add(headerName, emailHeaders[headerName]);
                }
            }
        }

        // Sets the attachments to the email from the log event
        private void SetAttachments(LogEvent logEvent, MailMessage mailMessage, List<MemoryStream> memoryStreams)
        {
            //   Checks to see if there are any attachments in the meta data.  If attachments are found in
            // the meta data, they are attached to the email.  If no attachments are provided in the
            // meta data, the configuration is checked for a single attachment (by file name), and that
            // is attached to the email.
            
            if (logEvent.MetaData != null && logEvent.MetaData.ContainsKey(MetaAttachments))
            {
                object emailAttachmentsObj = logEvent.MetaData[MetaAttachments];
                if (typeof(ICollection<EmailAttachment>).IsAssignableFrom(emailAttachmentsObj.GetType()))
                {
                    // An email attachment can either be a string path to a file, or a byte array.

                    var emailAttachments = (ICollection<EmailAttachment>)emailAttachmentsObj;
                    MemoryStream newMemoryStream;
                    foreach (var emailAttachment in emailAttachments)
                    {
                        if (emailAttachment.Data != null)
                        {
                            newMemoryStream = new MemoryStream(emailAttachment.Data);
                            memoryStreams.Add(newMemoryStream);
                            mailMessage.Attachments.Add(new Attachment(newMemoryStream, emailAttachment.AttachmentFileName));
                        }
                        else if (String.IsNullOrEmpty(emailAttachment.PhysicalFileName) == false)
                        {
                            if(File.Exists(emailAttachment.PhysicalFileName))
                            {
                                mailMessage.Attachments.Add(new Attachment(emailAttachment.PhysicalFileName)
                                {
                                    Name = String.IsNullOrEmpty(emailAttachment.AttachmentFileName)
                                        ? Path.GetFileName(emailAttachment.PhysicalFileName)
                                        : emailAttachment.AttachmentFileName
                                });
                            }
                            else
                            {
                                Trace.WriteLine(String.Format(AttachmentNotFoundMessage, emailAttachment.PhysicalFileName));
                            }
                        }
                    }
                }
            }
            else if (String.IsNullOrEmpty(Config.Attachment) == false)
            {
                if (File.Exists(Config.Attachment))
                {
                    mailMessage.Attachments.Add(new Attachment(Config.Attachment));
                }
                else
                {
                    Trace.WriteLine(String.Format(AttachmentNotFoundMessage, Config.Attachment));
                }
            }
        }

        #endregion

        #region Helpers

        // Helper method for distinctly adding string email addresses to to a list of parsed MailAddresses
        //  Gracefully ignores invalid email addresses
        private static void AddAddresses(ICollection<MailAddress> masterList, IEnumerable<string> childList)
        {
            if (childList != null)
                foreach (var address in childList)
                    if (masterList.Any(_ => _.Address == address) == false)
                        try
                        {
                            masterList.Add(new MailAddress(address));
                        }
                        catch
                        {
                            Trace.WriteLine(String.Format(ParseEmailAddressFailureMessage, address));
                        }
        }

        #endregion

    }
}
