using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Eddy
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                PrintHelp();
            }
            else
            {
                ProcessFile(args[0], args[1]);
            }
                      
        }

        static void PrintHelp()
        {
            Console.WriteLine("> Edison");
            Console.WriteLine(">> Automatically extract encrypted strings from AgentTesla samples");
            Console.WriteLine(">> @hariomenkel / https://github.com/hariomenkel/Edison");
            Console.WriteLine("Usage: " + System.AppDomain.CurrentDomain.FriendlyName + " <Input File> <Output File>");
            Console.WriteLine("Example: " + AppDomain.CurrentDomain.FriendlyName + " AgentTesla.exe DecryptedStrings.txt");
        }

        static void ProcessFile(string input, string output)
        {
            byte[] array_Key = new byte[32];
            byte[] array_IV = new byte[16];
            int keyLength = 32;
            int ivLength = 16;

            Assembly a = null;
            try
            {
                a = Assembly.LoadFile(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while loading the file: " + ex.Message);
                return;
            }
            
            Module[] modules = a.GetModules();
            var fields = modules[0].GetFields();
            foreach (var field in fields)
            {
                var objArr = field.GetValue(null);
                var values = (object[])objArr;

                for (int i = 0; i < values.Length; i++)
                {
                    try
                    {
                        uint[] encryptedValue = (uint[])values[i];
                        byte[] arrEncryptedValue = new byte[encryptedValue.Length * 4];
                        Buffer.BlockCopy(encryptedValue, 0, arrEncryptedValue, 0, encryptedValue.Length * 4);
                        byte[] arrPayload = arrEncryptedValue;
                        int offsetKeyAndIV = arrPayload.Length - (keyLength + ivLength);
                        byte[] array_EncryptedValue = new byte[offsetKeyAndIV];
                        Buffer.BlockCopy(arrPayload, 0, array_Key, 0, keyLength);
                        Buffer.BlockCopy(arrPayload, keyLength, array_IV, 0, ivLength);
                        Buffer.BlockCopy(arrPayload, keyLength + ivLength, array_EncryptedValue, 0, offsetKeyAndIV);

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(output, true))
                        {
                            Console.WriteLine(":: Success :: " + Encoding.UTF8.GetString(Decrypt(array_EncryptedValue, array_Key, array_IV)));
                            file.WriteLine(Encoding.UTF8.GetString(Decrypt(array_EncryptedValue, array_Key, array_IV)));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("!! Error !! " + ex.Message);
                    }                    
                }
            }
        }

        static byte[] Decrypt(byte[] A_0, byte[] A_1, byte[] A_2)
        {
            Rijndael rijndael = Rijndael.Create();
            rijndael.Key = A_1;
            rijndael.IV = A_2;
            return rijndael.CreateDecryptor().TransformFinalBlock(A_0, 0, A_0.Length);
        }
    }
}
