using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data.SqlClient;

namespace LoginClassLib
{
    [Serializable()]
    public class Register : System.ComponentModel.INotifyPropertyChanged
    {
        #region class variables and accessors
             
        private const string FileName = @"..\..\UserLogins.bin";
        private static User user;
        
        public User User
        {
            get { return user; }
            set
            {
                user = value;
                PropertyChanged(this,
                  new System.ComponentModel.PropertyChangedEventArgs("User"));
            }
        }

      

        [field: NonSerialized()]
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        #endregion


        public Register()
        {            
        }

        ///<summary>
        ///Create a new user
        ///</summary>
        ///<param name="password">string</param>
        ///<param name="confirmPW">string</param>
        ///<param name="regEmail">string</param>
        ///<returns>True if user create successfully</returns>
        ///<remarks>
        ///The user data will persist on local file until they are added to database
        ///</remarks>
        public static Boolean AddUser(string password, string confirmPW, string regEmail)
        {
            Boolean userAdded = false;
            //Make sure email is valid
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            try
            {

                Match match = regex.Match(regEmail);

                #region Check for existing email
                ////loop full through logins table
                //foreach (DataRow row in LoginDataSet.LoginsDataTable())
                //{

                //    //Look for matching email
                //    if (row.ItemArray[0].Equals(email))
                //    {
                //        Console.WriteLine("email already exists");
                //    }
                //}  
                #endregion



                //Confirm pass must equal passsword.
                if (password != confirmPW)
                {
                    Console.WriteLine("Passwords do not match");
                }

                //Password must be at least 8 characters long
                else if (password.Length < 8)
                {
                    Console.WriteLine("Passwords must be at 8 Characters long");
                }

                else if (!match.Success)
                {
                    Console.WriteLine("invalid email");
                }
                //If all is well, create the new user
                else
                {

                    User newUser = new User(regEmail, password);
                    user = newUser;
                    userAdded = true;
                }
     
                return userAdded;
            }
            finally
            {
                SaveUser();
            }
        }
      

        ///<summary>
        ///Serial user data and write to file
        ///</summary>
        ///<param name=""></param>
        ///<returns></returns>
        ///<remarks>
        ///The user data will be writen to bin file
        ///</remarks>
        public static void SaveUser()
        {
            using (Stream TestFileStream = File.Create(FileName))
            {
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(TestFileStream, user);
            }
        }

        ///<summary>
        ///Add user to Database
        ///</summary>
        ///<param name=""></param>
        ///<returns>Boolean value</returns>
        ///<remarks>
        ///
        ///</remarks>
        public static bool SubmitUser()
        {
            bool saved = false;
            string[] field = new string[] {"email","password"}; 
            string[] values = new string[] {user.Email, user.Password};

            DataTableOperations target = new DataTableOperations(Properties.Settings.Default.ConnectionString);
            target.InsertString("Login", field, values);
         
            return saved;
        }

        private static SqlDataAdapter GetAdapter(SqlConnection conn, string sqlSelect)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(sqlSelect, conn);
            return adapter;
        }

        private static DataSet BuildDataSet(SqlConnection conn, SqlDataAdapter adapter)
        {
            DataSet fill = new DataSet("LoginData");
            if (conn.State != ConnectionState.Open) { conn.Open(); }
            adapter.Fill(fill, "LoginData");
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            if (conn.State != ConnectionState.Closed) { conn.Close(); }
            return fill;
        }

    }
}

