using Godot;
using KludgeBox.Logging;
using Serilog;

namespace KludgeBox.Godot.Nodes.Process;

// ReSharper disable once Godot.MissingParameterlessConstructor
public partial class ProcessShutdowner : Node
{
    
    private static readonly long[] ProcessShutdownNotificationTypes =
    [
        NotificationWMCloseRequest, 
        NotificationCrash, 
        NotificationDisabled, 
        NotificationPredelete,
        NotificationExitTree
    ];
    
    private readonly ILogger _logger = LogFactory.GetForStatic<ProcessShutdowner>(); //TODO Проверить этот и другие логеры в либе, а можно ли их внедрить через DI?
    
    private readonly int _processPid;
    private readonly Func<int, string> _logMessageGenerator = pid => $"Kill process {pid}.";
    
    public ProcessShutdowner(int processPid, Func<int, string> logMessageGenerator = null)
    {
        _processPid = processPid;
        if (logMessageGenerator != null) _logMessageGenerator = logMessageGenerator;
    }
    
    public override void _Notification(int id)
    {
        if (ProcessShutdownNotificationTypes.Contains(id) && OS.IsProcessRunning(_processPid))
        {
            Shutdown();
        }
    }

    public void Shutdown()
    {
        _logger.Information(_logMessageGenerator(_processPid));
        OS.Kill(_processPid);
    }
}
