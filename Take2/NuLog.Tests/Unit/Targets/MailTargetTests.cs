/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using FakeItEasy;
using NuLog.Configuration;
using NuLog.LogEvents;
using NuLog.Targets;
using System.Collections.Generic;
using System.Net.Mail;
using Xunit;

namespace NuLog.Tests.Unit.Targets
{
    /// <summary>
    /// Documents (and verifies) the expected behavior of the mail target.
    /// </summary>
    [Trait("Category", "Unit")]
    public class MailTargetTests
    {
        /// <summary>
        /// The target should send an email for a logging event.
        /// </summary>
        [Fact(DisplayName = "Should_SendEmail")]
        public void Should_SendEmail()
        {
            // Setup
            var target = GetMailTarget();

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.org")
                .Build());
            target.Write(new LogEvent { Message = "Hello, MailTarget!" });

            // Verify
            A.CallTo(() => target.SmtpClient.Send(A<MailMessage>.Ignored)).MustHaveHappened();
        }

        /// <summary>
        /// The mail target should use its layout for formatting the body.
        /// </summary>
        [Fact(DisplayName = "Should_UseLayoutForBody")]
        public void Should_UseLayoutForBody()
        {
            // Setup
            var target = GetMailTarget();
            var logEvent = new LogEvent { Message = "Hello, MailTarget!" };
            A.CallTo(() => target.BodyLayout.Format(A<LogEvent>.Ignored)).Returns("Hello, BodyLayout!");

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.org")
                .Build());
            target.Write(logEvent);

