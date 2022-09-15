using System;

namespace Apache.Arrow.Ipc
{
    internal interface IDecompressor : IDisposable
    {
        ReadOnlyMemory<byte> Decompress(ReadOnlyMemory<byte> inputData);
    }
}
