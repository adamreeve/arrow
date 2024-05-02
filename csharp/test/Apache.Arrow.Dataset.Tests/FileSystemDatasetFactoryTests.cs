using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Apache.Arrow.Dataset.Tests;

public class FileSystemDatasetFactoryTests
{
    public FileSystemDatasetFactoryTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task CreateParquetFileSystemDataset()
    {
        var fileFormat = new ParquetFileFormat();
        using var datasetFactory = new FileSystemDatasetFactory(fileFormat);
        datasetFactory.SetFileSystem(new LocalFileSystem(new LocalFileSystemOptions()));
        datasetFactory.AddPath("/home/adam/dev/gross/parquet-issues/hive-partitioning/dataset");
        using var dataset = datasetFactory.Finish(null);

        Assert.NotNull(dataset);

        using var reader = dataset.GetRecordBatchReader();
        while (await reader.ReadNextRecordBatchAsync() is { } batch)
        {
            _testOutputHelper.WriteLine(batch.ToString());
        }
    }

    private readonly ITestOutputHelper _testOutputHelper;
}
