namespace KludgeBox;

internal static class KludgeBoxServices
{ 
    public static ExceptionHandlerService ExceptionHandler => ServiceLocator.Get<ExceptionHandlerService>();
    public static CmdArgsService CmdArgs => ServiceLocator.Get<CmdArgsService>();
    public static LogService Log => ServiceLocator.Get<LogService>();
    public static NotNullCheckerService NotNullChecker => ServiceLocator.Get<NotNullCheckerService>();
    public static RandomService Rand => ServiceLocator.Get<RandomService>();
    public static Math Math => ServiceLocator.Get<Math>();
    public static StringCompressService StringCompress => ServiceLocator.Get<StringCompressService>();
}