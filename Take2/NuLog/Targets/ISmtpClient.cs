/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;
using System.Net.Mail;

namespace NuLog.Targets
{
    /// <summary>
    /// A "shim" interface for testing the mail target in isolation.
    /// </summary>
    public interface ISmtpClient : IDisposable
    {
        /// <summary>
        /// Send the given mail message.
        /// </summary>
        void Send(MailMessage mailMessage);
    }
}