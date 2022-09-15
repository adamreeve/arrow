using System;

namespace Apache.Arrow.Ipc
{
    internal sealed class UncompressedBufferDecompressor : IBufferDecompressor
    {
        public ReadOnlyMemory<byte> Decompress(ReadOnlyMemory<byte> inputData)
        {
            return inputData;
        }

        public void Dispose()
        {
        }
    }
}
