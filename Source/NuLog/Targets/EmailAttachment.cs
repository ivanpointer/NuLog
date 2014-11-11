/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/11/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

namespace NuLog.Targets
{
    /// <summary>
    /// Represents an attachement to an email message
    /// </summary>
    public class EmailAttachment
    {
        /// <summary>
        /// Standard constructor
        /// </summary>
        public EmailAttachment() { }

        /// <summary>
        /// Constructor that points to a physical file on the file system
        /// </summary>
        /// <param name="fileName">The path/file name of the file to attach</param>
        public EmailAttachment(string fileName)
        {
            PhysicalFileName = fileName;
        }

        /// <summary>
        /// Constructor that attaches a physical file and renames it to "attachmentfileName"
        /// </summary>
        /// <param name="physicalFile">The physical path/name of the file to attach</param>
        /// <param name="attachmentFileName">The name of the file to use for attaching</param>
        public EmailAttachment(string physicalFile, string attachmentFileName)
        {
            PhysicalFileName = physicalFile;
            AttachmentFileName = attachmentFileName;
        }

        /// <summary>
        /// Attaches a file with the contents "attachmentData" and the name "attachmentName"
        /// </summary>
        /// <param name="attachmentData">The data which is the contents of the file to attach</param>
        /// <param name="attachmentName">The name of the file to attach</param>
        public EmailAttachment(byte[] attachmentData, string attachmentName)
        {
            Data = attachmentData;
            AttachmentFileName = attachmentName;
        }

        /// <summary>
        /// The file name/path of a physical file to attach to the email.  If "Data" is provided, this will be ignored.
        /// </summary>
        public string PhysicalFileName { get; set; }
        /// <summary>
        /// The name of the file as it is attached to the email
        /// </summary>
        public string AttachmentFileName { get; set; }
        /// <summary>
        /// Binary data to attach to the email as an attachment
        /// </summary>
        public byte[] Data { get; set; }
    }
}
