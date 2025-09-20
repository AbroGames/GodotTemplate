using System;

namespace KludgeBox.DI.Requests;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class LoggerAttribute : Attribute;