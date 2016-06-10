using System;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using MvcRefactorTest.BL.Interface;
using MvcRefactorTest.Domain;
using MvcRefactorTest.Infrastructure.Abstract;
using MvcRefactorTest.Common;

namespace MvcRefactorTest.Controllers
{
    [LoggingAspect]
    public class AccountController : Controller
    {
        private readonly ICustomMembershipProvider _authProvider;

        private readonly IUserService _userService;

        public AccountController(IUserService userService, ICustomMembershipProvider authProvider)
        {
            this._authProvider = authProvider;
            this._userService = userService;
        }

        [HttpGet]
        public ActionResult Login()
        {
            FormsAuthentication.SignOut();
            return this.View();
        }

        [HttpPost]
        public ActionResult Validate(string username, string password)
        {
            if (this.ModelState.IsValid)
            {
                
                    if (this._authProvider.Authenticate(username, password)) if (this.SetupFormsAuthTicket(username, password, false)) return this.Redirect(this.Url.Action("Index", "Home"));
               
            }
            return this.View("Login");
        }

        /// <summary>
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="persistanceFlag"></param>
        /// <returns></returns>
        private bool SetupFormsAuthTicket(string username, string password, bool persistanceFlag)
        {
            var success = false;

            
                User userObj;
                this._userService.GetUserBy(username, out userObj);

                var userId = userObj.id;
                var userData = userId.ToString(CultureInfo.InvariantCulture);
                var authTicket = new FormsAuthenticationTicket(
                    1, 

                    // version
                    username, 

                    // user name                    
                    DateTime.Now, 

                    // creation
                    DateTime.Now.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["SessionTimeOut"])), 

                    // Expiration
                    persistanceFlag, 

                    // Persistent
                    userData);

                var encTicket = FormsAuthentication.Encrypt(authTicket);
                this.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                success = true;
           

            return success;
        }
    }
}