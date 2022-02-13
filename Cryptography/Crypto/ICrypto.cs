namespace Cryptography.Crypto
{
    public interface ICrypto
    {
        public string Encrypt(string message);
        public string Decrypt(string message);
    }
}