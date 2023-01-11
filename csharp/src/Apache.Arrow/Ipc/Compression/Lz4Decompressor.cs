// Licensed to the Apache Software Foundation (ASF) under one or more
// contributor license agreements. See the NOTICE file distributed with
// this work for additional information regarding copyright ownership.
// The ASF licenses this file to You under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with
// the License.  You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
#if (NETSTANDARD2_0_OR_GREATER || NETCOREAPP3_1_OR_GREATER)
using CommunityToolkit.HighPerformance;
using K4os.Compression.LZ4.Streams;
#endif


namespace Apache.Arrow.Ipc.Compression
{
    /// <summary>
    /// Decompressor for the LZ4 Frame compression codec using K4os.Compression.LZ4.Streams.
    /// </summary>
    internal sealed class Lz4Decompressor : IDecompressor
    {
        public Lz4Decompressor()
        {
#if (NETSTANDARD2_0_OR_GREATER || NETCOREAPP3_1_OR_GREATER)
            // Try using something from K4os.Compression.LZ4.Streams and CommunityToolkit.HighPerformance
            // to verify they are available, so we can catch a FileNotFoundException in CompressionProvider
            // and provide a helpful error message rather than only encountering an error once Decompress is called.
            var settings = new LZ4DecoderSettings();
            var stream = System.Array.Empty<byte>().AsMemory().AsStream();
#else
            throw new NotSupportedException("LZ4 decompression is only supported with netstandard >= 2.0 or netcoreapp >= 3.1");
#endif
        }

        public int Decompress(ReadOnlyMemory<byte> source, Memory<byte> destination)
        {
#if (NETSTANDARD2_0_OR_GREATER || NETCOREAPP3_1_OR_GREATER)
            using var sourceStream = source.AsStream();
            using var destStream = destination.AsStream();
            using var decompressedStream = LZ4Stream.Decode(sourceStream);
            decompressedStream.CopyTo(destStream);
            return (int) destStream.Length;
#else
            throw new NotSupportedException("LZ4 decompression is only supported with netstandard >= 2.0 or netcoreapp >= 3.1");
#endif
        }

        public void Dispose()
        {
        }
    }
}
