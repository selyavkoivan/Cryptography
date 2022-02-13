using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Cryptography.Crypto;
using Cryptography.lab3.Generators;

namespace Cryptography.lab3.Rabin
{
    public class Rabin : ICrypto
    {
        private readonly BigInteger p;
        private readonly BigInteger q;

        private readonly BigInteger n;

        public Rabin()
        {
            p = PrimeGenerator.GeneratePrimeBigInteger();
            q = PrimeGenerator.GeneratePrimeBigInteger(p);
            n = p * q;
        }

        public string Encrypt(string message)
        {
            var bytes = message.Select(letter => GetShort(letter)).ToArray();
            var newValue = bytes.Select(b => (uint)(b * b % n)).ToArray();
            var mes = new StringBuilder();
            var m = newValue.Select(value => GetLetter(value)).ToArray();
            Array.ForEach(m, mm => mes.Append(mm));
            return mes.ToString();
        }

        private string GetLetter(uint value)
        {
            var message = Convert.ToString(value, 2).PadLeft(32, '0');
            var letters = new char[2];
            letters[0] = (char)Convert.ToInt32(message.Substring(0, 16), 2);
            letters[1] = (char)Convert.ToInt32(message.Substring(16), 2);
            return new string(letters);
        }


        private ushort GetShort(char letter)
        {
            var bitLetterString = Convert.ToString(letter, 2);
            bitLetterString = bitLetterString.PadLeft(16, '0');
            return Convert.ToUInt16(bitLetterString, 2);
        }

        private uint GetInt(char firstLetter, char secondLetter)
        {
            var bitLetterString = Convert.ToString(firstLetter, 2);
            bitLetterString = bitLetterString.PadLeft(16, '0');
            var secondBitLetterString = Convert.ToString(secondLetter, 2);
            secondBitLetterString = secondBitLetterString.PadLeft(16, '0');
            bitLetterString += secondBitLetterString;
            return Convert.ToUInt32(bitLetterString, 2);
        }

        public string Decrypt(string message)
        {
            var bytes = new List<uint>();
            for (var i = 0; i < message.Length; i += 2)
            {
                bytes.Add(GetInt(message[i], message[i + 1]));
            }

            var mp = bytes.Select(b => Power(b, (p + 1) / 4, p)).ToArray();
            var mq = bytes.Select(b => Power(b, (q + 1) / 4, q)).ToArray();
            BigInteger x = 0, y = 0;
            gcd(p, q, ref x, ref y);
            var decryptMessage = new StringBuilder();
            for (int i = 0; i < mp.Length; i++)
            {
                decryptMessage.Append(EachLetter(x, y, mp[i], mq[i]));
            }

            return decryptMessage.ToString();
        }


        //евклид
        void gcd(BigInteger a, BigInteger b, ref BigInteger x, ref BigInteger y)
        {
            var d0 = a;
            var d1 = b;
            BigInteger x0 = 1, x1 = 0, y0 = 0, y1 = 1;
            while (d1 > 1)
            {
                var q = d0 / d1;
                var d2 = d0 % d1;
                var x2 = x0 - q * x1;
                var y2 = y0 - q * y1;
                d0 = d1;
                d1 = d2;
                x0 = x1;
                x1 = x2;
                y0 = y1;
                y1 = y2;
            }

            x = x1;
            y = y1;
        }

        public BigInteger Power(BigInteger a, BigInteger z, BigInteger n)
        {
            BigInteger a1 = a;
            var z1 = z;
            BigInteger x = 1;
            while (z1 != 0)
            {
                while (z1 % 2 == 0)
                {
                    z1 = z1 / 2;
                    a1 = (a1 * a1) % n;
                }

                z1 = z1 - 1;
                x = (x * a1) % n;
            }

            return x;
        }

        private char EachLetter(BigInteger yp, BigInteger yq, BigInteger mp, BigInteger mq)
        {
            var ms = new BigInteger[4];
            ms[0] = (yp * p * mq + yq * q * mp) % n;
            ms[1] = n - ms[0];
            ms[2] = (yp * p * mq - yq * q * mp) % n;
            ms[3] = n - ms[2];
            char letter = default;
            foreach (var m in ms)
            {
                try
                {
                    letter = (char)m;
                }
                catch (OverflowException)
                {
                }
            }

            return letter;
        }
    }
}