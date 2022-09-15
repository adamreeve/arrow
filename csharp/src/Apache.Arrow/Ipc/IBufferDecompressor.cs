using System;

namespace Apache.Arrow.Ipc
{
    internal interface IBufferDecompressor : IDisposable
    {
        ReadOnlyMemory<byte> Decompress(ReadOnlyMemory<byte> inputData);
    }
}
