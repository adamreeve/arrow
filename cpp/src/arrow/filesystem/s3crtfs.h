// Licensed to the Apache Software Foundation (ASF) under one
// or more contributor license agreements.  See the NOTICE file
// distributed with this work for additional information
// regarding copyright ownership.  The ASF licenses this file
// to you under the Apache License, Version 2.0 (the
// "License"); you may not use this file except in compliance
// with the License.  You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.

#pragma once

#include <memory>
#include <string>
#include <vector>

#include "arrow/filesystem/filesystem.h"
#include "arrow/filesystem/s3fs.h"
#include "arrow/util/macros.h"
#include "arrow/util/uri.h"

namespace Aws {
namespace Auth {

class AWSCredentialsProvider;
class STSAssumeRoleCredentialsProvider;

}  // namespace Auth
namespace STS {
class STSClient;
}
}  // namespace Aws

namespace arrow {
namespace fs {

/// S3-backed FileSystem implementation.
///
/// Some implementation notes:
/// - buckets are special and the operations available on them may be limited
///   or more expensive than desired.
class ARROW_EXPORT S3CrtFileSystem : public FileSystem {
 public:
  ~S3CrtFileSystem() override;

  std::string type_name() const override { return "s3crt"; }

  /// Return the original S3 options when constructing the filesystem
  S3Options options() const;
  /// Return the actual region this filesystem connects to
  std::string region() const;

  bool Equals(const FileSystem& other) const override;
  Result<std::string> PathFromUri(const std::string& uri_string) const override;

  /// \cond FALSE
  using FileSystem::CreateDir;
  using FileSystem::DeleteDirContents;
  using FileSystem::DeleteDirContentsAsync;
  using FileSystem::GetFileInfo;
  using FileSystem::OpenAppendStream;
  using FileSystem::OpenOutputStream;
  /// \endcond

  Result<FileInfo> GetFileInfo(const std::string& path) override;
  Result<std::vector<FileInfo>> GetFileInfo(const FileSelector& select) override;

  FileInfoGenerator GetFileInfoGenerator(const FileSelector& select) override;

  Status CreateDir(const std::string& path, bool recursive) override;

  Status DeleteDir(const std::string& path) override;
  Status DeleteDirContents(const std::string& path, bool missing_dir_ok) override;
  Future<> DeleteDirContentsAsync(const std::string& path, bool missing_dir_ok) override;
  Status DeleteRootDirContents() override;

  Status DeleteFile(const std::string& path) override;

  Status Move(const std::string& src, const std::string& dest) override;

  Status CopyFile(const std::string& src, const std::string& dest) override;

  /// Create a sequential input stream for reading from a S3 object.
  ///
  /// NOTE: Reads from the stream will be synchronous and unbuffered.
  /// You way want to wrap the stream in a BufferedInputStream or use
  /// a custom readahead strategy to avoid idle waits.
  Result<std::shared_ptr<io::InputStream>> OpenInputStream(
      const std::string& path) override;
  /// Create a sequential input stream for reading from a S3 object.
  ///
  /// This override avoids a HEAD request by assuming the FileInfo
  /// contains correct information.
  Result<std::shared_ptr<io::InputStream>> OpenInputStream(const FileInfo& info) override;

  /// Create a random access file for reading from a S3 object.
  ///
  /// See OpenInputStream for performance notes.
  Result<std::shared_ptr<io::RandomAccessFile>> OpenInputFile(
      const std::string& path) override;
  /// Create a random access file for reading from a S3 object.
  ///
  /// This override avoids a HEAD request by assuming the FileInfo
  /// contains correct information.
  Result<std::shared_ptr<io::RandomAccessFile>> OpenInputFile(
      const FileInfo& info) override;

  /// Create a sequential output stream for writing to a S3 object.
  ///
  /// NOTE: Writes to the stream will be buffered.  Depending on
  /// S3Options.background_writes, they can be synchronous or not.
  /// It is recommended to enable background_writes unless you prefer
  /// implementing your own background execution strategy.
  Result<std::shared_ptr<io::OutputStream>> OpenOutputStream(
      const std::string& path,
      const std::shared_ptr<const KeyValueMetadata>& metadata) override;

  Result<std::shared_ptr<io::OutputStream>> OpenAppendStream(
      const std::string& path,
      const std::shared_ptr<const KeyValueMetadata>& metadata) override;

  /// Create a S3CrtFileSystem instance from the given options.
  static Result<std::shared_ptr<S3CrtFileSystem>> Make(
      const S3Options& options, const io::IOContext& = io::default_io_context());

 protected:
  explicit S3CrtFileSystem(const S3Options& options, const io::IOContext&);

  class Impl;
  std::shared_ptr<Impl> impl_;
};

void FinalizeS3Crt();

void LeakS3CrtClients();

}  // namespace fs
}  // namespace arrow
