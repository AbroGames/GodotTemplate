using Godot;
using KludgeBox.Godot.Extensions;
using KludgeBox.Logging;
using Serilog;

namespace KludgeBox;

//TODO Bronuh: переделать/объединить с ServiceFactory. _logger внедрять через Di по возможности.
public class LibInitializer
{
    private readonly ILogger _logger = LogFactory.GetForStatic<LibInitializer>();
    
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
        
        _logger.Information("Lib initializing...");
        
        // Del: KludgeBoxServices.CmdArgs.LogCmdArgs();
        NodeNetworkExtensionsState.SetIsClientChecker(_nodeNetworkExtensionsIsClientChecker); //TODO Переделать на сервис, в котором задается стейт, а не статик
    }
}