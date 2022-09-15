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

        public ReadOnlyMemory<byte> Decompress(ReadOnlyMemory<byte> inputData)
        {
            return _decompressor.Unwrap(inputData.Span);
        }

        public void Dispose()
        {
            _decompressor.Dispose();
        }
    }
}
