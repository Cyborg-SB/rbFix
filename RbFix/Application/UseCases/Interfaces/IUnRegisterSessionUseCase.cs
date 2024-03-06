namespace RbFix.Application.UseCases.Interfaces
{
    public interface IUnRegisterSessionUseCase
    {
        bool UnRegisterSessionAsync(string name);
        bool UnRegisterAllSessionAsync();
    }
}
