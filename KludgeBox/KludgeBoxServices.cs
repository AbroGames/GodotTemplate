using KludgeBox.Core;
using KludgeBox.Core.Random;
using KludgeBox.DI;

namespace KludgeBox;

//TODO Проверить какие сервисы реально нужны
//TODO Добавить сервис с хешами/деревом, и ноду StateCheker. Мб что-то ещё из фантории. (MpSpawner? MpSync?)
internal static class KludgeBoxServices
{
    public static DependencyInjector Di = new DependencyInjector();
    public static RandomService Rand = new RandomService();
    
    public static class Global
    {
        public static DependencyInjector Di => KludgeBoxServices.Di;
    }
}