using System;

namespace Apache.Arrow.Ipc
{
    internal sealed class UncompressedBufferDecompressor : IBufferDecompressor
    {
        public ArrowBuffer Decompress(ReadOnlyMemory<byte> inputData)
        {
            return new ArrowBuffer(inputData);
        }

        public void Dispose()
        {
        }
    }
}
