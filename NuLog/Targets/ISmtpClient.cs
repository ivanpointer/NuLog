/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;
using System.Net.Mail;

namespace NuLog.Targets {

    /// <summary>
    /// A "shim" interface for testing the mail target in isolation.
    /// </summary>
    public interface ISmtpClient : IDisposable {

        /// <summary>
        /// Sets the credentials to use in connecting to the SMTP server.
        /// </summary>
        void SetCredentials(string userName, string password);

        /// <summary>
        /// Sets the "enable SSL" flag.
        /// </summary>
        void SetEnableSsl(bool enableSsl);

        /// <summary>
        /// Sets the SMTP server.
        /// </summary>
        void SetSmtpServer(string smtpServer);

        /// <summary>
        /// Sets the SMTP port.
        /// </summary>
        void SetSmtpPort(int port);

        /// <summary>
        /// Sets the SMTP delivery method.
        /// </summary>
        void SetSmtpDeliveryMethod(SmtpDeliveryMethod smtpDeliveryMethod);

        /// <summary>
        /// Sets the pickup directory location.
        /// </summary>
        void SetPickupDirectoryLocation(string pickupDirectoryLocation);

        /// <summary>
        /// Sets the timeout for connecting to the SMTP server.
        /// </summary>
        void SetTimeout(int timeout);

        /// <summary>
        /// Send the given mail message.
        /// </summary>
        void Send(MailMessage mailMessage);
    }
}