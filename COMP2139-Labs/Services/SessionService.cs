//using System.Text.Json; // Week 14

//namespace COMP2139_Labs.Services
//{
//    public class SessionService : ISessionService
//    {
//        private readonly IHttpContextAccessor _contextAccessor;

//        public SessionService(IHttpContextAccessor contextAccessor)
//        {
//            _contextAccessor = contextAccessor;
//        }

//        public void SetSessionData<T>(string key, T value)
//        {
//            var session = _contextAccessor.HttpContext.Session;
//            session.SetString(key, JsonSerializer.Serialize(value));
//        }

//        public T GetSessionData<T>(string key)
//        {
//            var session = _contextAccessor.HttpContext.Session;
//            var value = session.GetString(key);
//            return value == null ? default : JsonSerializer.Deserialize<T>(value);
//        }
//    }
//}
