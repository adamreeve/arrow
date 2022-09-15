using System;
using CommunityToolkit.HighPerformance;
using K4os.Compression.LZ4.Streams;

namespace Apache.Arrow.Ipc
{
    internal sealed class Lz4Decompressor : IDecompressor
    {
        public int Decompress(ReadOnlyMemory<byte> source, Memory<byte> destination)
        {
            using var decompressedStream = LZ4Stream.Decode(source.AsStream());
            var outputStream = destination.AsStream();
            decompressedStream.CopyTo(outputStream);
            return (int) outputStream.Length;
        }

        public void Dispose()
        {
        }
    }
}
