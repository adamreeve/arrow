using System;

namespace Apache.Arrow.Ipc
{
    internal interface IBufferDecompressor : IDisposable
    {
        ArrowBuffer Decompress(ReadOnlyMemory<byte> inputData);
    }
}
