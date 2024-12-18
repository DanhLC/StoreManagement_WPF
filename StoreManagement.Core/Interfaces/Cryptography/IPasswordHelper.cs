namespace StoreManagement.Core.Interfaces.Cryptography
{
    public interface IPasswordHelper
    {
        (string hashedPassword, string salt) HashPassword(string password);
        bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt);
    }
}
