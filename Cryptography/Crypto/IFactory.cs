namespace Cryptography.Crypto
{
    public interface IFactory
    {
        public ICrypto CreateCrypto();
    }
}