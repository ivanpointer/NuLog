/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/7/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.Targets;
using System;
using System.Collections.Generic;

namespace NuLog.Extensions.Email
{
    /// <summary>
    /// A helper class for building a log event with meta data information specific to
    ///  email log events
    /// </summary>
    public class EmailLogEventBuilder
    {
        #region Members and Constructors

        protected LogEvent LogEvent { get; set; }
        protected string Subject { get; set; }
        protected ICollection<EmailAttachment> Attachments { get; set; }
        protected IDictionary<string, string> Headers { get; set; }
        protected ICollection<string> To { get; set; }
        protected ICollection<string> CC { get; set; }
        protected ICollection<string> BCC { get; set; }

        // Private internal constructor
        private EmailLogEventBuilder()
        {
            // Initialize the settings
            LogEvent = new LogEvent();
            Attachments = new List<EmailAttachment>();
            Headers = new Dictionary<string, string>();

            To = new List<string>();
            CC = new List<string>();
            BCC = new List<string>();
        }

        /// <summary>
        /// Creates a base SmtpLogEventBuilder with no pre-set meta data
        /// </summary>
        /// <returns>A new instance of SmtpLogEventBuilder</returns>
        public static EmailLogEventBuilder Create()
        {
            return new EmailLogEventBuilder();
        }

        /// <summary>
        /// Creates a new SmtpLogEventBuilder based on the passed log event
        /// </summary>
        /// <param name="logEvent">The log event to use as a base</param>
        /// <returns>A new instance of SmtpLogEventBuilder</returns>
        public static EmailLogEventBuilder Create(LogEvent logEvent)
        {
            return new EmailLogEventBuilder
            {
                LogEvent = logEvent
            };
        }

        /// <summary>
        /// Creates a new SmtpLogEventBuilder using the message and tags provided
        /// </summary>
        /// <param name="message">The message for the new log event</param>
        /// <param name="tags">The tags for the new log event</param>
        /// <returns>A new instance of SmtpLogEventBuilder</returns>
        public static EmailLogEventBuilder Create(string message, params string[] tags)
        {
            return new EmailLogEventBuilder
            {
                LogEvent = new LogEvent
                {
                    Message = message,
                    Tags = tags
                }
            };
        }

        /// <summary>
        /// Builds a new SmtpLogEventBuilder using the provided message, meta data and tags
        /// </summary>
        /// <param name="message">The message for the new log event</param>
        /// <param name="metaData">The meta data for the new log event</param>
        /// <param name="tags">The tags for the new log event</param>
        /// <returns>A new instance of SmtpLogEventBuilder</returns>
        public static EmailLogEventBuilder Create(string message, IDictionary<string, object> metaData, params string[] tags)
        {
            return new EmailLogEventBuilder
            {
                LogEvent = new LogEvent
                {
                    Message = message,
                    MetaData = metaData,
                    Tags = tags
                }
            };
        }

        /// <summary>
        /// Creates a new SmtpLogEventBuilder using the provided messge, exception and tags
        /// </summary>
        /// <param name="message">The message for the new log event</param>
        /// <param name="exception">The exception for the new log event</param>
        /// <param name="tags">The tags for the new log event</param>
        /// <returns>A new instance of SmtpLogEventBuilder</returns>
        public static EmailLogEventBuilder Create(string message, Exception exception, params string[] tags)
        {
            return new EmailLogEventBuilder
            {
                LogEvent = new LogEvent
                {
                    Message = message,
                    Exception = exception,
                    Tags = tags
                }
            };
        }

        /// <summary>
        /// Creates a new SmtpLogEventBuilder using the provided message, exception, meta data and tags
        /// </summary>
        /// <param name="message">The message for the new log event</param>
        /// <param name="exception">The exception for the new log event</param>
        /// <param name="metaData">The meta data for the new log event</param>
        /// <param name="tags">The tags for the new log event</param>
        /// <returns>A new instance of SmtpLogEventBuilder</returns>
        public static EmailLogEventBuilder Create(string message, Exception exception, IDictionary<string, object> metaData, params string[] tags)
        {
            return new EmailLogEventBuilder
            {
                LogEvent = new LogEvent
                {
                    Message = message,
                    Exception = exception,
                    MetaData = metaData,
                    Tags = tags
                }
            };
        }

