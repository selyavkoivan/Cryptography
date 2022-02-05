using System;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using Cryptography.Crypto;

namespace Cryptography.lab1
{
    public class RailwayFence : IEncryptor
    {
        private readonly int key;
        private readonly bool isVisible;

        public RailwayFence(bool isVisible = false, int key = 3)
        {
            if (key <= 1) throw new IncorrectValueException();
            this.key = key;
            this.isVisible = isVisible;
        }

        public string Encrypt(string message)
        {
            StringBuilder[] fence = MakeFence(message);
            if (isVisible) Print(fence);
            return MakeString(fence);
        }

        private StringBuilder[] MakeFence(string message)
        {
            StringBuilder[] fence = InitializeFence();
            return FillFence(message, fence);
        }

        private StringBuilder[] FillFence(string message, StringBuilder[] fence)
        {
            bool isRising = true;
            for (int i = 0, j = 0; i < message.Length; i++)
            {
                fence[j].Append(message[i]);
                ControlLoopVariables(ref isRising, ref j);
            }

            return fence;
        }

        private string MakeString(StringBuilder[] fence)
        {
            var encodedMessage = new StringBuilder();
            foreach (var fenceString in fence)
            {
                encodedMessage.Append(fenceString);
            }

            return encodedMessage.ToString();
        }

        public string Decrypt(string message)
        {
            StringBuilder[] fence = MakeDecodeFence(message, isVisible);
            return MakeDecodeString(fence);
        }

        private StringBuilder[] MakeDecodeFence(string message, bool isVisible)
        {
            StringBuilder[] fence = InitializeFence();
            MakeTemplate(message, ref fence, isVisible);
            return FillDecodeFence(message, fence, isVisible);
        }

        private void MakeTemplate(string message, ref StringBuilder[] fence, bool isVisible)
        {
            bool isRising = true;
            for (int i = 0, j = 0; i < message.Length; i++)
            {
                fence[j].Append("x");
                ControlLoopVariables(ref isRising, ref j);
            }

            if (isVisible) Print(fence);
        }

        private StringBuilder[] FillDecodeFence(string message, StringBuilder[] fence, bool isVisible)
        {
            for (int i = 0, k = 0; i < fence.Length; i++)
            {
                for (int j = 0; j < fence[i].Length; j++, k++)
                {
                    fence[i][j] = message[k];
                }
            }

            if (isVisible) Print(fence);
            return fence;
        }

        private string MakeDecodeString(StringBuilder[] fence)
        {
            int length = GetLength(fence);
            StringBuilder message = new StringBuilder();
            int[] indices = new int[key];
            bool isRising = true;
            for (int i = 0, j = 0; j < length; j++)
            {
                message.Append(fence[i][indices[i]]);
                indices[i]++;
                ControlLoopVariables(ref isRising, ref i);
            }

            return message.ToString();
        }

        private int GetLength(StringBuilder[] fence)
        {
            int length = 0;
            foreach (var VARIABLE in fence)
            {
                length += VARIABLE.Length;
            }

            return length;
        }

        private StringBuilder[] InitializeFence()
        {
            return new StringBuilder[key].Select(f => new StringBuilder()).ToArray();
        }

        private void Print(StringBuilder[] fence)
        {
            foreach (var fenceString in fence)
            {
                Console.WriteLine(fenceString);
            }
        }

        private void ControlLoopVariables(ref bool isRising, ref int j)
        {
            isRising = j == 0 ? true :
                j == key - 1 ? false : isRising;
            j = isRising ? ++j : --j;
        }
    }
}