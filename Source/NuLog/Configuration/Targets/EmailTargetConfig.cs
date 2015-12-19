/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/5/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using Newtonsoft.Json.Linq;
using NuLog.Configuration.Layouts;
using NuLog.Targets;
using System.Collections.Generic;
using System.Linq;

namespace NuLog.Configuration.Targets
{
    /// <summary>
    /// The configuration that represents an email target
    /// </summary>
    public class EmailTargetConfig : TargetConfig
    {
        #region Constants

        private const string DefaultName = "email";
        private static readonly string DefaultType = typeof(EmailTarget).FullName;

        private const string DefaultSubjectLayout = "${EmailSubject}";
        private const string DefaultBodyLayout = "${DateTime:'{0:MM/dd/yyyy hh:mm:ss.fff}'} | ${Thread.ManagedThreadId:'{0,4}'} | ${Tags} | ${Message}${?Exception:'\r\n{0}'}\r\n";

        // JSON tokens
        private const string HostTokenName = "host";

        private const string PortTokenName = "port";
        private const string UserNameTokenName = "userName";
        private const string PasswordTokenName = "password";
        private const string EnableSSLTokenName = "enableSSL";
        private const string FromAddressTokenName = "fromAddress";
        private const string SubjectTokenName = "subject";
        private const string ReplyToTokenName = "replyTo";
        private const string ToTokenName = "to";
        private const string CCTokenName = "cc";
        private const string BCCTokenName = "bcc";
        private const string SubjectLayoutTokenName = "subjectLayout";
        private const string BodyLayoutTokenName = "bodyLayout";
        private const string HeadersTokenName = "headers";
        private const string HeaderNameTokenName = "name";
        private const string HeaderValueTokenName = "value";
        private const string BodyFileTokenName = "bodyFile";
        private const string IsBodyHTMLTokenName = "isBodyHtml";
        private const string AttachmentTokenName = "attachment";

        // Default Values
        private const string DefaultHost = "localhost";

        private const int DefaultPort = 25;
        private const string DefaultSubject = "Message from Logging System";

        #endregion Constants

        #region Members

        // Server Information
        /// <summary>
        /// The SMTP server host address
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// The SMTP server host port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// The user name used for authenticating to the SMTP server
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The password used for authenticating to the SMTP server
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Whether or not to use SSL in communications with the SMTP server
        /// </summary>
        public bool EnableSSL { get; set; }

        // Message Information
        /// <summary>
        /// The from address to use in emails sent from the target
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        /// The reply-to address to be used in emails sent from the target
        /// </summary>
        public ICollection<string> ReplyTo { get; set; }

        /// <summary>
        /// The list of recipients to send the email messages from the target to
        /// </summary>
        public ICollection<string> To { get; set; }

        /// <summary>
        /// The list of recipients to be copied on the email messages sent from the target
        /// </summary>
        public ICollection<string> CC { get; set; }

        /// <summary>
        /// The list of recipients to be blind-copied on the email messages sent from the target
        /// </summary>
        public ICollection<string> BCC { get; set; }

        /// <summary>
        /// The default subject for email messages sent from the target
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The layout of the subject for email messages sent from the target
        /// </summary>
        public LayoutConfig SubjectLayout { get; set; }

        /// <summary>
        /// The layout of the body for email messages sent from the target
        /// </summary>
        public LayoutConfig BodyLayout { get; set; }

        /// <summary>
        /// An optional file which contains the body layout for email messages sent from the target
        /// </summary>
        public string BodyFile { get; set; }

        /// <summary>
        /// A flag indicating whether or not the body is HTML content (as opposed to plain-text content)
        /// </summary>
        public bool IsBodyHtml { get; set; }

        /// <summary>
        /// Any additional headers to be set on email messages sent from the target
        /// </summary>
        public IDictionary<string, string> Headers { get; set; }

        /// <summary>
        /// The file name/path to a file to attach to the email.
        /// </summary>
        public string Attachment { get; set; }

        #endregion Members

