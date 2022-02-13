using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;   //1111110000100000
using System.Threading;
using Cryptography.Crypto;


namespace Cryptography.S_DES
{
    public class Esdeath : ICrypto
    {
        private static readonly int[] P10 = { 3, 5, 2, 7, 4, 10, 1, 9, 8, 6 };
        private static readonly int[] P8 = { 6, 3, 7, 4, 8, 5, 10, 9 };
        private static readonly int[] P4 = { 2, 4, 3, 1 };
        private static readonly int[] IP = { 2, 6, 3, 1, 4, 8, 5, 7 };
        private static readonly int[] IP_1 = { 4, 1, 3, 5, 7, 2, 8, 6 };
        private static readonly int[] EP = { 4, 1, 2, 3, 2, 3, 4, 1 };
        private static readonly int[,] SBlock1 =
        {
            { 1, 0, 3, 2 },
            { 3, 2, 1, 0 },
            { 0, 2, 1, 3 },
            { 3, 1, 3, 2 }
        };
        private static readonly int[,] SBlock2 =
        {
            { 0, 1, 2, 3 },
            { 2, 0, 1, 3 },
            { 3, 0, 1, 0 },
            { 2, 1, 0, 3 }
        };

        private readonly bool[] key;
        private readonly bool[] k1;
        private readonly bool[] k2;

        public Esdeath(string key)
        {
            this.key = ConvertToBools(key, 10);
            k1 = GenerateK1();
            k2 = GenerateK2();
        }

        public string Encrypt(string message)
        {
            var encryptMessageBools = message.Select(letter => Encrypt(letter)).ToList();
            return ConvertToString(encryptMessageBools);
        }

        public string Decrypt(string message)
        {
            var decryptMessageBools = message.Select(letter => Decrypt(letter)).ToList();
            return ConvertToString(decryptMessageBools);
        }

        private bool[] Encrypt(char letter)
        {
            var bitLetter = GetBits(letter);
            var encryptBits = new List<bool>();
            foreach (var bitPartOFLetter in bitLetter)
            {
                var IP = GenerateIP(bitPartOFLetter);
                var firstRound = Round(IP, k1);
                var splitFirstRound = SplitBits(firstRound);
                var swapFirstRound = ConnectBools(splitFirstRound[1], splitFirstRound[0]);
                var secondRound = Round(swapFirstRound, k2);
                var IP_1 = GenerateIP_1(secondRound);
                encryptBits.AddRange(IP_1);
            }
            return encryptBits.ToArray();
        }

        private bool[] Decrypt(char letter)
        {
            var bitLetter = GetBits(letter);
            var decryptBits = new List<bool>();
            foreach (var bitPartOFLetter in bitLetter)
            {
                var IP = GenerateIP(bitPartOFLetter);
                var firstRound = Round(IP, k2);
                var splitFirstRound = SplitBits(firstRound);
                var swapFirstRound = ConnectBools(splitFirstRound[1], splitFirstRound[0]);
                var secondRound = Round(swapFirstRound, k1);
                var IP_1 = GenerateIP_1(secondRound);
                decryptBits.AddRange(IP_1);
            }

            return decryptBits.ToArray();
        }

        private bool[] Round(bool[] IP, bool[] key)
        {
            var splitIP = SplitBits(IP);
            var EP = GenerateEP(splitIP[1]);
            var XOREP = XOR(EP, key);
            var SBlock = GenerateSBlock(XOREP);
            var P4 = GenerateP4(SBlock);
            var XORP4 = XOR(P4, splitIP[0]);
            return ConnectBools(XORP4, splitIP[1]);
        }

        private bool[] GenerateSBlock(bool[] XOREP)
        {
            var splitXOREP = SplitBits(XOREP);
            var leftSBlock = GenerateLeftSBlock(splitXOREP[0]);
            var rightSBlock = GenerateRightSBlock(splitXOREP[1]);
            return ConnectBools(leftSBlock, rightSBlock);
        }

        private string ConvertToString(List<bool[]> encryptMessageBools)
        {
            var encryptMessage = new StringBuilder();
            foreach (var encryptMessageBoolLetter in encryptMessageBools)
            {
                encryptMessage.Append(ConvertToChar(encryptMessageBoolLetter));
            }

            return encryptMessage.ToString();
        }

        private char ConvertToChar(bool[] encryptMessageBoolLetter)
        {
            var stringEncryptMessageBoolLetter = ConvertToStringBits(encryptMessageBoolLetter);
            return (char)Convert.ToInt32(stringEncryptMessageBoolLetter, 2);
        }

        private string ConvertToStringBits(bool[] encryptMessageBoolLetter)
        {
            var stringEncryptMessageBoolLetter = new StringBuilder();
            foreach (var letterBit in encryptMessageBoolLetter)
            {
                stringEncryptMessageBoolLetter.Append(letterBit ? '1' : '0');
            }

            return stringEncryptMessageBoolLetter.ToString();
        }

        private bool[][] GetBits(char letter)
        {
            var bitLetter = new bool[2][];
            var bitLetterString = Convert.ToString(letter, 2);
            bitLetterString = bitLetterString.PadLeft(16, '0');
            bitLetter[0] = ConvertToBools(bitLetterString.Substring(0, 8));
            bitLetter[1] = ConvertToBools(bitLetterString.Substring(8));
            return bitLetter;
        }

