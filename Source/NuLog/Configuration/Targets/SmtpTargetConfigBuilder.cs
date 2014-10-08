﻿/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/5/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */
using NuLog.Configuration.Layouts;
using System.Collections.Generic;

namespace NuLog.Configuration.Targets
{
    /// <summary>
    /// Used to build a smtp target config at runtime
    /// </summary>
    public class SmtpTargetConfigBuilder
    {
        /// <summary>
        /// The config being built
        /// </summary>
        protected SmtpTargetConfig Config { get; set; }

        /// <summary>
        /// The private instantiation
        /// </summary>
        private SmtpTargetConfigBuilder()
        {
            Config = new SmtpTargetConfig();
        }

        /// <summary>
        /// Creates a new instance of this builder
        /// </summary>
        /// <returns>A new instance of this builder</returns>
        public static SmtpTargetConfigBuilder Create()
        {
            return new SmtpTargetConfigBuilder();
        }

        /// <summary>
        /// Sets the name of the target in the config
        /// </summary>
        /// <param name="name">The name to set</param>
        /// <returns>This SmtpTargetCofigBuilder</returns>
        public SmtpTargetConfigBuilder SetName(string name)
        {
            Config.Name = name;
            return this;
        }

        /// <summary>
        /// Sets the synchronous flag of the target in the config
        /// </summary>
        /// <param name="synchronous">The synchronous flag to set</param>
        /// <returns>This SmtpTargetCofigBuilder</returns>
        public SmtpTargetConfigBuilder SetSynchronous(bool synchronous)
        {
            Config.Synchronous = synchronous;
            return this;
        }

        /// <summary>
        /// Sets the host of the target in the config
        /// </summary>
        /// <param name="host">The host to set</param>
        /// <returns>This SmtpTargetCofigBuilder</returns>
        public SmtpTargetConfigBuilder SetHost(string host)
        {
            Config.Host = host;
            return this;
        }

        /// <summary>
        /// Sets the port of the target in the config
        /// </summary>
        /// <param name="port">The port to set</param>
        /// <returns>This SmtpTargetCofigBuilder</returns>
        public SmtpTargetConfigBuilder SetPort(int port)
        {
            Config.Port = port;
            return this;
        }

        /// <summary>
        /// Sets the user name of the target in the config
        /// </summary>
        /// <param name="userName">The user name to set</param>
        /// <returns>This SmtpTargetCofigBuilder</returns>
        public SmtpTargetConfigBuilder SetUserName(string userName)
        {
            Config.UserName = userName;
            return this;
        }

        /// <summary>
        /// Sets the password of the target in the config
        /// </summary>
        /// <param name="password">The password to set</param>
        /// <returns>This SmtpTargetCofigBuilder</returns>
        public SmtpTargetConfigBuilder SetPassword(string password)
        {
            Config.Password = password;
            return this;
        }

        /// <summary>
        /// Sets the ssl flag of the target in the config
        /// </summary>
        /// <param name="enableSSL">The ssl flag to set</param>
        /// <returns>This SmtpTargetCofigBuilder</returns>
        public SmtpTargetConfigBuilder SetEnableSSL(bool enableSSL)
        {
            Config.EnableSSL = enableSSL;
            return this;
        }

        /// <summary>
        /// Sets the from address of the target in the config
        /// </summary>
        /// <param name="fromAddress">The from address to set</param>
        /// <returns>This SmtpTargetCofigBuilder</returns>
        public SmtpTargetConfigBuilder SetFromAddress(string fromAddress)
        {
            Config.FromAddress = fromAddress;
            return this;
        }

        /// <summary>
        /// Adds a reply-to address to the target in the config
        /// </summary>
        /// <param name="reployTo">The reply-to address to add</param>
        /// <returns>This SmtpTargetCofigBuilder</returns>
        public SmtpTargetConfigBuilder AddReplyTo(string reployTo)
        {
            if (Config.ReplyTo == null)
                Config.ReplyTo = new List<string>();

            Config.ReplyTo.Add(reployTo);

            return this;
        }