        /// <summary>
        /// Builds a SMTP target config using the passed JSON token
        /// </summary>
        /// <param name="jToken">The JSON token to be used to build the SMTP target config</param>
        public EmailTargetConfig(JToken jToken = null)
        {
            // Set the default values
            Defaults();

            // Load the configuration from the JSON token
            if (jToken != null)
            {
                Host = GetValue<string>(jToken, HostTokenName, Host);
                Port = GetValue<int>(jToken, PortTokenName, Port);
                UserName = GetValue<string>(jToken, UserNameTokenName, UserName);
                Password = GetValue<string>(jToken, PasswordTokenName, Password);
                EnableSSL = GetValue<bool>(jToken, EnableSSLTokenName, EnableSSL);
                FromAddress = GetValue<string>(jToken, FromAddressTokenName, FromAddress);
                Subject = GetValue<string>(jToken, SubjectTokenName, Subject);

                var replyToToken = jToken[ReplyToTokenName];
                if (replyToToken != null && replyToToken.Type == JTokenType.Array)
                    ReplyTo = replyToToken.Values<string>().ToList();

                var toToken = jToken[ToTokenName];
                if (toToken != null && toToken.Type == JTokenType.Array)
                    To = toToken.Values<string>().ToList();

                var ccToken = jToken[CCTokenName];
                if (ccToken != null && toToken.Type == JTokenType.Array)
                    CC = ccToken.Values<string>().ToList();

                var bccToken = jToken[BCCTokenName];
                if (bccToken != null && bccToken.Type == JTokenType.Array)
                    BCC = bccToken.Values<string>().ToList();

                Subject = GetValue<string>(jToken, SubjectTokenName, Subject);

                var subjectLayoutToken = jToken[SubjectLayoutTokenName];
                SubjectLayout = subjectLayoutToken != null
                    ? new LayoutConfig(subjectLayoutToken)
                    : new LayoutConfig(DefaultSubjectLayout);

                var bodyToken = jToken[BodyLayoutTokenName];
                BodyLayout = bodyToken != null
                    ? new LayoutConfig(bodyToken)
                    : new LayoutConfig(DefaultBodyLayout);

                // Load the additional headers from the JSON array
                var headersToken = jToken[HeadersTokenName];
                if (headersToken != null && headersToken.Type == JTokenType.Object)
                    Headers = headersToken.ToObject<Dictionary<string, string>>();

                BodyFile = GetValue<string>(jToken, BodyFileTokenName, BodyFile);
                IsBodyHtml = GetValue<bool>(jToken, IsBodyHTMLTokenName, IsBodyHtml);

                Attachment = GetValue<string>(jToken, AttachmentTokenName, Attachment);
            }
        }

        /// <summary>
        /// Sets the defaults for this configuration
        /// </summary>
        private void Defaults()
        {
            Name = DefaultName;
            Type = DefaultType;

            Host = DefaultHost;
            Port = DefaultPort;
            UserName = null;
            Password = null;
            EnableSSL = false;
            FromAddress = null;
            ReplyTo = null;
            To = null;
            CC = null;
            BCC = null;
            Subject = DefaultSubject;
            SubjectLayout = null;
            BodyLayout = null;
            BodyFile = null;
            IsBodyHtml = false;
        }

        #region Helpers

        /// <summary>
        /// Gets a value from a JSON token, with a specified default value if the value is not in the JSON config
        /// </summary>
        /// <typeparam name="T">The type of the value being retrieved</typeparam>
        /// <param name="jToken">The JSON token from which to retrieve the value</param>
        /// <param name="name">The name of the child token being retrieved</param>
        /// <param name="defVal">The default value to return if the vaue is not found in the JSON token</param>
        /// <returns>The value from the JSON token, or the default if not found</returns>
        private T GetValue<T>(JToken jToken, string name, T defVal)
        {
            var child = jToken[name];
            if (child != null)
                return child.Value<T>();
            return defVal;
        }

        #endregion Helpers
    }
}