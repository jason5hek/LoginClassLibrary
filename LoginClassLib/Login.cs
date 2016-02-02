using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace LoginClassLib
{
    public class Login
    {
        private const string FileName = @"..\..\UserLogins.bin";

        public string Filename
        {
            get { return FileName; }
        } 


        public Login()
        {
        }

        ///<summary>
        ///validate a User Login
        ///</summary>
        ///<param name="email">string</param>
        ///<param name="password">string</param>
        ///<returns>True if user login is valid</returns>
        ///<remarks>
        ///</remarks>
        public bool validUser(string email, string password)
        {
            bool valid = false;


            if (retreiveUser(email, password) != null)
            {
                valid = true;
            }
      
            return valid;   
        }

        ///<summary>
        ///Retrieve user from file
        ///</summary>
        ///<param name="email">string</param>
        ///<returns>User class object</returns>
        ///<remarks>
        ///</remarks>
        public User retreiveUser(string email)
        {

            User rUser = null;
        
            //Get User from file
            if (File.Exists(FileName))
            {
                using (Stream TestFileStream = File.OpenRead(FileName))
                {
                    BinaryFormatter deserializer = new BinaryFormatter();
                    rUser = (User)deserializer.Deserialize(TestFileStream);
                }
            }
            else
            {
                Console.Write("File not found : " + FileName);
            }

            return rUser;
        }

        ///<summary>
        ///Retrieve user from file
        ///</summary>
        ///<param name="email">string</param>
        ///<param name="email">string</param>
        ///<returns>User class object</returns>
        ///<remarks>
        ///</remarks>
        public User retreiveUser(string email, string password)
        {

            User rUser = null;

            //find user from database
            DataTableOperations source = 
                new DataTableOperations(Properties.Settings.Default.ConnectionString,
                     Properties.Settings.Default.LoginSQL, "Login");

            DataTable t = new DataTable();
            t = source.Datatable;
            foreach (DataRow item in t.Rows)
            {
                if (item["email"].ToString().Equals(email)) 
                {           
                    if (item["password"].ToString().Equals(User.HashPass(password,email)))
                    {
                        rUser = new User(email, password);
                    }
                }
                else
                {
                    Console.Write("Record not found");
                }      
                
            }

            return rUser;
        }

    }
}
