namespace KludgeBox.DI.Access;

public interface IMemberGetter : IBaseMemberInfo
{
    object GetValue(object target);
}