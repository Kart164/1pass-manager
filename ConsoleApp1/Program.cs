using _1Pass.NetStandart.Libs.DBAPI;
using _1Pass.Encryption;
using System;
using System.Text;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var db = new Database("./1Pass.db") { Password="123"};
            db.CreateDatabase();
            Console.WriteLine(db.TryLogin("123").Result);

            //var randompass = PasswordGenerator.Generate(12);
            //Console.WriteLine(randompass);
            //var encrypted = Encrypter.Encrypt("1say_amen111", randompass, "kart@mail.ru").Result;
            //Console.WriteLine(encrypted);
            //var decrypt = Encrypter.Decrypt("1say_amen111", encrypted, "kart@mail.ru").Result;
            //Console.WriteLine(decrypt);
        }
    }
}
