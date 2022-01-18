using System.IO;
using System.Threading.Tasks;

namespace SvwDesign.Notification.Interfaces
{
    public interface IAttachmentBuilder
    {
        Task<Stream> BuildAttachment(byte[] file);
    }
}