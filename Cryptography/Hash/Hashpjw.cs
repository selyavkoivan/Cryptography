using System;

namespace Cryptography.Hash
{
    public class Hashpjw : IHash
    {
        public uint GetUHashCode(string input)
        {
            uint hash = 0;
            foreach (var item in input)
            {
                var byte_of_data = (byte)item;
                hash = (hash << 4) + byte_of_data;
                var h1 = hash & 0xf0000000;
                if (h1 != 0)
                {
                    hash = ((hash ^ (h1 >> 24)) & (0xfffffff));
                }
            }
            return hash;
        }
        
        public static uint GetUHashCode(byte[] input)
        {
            uint hash = 0;
            foreach (var item in input)
            {
                var byte_of_data = item;
                hash = (hash << 4) + byte_of_data;
                var h1 = hash & 0xf0000000;
                if (h1 != 0)
                {
                    hash = ((hash ^ (h1 >> 24)) & (0xfffffff));
                }
            }
            return hash;
        }
    }
}