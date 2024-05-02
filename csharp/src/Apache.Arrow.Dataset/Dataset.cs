using System;
using Apache.Arrow.C;
using Apache.Arrow.Ipc;

namespace Apache.Arrow.Dataset;

public partial class Dataset
{
    public unsafe IArrowArrayStream GetRecordBatchReader()
    {
        var recordBatchReader = ToRecordBatchReader();
        var arrayStreamPtr = recordBatchReader.Export();
        try
        {
            return CArrowArrayStreamImporter.ImportArrayStream((CArrowArrayStream*)arrayStreamPtr);
        }
        finally
        {
            if (arrayStreamPtr != IntPtr.Zero)
            {
                GLib.Marshaller.Free(arrayStreamPtr);
            }
        }
    }
}
