/* © 2019 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using FakeItEasy;
using NuLog.Configuration;
using NuLog.LogEvents;
using NuLog.Targets;
using System;
using System.Collections.Generic;
using Xunit;

namespace NuLog.Tests.Integration.Targets {

    /// <summary>
    /// Documents (and verifies) the expected behavior of the mail target.
    /// </summary>
    [Trait("Category", "Unit")]
    public class MailTargetTests {

        /// <summary>
        /// The mail target should use a default body layout.
        /// </summary>
        [Fact(DisplayName = "Should_DefaultBodyLayout")]
        public void Should_DefaultBodyLayout() {
            // Setup
            var properties = new Dictionary<string, object>
            {
                { "subject", "Hello, Subject!" }
            };
            var config = new TargetConfig {
                Properties = properties
            };
            ISmtpClient smtpClient;
            var target = GetMailTarget(out smtpClient);

            var layout = A.Fake<ILayout>();
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Hello, Layout!");
            var layoutFactory = A.Fake<ILayoutFactory>();
            A.CallTo(() => layoutFactory.MakeLayout(A<string>.Ignored))
                .Returns(layout);

            // Execute
            target.Configure(config, layoutFactory);

            // Verify
            A.CallTo(() => layoutFactory.MakeLayout(A<string>.That.Matches(m => string.IsNullOrEmpty(m) == false && m != "Hello, Subject!"))).MustHaveHappened();
        }

        /// <summary>
        /// The mail target should load the body layout from the config.
        /// </summary>
        [Fact(DisplayName = "Should_LoadBodyLayoutFromConfig")]
        public void Should_LoadBodyLayoutFromConfig() {
            // Setup
            var properties = new Dictionary<string, object>
            {
                { "body", "${Message}" },
                { "subject", "Hello, Subject!" }
            };
            var config = new TargetConfig {
                Properties = properties
            };
            ISmtpClient smtpClient;
            var target = GetMailTarget(out smtpClient);

            var layout = A.Fake<ILayout>();
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Hello, Body Layout!");
            var layoutFactory = A.Fake<ILayoutFactory>();
            A.CallTo(() => layoutFactory.MakeLayout(A<string>.Ignored))
                .Returns(layout);

            // Execute
            target.Configure(config, layoutFactory);

            // Verify
            A.CallTo(() => layoutFactory.MakeLayout(A<string>.That.Matches(m => m == "${Message}"))).MustHaveHappened();
        }

        /// <summary>
        /// The mail target should load the subject layout from the config.
        /// </summary>
        [Fact(DisplayName = "Should_LoadSubjectLayoutFromConfig")]
        public void Should_LoadSubjectLayoutFromConfig() {
            // Setup
            var properties = new Dictionary<string, object>
            {
                { "subject", "Hello, Subject!" }
            };
            var config = new TargetConfig {
                Properties = properties
            };
            ISmtpClient smtpClient;
            var target = GetMailTarget(out smtpClient);

            var layout = A.Fake<ILayout>();
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Hello, Subject Layout!");
            var layoutFactory = A.Fake<ILayoutFactory>();
            A.CallTo(() => layoutFactory.MakeLayout(A<string>.Ignored))
                .Returns(layout);

            // Execute
            target.Configure(config, layoutFactory);

            // Verify
            A.CallTo(() => layoutFactory.MakeLayout(A<string>.That.Matches(m => m == "Hello, Subject!"))).MustHaveHappened();
        }

        /// <summary>
        /// The mail target should require the subject to be configured.
        /// </summary>
        [Fact(DisplayName = "Should_RequireSubjectInConfig")]
        public void Should_RequireSubjectInConfig() {
            // Setup
            var properties = new Dictionary<string, object> {
                // Intentionally empty
            };
            var config = new TargetConfig {
                Properties = properties
            };
            ISmtpClient smtpClient;
            var target = GetMailTarget(out smtpClient);

            var layout = A.Fake<ILayout>();
            A.CallTo(() => layout.Format(A<LogEvent>.Ignored))
                .Returns("Hello, Subject Layout!");
            var layoutFactory = A.Fake<ILayoutFactory>();
            A.CallTo(() => layoutFactory.MakeLayout(A<string>.Ignored))
                .Returns(layout);

            // Execute / Verify
            Assert.Throws(typeof(InvalidOperationException), () => {
                target.Configure(config, layoutFactory);
            });
        }

        protected MailTarget GetMailTarget() {
            ISmtpClient smtpClient;
            return GetMailTarget(out smtpClient);
        }

        protected MailTarget GetMailTarget(out ISmtpClient smtpClient) {
            smtpClient = A.Fake<ISmtpClient>();
            var target = new MailTarget(smtpClient);
            return target;
        }
    }
}