            // Verify
            A.CallTo(() => target.BodyLayout.Format(logEvent)).MustHaveHappened();
            A.CallTo(() => target.SmtpClient.Send(A<MailMessage>.That.Matches(m => m != null && m.Body == "Hello, BodyLayout!"))).MustHaveHappened();
        }

        /// <summary>
        /// The mail target should use its layout for formatting the subject.
        /// </summary>
        [Fact(DisplayName = "Should_UseLayoutForSubject")]
        public void Should_UseLayoutForSubject()
        {
            // Setup
            var target = GetMailTarget();
            var logEvent = new LogEvent { Message = "Hello, MailTarget!" };
            A.CallTo(() => target.SubjectLayout.Format(A<LogEvent>.Ignored)).Returns("Hello, SubjectLayout!");

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.org")
                .Build());
            target.Write(logEvent);

            // Verify
            A.CallTo(() => target.SubjectLayout.Format(logEvent)).MustHaveHappened();
            A.CallTo(() => target.SmtpClient.Send(A<MailMessage>.That.Matches(m => m != null && m.Subject == "Hello, SubjectLayout!"))).MustHaveHappened();
        }

        /// <summary>
        /// The target should load the HTML flag from the config
        /// </summary>
        [Fact(DisplayName = "Should_LoadHtmlFlag")]
        public void Should_LoadHtmlFlag()
        {
            // Setup
            var target = GetMailTarget();

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("html", "true")
                .Add("to", "someone@somewhere.org")
                .Build());
            target.Write(new LogEvent());

            // Verify
            A.CallTo(() => target.SmtpClient.Send(A<MailMessage>.That.Matches(m => m.IsBodyHtml == true))).MustHaveHappened();
        }

        /// <summary>
        /// The target should default the HTML flag to false.
        /// </summary>
        [Fact(DisplayName = "Should_DefaultHtmlFlag")]
        public void Should_DefaultHtmlFlag()
        {
            // Setup
            var target = GetMailTarget();

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.org")
                .Build());
            target.Write(new LogEvent());

            // Verify
            A.CallTo(() => target.SmtpClient.Send(A<MailMessage>.That.Matches(m => m.IsBodyHtml == false))).MustHaveHappened();
        }

        /// <summary>
        /// The target should load the "convertNewlineInHtml" flag from the config.
        /// </summary>
        [Fact(DisplayName = "Should_LoadConvertNewlineInHtmlFlag")]
        public void Should_LoadConvertNewlineInHtmlFlag()
        {
            // Setup
            var properties = new Dictionary<string, object>
            {
                { "convertNewlineInHtml", "true" },
                { "html", "true" },
                { "to", "someone@somewhere.net" }
            };
            var config = new TargetConfig
            {
                Properties = properties
            };
            var target = GetMailTarget();

            A.CallTo(() => target.BodyLayout.Format(A<LogEvent>.Ignored)).Returns("Hello\r\nworld!");

            // Execute
            target.Configure(config);
            target.Write(new LogEvent());

            // Verify
            A.CallTo(() => target.SmtpClient.Send(A<MailMessage>.That.Matches(m => m.Body == @"Hello<br />world!"))).MustHaveHappened();
        }

        /// <summary>
        /// The target should default the "convert newline in HTML" flag to false.
        /// </summary>
        [Fact(DisplayName = "Should_DefaultConvertNewlineInHtmlFlag")]
        public void Should_DefaultConvertNewlineInHtmlFlag()
        {
            // Setup
            var target = GetMailTarget();

            A.CallTo(() => target.BodyLayout.Format(A<LogEvent>.Ignored)).Returns("Hello\r\nworld!");

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.org")
                .Build());
            target.Write(new LogEvent());

            // Verify
            A.CallTo(() => target.SmtpClient.Send(A<MailMessage>.That.Matches(m => m.Body == "Hello\r\nworld!"))).MustHaveHappened();
        }

        /// <summary>
        /// The target should only convert newlines if the body is marked as HTML.
        /// </summary>
        [Fact(DisplayName = "ShouldNotConvertNewlineWhenNotHtml")]
        public void ShouldNotConvertNewlineWhenNotHtml()
        {
            // Setup
            var target = GetMailTarget();

            A.CallTo(() => target.BodyLayout.Format(A<LogEvent>.Ignored)).Returns("Hello\r\nworld!");

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.org")
                .Add("convertNewlineInHtml", "true")
                .Build());
            target.Write(new LogEvent());

            // Verify
            A.CallTo(() => target.SmtpClient.Send(A<MailMessage>.That.Matches(m => m.Body == "Hello\r\nworld!"))).MustHaveHappened();
        }

        /// <summary>
        /// The target should parse a "to" address from the config.
        /// </summary>
        [Fact(DisplayName = "Should_ParseToAddress")]
        public void Should_ParseToAddress()
        {
            // Setup
            var target = GetMailTarget();

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.net")
                .Build());
            target.Write(new LogEvent());

            // Verify
            A.CallTo(() => target.SmtpClient.Send(A<MailMessage>.That.Matches(m => m.To.Count == 1 && m.To.Contains(new MailAddress("someone@somewhere.net"))))).MustHaveHappened();
        }

        /// <summary>
        /// The target should parse a "to" address from the config.
        /// </summary>
        [Fact(DisplayName = "Should_ParseMultipleToAddresses")]
        public void Should_ParseMultipleToAddresses()
        {
            // Setup
            var target = GetMailTarget();

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.net;another@somewhere.net")
                .Build());
            target.Write(new LogEvent());

            // Verify
            A.CallTo(() => target.SmtpClient.Send(A<MailMessage>.That.Matches(
                m => m.To.Count == 2
                && m.To.Contains(new MailAddress("someone@somewhere.net"))
                && m.To.Contains(new MailAddress("another@somewhere.net"))
            ))).MustHaveHappened();
        }

        /// <summary>
        /// The target should parse a "from" address from the config.
        /// </summary>
        [Fact(DisplayName = "Should_ParseFromAddress")]
        public void Should_ParseFromAddress()
        {
            // Setup
            var target = GetMailTarget();

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.net")
                .Add("from", "someone@somewhere.net")
                .Build());
            target.Write(new LogEvent());

            // Verify
            A.CallTo(() => target.SmtpClient.Send(A<MailMessage>.That.Matches(m => m.From.Address == "someone@somewhere.net")))
                .MustHaveHappened();
        }

        /// <summary>
        /// If the from address isn't specified, it should remain null.
        /// </summary>
        [Fact(DisplayName = "Should_DefaultFromAddress")]
        public void Should_DefaultFromAddress()
        {
            // Setup
            var target = GetMailTarget();

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.net")
                .Build());
            target.Write(new LogEvent());

            // Verify
            A.CallTo(() => target.SmtpClient.Send(A<MailMessage>.That.Matches(m => m.From == null))).MustHaveHappened();
        }

        /// <summary>
        /// Should parse out the SMTP user name
        /// </summary>
        [Fact(DisplayName = "Should_ParseSmtpUserName")]
        public void Should_ParseSmtpUserName()
        {
            // Setup
            var target = GetMailTarget();

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.net")
                .Add("smtpUserName", "alincoln")
                .Build());

            // Verify
            A.CallTo(() => target.SmtpClient.SetCredentials(A<string>.That.Matches(s => s == "alincoln"), A<string>.Ignored)).MustHaveHappened();
        }

        /// <summary>
        /// Should parse out the SMTP password.
        /// </summary>
        [Fact(DisplayName = "Should_ParseSmtpPassword")]
        public void Should_ParseSmtpPassword()
        {
            // Setup
            var target = GetMailTarget();

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.net")
                .Add("smtpPassword", "apassword")
                .Build());

            // Verify
            A.CallTo(() => target.SmtpClient.SetCredentials(A<string>.Ignored, A<string>.That.Matches(s => s == "apassword"))).MustHaveHappened();
        }

        /// <summary>
        /// The target should parse the enable SSL flag.
        /// </summary>
        [Fact(DisplayName = "Should_ParseEnableSslFlag")]
        public void Should_ParseEnableSslFlag()
        {
            // Setup
            var target = GetMailTarget();

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.net")
                .Add("enableSsl", "true")
                .Build());

            // Verify
            A.CallTo(() => target.SmtpClient.SetEnableSsl(true)).MustHaveHappened();
        }

        /// <summary>
        /// If the "enable SSL" flag isn't set, no call should be made
        /// </summary>
        [Fact(DisplayName = "Should_IgnoreEnableSslFlagIfNotSet")]
        public void Should_IgnoreEnableSslFlagIfNotSet()
        {
            // Setup
            var target = GetMailTarget();

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.net")
                .Build());

            // Verify
            A.CallTo(() => target.SmtpClient.SetEnableSsl(A<bool>.Ignored)).MustNotHaveHappened();
        }

        /// <summary>
        /// The user name and password should be passed in a single call to "SetCredentials"
        /// </summary>
        [Fact(DisplayName = "Should_ParseUserNameAndPasswordTogether")]
        public void Should_ParseUserNameAndPasswordTogether()
        {
            // Setup
            var target = GetMailTarget();

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.net")
                .Add("smtpPassword", "apassword")
                .Add("smtpUserName", "alincoln")
                .Build());

            // Verify
            A.CallTo(() => target.SmtpClient.SetCredentials(A<string>.That.Matches(s => s == "alincoln"), A<string>.That.Matches(s => s == "apassword"))).MustHaveHappened();
        }

        /// <summary>
        /// Should parse the SMTP server setting.
        /// </summary>
        [Fact(DisplayName = "Should_ParseSmtpServer")]
        public void Should_ParseSmtpServer()
        {
            // Setup
            var target = GetMailTarget();

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.net")
                .Add("smtpServer", "my.smtp.me.com")
                .Build());

            // Verify
            A.CallTo(() => target.SmtpClient.SetSmtpServer("my.smtp.me.com")).MustHaveHappened();
        }

        /// <summary>
        /// The target should ignore the SMTP server setting, if not given.
        /// </summary>
        [Fact(DisplayName = "Should_IgnoreSmtpServerIfNotSet")]
        public void Should_IgnoreSmtpServerIfNotSet()
        {
            // Setup
            var target = GetMailTarget();

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.net")
                .Build());

            // Verify
            A.CallTo(() => target.SmtpClient.SetSmtpServer(A<string>.Ignored)).MustNotHaveHappened();
        }

        /// <summary>
        /// The target should set the SMTP port, if configured.
        /// </summary>
        [Fact(DisplayName = "Should_SetSmtpPort")]
        public void Should_SetSmtpPort()
        {
            // Setup
            var target = GetMailTarget();

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.net")
                .Add("smtpPort", "4242")
                .Build());

            // Verify
            A.CallTo(() => target.SmtpClient.SetSmtpPort(4242)).MustHaveHappened();
        }

        /// <summary>
        /// The target should not set the SMTP port, if not configured.
        /// </summary>
        [Fact(DisplayName = "Should_IgnoreSmtpPort")]
        public void Should_IgnoreSmtpPort()
        {
            // Setup
            var target = GetMailTarget();

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.net")
                .Build());

            // Verify
            A.CallTo(() => target.SmtpClient.SetSmtpPort(A<int>.Ignored)).MustNotHaveHappened();
        }

        /// <summary>
        /// The mail target should set the SMTP delivery method, if configured.
        /// </summary>
        [Theory(DisplayName = "Should_SetSmtpDeliveryMethod")]
        [InlineData("SpecifiedPickupDirectory", SmtpDeliveryMethod.SpecifiedPickupDirectory)]
        [InlineData("PickupDirectoryFromIis", SmtpDeliveryMethod.PickupDirectoryFromIis)]
        [InlineData("Network", SmtpDeliveryMethod.Network)]
        public void Should_SetSmtpDeliveryMethod(string settingValue, SmtpDeliveryMethod expectedCall)
        {
            // Setup
            var target = GetMailTarget();

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.net")
                .Add("smtpDeliveryMethod", settingValue)
                .Build());

            // Verify
            A.CallTo(() => target.SmtpClient.SetSmtpDeliveryMethod(expectedCall)).MustHaveHappened();
        }

        /// <summary>
        /// If the SMTP delivery method is not configured, it shouldn't be set
        /// </summary>
        [Fact(DisplayName = "Should_IgnoreSmtpDeliveryMethodIfNotSet")]
        public void Should_IgnoreSmtpDeliveryMethodIfNotSet()
        {
            // Setup
            var target = GetMailTarget();

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.net")
                .Build());

            // Verify
            A.CallTo(() => target.SmtpClient.SetSmtpDeliveryMethod(A<SmtpDeliveryMethod>.Ignored)).MustNotHaveHappened();
        }

        /// <summary>
        /// The pickup directory location should be pulled out of the config.
        /// </summary>
        [Fact(DisplayName = "Should_SetPickupDirectoryLocation")]
        public void Should_SetPickupDirectoryLocation()
        {
            // Setup
            var target = GetMailTarget();

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.net")
                .Add("pickupDirectoryLocation", "SomeDir")
                .Build());

            // Verify
            A.CallTo(() => target.SmtpClient.SetPickupDirectoryLocation("SomeDir")).MustHaveHappened();
        }

        /// <summary>
        /// The pickup directory should be ignored, if not set in the config.
        /// </summary>
        [Fact(DisplayName = "Should_SetPickupDirectoryLocation")]
        public void Should_IgnorePickupDirectoryLocation()
        {
            // Setup
            var target = GetMailTarget();

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.net")
                .Build());

            // Verify
            A.CallTo(() => target.SmtpClient.SetPickupDirectoryLocation(A<string>.Ignored)).MustNotHaveHappened();
        }

        /// <summary>
        /// The target should set the SMTP timeout from the config.
        /// </summary>
        [Fact(DisplayName = "Should_SetTiemout")]
        public void Should_SetTiemout()
        {
            // Setup
            var target = GetMailTarget();

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.net")
                .Add("timeout", "8675309")
                .Build());

            // Verify
            A.CallTo(() => target.SmtpClient.SetTimeout(8675309)).MustHaveHappened();
        }

        /// <summary>
        /// The target shouldn't set the timeout, when not in the config.
        /// </summary>
        [Fact(DisplayName = "Should_IgnoreTimeoutWhenNotSet")]
        public void Should_IgnoreTimeoutWhenNotSet()
        {
            // Setup
            var target = GetMailTarget();

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.net")
                .Build());

            // Verify
            A.CallTo(() => target.SmtpClient.SetTimeout(A<int>.Ignored)).MustNotHaveHappened();
        }

        /// <summary>
        /// The target should create a new SMTP client when "Config" is called, if one isn't set yet.
        /// </summary>
        [Fact(DisplayName = "Should_CreateNewSmtpClientOnConfig")]
        public void Should_CreateNewSmtpClientOnConfig()
        {
            // Setup
            var target = new MailTarget();

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.net")
                .Build());

            // Verify
            Assert.NotNull(target.SmtpClient);
        }

        /// <summary>
        /// The target should set the "Dispose" flag, when the SmtpClient is created for the target,
        /// by the config.
        /// </summary>
        [Fact(DisplayName = "Should_SetDisposeFlagOnCreateClient")]
        public void Should_SetDisposeFlagOnCreateClient()
        {
            // Setup
            var target = new MailTarget();

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.net")
                .Build());

            // Verify
            Assert.True(target.DisposeSmtpClientOnDispose);
        }

        /// <summary>
        /// When the SMTP client is set externally, the "dispose" flag should be left "false".
        /// </summary>
        [Fact(DisplayName = "Should_DefaultDisposeFlagWhenNotCreatedOnConfig")]
        public void Should_DefaultDisposeFlagWhenNotCreatedOnConfig()
        {
            // Setup
            var target = GetMailTarget();

            // Execute
            target.Configure(TargetConfigBuilder.Start()
                .Add("to", "someone@somewhere.net")
                .Build());

            // Verify
            Assert.False(target.DisposeSmtpClientOnDispose);
        }

        /// <summary>
        /// The SMTP client should be disposed, if the dispose flag is set.
        /// </summary>
        [Fact(DisplayName = "Should_DisposeSmtpClientOnDispose")]
        public void Should_DisposeSmtpClientOnDispose()
        {
            // Setup
            var target = GetMailTarget();
            target.DisposeSmtpClientOnDispose = true;

            // Execute
            target.Dispose();

            // Verify
            A.CallTo(() => target.SmtpClient.Dispose()).MustHaveHappened();
        }

        /// <summary>
        /// When the dispose flag is false, the SMTP client shouldn't be disposed when the target is disposed.
        /// </summary>
        [Fact(DisplayName = "Should_IgnoreDisposeWhenDisposeFlagFalse")]
        public void Should_IgnoreDisposeWhenDisposeFlagFalse()
        {
            // Setup
            var target = GetMailTarget();
            target.DisposeSmtpClientOnDispose = false;

            // Execute
            target.Dispose();

            // Verify
            A.CallTo(() => target.SmtpClient.Dispose()).MustNotHaveHappened();
        }

        protected MailTarget GetMailTarget()
        {
            var target = new MailTarget();
            target.SmtpClient = A.Fake<ISmtpClient>();
            target.BodyLayout = A.Fake<ILayout>();
            target.SubjectLayout = A.Fake<ILayout>();
            return target;
        }
    }

    internal class TargetConfigBuilder
    {
        private readonly TargetConfig targetConfig;

        private TargetConfigBuilder()
        {
            targetConfig = new TargetConfig
            {
                Properties = new Dictionary<string, object>()
            };
        }

        public static TargetConfigBuilder Start()
        {
            return new TargetConfigBuilder();
        }

        public TargetConfigBuilder Add(string key, string value)
        {
            targetConfig.Properties.Add(key, value);
            return this;
        }

        public TargetConfig Build()
        {
            return this.targetConfig;
        }
    }
}