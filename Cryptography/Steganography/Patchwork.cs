using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using Cryptography.S_DES;

namespace Cryptography.Steganography
{
    public class Patchwork
    {

        private const int N = 20000;
        private const byte Q = 4;
        
       
        public void SetWaterMark(byte[] sound, int key)
        {
            var rnd = new Random(key);
            for (var i = 0; i < N; i++)
            {
                var a = rnd.Next(100, sound.Length - 1);
                var b = rnd.Next(100, sound.Length - 1);

                sound[a] = (byte)((sound[a] + Q) > 255 ? 255 : sound[a] + Q);
                sound[b] = (byte)((sound[b] - Q) < 0 ? 0 : sound[b] - Q);
            }
        }


        public bool CheckPictureHasWaterMark(byte[] originalBytes, byte[] bytesWithWaterMark, int key)
        {
            Console.WriteLine(GetSumOfDifferencesBytes(originalBytes, key));
            Console.WriteLine(GetSumOfDifferencesBytes(bytesWithWaterMark, key));
            return (2 * N * Q - 30000) <= GetSumOfDifferencesBytes(bytesWithWaterMark, key) -  GetSumOfDifferencesBytes(originalBytes, key) ;
        }

        private BigInteger GetSumOfDifferencesBytes(byte[] bytes, int key)
        {
            BigInteger sumOfDifferences = 0;
            var rnd = new Random(key);
            for (int i = 0; i < N; i++)
            {
                var a =rnd.Next(100, bytes.Length - 1);
                var b =rnd.Next(100, bytes.Length - 1);
                sumOfDifferences += (bytes[a] - bytes[b]);
            }
            return sumOfDifferences;
        }

      
    }
}