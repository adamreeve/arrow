using System;

namespace Apache.Arrow.Dataset;

/// <summary>
/// Base class for all file format implementations
/// </summary>
public class FileFormat : IDisposable
{
    protected FileFormat(GLibBindings.FileFormat gObj)
    {
        GObj = gObj;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            GObj.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    internal readonly GLibBindings.FileFormat GObj;
}
