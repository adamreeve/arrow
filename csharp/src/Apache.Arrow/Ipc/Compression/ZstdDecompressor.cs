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
using ZstdNet;
#endif

namespace Apache.Arrow.Ipc.Compression
{
    /// <summary>
    /// Decompressor for the ZSTD compression codec using ZstdNet.
    /// </summary>
    internal sealed class ZstdDecompressor : IDecompressor
    {
#if (NETSTANDARD2_0_OR_GREATER || NETCOREAPP3_1_OR_GREATER)
        private readonly Decompressor _decompressor;
#endif

        public ZstdDecompressor()
        {
#if (NETSTANDARD2_0_OR_GREATER || NETCOREAPP3_1_OR_GREATER)
            _decompressor = new Decompressor();
#else
            throw new NotSupportedException(
                "ZSTD decompression support requires at least netstandard 2.0 or netcoreapp 3.1, and the ZstdNet package to be installed");
#endif
        }

        public int Decompress(ReadOnlyMemory<byte> source, Memory<byte> destination)
        {
#if (NETSTANDARD2_0_OR_GREATER || NETCOREAPP3_1_OR_GREATER)
            return _decompressor.Unwrap(source.Span, destination.Span);
#else
            throw new NotSupportedException("LZ4 decompression is only supported with netstandard >= 2.0 or netcoreapp >= 3.1");
#endif
        }

        public void Dispose()
        {
#if (NETSTANDARD2_0_OR_GREATER || NETCOREAPP3_1_OR_GREATER)
            _decompressor.Dispose();
#endif
        }
    }
}
