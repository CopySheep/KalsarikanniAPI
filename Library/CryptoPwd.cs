using System.Security.Cryptography;
using System.Text;

namespace HotelFuen31.APIs.Library
{
    public class CryptoPwd
    {
        public string NewConfirmCode()
        {
            Random rand = new Random();

            string ConfirmCode = "";

            for(int i = 0; i < 4; i++)
            {
                ConfirmCode += rand.Next(0, 10).ToString();
            }

            return ConfirmCode;
        }

        //產生一組由16個字組成的金鑰
        public string NewKey()
        {
            byte[] data = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(data);
            }

            //將陣列轉成16進位的字串
            string NewKey = BitConverter.ToString(data).Replace("-", "");

            return NewKey;
        }

        //產生一組由8個字組成的鹽
        public string NewSalt()
        {
            byte[] data = new byte[4];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(data);
            }

            //將陣列轉成16進位的字串
            string NewKey = BitConverter.ToString(data).Replace("-", "");

            return NewKey;
        }

        //密碼加鹽再雜湊
        public string CryptoPWD(string pwd, string salt)
        {
            byte[] PWDAddSalt = PasswordAddSalt(pwd, salt);
            string PWDToHash = HashPWD(PWDAddSalt);

            return PWDToHash;
        }

        //以下不給外部使用----------------------------------------------------

        //將密碼加鹽並回傳加密用的格式(Byte[])
        public static byte[] PasswordAddSalt(string password, string salt)
        {
            return Encoding.UTF8.GetBytes(password + salt);
        }

        //將密碼雜湊 => 多載_1 : 給byte[]的值
        public static string HashPWD(byte[] pwd)
        {
            using (SHA256 Hash = SHA256.Create())
            {
                pwd = Hash.ComputeHash(pwd);
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < pwd.Length; i++)
                {
                    builder.Append(pwd[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        //將密碼雜湊 => 多載_2 : 給字串的值
        public static string HashPWD(string pwd)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(pwd);
            using (SHA256 Hash = SHA256.Create())
            {
                bytes = Hash.ComputeHash(bytes);
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
