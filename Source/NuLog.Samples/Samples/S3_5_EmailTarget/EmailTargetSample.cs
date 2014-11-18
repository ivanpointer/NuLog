/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/11/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.Configuration;
using NuLog.Configuration.Targets;
using NuLog.Extensions.Email;
using NuLog.Targets;
using System;
using System.Collections.Generic;

namespace NuLog.Samples.Samples.S3_5_EmailTarget
{
    /// <summary>
    /// An example showing the email target in action.  The narration of this sample can be
    /// found at:
    /// https://github.com/ivanpointer/NuLog/wiki/3.5-Email-Target
    /// </summary>
    public class EmailTargetSample : SampleBase
    {

        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public EmailTargetSample(string section, string sample) : base(section, sample) { }

        #endregion

        // Logging Example
        public override void ExecuteSample()
        {
            ExecuteJSON();

            ExecuteRuntime();

            ExecuteMetaData();
        }

        // Example using JSON configuration
        private void ExecuteJSON()
        {
            LoggerFactory.Initialize("Samples/S3_5_EmailTarget/NuLog.json");
            var jsonLogger = LoggerFactory.GetLogger();
            jsonLogger.LogNow("Hello from JSON config");
            Console.Out.WriteLine("Sent first message: \"Hello from JSON config\"");
        }

        // Example using runtime configuration
        private void ExecuteRuntime()
        {
            var config = LoggingConfigBuilder.CreateLoggingConfig()
                .AddTarget(EmailTargetConfigBuilder.Create()
                    .SetHost("smtp.mailgun.org")
                    .SetPort(587)
                    .SetEnableSSL(true)
                    .SetUserName("postmaster@mg.pointerplace.us")
                    .SetPassword("censored")

                    .SetFromAddress("test@mg.pointerplace.us")
                    .AddReplyTo("reply@mg.pointerplace.us")
                    .AddTo("ivan@pointerplace.us")
                    .AddCC("cc@mg.pointerplace.us")
                    .AddBCC("bcc@mg.pointerplace.us")

                    .SetSubjectLayout("TEST ${EmailSubject}")
                    .SetSubject("Hardcoded test subject")

                    //.SetBodyLayout("${DateTime:'{0:MM/dd/yyyy hh:mm:ss.fff}'} | ${Thread.ManagedThreadId:'{0,4}'} | ${Tags} | ${Message}${?Exception:'\r\n{0}'}\r\n")
                    .SetBodyFile("Samples/S3_5_EmailTarget/bodytemplate.html")
                    .SetIsBodyHtml(true)

                    .AddHeader("X-Priority", "1")

                    .Build());

            LoggerFactory.Initialize(config);
            var runtimeLogger = LoggerFactory.GetLogger();
            runtimeLogger.LogNow("Hello from runtime config");

            Console.Out.WriteLine("Sent second message: \"Hello from runtime config\"");
        }

        // Example showing setting email message specifics with meta data
        private void ExecuteMetaData()
        {
            LoggerFactory.Initialize("Samples/S3_5_EmailTarget/NuLog.json");
            var jsonLogger = LoggerFactory.GetLogger();

            // Send message for the helper
            jsonLogger.LogNow(EmailLogEventBuilder.Create("Message using EmailLogEventBuilder")
                .SetSubject("my meta builder subject")
                .SetHeader("X-Priority", "4")
                .AddAttachment("Samples/S3_5_EmailTarget/TestAttachment.txt")
                .AddToAddress("otherguy@pointerplace.us")
                .AddCCAddress("ivan@pointerplace.us")
                .AddBCCAdress("secretguy@pointerplace.us")
                .Build());

            Console.Out.WriteLine("Sent third message: \"Message using EmailLogEventBuilder\"");

            // Send message manually
            var metaData = new Dictionary<string, object>();
            metaData[EmailTarget.MetaSubject] = "my meta subject";
            metaData[EmailTarget.MetaHeaders] = new Dictionary<string, string> { { "X-Priority", "4" } };
            metaData[EmailTarget.MetaAttachments] = new List<EmailAttachment> { new EmailAttachment() { PhysicalFileName = "Samples/S3_5_EmailTarget/TestAttachment.txt" } };
            metaData[EmailTarget.MetaTo] = new List<string> { "otherguy@pointerplace.us" };
            metaData[EmailTarget.MetaCC] = new List<string> { "ivan@pointerplace.us" };
            metaData[EmailTarget.MetaBCC] = new List<string> { "secretguy@pointerplace.us" };

            jsonLogger.LogNow("Hello from manual meta data", metaData);

            Console.Out.WriteLine("Sent fourth message: \"Hello from manual meta data\"");
        }

    }
}
