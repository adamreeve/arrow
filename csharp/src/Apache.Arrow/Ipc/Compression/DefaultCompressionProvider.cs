using System;
using System.IO;

namespace Apache.Arrow.Ipc.Compression
{
    /// <summary>
    /// Default compression provider that uses common compression packages.
    /// </summary>
    internal sealed class DefaultCompressionProvider : ICompressionProvider
    {
        public IDecompressor GetDecompressor(CompressionType compressionType)
        {
            switch (compressionType)
            {
                case CompressionType.Lz4Frame:
                {
                    try
                    {
                        return new Lz4Decompressor();
                    }
                    catch (FileNotFoundException exc)
                    {
                        throw new Exception($"Error finding LZ4 decompression dependency ({exc.Message.Trim()}). " +
                                            "LZ4 decompression support requires the K4os.Compression.LZ4.Streams and " +
                                            "CommunityToolkit.HighPerformance packages to be installed");
                    }
                }
                case CompressionType.Zstd:
                {
                    try
                    {
                        return new ZstdDecompressor();
                    }
                    catch (FileNotFoundException exc)
                    {
                        throw new Exception($"Error finding ZSTD decompression dependency ({exc.Message.Trim()}). " +
                                            "ZSTD decompression support requires the ZstdNet package to be installed");
                    }
                }
                default:
                {
                    throw new NotImplementedException($"Compression type {compressionType} is not supported");
                }
            }
        }
    }
}
