using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading;

namespace Cryptography.lab3.Generators
{
    public static class PrimeGenerator
    {
        // Random generator (thread safe)
        private static ThreadLocal<Random> s_Gen = new ThreadLocal<Random>(
            () => { return new Random(); }
        );

        // Random generator (thread safe)
        private static Random Gen => s_Gen.Value;

        private static bool IsProbablyPrime(this BigInteger n, int k = 10)
        {
            // если n == 2 или n == 3 - эти числа простые, возвращаем true
            if (n == 2 || n == 3)
                return true;

            // если n < 2 или n четное - возвращаем false
            if (n < 2 || n % 2 == 0)
                return false;

            // представим n − 1 в виде (2^s)·t, где t нечётно, это можно сделать последовательным делением n - 1 на 2
            BigInteger t = n - 1;

            int s = 0;

            while (t % 2 == 0)
            {
                t /= 2;
                s += 1;
            }

            // повторить k раз
            for (int i = 0; i < k; i++)
            {
                // выберем случайное целое число a в отрезке [2, n − 2]
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                byte[] _a = new byte[n.ToByteArray().LongLength];

                BigInteger a;

                do
                {
                    rng.GetBytes(_a);
                    a = new BigInteger(_a);
                } while (a < 2 || a >= n - 2);

                // x ← a^t mod n, вычислим с помощью возведения в степень по модулю
                BigInteger x = BigInteger.ModPow(a, t, n);

                // если x == 1 или x == n − 1, то перейти на следующую итерацию цикла
                if (x == 1 || x == n - 1)
                    continue;

                // повторить s − 1 раз
                for (int r = 1; r < s; r++)
                {
                    // x ← x^2 mod n
                    x = BigInteger.ModPow(x, 2, n);

                    // если x == 1, то вернуть "составное"
                    if (x == 1)
                        return false;

                    // если x == n − 1, то перейти на следующую итерацию внешнего цикла
                    if (x == n - 1)
                        break;
                }

                if (x != n - 1)
                    return false;
            }

            // вернуть "вероятно простое"
            return true;
        }

        public static BigInteger GeneratePrimeModBigInteger(BigInteger theOtherBigInteger = default)
        {
            BigInteger bigNumber;
            do
            {
                var rng = new RNGCryptoServiceProvider();
                var bytes = new byte[256 / 8];
                rng.GetBytes(bytes);
                bigNumber = new BigInteger(bytes);
            } while (!bigNumber.IsProbablyPrime() || bigNumber % 4 != 3 || bigNumber == theOtherBigInteger);

            return bigNumber;
        }

        public static BigInteger GeneratePrimeBigInteger(BigInteger theOtherBigInteger = default)
        {
            BigInteger bigNumber;
            do
            {
                var rng = new RNGCryptoServiceProvider();
                var bytes = new byte[256 / 8];
                rng.GetBytes(bytes);
                bigNumber = new BigInteger(bytes);
            } while (!bigNumber.IsProbablyPrime() || bigNumber == theOtherBigInteger);

            return bigNumber;
        }

        public static BigInteger GenerateBigInteger()
        {
            var rng = new RNGCryptoServiceProvider();
            var bytes = new byte[256 / 8];
            rng.GetBytes(bytes);
            return new BigInteger(bytes);
        }
    }
}