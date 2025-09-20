namespace KludgeBox.DI.Access;

public interface IMemberSetter : IBaseMemberInfo
{
    void SetValue(object target, object value);
}