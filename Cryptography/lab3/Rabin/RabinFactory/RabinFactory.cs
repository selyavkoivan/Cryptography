using Cryptography.Crypto;

namespace Cryptography.lab3.Rabin.RabinFactory
{
    public class RabinFactory : IFactory
    {
        public ICrypto CreateCrypto()
        {
            return new Rabin();
        }
    }
}