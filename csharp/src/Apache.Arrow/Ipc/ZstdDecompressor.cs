using System;
using ZstdNet;

namespace Apache.Arrow.Ipc
{
    internal sealed class ZstdDecompressor : IDecompressor
    {
        private readonly Decompressor _decompressor;

        public ZstdDecompressor()
        {
            _decompressor = new Decompressor();
        }

        public int Decompress(ReadOnlyMemory<byte> inputData, Memory<byte> destination)
        {
            return _decompressor.Unwrap(inputData.Span, destination.Span);
        }

        public void Dispose()
        {
            _decompressor.Dispose();
        }
    }
}
