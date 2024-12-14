using System.ComponentModel;

namespace StoreManagement.Core.Interfaces.Services
{
    public interface ISessionManager : INotifyPropertyChanged
    {
        int UserId { get; set; }
        string Username { get; set; }
        string? FullName { get; set; }
        string? Role { get; set; }
        string? Email { get; set; }
        void Clear();
    }
}
