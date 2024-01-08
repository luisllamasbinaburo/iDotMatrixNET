using System.Runtime.Intrinsics.Arm;
using Windows.Media.Capture;

namespace iDotMatrixNET.Classes
{
    public class Gif
    {

        public byte[] LoadGif(string filePath)
        {
            using (FileStream file = File.OpenRead(filePath))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

        public List<byte[]> SplitIntoChunks(byte[] data, int chunkSize)
        {
            return Enumerable.Range(0, data.Length / chunkSize + 1)
                .Select(i => data.Skip(i * chunkSize).Take(chunkSize).ToArray())
                .ToList();
        }

        public List<byte[]> CreatePayloads(byte[] gifData)
        {
            uint crc = Crc32.ComputeChecksum(gifData);

            byte[] header = new byte[]
            {
                255, 255, 1, 0, 0, 255, 255, 255, 255, 255, 255, 255, 255, 5, 0, 13
            };

            byte[] lengthBytes = BitConverter.GetBytes(gifData.Length + header.Length);
            Array.Copy(lengthBytes, 0, header, 5, 4);

            byte[] crcBytes = BitConverter.GetBytes(crc);
            Array.Copy(crcBytes, 0, header, 9, 4);

            List<byte[]> gifChunks = SplitIntoChunks(gifData, 4096);
            List<byte[]> payloads = new List<byte[]>();

            for (int i = 0; i < gifChunks.Count; i++)
            {
                header[4] = (byte)(i > 0 ? 2 : 0);
                int chunkLen = gifChunks[i].Length + header.Length;
                byte[] chunkLenBytes = BitConverter.GetBytes((ushort)chunkLen);
                Array.Copy(chunkLenBytes, 0, header, 0, 2);

                byte[] payload = header.Concat(gifChunks[i]).ToArray();
                payloads.Add(payload);
            }

            return payloads;
        }
    }


    public static class Crc32
    {
        private static uint[] _table;

        public static uint ComputeChecksum(byte[] bytes)
        {
            if (_table == null)
            {
                _table = CreateTable();
            }

            uint crc = 0xffffffff;
            for (int i = 0; i < bytes.Length; ++i)
            {
                byte index = (byte)(((crc) & 0xff) ^ bytes[i]);
                crc = (_table[index] ^ (crc >> 8));
            }
            return ~crc;
        }

        private static uint[] CreateTable()
        {
            uint[] table = new uint[256];
            const uint polynomial = 0xedb88320;
            for (uint i = 0; i < 256; ++i)
            {
                uint crc = i;
                for (uint j = 8; j > 0; --j)
                {
                    if ((crc & 1) == 1)
                    {
                        crc = (crc >> 1) ^ polynomial;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
                table[i] = crc;
            }
            return table;
        }
    }
}