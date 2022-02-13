using System;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using Cryptography.Crypto;
using Cryptography.lab1;
using Cryptography.lab3;
using Cryptography.lab3.Generators;
using Cryptography.lab3.Rabin;
using Cryptography.lab3.Rabin.RabinFactory;
using Cryptography.S_DES;


namespace Cryptography
{
    static class Program
    {
        delegate ICrypto CryptoFactory();
        
        static Program()
        {
            Console.WriteLine((char)1105);
            Console.OutputEncoding = Encoding.UTF8;
        }
        
        static void Main(string[] args)
        {
            var p = PrimeGenerator.GeneratePrimeBigInteger();
            var q = PrimeGenerator.GeneratePrimeBigInteger(p);
            try
            {
                while (true)
                {
                    Console.WriteLine("1. Железнодорожная изгородь");
                    Console.WriteLine("2. Ключевая фраза");
                    Console.WriteLine("3. S-DES");
                    Console.WriteLine("4. Алгоритм Рабина");
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
                        case 4:
                        {
                            TestUnstableEncryptor(new RabinFactory());
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

        private static void TestEncryptor(ICrypto crypto)
        {
            Console.Write("Введите строку : ");
            var str = Console.ReadLine();

            if (str == string.Empty) throw new IncorrectValueException();
            while (true)
            {
                crypto = new Rabin();
                var message = crypto.Encrypt(str);
                Console.WriteLine($"Защифрованная строка : {message}");

                Console.WriteLine($"Расшифрованная строка : {crypto.Decrypt(message)}");
                Console.ReadKey();     
            }

           
        }
        
        private static void TestUnstableEncryptor(IFactory factory)
        {
            var crypto = factory.CreateCrypto();
            Console.Write("Введите строку : ");
            var str = Console.ReadLine();
            if (str == string.Empty) throw new IncorrectValueException();
            var message = crypto.Encrypt(str);
            Console.WriteLine($"Защифрованная строка : {message}");

            var decryptMessage = string.Empty;
            char letter = default;
            while (true)
            {
                decryptMessage = crypto.Decrypt(message);
                Console.WriteLine($"Попытка расшифровки : {decryptMessage}");
                if(decryptMessage != str) crypto = factory.CreateCrypto();
                else break;
            }
            
            Console.WriteLine($"Расшифрованная строка : {decryptMessage}");
            Console.ReadKey();     
            }

           
        

        private static void TestFileEncryptor(ICrypto crypto)
        {
            var message = crypto.Encrypt(ReadFile("X:\\bsuir\\сем 6\\КИОКИ\\Cryptography\\source.txt"));
            Console.WriteLine($"Защифрованная строка :\n{message}");
            WriteFile(message, "X:\\bsuir\\сем 6\\КИОКИ\\Cryptography\\result.txt");
            var decryptMessage = crypto.Decrypt(ReadFile("X:\\bsuir\\сем 6\\КИОКИ\\Cryptography\\result.txt"));
            Console.WriteLine($"Расшифрованная строка :\n{decryptMessage}");
            WriteFile(decryptMessage, "X:\\bsuir\\сем 6\\КИОКИ\\Cryptography\\decryptResult.txt");
        }

        private static string ReadFile(string fileName)
        {
            var file = new StreamReader(new FileStream(fileName, FileMode.Open));
            var fileData = file.ReadToEnd();
            file.Close();
            return fileData;
        }

        private static void WriteFile(string message, string fileName)
        {
            var file = new StreamWriter(new FileStream(fileName, FileMode.Create));
            file.Write(message);
            file.Close();
        }
    }
}