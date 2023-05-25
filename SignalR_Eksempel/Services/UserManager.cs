using SignalR_Eksempel.Models;

namespace SignalR_Eksempel.Services
{
    public class UserManager
    {
        
        private List<ChatUser> _users = new();

        public void CreateUser(string Username,string UserId)
        {
            if (!_users.Where(u => u.UserName == Username).Any()) 
            {
                _users.Add(new ChatUser { UserId = UserId, UserName = Username });
            }
            else
            {
                throw new Exception("Username is already taken");
            }
            
        }

        public List<ChatUser> GetUsers() => _users.ToList();

        public ChatUser GetUser(string Username)
        {
            return _users.Where(u => u.UserName == Username).FirstOrDefault() ?? throw new KeyNotFoundException();
        }

        public void RemoveUser(string UserId)
        {
            var user = _users.Where(u => u.UserId == UserId).FirstOrDefault();
            
            if (user != null)
            {
                _users.Remove(user);
            }
        }

        public ChatUser GetUserbyId(string UserId)
        {
            return _users.Where(u => u.UserId == UserId).FirstOrDefault() ?? throw new EntryPointNotFoundException();
        }

    }
}
