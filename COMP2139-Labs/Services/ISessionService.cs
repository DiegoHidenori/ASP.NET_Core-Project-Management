namespace COMP2139_Labs.Services
{
    public interface ISessionService
    {
        void SetSessionData<T>(string key, T value);
        T GetSessionData<T>(string key);
    }
}
