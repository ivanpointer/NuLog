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
            target.Write(new LogEvent
            {
                Message = "Hello, MailTarget!"
            });

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

            var logEvent = new LogEvent
            {
                Message = "Hello, MailTarget!"
            };

            A.CallTo(() => target.BodyLayout.Format(A<LogEvent>.Ignored)).Returns("Hello, BodyLayout!");

            // Execute
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

            var logEvent = new LogEvent
            {
                Message = "Hello, MailTarget!"
            };

            A.CallTo(() => target.SubjectLayout.Format(A<LogEvent>.Ignored)).Returns("Hello, SubjectLayout!");

            // Execute
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
            var properties = new Dictionary<string, object>
            {
                { "html", "true" }
            };
            var config = new TargetConfig
            {
                Properties = properties
            };
            var target = GetMailTarget();

            // Execute
            target.Configure(config);

            // Verify
            Assert.True(target.BodyIsHtml);
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
            target.Configure(new TargetConfig());

            // Verify
            Assert.False(target.BodyIsHtml);
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
                { "convertNewlineInHtml", "true" }
            };
            var config = new TargetConfig
            {
                Properties = properties
            };
            var target = GetMailTarget();

            // Execute
            target.Configure(config);

            // Verify
            Assert.True(target.ConvertNewlineInHtml);
        }

        /// <summary>
        /// The target should default the "convert newline in HTML" flag to false.
        /// </summary>
        [Fact(DisplayName = "Should_DefaultConvertNewlineInHtmlFlag")]
        public void Should_DefaultConvertNewlineInHtmlFlag()
        {
            // Setup
            var target = GetMailTarget();

            // Execute
            target.Configure(new TargetConfig());

            // Verify
            Assert.False(target.ConvertNewlineInHtml);
        }

        /// <summary>
        /// The target should parse a "to" address from the config.
        /// </summary>
        [Fact(DisplayName = "Should_ParseToAddress")]
        public void Should_ParseToAddress()
        {
            // Setup
            var properties = new Dictionary<string, object>
            {
                { "to", "someone@somewhere.net" }
            };
            var config = new TargetConfig
            {
                Properties = properties
            };
            var target = GetMailTarget();

            // Execute
            target.Configure(config);

            // Verify
            Assert.Equal(1, target.To.Count);
            Assert.True(target.To.Contains(new MailAddress("someone@somewhere.net")));
        }

        /// <summary>
        /// The target should parse a "to" address from the config.
        /// </summary>
        [Fact(DisplayName = "Should_ParseMultipleToAddresses")]
        public void Should_ParseMultipleToAddresses()
        {
            // Setup
            var properties = new Dictionary<string, object>
            {
                { "to", "someone@somewhere.net;another@somewhere.net" }
            };
            var config = new TargetConfig
            {
                Properties = properties
            };
            var target = GetMailTarget();

            // Execute
            target.Configure(config);

            // Verify
            Assert.Equal(2, target.To.Count);
            Assert.True(target.To.Contains(new MailAddress("someone@somewhere.net")));
            Assert.True(target.To.Contains(new MailAddress("another@somewhere.net")));
        }

        /// <summary>
        /// The target should parse a "from" address from the config.
        /// </summary>
        [Fact(DisplayName = "Should_ParseFromAddress")]
        public void Should_ParseFromAddress()
        {
            // Setup
            var properties = new Dictionary<string, object>
            {
                { "from", "someone@somewhere.net" }
            };
            var config = new TargetConfig
            {
                Properties = properties
            };
            var target = GetMailTarget();

            // Execute
            target.Configure(config);

            // Verify
            Assert.Equal(new MailAddress("someone@somewhere.net"), target.From);
        }

        /// <summary>
        /// If the from address isn't specified, it should remain null.
        /// </summary>
        [Fact(DisplayName = "Should_DefaultFromAddress")]
        public void Should_DefaultFromAddress()
        {
            // Setup
            var config = new TargetConfig();
            var target = GetMailTarget();

            // Execute
            target.Configure(config);

            // Verify
            Assert.Null(target.From);
        }

        /// <summary>
        /// Should parse out the SMTP user name
        /// </summary>
        [Fact(DisplayName = "Should_ParseSmtpUserName")]
        public void Should_ParseSmtpUserName()
        {
            // Setup
            var properties = new Dictionary<string, object>
            {
                { "smtpUserName", "alincoln" }
            };
            var config = new TargetConfig
            {
                Properties = properties
            };
            var target = GetMailTarget();

            // Execute
            target.Configure(config);

            // Verify
            Assert.Equal("alincoln", target.SmtpUserName);
        }

        /// <summary>
        /// Should default the user name to null when not configured
        /// </summary>
        [Fact(DisplayName = "Should_DefaultSmtpUserName")]
        public void Should_DefaultSmtpUserName()
        {
            // Setup
            var config = new TargetConfig();
            var target = GetMailTarget();

            // Execute
            target.Configure(config);

            // Verify
            Assert.Null(target.SmtpUserName);
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
}

/*
 <!--<target xsi:type="Mail"
		name="String"
	- header="Layout"
	- footer="Layout"
		x layout="Layout"
		x html="Boolean"
	- addNewLines="Boolean"
		x replaceNewlineWithBrTagInHtml="Boolean"
	- encoding="Encoding"
		x subject="Layout"
		x to="Layout"
	- bcc="Layout"
	- cc="Layout"
		x from="Layout"
		x body="Layout"

		x smtpUserName="Layout"
		enableSsl="Boolean"
		smtpPassword="Layout"
		smtpAuthentication="Enum"
		smtpServer="Layout"
		smtpPort="Integer"
		useSystemNetMailSettings="Boolean"

        deliveryMethod="Enum"
		pickupDirectoryLocation="String"

        timeout="Integer" />-->
     */