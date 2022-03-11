using System;
using System.Numerics;
using Cryptography.Hash;
using Cryptography.lab3.Generators;
using Cryptography.lab3.Rabin;

namespace Cryptography.DigitalSignature
{
    public class RSADigitalSignature : Rabin
    {
        private readonly BigInteger p;
        private readonly BigInteger q;
        public readonly BigInteger r;
        private readonly BigInteger phi;
        public readonly BigInteger e;
        private readonly BigInteger d;

        public RSADigitalSignature()
        {
            do
            {
                p = PrimeGenerator.GeneratePrimeBigInteger();
                q = PrimeGenerator.GeneratePrimeBigInteger(p);
                r = p * q;
                phi = (p - 1) * (q - 1);
                e = CalculateE(phi);
                d = CalculateD(e, phi);
            } while (d <= 0);

        }

        private BigInteger CalculateE(BigInteger phi)
        {
            var bigInteger = new BigInteger();
            var e = new BigInteger();
            do
            {
                e = PrimeGenerator.GenerateBigInteger();
            } while (e >= phi || e <= 0 || EuclidSAlgorithm(e, phi, ref bigInteger, ref bigInteger) != 1);

            return e;
        }

        private BigInteger CalculateD(BigInteger e, BigInteger phi)
        {
            var d = new BigInteger();
            var x = new BigInteger();

            EuclidSAlgorithm(phi, e, ref x, ref d);

            return d;
        }


        public BigInteger GetDigitalSignature(string text)
        {
            return Power(new Hashpjw().GetUHashCode(text), d, r);
        }
        
        public bool CheckDigitalSignature(string text, BigInteger digitalSignature)
        {
            var hash = new Hashpjw().GetUHashCode(text);
            var signature = Power(digitalSignature, e, r);
            return hash == signature;
        }
    }
}