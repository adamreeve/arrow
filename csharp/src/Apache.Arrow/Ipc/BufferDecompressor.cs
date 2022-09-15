using System;
using System.Buffers.Binary;

namespace Apache.Arrow.Ipc
{
    internal sealed class BufferDecompressor : IBufferDecompressor
    {
        private readonly IDecompressor _decompressor;

        public BufferDecompressor(IDecompressor decompressor)
        {
            _decompressor = decompressor;
        }

        public ReadOnlyMemory<byte> Decompress(ReadOnlyMemory<byte> inputData)
        {
            if (inputData.Length >= 8)
            {
                // First 8 bytes give the uncompressed data length
                var uncompressedLength = BinaryPrimitives.ReadInt64LittleEndian(inputData.Span.Slice(0, 8));
                if (uncompressedLength == -1)
                {
                    // The buffer is not actually compressed
                    return inputData.Slice(8);
                }
            }
            else
            {
                throw new Exception($"Invalid compressed data buffer size ({inputData.Length}), expected at least 8 bytes");
            }
            return _decompressor.Decompress(inputData.Slice(8));
        }

        public void Dispose()
        {
            _decompressor.Dispose();
        }
    }
}
