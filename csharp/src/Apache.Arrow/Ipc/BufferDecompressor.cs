using System;
using System.Buffers.Binary;
using Apache.Arrow.Memory;

namespace Apache.Arrow.Ipc
{
    internal sealed class BufferDecompressor : IBufferDecompressor
    {
        private readonly IDecompressor _decompressor;
        private readonly MemoryAllocator _allocator;

        public BufferDecompressor(IDecompressor decompressor, MemoryAllocator allocator)
        {
            _decompressor = decompressor;
            _allocator = allocator;
        }

        public ArrowBuffer Decompress(ReadOnlyMemory<byte> inputData)
        {
            if (inputData.Length < 8)
            {
                throw new Exception($"Invalid compressed data buffer size ({inputData.Length}), expected at least 8 bytes");
            }

            // First 8 bytes give the uncompressed data length
            var uncompressedLength = BinaryPrimitives.ReadInt64LittleEndian(inputData.Span.Slice(0, 8));
            if (uncompressedLength == -1)
            {
                // The buffer is not actually compressed
                return new ArrowBuffer(inputData.Slice(8));
            }

            var outputData = _allocator.Allocate(Convert.ToInt32(uncompressedLength));
            var decompressedLength = _decompressor.Decompress(inputData.Slice(8), outputData.Memory);
            if (decompressedLength != uncompressedLength)
            {
                throw new Exception($"Expected to decompress {uncompressedLength} bytes, but got {decompressedLength} bytes");
            }

            return new ArrowBuffer(outputData);
        }

        public void Dispose()
        {
            _decompressor.Dispose();
        }
    }
}
