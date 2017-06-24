/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using NuLog.LogEvents;
using System;
using System.Net.Mail;

namespace NuLog.Targets
{
    /// <summary>
    /// The standard target for sending log events via email.
    /// </summary>
    public class MailTarget : TargetBase, ILayoutTarget
    {
        private static readonly Type SmtpDeliveryMethodType = typeof(SmtpDeliveryMethod);

        private const string DefaultBodyLayoutFormat = "${DateTime:'{0:MM/dd/yyyy hh:mm:ss.fff}'} | ${Thread.ManagedThreadId:'{0,4}'} | ${Tags} | ${Message}${?Exception:'\r\n{0}'}\r\n";

        /// <summary>
        /// The SmtpClient to use when sending email.
        /// </summary>
        private readonly ISmtpClient smtpClient;

        /// <summary>
        /// The layout for formatting the body.
        /// </summary>
        public ILayout BodyLayout { get; set; }

        /// <summary>
        /// The layout for formatting the subject.
        /// </summary>
        public ILayout SubjectLayout { get; set; }

        /// <summary>
        /// A flag indicating whether or not the body is formatted as HTML.
        /// </summary>
        private bool bodyIsHtml;

        /// <summary>
        /// A flag indicating whether or not newlines are converted to break tags in HTML.
        /// </summary>
        private bool convertNewlineInHtml;

        /// <summary>
        /// The recipient addresses for the log events.
        /// </summary>
        private string[] to;

        /// <summary>
        /// The from address for the log events.
        /// </summary>
        private MailAddress from;

        /// <summary>
        /// Indicates whether or not the target should dispose the STMP client, when it is disposed.
        /// </summary>
        public bool DisposeSmtpClientOnDispose { get; set; }

        public MailTarget()
        {
            this.smtpClient = new SmtpClientShim();
            DisposeSmtpClientOnDispose = true;
        }

        public MailTarget(ISmtpClient smtpClient)
        {
            this.smtpClient = smtpClient;
        }

        public override void Write(LogEvent logEvent)
        {
            var body = FormatBody(logEvent);
            var subject = SubjectLayout.Format(logEvent);

            // Build the message
            var message = new MailMessage
            {
                Body = body,
                Subject = subject,
                IsBodyHtml = bodyIsHtml
            };

            // Set the "from" address, if given
            if (from != null)
            {
                message.From = from;
            }

            // Add the recipients
            foreach (var addressee in to)
            {
                message.To.Add(addressee);
            }

            // Send the message
            smtpClient.Send(message);
        }

        /// <summary>
        /// Format the body using the layout, and replace newlines if we're configured to.
        /// </summary>
        private string FormatBody(LogEvent logEvent)
        {
            var body = BodyLayout.Format(logEvent);
            if (bodyIsHtml && convertNewlineInHtml)
            {
                body = body.Replace("\r\n", "<br />");
            }
            return body;
        }

        public override void Configure(TargetConfig config)
        {
            // Parse out the HTML flag
            var htmlFlagRaw = GetProperty<string>(config, "html");
            bool htmlFlag;
            if (bool.TryParse(htmlFlagRaw, out htmlFlag))
            {
                bodyIsHtml = htmlFlag;
            }

            // Parse out the "convert newline in HTML" flag
            var convertNewlineInHtmlFlagRaw = GetProperty<string>(config, "convertNewlineInHtml");
            bool convertNewlinInHtmlFlag;
            if (bool.TryParse(convertNewlineInHtmlFlagRaw, out convertNewlinInHtmlFlag))
            {
                convertNewlineInHtml = convertNewlinInHtmlFlag;
            }

            // Parse out the recipient addresses
            var recipientsString = GetProperty<string>(config, "to");
            if (string.IsNullOrEmpty(recipientsString) == false)
            {
                to = recipientsString.Split(';');
            }

            // Parse out the from address
            var fromString = GetProperty<string>(config, "from");
            if (string.IsNullOrEmpty(fromString) == false)
            {
                from = new MailAddress(fromString);
            }

            // Parse out the SMTP user name and password
            var userNameString = GetProperty<string>(config, "smtpUserName");
            var password = GetProperty<string>(config, "smtpPassword");
            smtpClient.SetCredentials(userNameString, password);

            // Parse out the "enable SSL" flag
            var enableSslFlagRaw = GetProperty<string>(config, "enableSsl");
            bool enableSslFlag;
            if (bool.TryParse(enableSslFlagRaw, out enableSslFlag))
            {
                smtpClient.SetEnableSsl(enableSslFlag);
            }

            // Parse out the SMTP server
            var smtpServer = GetProperty<string>(config, "smtpServer");
            if (string.IsNullOrEmpty(smtpServer) == false)
            {
                smtpClient.SetSmtpServer(smtpServer);
            }

            // Parse out the SMTP port
            var smtpPortRaw = GetProperty<string>(config, "smtpPort");
            int smtpPort;
            if (int.TryParse(smtpPortRaw, out smtpPort))
            {
                smtpClient.SetSmtpPort(smtpPort);
            }

            // Parse out the SMTP delivery method
            var smtpDeliveryMethodRaw = GetProperty<string>(config, "smtpDeliveryMethod");
            if (string.IsNullOrEmpty(smtpDeliveryMethodRaw) == false)
            {
                var smtpDeliveryMethod = (SmtpDeliveryMethod)Enum.Parse(SmtpDeliveryMethodType, smtpDeliveryMethodRaw);
                smtpClient.SetSmtpDeliveryMethod(smtpDeliveryMethod);
            }

            // Parse out the pickup directory location
            var pickupDirectoryLocation = GetProperty<string>(config, "pickupDirectoryLocation");
            if (string.IsNullOrEmpty(pickupDirectoryLocation) == false)
            {
                smtpClient.SetPickupDirectoryLocation(pickupDirectoryLocation);
            }

            // Parse out the timeout
            var timeoutRaw = GetProperty<string>(config, "timeout");
            int timeout;
            if (int.TryParse(timeoutRaw, out timeout))
            {
                smtpClient.SetTimeout(timeout);
            }

            // Let the base configure itself
            base.Configure(config);
        }

        public void Configure(TargetConfig config, ILayoutFactory layoutFactory)
        {
            // Parse out the body layout
            var bodyFormat = GetProperty<string>(config, "body");
            if (string.IsNullOrEmpty(bodyFormat))
            {
                bodyFormat = DefaultBodyLayoutFormat;
            }
            this.BodyLayout = layoutFactory.GetLayout(bodyFormat);

            // Parse out the subject layout
            var subjectFormat = GetProperty<string>(config, "subject");
            if (string.IsNullOrEmpty(subjectFormat) == false)
            {
                this.SubjectLayout = layoutFactory.GetLayout(subjectFormat);
            }
            else
            {
                throw new InvalidOperationException("Subject is required when configuring the mail target; \"subject\" was not found in the target configuration.");
            }
        }

        private bool disposedValue = false; // To detect redundant calls

        protected override void Dispose(bool disposing)
        {
            // Dispose the base
            base.Dispose(disposing);

            if (!disposedValue)
            {
                if (disposing)
                {
                    // Dispose the SMTP client
                    if (DisposeSmtpClientOnDispose)
                    {
                        smtpClient.Dispose();
                    }
                }

                disposedValue = true;
            }
        }
    }
}