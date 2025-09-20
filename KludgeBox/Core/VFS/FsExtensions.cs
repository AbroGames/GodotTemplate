using Fantoria.Lib.Utils.VFS.Base;

namespace Fantoria.Lib.Utils.VFS;

public static class FsExtensions
{
    public static string GetRealPath(this IProxyFileSystem fileSystem, string path)
    {
        return PathHelper.Combine(fileSystem.RealPath, PathHelper.RemoveRoot(path));
    }
}
