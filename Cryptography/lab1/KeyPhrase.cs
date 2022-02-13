using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cryptography.Crypto;

namespace Cryptography.lab1
{
    public class KeyPhrase : ICrypto
    {
        private const string alphabet = "abcdefghijklmnopqrstuvwxyzабвгдеёжзийклмнопрстуфхцчшщъыьэюя ";
        private readonly string keyword;

        delegate void AssignAValue(ref char[] charArray, ref StringBuilder stringBuilder,
            Dictionary<int, char> keywordIndices, int i);

        public KeyPhrase(string keyword)
        {
            if (keyword == string.Empty) throw new IncorrectValueException();
            this.keyword = keyword;
        }

        public string Encrypt(string message)
        {
            var splitMessage = SplitPhraseByKeywordLength(message);
            for (var i = 0; i < splitMessage.Length; i++)
            {
                splitMessage[i] = GetChangedPartOfPhrase(splitMessage[i], EncryptAssign);
            }

            return Join(splitMessage);
        }

        public string Decrypt(string message)
        {
            var splitMessage = SplitPhraseByKeywordLength(message);
            for (var i = 0; i < splitMessage.Length; i++)
            {
                splitMessage[i] = GetChangedPartOfPhrase(splitMessage[i], DecryptAssign);
            }

            return Join(splitMessage);
        }

        private StringBuilder GetChangedPartOfPhrase(StringBuilder partOfPhrase, AssignAValue assignAValue)
        {
            var encryptedPartOfPhrase = new char[partOfPhrase.Length];
            var keywordIndices = GetKeywordIndicesAlphabetically(partOfPhrase);
            for (var i = 0; i < partOfPhrase.Length; i++)
            {
                assignAValue(ref encryptedPartOfPhrase, ref partOfPhrase, keywordIndices, i);
            }

            return new StringBuilder(new string(encryptedPartOfPhrase));
        }

        private StringBuilder[] SplitPhraseByKeywordLength(string phrase)
        {
            var splitPhrase = Initialize(phrase);
            try
            {
                for (var i = 0; i < splitPhrase.Length; i++)
                {
                    for (var j = 0; j < keyword.Length; j++)
                    {
                        splitPhrase[i].Append(phrase[i * keyword.Length + j]);
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
            }

            return splitPhrase;
        }

        private int GetKeyIndex(int i, Dictionary<int, char> dictionary)
        {
            return dictionary.ToList().IndexOf(dictionary.First(k => k.Key == i));
        }

        private Dictionary<int, char> GetKeywordIndicesAlphabetically(StringBuilder partOfPhrase)
        {
            return GetStringIndicesAlphabetically(keyword.Substring(0, partOfPhrase.Length));
        }

        private Dictionary<int, char> GetStringIndicesAlphabetically(string phrasePart)
        {
            var indices = new Dictionary<int, char>();
            var orderedPhrasePart = new StringBuilder(string.Join(string.Empty, phrasePart.OrderBy(char.ToLower)));
            foreach (var phraseLetter in phrasePart)
            {
                var key = orderedPhrasePart.ToString().IndexOf(phraseLetter);
                indices.Add(key, phraseLetter);
                orderedPhrasePart[key] = default;
            }

            return indices;
        }

        private StringBuilder[] Initialize(string phrase)
        {
            return new StringBuilder[(int)Math.Ceiling((double)phrase.Length / keyword.Length)]
                .Select(s => new StringBuilder()).ToArray();
        }

        private string Join(StringBuilder[] splitMessage)
        {
            return splitMessage.Aggregate((current, partOfSplitMessage) => current.Append(partOfSplitMessage))
                .ToString();
        }

        private void DecryptAssign(ref char[] charArray, ref StringBuilder stringBuilder,
            Dictionary<int, char> keywordIndices, int i)
        {
            charArray[GetKeyIndex(i, keywordIndices)] = stringBuilder[i];
        }

        private void EncryptAssign(ref char[] charArray, ref StringBuilder stringBuilder,
            Dictionary<int, char> keywordIndices, int i)
        {
            charArray[i] = stringBuilder[GetKeyIndex(i, keywordIndices)];
        }
    }
}