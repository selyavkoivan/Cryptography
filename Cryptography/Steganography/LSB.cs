using System;
using System.Linq;
using System.Text;
using Cryptography.S_DES;

namespace Cryptography.Steganography
{
    public class LSB
    {
        public byte[] Encrypt(string text, byte[] image)
        {
            var bitsBuilder = new StringBuilder();
            Array.ForEach(text.ToCharArray(), letter => bitsBuilder.Append(GetBits(letter)));
            bitsBuilder.Append("0000000000000000");
            var bits = bitsBuilder.ToString();
            for (int i = 1000, j = 0; j < bitsBuilder.Length; i++, j+=2)
            {
                image[i] = ChangeBits(image[i], bits.Substring(j, 2));
            }
            return image;
        }

        private string GetBits(char letter)
        {
            var bitLetterString = Convert.ToString(letter, 2);
            bitLetterString = bitLetterString.PadLeft(16, '0');
            return bitLetterString;
        }

        private byte ChangeBits(byte imageByte, string bits)
        {
            var imageBitsBuilder = new StringBuilder(Convert.ToString(imageByte, 2).PadLeft(8, '0'));
            imageBitsBuilder[6] = bits[0];
            imageBitsBuilder[7] = bits[1];
            return Convert.ToByte(imageBitsBuilder.ToString(), 2);
        }
        
        private string GetBits(byte imageByte)
        {
            var imageBitsBuilder = Convert.ToString(imageByte, 2).PadLeft(8, '0');
            return imageBitsBuilder.Substring(6, 2);
           
        }

        public string Decrypt(byte[] readPicture)
        {
            var message = new StringBuilder();
            for (int i = 1000; i < readPicture.Length; i++)
            {
                message.Append(GetBits(readPicture[i]));
            }
            
            var dectyptMessage = new StringBuilder();
            for (var i = 0; i < message.Length; i += 16)
            {
                dectyptMessage.Append((char)Convert.ToUInt16(message.ToString().Substring(i, 16), 2));
                if(dectyptMessage[^1] == 0) break;
            }

            return dectyptMessage.ToString();
        }
    }
}