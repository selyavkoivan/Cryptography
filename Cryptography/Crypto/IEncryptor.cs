namespace Cryptography.Crypto
{
    public interface IEncryptor
    {
        public string Encrypt(string message);
        public string Decrypt(string message);
    }
}