        /// <summary>
        /// Adds a to address to the target in the config
        /// </summary>
        /// <param name="to">The to address to add</param>
        /// <returns>This SmtpTargetCofigBuilder</returns>
        public SmtpTargetConfigBuilder AddTo(string to)
        {
            if (Config.To == null)
                Config.To = new List<string>();

            Config.To.Add(to);

            return this;
        }

        /// <summary>
        /// Adds a CC address to the target in the config
        /// </summary>
        /// <param name="cc">The CC address to add</param>
        /// <returns>This SmtpTargetCofigBuilder</returns>
        public SmtpTargetConfigBuilder AddCC(string cc)
        {
            if (Config.CC == null)
                Config.CC = new List<string>();

            Config.CC.Add(cc);

            return this;
        }

        /// <summary>
        /// Adds a BCC address to the target in the config
        /// </summary>
        /// <param name="bcc">The BCC address to add</param>
        /// <returns>This SmtpTargetCofigBuilder</returns>
        public SmtpTargetConfigBuilder AddBCC(string bcc)
        {
            if (Config.BCC == null)
                Config.BCC = new List<string>();

            Config.BCC.Add(bcc);

            return this;
        }

        /// <summary>
        /// Sets the subject of the target in the config
        /// </summary>
        /// <param name="subject">The subject to set</param>
        /// <returns>This SmtpTargetCofigBuilder</returns>
        public SmtpTargetConfigBuilder SetSubject(string subject)
        {
            Config.Subject = subject;

            return this;
        }

        /// <summary>
        /// Sets the subject layout to the target in the config
        /// </summary>
        /// <param name="subjectLayout">The subject layout to set</param>
        /// <returns>This SmtpTargetCofigBuilder</returns>
        public SmtpTargetConfigBuilder SetSubjectLayout(LayoutConfig subjectLayout)
        {
            Config.SubjectLayout = subjectLayout;

            return this;
        }
        
        /// <summary>
        /// Sets the body layout to the target in the config
        /// </summary>
        /// <param name="bodyLayout">The body layout to set</param>
        /// <returns></returns>
        public SmtpTargetConfigBuilder SetBodyLayout(LayoutConfig bodyLayout)
        {
            Config.BodyLayout = bodyLayout;

            return this;
        }

        /// <summary>
        /// Sets the body file to the target in the config
        /// </summary>
        /// <param name="bodyFile">The body file to set</param>
        /// <returns>This SmtpTargetCofigBuilder</returns>
        public SmtpTargetConfigBuilder SetBodyFile(string bodyFile)
        {
            Config.BodyFile = bodyFile;

            return this;
        }

        /// <summary>
        /// Sets the "is body HTML" flag to the target in the config
        /// </summary>
        /// <param name="isBodyHtml">The "is body HTML" flag to set</param>
        /// <returns>This SmtpTargetCofigBuilder</returns>
        public SmtpTargetConfigBuilder SetIsBodyHtml(bool isBodyHtml)
        {
            Config.IsBodyHtml = isBodyHtml;

            return this;
        }

        /// <summary>
        /// Adds a header to the target in the config
        /// </summary>
        /// <param name="name">The name of the header to add</param>
        /// <param name="value">The value of the header to add</param>
        /// <returns>This SmtpTargetCofigBuilder</returns>
        public SmtpTargetConfigBuilder AddHeader(string name, string value)
        {
            if (Config.Headers == null)
                Config.Headers = new Dictionary<string, string>();

            Config.Headers[name] = value;

            return this;
        }
        
        /// <summary>
        /// Returns the built config
        /// </summary>
        /// <returns>The built config</returns>
        public SmtpTargetConfig Build()
        {
            return Config;
        }
    }
}
