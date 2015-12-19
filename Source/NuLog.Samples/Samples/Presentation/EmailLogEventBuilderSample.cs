/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/20/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.Extensions.Email;
using NuLog.Targets;

namespace NuLog.Samples.Samples.Presentation
{
    public class EmailLogEventBuilderSample : SampleBase
    {
        public EmailLogEventBuilderSample(string section, string sample) : base(section, sample)
        {
        }

        public override void ExecuteSample()
        {
            var logger = LoggerFactory.GetLogger();

            logger.Log(EmailLogEventBuilder.Create("Hello, World!", "mytag")
                .AddToAddress("someguy@somewhere.org")
                .AddCCAddress("someotherguy@somewhere.org")
                .AddAttachment(new EmailAttachment
                {
                    PhysicalFileName = "mytextfile.txt",
                    AttachmentFileName = "renamed.txt"
                })
                .SetSubject("Hello, Subject!")
                .SetHeader("someheader", "headervalue")
                .Build());
        }
    }
}