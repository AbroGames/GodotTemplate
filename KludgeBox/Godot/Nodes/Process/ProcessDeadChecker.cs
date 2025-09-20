using Godot;
using KludgeBox.Core.Cooldown;
using KludgeBox.Logging;
using Serilog;

namespace KludgeBox.Godot.Nodes.Process;

public partial class ProcessDeadChecker : Node
{
    
    private readonly ILogger _logger = LogFactory.GetForStatic<ProcessDeadChecker>();
    
    private readonly int? _processPid;
    private readonly Func<int, string> _logMessageGenerator = pid => $"Process {pid} is dead.";
    private readonly Action _actionWhenDead;
    
    private readonly AutoCooldown _processDeadCheckCooldown = new(5);
    
    public ProcessDeadChecker(int processPid, Action actionWhenDead, Func<int, string> logMessageGenerator = null)
    {
        _processPid = processPid;
        _actionWhenDead = actionWhenDead;
        if (logMessageGenerator != null) _logMessageGenerator = logMessageGenerator;
        
        _processDeadCheckCooldown.ActionWhenReady += CheckProcessIsDead;
    }

    public override void _Process(double delta)
    {
        _processDeadCheckCooldown.Update(delta);
    }

    private void CheckProcessIsDead()
    {
        if (_processPid.HasValue && !System.Diagnostics.Process.GetProcesses().Any(x => x.Id == _processPid.Value))
        {
            _logger.Information(_logMessageGenerator(_processPid.Value));
            _actionWhenDead?.Invoke();
        }
    }
}
