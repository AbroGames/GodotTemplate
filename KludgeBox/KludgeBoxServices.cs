using KludgeBox.Core;

namespace KludgeBox;

internal static class KludgeBoxServices
{
    public static CmdArgsService CmdArgs = new CmdArgsService();
    public static RandomService Rand = new RandomService();
    public static MathService Math = new MathService();
    public static StringCompressService StringCompress = new StringCompressService();
}