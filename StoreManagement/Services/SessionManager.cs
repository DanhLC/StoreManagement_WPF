namespace StoreManagement.Services
{
    public class SessionManager
    {
        private static SessionManager _instance;

        public static SessionManager Instance => _instance ??= new SessionManager();

        public int UserId { get; set; }
        public string Username { get; set; }
        public string? FullName { get; set; }
        public string? Role { get; set; }
        public string? Email { get; set; }

        private SessionManager()
        {
        }

        public void Clear()
        {
            UserId = 0;
            Username = string.Empty;
            FullName = string.Empty;
            Role = string.Empty;
            Email = string.Empty;
        }
    }
}
