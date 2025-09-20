using Godot;

namespace KludgeBox;

//TODO Bronuh: переделать/объединить с ServiceFactory
public class LibInitializer
{

    private Func<Node, bool> _nodeNetworkExtensionsIsClientChecker;

    public LibInitializer SetNodeNetworkExtensionsIsClientChecker(Func<Node, bool> isClientChecker)
    {
        _nodeNetworkExtensionsIsClientChecker = isClientChecker;
        return this;
    }
    
    public void Init()
    {
        if (_nodeNetworkExtensionsIsClientChecker == null)
        {
            throw new Exception("You must set NodeNetworkExtensionsIsClientChecker before calling Init function");
        }
        
        ServiceLocator.Init();
        Log.Info("Lib initializing...");
        
        KludgeBoxServices.ExceptionHandler.AddExceptionHandlerForUnhandledException();
        KludgeBoxServices.CmdArgs.LogCmdArgs();
        NodeNetworkExtensionsState.SetIsClientChecker(_nodeNetworkExtensionsIsClientChecker); //TODO Переделать на сервис, в котором задается стейт, а не статик
    }
}