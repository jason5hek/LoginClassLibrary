using System;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using LoginClassLib;


namespace LoginUnitTests
{
    public class TestClass
    {

        public TestClass()
        {
           
        }

        ///<summary>
        ///Testing if user are added
        ///</summary>
        [Fact]
        public void AddUserTest()
        {
            //string username, string password, string ConfirmPW, string regEmail

            //all field ok
            Assert.Equal(true,Register.AddUser("Password123", "Password123","jason.shek@gmail.com"));
            //all field mis match password
            Assert.Equal(false, Register.AddUser("Password123", "Password", "jason.shek@gmail.com"));
            //all field invalid email
            Assert.Equal(false, Register.AddUser("Password123", "Password", "jason.shekgmail.com"));
            //all field invalid password
            Assert.Equal(false, Register.AddUser("Pass", "Pass", "jason.shekgmail.com"));
        }

        ///<summary>
        ///Testing if login validation works
        ///</summary>
        [Theory]
        [InlineData(true, "jason.shek@gmail.com", "Password12345")]
        [InlineData(true, "jason.shek@gmail.com", "Password12345")]
        [InlineData(false,"js@hotmail.co.uk", "Jason123")]
        [InlineData(false,"jssdfa@hotmail.co.uk", "Jfadsfas")]
        public void VerifyUserTest(bool valid, string email, string password)
        {
            Login login = new Login();
            Assert.Equal(valid, login.validUser(email, password));
        }



          ///<summary>
          ///Testing if user retrieval from file works
          ///</summary>
          [Theory]
          [InlineData("Password123","js@gmail.com")]
          [InlineData("Password567", "cs@gmail.com")]
          [InlineData("Password898", "es@gmail.com")]
          public void RetreiveUserTest(string pass, string email)
          {
              Login login = new Login();
              Register.AddUser(pass, pass, email);
              Assert.Equal(email, login.retreiveUser(email).Email);
          }

          ///<summary>
          ///Testing if user DB submitted
          ///</summary>
          [Theory]
          [InlineData("Password123", "js@gmail.com")]
          [InlineData("Password567", "cs@gmail.com")]
          [InlineData("Password898", "es@gmail.com")]
          public void SubmitToDBTest(string pass, string email)
          {
              Login login = new Login();

              Register.AddUser(pass, pass, email);
              Register.SubmitUser();
              Assert.True(login.validUser(email, pass));
          }

      
    }
}
