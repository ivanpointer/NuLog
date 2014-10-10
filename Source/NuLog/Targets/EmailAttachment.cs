/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/8/2014
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
