using PPMdArchiver.Core.IO;
using PPMdArchiver.Core.Utils;

namespace PPMdArchiver.Core.Models
{
    public class ArchiveHeader
    {
        public int OriginalLength { get; private set; }
        public int Order { get; private set; }

        public ArchiveHeader(int originalLength, int order)
        {
            OriginalLength = originalLength;
            Order = order;
        }

        public bool Write(IIOHandler ioHandler)
        {
            byte[] header = new byte[5];
            header[0] = (byte)(OriginalLength >> 24);
            header[1] = (byte)(OriginalLength >> 16);
            header[2] = (byte)(OriginalLength >> 8);
            header[3] = (byte)OriginalLength;
            header[4] = (byte)Order;
            return ioHandler.WriteBlock(header, header.Length);
        }

        public static ArchiveHeader Read(IIOHandler ioHandler)
        {
            byte[] header = new byte[5];
            int bytesRead;
            if (!ioHandler.ReadBlock(header, out bytesRead) || bytesRead < 5)
                throw new InvalidOperationException("Invalid header.");

            int length = (header[0] << 24) | (header[1] << 16) | (header[2] << 8) | header[3];
            int order = header[4];
            return new ArchiveHeader(length, order);
        }
    }
}