using SvwDesign.Notification.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace SvwDesign.Notification.Services
{
    public class AttachmentBuilder : IAttachmentBuilder
    {
        public async Task<Stream> BuildAttachment(byte[] file)
        {
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream);
            await memoryStream.WriteAsync(file);
            memoryStream.Seek(0, SeekOrigin.Begin); // Reset stream back to beginning

            return memoryStream;
        }
    }
}
