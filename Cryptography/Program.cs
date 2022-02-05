using System;
using System.IO;
using System.Text;
using Cryptography.Crypto;
using Cryptography.lab1;
using Cryptography.S_DES;


namespace Cryptography
{
    static class Program
    {
        static Program()
        {
            Console.OutputEncoding = Encoding.UTF8;
        }
        
        static void Main(string[] args)
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("1. Железнодорожная изгородь");
                    Console.WriteLine("2. Ключевая фраза");
                    Console.WriteLine("3. S-DES");
                    Console.WriteLine("9. Выход");
                    var choice = int.Parse(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                        {
                            TestEncryptor(new RailwayFence(true));
                            break;
                        }
                        case 2:
                        {
                            Console.WriteLine("Введите ключевую фразу");
                            TestEncryptor(new KeyPhrase(Console.ReadLine()));
                            break;
                        }
                        case 3:
                        {
                            TestFileEncryptor(new Esdeath("1001010011"));
                            break;
                        }
                        case 9: return;
                    }
                }
            }
            catch (IncorrectValueException e)
            {
                ExceptionHandling(e);
            }
        }

        private static void ExceptionHandling(IncorrectValueException e)
        {
            Console.WriteLine("Словлена исключительная ситуация");
        }

        private static void TestEncryptor(IEncryptor encryptor)
        {
            Console.Write("Введите строку : ");
            var str = Console.ReadLine();

            if (str == string.Empty) throw new IncorrectValueException();

            var message = encryptor.Encrypt(str);
            Console.WriteLine($"Защифрованная строка : {message}");

            Console.WriteLine($"Расшифрованная строка : {encryptor.Decrypt(message)}");
            Console.ReadKey();
        }

        private static void TestFileEncryptor(IEncryptor encryptor)
        {
            var message = encryptor.Encrypt(ReadFile("X:\\bsuir\\сем 6\\КИОКИ\\Cryptography\\source.txt"));
            Console.WriteLine($"Защифрованная строка :\n{message}");
            WriteFile(message, "X:\\bsuir\\сем 6\\КИОКИ\\Cryptography\\result.txt");
            var decryptMessage = encryptor.Decrypt(ReadFile("X:\\bsuir\\сем 6\\КИОКИ\\Cryptography\\result.txt"));
            Console.WriteLine($"Расшифрованная строка :\n{decryptMessage}");
            WriteFile(decryptMessage, "X:\\bsuir\\сем 6\\КИОКИ\\Cryptography\\decryptResult.txt");
            Console.ReadKey();
        }

        private static string ReadFile(string fileName)
        {
            var file = new StreamReader(new FileStream(fileName, FileMode.Open), Encoding.UTF8);
            var fileData = file.ReadToEnd();
            file.Close();
            return fileData;
        }

        private static void WriteFile(string message, string fileName)
        {
            var file = new StreamWriter(new FileStream(fileName, FileMode.Create), Encoding.UTF8);
            file.Write(message);
            file.Close();
        }
    }
}