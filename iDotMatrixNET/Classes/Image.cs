namespace iDotMatrixNET.Classes
{
    public class Image
    {
        public byte[] Show(int mode = 1)
        {
            try
            {
                return new byte[]
                {
                5,
                0,
                4,
                1,
                (byte)(mode % 256)
                };
            }
            catch (Exception error)
            {
                Environment.Exit(1);
                return null; // Unreachable code, just to satisfy compiler
            }
        }

        public byte[] LoadPng(string filePath)
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

        public List<byte[]> CreatePayloads(byte[] pngData)
        {
            List<byte[]> pngChunks = SplitIntoChunks(pngData, 4096);
            int idk = pngData.Length + pngChunks.Count;
            byte[] idkBytes = BitConverter.GetBytes((short)idk);
            byte[] pngLenBytes = BitConverter.GetBytes(pngData.Length);

            List<byte[]> payloads = new List<byte[]>();
            foreach (var (i, chunk) in pngChunks.Select((value, index) => (index, value)))
            {
                byte[] payload = idkBytes.Concat(new byte[] { 0, 0, (byte)(i > 0 ? 2 : 0) })
                    .Concat(pngLenBytes)
                    .Concat(chunk)
                    .ToArray();

                payloads.Add(payload);
            }
            return payloads;
        }

        public List<byte[]> UploadUnprocessed(string filePath)
        {
            byte[] pngData = LoadPng(filePath);
            return CreatePayloads(pngData);
        }      
    }
}