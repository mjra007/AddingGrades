using OpenQA.Selenium;
using OpenQA.Selenium.Chrome; 
using System.Net;
using System.Security;


#nullable enable
namespace AddinGrades
{
    public class LoginController
    {
        private string LoginURL;

        public LoginController(string loginURL = "https://jobra.eschoolingserver.com/") => this.LoginURL = loginURL;

        public void Login(ChromeDriver driver, string email, SecureString password)
        {
            driver.Navigate().GoToUrl(this.LoginURL);
            driver.InsertInputById("txtUser", email);
            driver.InsertInputById("txtPwd", new NetworkCredential("", password).Password);
            driver.FindElement(By.Id("Entrar")).Click();
        }
    }
}
