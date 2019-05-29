/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;
using System.Net;
using System.Net.Mail;

namespace NuLog.Targets {

    /// <summary>
    /// A shim to the real SmtpClient.
    /// </summary>
    public class SmtpClientShim : ISmtpClient {

        /// <summary>
        /// The SMTP client this shim wraps.
        /// </summary>
        private readonly SmtpClient smtpClient;

        public SmtpClientShim() {
            this.smtpClient = new SmtpClient();
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
#if !PRENET4
                    this.smtpClient.Dispose();
#endif
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);

            // Tell the GC that we've got it
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support

        public void Send(MailMessage mailMessage) {
            this.smtpClient.Send(mailMessage);
        }

        public void SetCredentials(string userName, string password) {
            this.smtpClient.Credentials = new NetworkCredential(userName, password);
        }

        public void SetEnableSsl(bool enableSsl) {
            this.smtpClient.EnableSsl = enableSsl;
        }

        public void SetPickupDirectoryLocation(string pickupDirectoryLocation) {
            this.smtpClient.PickupDirectoryLocation = pickupDirectoryLocation;
        }

        public void SetSmtpDeliveryMethod(SmtpDeliveryMethod smtpDeliveryMethod) {
            this.smtpClient.DeliveryMethod = smtpDeliveryMethod;
        }

        public void SetSmtpPort(int port) {
            this.smtpClient.Port = port;
        }

        public void SetSmtpServer(string smtpServer) {
            this.smtpClient.Host = smtpServer;
        }

        public void SetTimeout(int timeout) {
            this.smtpClient.Timeout = timeout;
        }
    }
}