using KludgeBox.DI.Access;

namespace KludgeBox.DI.Requests;

public interface IProcessingRequestScanner
{
    bool TryGetRequest(IMemberAccessor accessor, out IProcessingRequest? injectionRequest);
}