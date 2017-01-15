/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

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
            using (var factory = new LoggerFactory())
            {
                var logger = factory.Logger();

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
}