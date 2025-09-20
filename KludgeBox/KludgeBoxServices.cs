using KludgeBox.Core;

namespace KludgeBox;

//TODO Проверить какие сервисы реально нужны
//TODO Добавить сервис с хешами/деревом, и ноду StateCheker. Мб что-то ещё из фантории. (MpSpawner? MpSync?)
internal static class KludgeBoxServices
{
    public static CmdArgsService CmdArgs = new CmdArgsService();
    public static RandomService Rand = new RandomService();
    public static MathService Math = new MathService();
    public static StringCompressService StringCompress = new StringCompressService();
}