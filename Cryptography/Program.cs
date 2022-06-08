using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Mime;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using Cryptography.Crypto;
using Cryptography.DigitalSignature;
using Cryptography.Hash;
using Cryptography.lab1;
using Cryptography.lab3;
using Cryptography.lab3.Generators;
using Cryptography.lab3.Rabin;
using Cryptography.lab3.Rabin.RabinFactory;
using Cryptography.S_DES;
using Cryptography.Steganography;


namespace Cryptography
{
    static class Program
    {
        delegate ICrypto CryptoFactory();

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
                    Console.WriteLine("4. Алгоритм Рабина");
                    Console.WriteLine("5. PJW-32");
                    Console.WriteLine("6. RSA Digital Signature");
                    Console.WriteLine("7. LSB");
                    Console.WriteLine("8. Patchwork");
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
                            TestFileUnstableEncryptor(new RabinFactory());
                            break;
                        }
                        case 5:
                        {
                            TestFileHashCode(new Hashpjw());
                            break;
                        }
                        case 6:
                        {
                            TestFileDigitalSignature(new RSADigitalSignature());
                            break;
                        }
                        case 7:
                        {
                            var crypto = new LSB();
                            var bytes = crypto.Encrypt(ReadFile("X:\\bsuir\\сем 6\\КИОКИ\\Cryptography\\source.txt"),
                                ReadPicture("X:\\bsuir\\сем 6\\КИОКИ\\Cryptography\\изображение124.tif"));
                            WritePicture(bytes, "X:\\bsuir\\сем 6\\КИОКИ\\Cryptography\\изображениеLSB.tif");
                            Console.WriteLine("Текст из картинки :\n" +
                                crypto.Decrypt(ReadPicture("X:\\bsuir\\сем 6\\КИОКИ\\Cryptography\\изображениеLSB.tif")));
                            break;
                        }
                        case 8:
                        {
                            // звук.wav
                            var crypto = new Patchwork();
                            
                            var bytes = ReadPicture("X:\\bsuir\\сем 6\\КИОКИ\\Cryptography\\звук.wav");
                            var originalBytes = ReadPicture("X:\\bsuir\\сем 6\\КИОКИ\\Cryptography\\звук.wav");
                      
                            crypto.SetWaterMark(bytes, 12);


                            Console.WriteLine(crypto.CheckPictureHasWaterMark(originalBytes, bytes, 12));
                            WritePicture(bytes, "X:\\bsuir\\сем 6\\КИОКИ\\Cryptography\\patchwork.wav");
                          
                            Console.WriteLine(crypto.CheckPictureHasWaterMark(originalBytes, originalBytes, 12));
                            break;
                    }
                        case 9: return;
                    }

                    Console.ReadKey();
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

            var message = crypto.Encrypt(str);
            Console.WriteLine($"Защифрованная строка : {message}");
            Console.WriteLine($"Расшифрованная строка : {crypto.Decrypt(message)}");
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
                if (decryptMessage != str) crypto = factory.CreateCrypto();
                else break;
            }

            Console.WriteLine($"Расшифрованная строка : {decryptMessage}");
        }

        private static void TestFileUnstableEncryptor(IFactory factory)
        {
            var crypto = factory.CreateCrypto();
            var str = ReadFile("X:\\bsuir\\сем 6\\КИОКИ\\Cryptography\\source.txt");
            if (str == string.Empty) throw new IncorrectValueException();
            var message = crypto.Encrypt(str);
            Console.WriteLine($"Защифрованная строка : {message}");
            WriteFile(message, "X:\\bsuir\\сем 6\\КИОКИ\\Cryptography\\result.txt");
            var decryptMessage = string.Empty;
            char letter = default;
            byte counter = 0;
            while (true)
            {
                counter++;
                decryptMessage = crypto.Decrypt(message);
                //Console.WriteLine($"Попытка расшифровки : {decryptMessage}");
                if (decryptMessage != str) crypto = factory.CreateCrypto();
                else break;
            }

            Console.WriteLine($"Расшифрованная строка :\n{decryptMessage}");
            Console.WriteLine(counter);
            WriteFile(decryptMessage, "X:\\bsuir\\сем 6\\КИОКИ\\Cryptography\\decryptResult.txt");
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

        private static void TestFileHashCode(IHash cryptoHash)
        {
            var text = ReadFile("X:\\bsuir\\сем 6\\КИОКИ\\Cryptography\\source.txt");
            Console.WriteLine($"Исходный текст :\n{text}");
            Console.WriteLine($"Хэш код :\n0x{cryptoHash.GetUHashCode(text):x}");
            
            Console.WriteLine($"Исходный текст :\nBSUIR");
            Console.WriteLine($"Хэш код :\n0x{cryptoHash.GetUHashCode("BSUIR"):x}");
        }

        private static void TestFileDigitalSignature(RSADigitalSignature digitalSignature)
        {
            var text = /*ReadFile("X:\\bsuir\\сем 6\\КИОКИ\\Cryptography\\source.txt")*/ "bsuirr";
            Console.WriteLine($"Исходный текст :\n{text}");
            var signature = digitalSignature.GetDigitalSignature(text);
            Console.WriteLine($"Цифровая подпись :\n{signature}\n****** ПРОВЕРКА");
            var checkText = "BSUIR";    
            
            Console.WriteLine($"Текст для проверки #1 :\n{checkText}");
            Console.WriteLine($"Цифровая подпись верна : {digitalSignature.CheckDigitalSignature(checkText, signature)}");
            
            Console.WriteLine($"Текст для проверки #2 :\n{text}");
            Console.WriteLine($"Цифровая подпись верна : {digitalSignature.CheckDigitalSignature(text, signature)}");
           
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

        private static byte[] ReadPicture(string filePath)
        {
            return File.ReadAllBytes(filePath);
        }

        private static void WritePicture(byte[] imageBytes, string filePath)
        {
            File.WriteAllBytes(filePath, imageBytes);
        }
    }
}