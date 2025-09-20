using System;

namespace KludgeBox.DI.Requests;

public class NotNullCheckFailedException : Exception
{
    public NotNullCheckFailedException(string message) : base(message){}
}