        private bool[] GenerateIP(bool[] bitPartOFLetter)
        {
            var IP = new bool[8];
            for (var i = 0; i < 8; i++)
            {
                IP[i] = bitPartOFLetter[Esdeath.IP[i] - 1];
            }

            return IP;
        }

        private bool[] GenerateEP(bool[] splitIPRight)
        {
            var EP = new bool[8];
            for (var i = 0; i < 8; i++)
            {
                EP[i] = splitIPRight[Esdeath.EP[i] - 1];
            }

            return EP;
        }

        private bool[] GenerateLeftSBlock(bool[] leftSplitXOREP)
        {
            var firstBit = (byte)SBlock1[GenerateDemicalNumber(leftSplitXOREP[0], leftSplitXOREP[3]),
                GenerateDemicalNumber(leftSplitXOREP[1], leftSplitXOREP[2])];
            var stringBinaryFirstBit = Convert.ToString(firstBit, 2);
            return ConvertToBools(stringBinaryFirstBit.PadLeft(2, '0'), 2);
        }

        private bool[] GenerateRightSBlock(bool[] rightSplitXOREP)
        {
            var secondBit = (byte)SBlock2[GenerateDemicalNumber(rightSplitXOREP[0], rightSplitXOREP[3]),
                GenerateDemicalNumber(rightSplitXOREP[1], rightSplitXOREP[2])];
            var stringBinarySecondBit = Convert.ToString(secondBit, 2);
            return ConvertToBools(stringBinarySecondBit.PadLeft(2, '0'), 2);
        }

        private bool[] GenerateP4(bool[] SBlock)
        {
            var P4 = new bool[4];
            for (var i = 0; i < 4; i++)
            {
                P4[i] = SBlock[Esdeath.P4[i] - 1];
            }

            return P4;
        }

        private bool[] GenerateIP_1(bool[] secondRound)
        {
            var IP_1 = new bool[8];
            for (var i = 0; i < 8; i++)
            {
                IP_1[i] = secondRound[Esdeath.IP_1[i] - 1];
            }

            return IP_1;
        }

        private bool[] GenerateK1()
        {
            var P10 = GenerateP10();
            var toLeft = ToLeft(P10, 1);
            return GenerateP8(toLeft);
        }

        private bool[] GenerateP10()
        {
            var P10 = new bool[10];
            for (int i = 0; i < 10; i++)
            {
                P10[i] = key[Esdeath.P10[i] - 1];
            }

            return P10;
        }

        private bool[] GenerateK2()
        {
            var P10 = GenerateP10();
            var toLeft = ToLeft(P10, 3);
            return GenerateP8(toLeft);
        }

        private bool[] GenerateP8(bool[] toLeft)
        {
            var P8 = new bool[8];
            for (var i = 0; i < 8; i++)
            {
                P8[i] = toLeft[Esdeath.P8[i] - 1];
            }

            return P8;
        }

        private bool[] ConnectBools(params bool[][] boolsMatrix)
        {
            var connectBools = new List<bool>();
            foreach (var bools in boolsMatrix)
            {
                connectBools.AddRange(bools);
            }

            return connectBools.ToArray();
        }

        private bool[][] SplitBits(bool[] bits)
        {
            var splitBits = new bool[2][];
            for (var i = 0; i < splitBits.Length; i++)
            {
                splitBits[i] = new bool[bits.Length / 2];
                for (var j = 0; j < splitBits[i].Length; j++)
                {
                    splitBits[i][j] = bits[i * (bits.Length / 2) + j];
                }
            }

            return splitBits;
        }

        private bool[] ConvertToBools(string binaryString, int size = 8)
        {
            if (binaryString.Length != size) throw new IncorrectValueException();
            var convertedKey = new bool[size];
            for (var i = 0; i < size; i++)
            {
                convertedKey[i] = binaryString[i] == '0' ? false :
                    binaryString[i] == '1' ? true : throw new IncorrectValueException();
            }

            return convertedKey;
        }

        private bool[] XOR(bool[] firstValue, bool[] secondValue)
        {
            var resultValue = new bool[firstValue.Length];
            for (var i = 0; i < resultValue.Length; i++)
            {
                resultValue[i] = firstValue[i] ^ secondValue[i];
            }

            return resultValue;
        }

        private bool[] ToLeft(bool[] bits, int step)
        {
            var toLeft = new bool[10];
            for (var i = 0; i < 5; i++)
            {
                toLeft[i] = bits[i + step < 5 ? i + step : i + step - 5];
            }

            for (var i = 5; i < 10; i++)
            {
                toLeft[i] = bits[i + step < 10 ? i + step : i + step - 5];
            }

            return toLeft;
        }

        private byte GenerateDemicalNumber(bool firstBit, bool secondBit)
        {
            var stringDemicalNumber = Convert.ToByte(firstBit).ToString() + Convert.ToByte(secondBit);
            return Convert.ToByte(stringDemicalNumber, 2);
        }
    }
}