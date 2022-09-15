using System;

namespace Apache.Arrow.Ipc
{
    internal interface IDecompressor : IDisposable
    {
        int Decompress(ReadOnlyMemory<byte> source, Memory<byte> destination);
    }
}
