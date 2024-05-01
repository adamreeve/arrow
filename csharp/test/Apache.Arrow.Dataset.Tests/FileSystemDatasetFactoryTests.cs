using Xunit;

namespace Apache.Arrow.Dataset.Tests;

public class FileSystemDatasetFactoryTests
{
    [Fact]
    public void CreateParquetFileSystemDataset()
    {
        var fileFormat = new ParquetFileFormat();
        using var datasetFactory = new FileSystemDatasetFactory(fileFormat);
        datasetFactory.SetFileSystem(new LocalFileSystem(new LocalFileSystemOptions()));
        datasetFactory.AddPath("/home/adam/dev/gross/parquet-issues/hive-partitioning/dataset");
        using var dataset = datasetFactory.Finish(null);

        Assert.NotNull(dataset);
    }
}
