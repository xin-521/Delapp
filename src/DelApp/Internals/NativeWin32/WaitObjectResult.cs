// NOTE: not currently compiled; removed from DelApp.csproj. Kept for reference.
namespace DelApp.Internals.Win32
{

    internal enum WaitObjectResult : uint
    {
        WAIT_OBJECT_0 = 0x00000000,
        WAIT_TIMEOUT = 0x00000102,
        WAIT_FAILED = 0xFFFFFFFF,
        WAIT_ABANDONED = 0x00000080
    }
}
