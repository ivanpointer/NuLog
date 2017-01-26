/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using NuLog.LogEvents;
using System.Net.Mail;

namespace NuLog.Targets
{
    /// <summary>
    /// The standard target for sending log events via email.
    /// </summary>
    public class MailTarget : TargetBase
    {
        /// <summary>
        /// The SmtpClient to use when sending email.
        /// </summary>
        public ISmtpClient SmtpClient { get; set; }

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
        public bool BodyIsHtml { get; set; }

        /// <summary>
        /// A flag indicating whether or not newlines are converted to break tags in HTML.
        /// </summary>
        public bool ConvertNewlineInHtml { get; set; }

        /// <summary>
        /// The recipient addresses for the log events.
        /// </summary>
        public MailAddressCollection To { get; set; }

        /// <summary>
        /// The from address for the log events.
        /// </summary>
        public MailAddress From { get; set; }

        /// <summary>
        /// The user name for connecting to SMTP.
        /// </summary>
        public string SmtpUserName { get; set; }

        /// <summary>
        /// Indicates whether or not the SMTP client "belongs" to this target. A SMTP client that
        /// "belongs" to this client must needs be "disposed" on disposal, or replacement.
        /// </summary>
        private bool isMySmtpClient;

        public override void Write(LogEvent logEvent)
        {
            var body = BodyLayout.Format(logEvent);
            var subject = SubjectLayout.Format(logEvent);
            SmtpClient.Send(new MailMessage
            {
                Body = body,
                Subject = subject
            });
        }

        public override void Configure(TargetConfig config)
        {
            // Parse out the html flag
            var htmlFlagRaw = GetProperty<string>(config, "html");
            bool htmlFlag;
            if (bool.TryParse(htmlFlagRaw, out htmlFlag))
            {
                BodyIsHtml = htmlFlag;
            }

            // Parse out the "convert newline in HTML" flag
            var convertNewlineInHtmlFlagRaw = GetProperty<string>(config, "convertNewlineInHtml");
            bool convertNewlinInHtmlFlag;
            if (bool.TryParse(convertNewlineInHtmlFlagRaw, out convertNewlinInHtmlFlag))
            {
                ConvertNewlineInHtml = convertNewlinInHtmlFlag;
            }

            // Parse out the recipient addresses
            var recipientsString = GetProperty<string>(config, "to");
            if (string.IsNullOrEmpty(recipientsString) == false)
            {
                var recipients = recipientsString.Split(';');
                To = new MailAddressCollection();
                foreach (var recipient in recipients)
                {
                    To.Add(recipient);
                }
            }

            // Parse out the from address
            var fromString = GetProperty<string>(config, "from");
            if (string.IsNullOrEmpty(fromString) == false)
            {
                From = new MailAddress(fromString);
            }

            // Parse out the SMTP user name
            var userNameString = GetProperty<string>(config, "smtpUserName");
            if (string.IsNullOrEmpty(userNameString) == false)
            {
                SmtpUserName = userNameString;
            }

            // Let the base configure itself
            base.Configure(config);
        }
    }
}