        #endregion Members and Constructors

        #region Email Settings

        /// <summary>
        /// Sets the email subject for the new log event
        /// </summary>
        /// <param name="subject">The email subject</param>
        /// <returns>This SmtpLogEventBuilder</returns>
        public EmailLogEventBuilder SetSubject(string subject)
        {
            Subject = subject;
            return this;
        }

        /// <summary>
        /// Adds a new email recipient/"to address" for the new log event
        /// </summary>
        /// <param name="toAddress">The new "to address" to add</param>
        /// <returns>This SmtpLogEventBuilder</returns>
        public EmailLogEventBuilder AddToAddress(string toAddress)
        {
            To.Add(toAddress);
            return this;
        }

        /// <summary>
        /// Adds a new email CC address for the new log event
        /// </summary>
        /// <param name="ccAddress">The new CC address to add</param>
        /// <returns>This SmtpLogEventBuilder</returns>
        public EmailLogEventBuilder AddCCAddress(string ccAddress)
        {
            CC.Add(ccAddress);
            return this;
        }

        /// <summary>
        /// Adds a new BCC address for the new log event
        /// </summary>
        /// <param name="bccAddress">The new BCC address to add</param>
        /// <returns>This SmtpLogEventBuilder</returns>
        public EmailLogEventBuilder AddBCCAdress(string bccAddress)
        {
            BCC.Add(bccAddress);
            return this;
        }

        /// <summary>
        /// Adds the file at the given location to the log event as an email attachment
        /// </summary>
        /// <param name="fileName">The path/name of the file to add as an attachment</param>
        /// <returns>This SmtpLogEventBuilder</returns>
        public EmailLogEventBuilder AddAttachment(string fileName)
        {
            Attachments.Add(new EmailAttachment(fileName));
            return this;
        }

        /// <summary>
        /// Adds the given data with the given name to the log event as an email attachment
        /// </summary>
        /// <param name="data">The byte data which comprises the email attachment</param>
        /// <param name="name">The name representing the byte data which comprises the email attachment (file name)</param>
        /// <returns>This SmtpLogEventBuilder</returns>
        public EmailLogEventBuilder AddAttachment(byte[] data, string name)
        {
            Attachments.Add(new EmailAttachment
            {
                Data = data,
                PhysicalFileName = name
            });
            return this;
        }

        /// <summary>
        /// Adds the passed SmtpAttachment to the log event as an email attachment
        /// </summary>
        /// <param name="attachment">The attachment to add to the log event</param>
        /// <returns>This SmtpLogEventBuilder</returns>
        public EmailLogEventBuilder AddAttachment(EmailAttachment attachment)
        {
            Attachments.Add(attachment);
            return this;
        }

        /// <summary>
        /// Sets a custom header to the log event for the email
        /// </summary>
        /// <param name="name">The name of the header to set</param>
        /// <param name="value">The value of the header to set</param>
        /// <returns>This SmtpLogEventBuilder</returns>
        public EmailLogEventBuilder SetHeader(string name, string value)
        {
            if (string.IsNullOrEmpty(name) == false)
                Headers[name] = value;
            return this;
        }

        #endregion Email Settings

        #region Build

        /// <summary>
        /// Builds the log event based on the information setup in this SmtpLogEventBuilder
        /// </summary>
        /// <returns>The built LogEvent</returns>
        public LogEvent Build()
        {
            // Build out the meta data for the log event using the collected information

            if (LogEvent.MetaData == null)
                LogEvent.MetaData = new Dictionary<string, object>();

            if (string.IsNullOrEmpty(Subject) == false)
                LogEvent.MetaData[EmailTarget.MetaSubject] = Subject;

            if (Attachments.Count > 0)
                LogEvent.MetaData[EmailTarget.MetaAttachments] = Attachments;

            if (Headers.Count > 0)
                LogEvent.MetaData[EmailTarget.MetaHeaders] = Headers;

            if (To.Count > 0)
                LogEvent.MetaData[EmailTarget.MetaTo] = To;

            if (CC.Count > 0)
                LogEvent.MetaData[EmailTarget.MetaCC] = CC;

            if (BCC.Count > 0)
                LogEvent.MetaData[EmailTarget.MetaBCC] = BCC;

            return LogEvent;
        }

        #endregion Build
    }
}