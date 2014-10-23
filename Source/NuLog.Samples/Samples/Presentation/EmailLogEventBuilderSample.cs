using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuLog.Extensions.Email;
using NuLog.Targets;

namespace NuLog.Samples.Samples.Presentation
{
    public class EmailLogEventBuilderSample : SampleBase
    {
        public EmailLogEventBuilderSample(string section, string sample) : base(section, sample) { }

        public override void ExecuteSample(Arguments args)
        {

            var logger = LoggerFactory.GetLogger();

            logger.Log(EmailLogEventBuilder.Create("Hello, World!", "mytag")
                .AddToAddress("someguy@somewhere.org")
                .AddCCAddress("someotherguy@somewhere.org")
                .AddAttachment(new EmailAttachment {
                    PhysicalFileName = "mytextfile.txt",
                    AttachmentFileName = "renamed.txt" })
                .SetSubject("Hello, Subject!")
                .SetHeader("someheader", "headervalue")
                .Build());

        }
    }
}
