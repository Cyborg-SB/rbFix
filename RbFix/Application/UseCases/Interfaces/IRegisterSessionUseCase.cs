namespace RbFix.Application.UseCases.Interfaces
{
    public interface IRegisterSessionUseCase
    {
        bool RegisterSession(string sessionName);
        bool RegisterAllSession(string sessionName);
    }
}
