using KludgeBox.Core.VFS.Base;

namespace KludgeBox.Core.VFS;

public static class FsExtensions
{
    public static string GetRealPath(this IProxyFileSystem fileSystem, string path)
    {
        return PathHelper.Combine(fileSystem.RealPath, PathHelper.RemoveRoot(path));
    }
}
