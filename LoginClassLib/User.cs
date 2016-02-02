using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LoginClassLib
{
    [Serializable()]
    public class User 
    {
        private static string email;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public User(string email, string password)
        {
            Email = email;
            this.password = HashPass(password, email);
        }

        ///<summary>
        ///Create a hash password using 2 string value
        ///</summary>
        ///<param name="password">string</param>
        ///<param name="email">string</param>
        ///<returns>String of a hashed value</returns>
        ///<remarks>
        ///Uses the SHA256 to encypt string
        ///</remarks> 
        public static string HashPass(string password, string email)
        {

            SHA256 sha = new SHA256CryptoServiceProvider();

            //compute hash from the bytes of text
            sha.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password + email));

            //get hash result after compute it
            byte[] result = sha.Hash;

            StringBuilder strBuilder = new StringBuilder();

            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
      
    }
}
