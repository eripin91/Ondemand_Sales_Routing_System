using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using AjaxControlToolkit;
using iSchedule.BLL;
using Microsoft.AspNet.Identity.Owin;

namespace iSchedule.Views
{
    public partial class Login : System.Web.UI.Page
    {
        Repository repo = Repository.Instance;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (User.Identity.IsAuthenticated)
                {
                    if(User.IsInRole("Superusers"))
                        Response.Redirect("~/UI/AdminUsers.aspx");
                    else Response.Redirect("~/UI/Settings.aspx");
                }
                else
                {
                    Version.InnerText = typeof(Login).Assembly.GetName().Version.ToString();
                    lblDT.Text = @System.DateTime.Now.Year.ToString();
                }
            }
        }


        protected void Login_Click(object sender, EventArgs e)
        {
            //Validation
            if (UserName.Text == "" || PassWord.Text == "")
            {
                lblModal.Text = "Please key in proper login values!";
                   ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
                return;
            }

            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            var user = userManager.Find(UserName.Text, PassWord.Text);
            string returnURL = string.Empty;

            if (user != null)
            {
                if (UserName.Text.Equals("ischedule",StringComparison.InvariantCultureIgnoreCase)
                    && PassWord.Text == repo.ContestAdminPW)
                {
                    userManager.AddToRole(user.Id, "Superusers");
                    returnURL = "~/UI/AdminUsers.aspx";
                }
                else
                {
                    userManager.AddToRole(user.Id, "Users");
                    returnURL = "~/UI/Settings.aspx";
                }
                var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userIdentity);
                
                Response.Redirect(returnURL);
            }
            else
            {
                lblModal.Text = "Login failed!";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
                return;
            }

        }

        //protected void Login_Click(object sender, EventArgs e)
        //{
        //    //Validation
        //    if (UserName.Text == "" || PassWord.Text == "")
        //    {
        //        lblModal.Text = "Please key in proper login values!";
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
        //        return;
        //    }


        //    var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDbContext()));
        //    if (!rm.RoleExists("Superusers"))
        //    {
        //        var roleResult = rm.Create(new IdentityRole("Superusers"));
        //    }
        //    if (!rm.RoleExists("Users"))
        //    {
        //        var roleResult2 = rm.Create(new IdentityRole("Users"));
        //    }

        //    var dictuser = new Dictionary<string, object>()
        //    {
        //        { "UserName", UserName.Text},
        //        { "PassWord", PassWord.Text }
        //    };

        //    //$scope.User.UserName == "" || $scope.User.PassWord == ""
        //    //User Login will verify against Contest Login
        //    if ((dictuser["UserName"].ToString().ToUpper() != repo.ContestUser.ToUpper() ||
        //        dictuser["PassWord"].ToString() != repo.ContestPW) &&
        //        (dictuser["UserName"].ToString().ToUpper() != repo.ContestUser.ToUpper() ||
        //        dictuser["PassWord"].ToString() != repo.ContestAdminPW))
        //    {
        //        lblModal.Text = "Login failed!";
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
        //        return;
        //    }


        //    //Then Api will Login to Identity Automatically

        //    //Try to create User.
        //    // Default UserStore constructor uses the default connection string named: DefaultConnection
        //    var userStore = new UserStore<IdentityUser>();
        //    var manager = new UserManager<IdentityUser>(userStore);

        //    var createuser = new IdentityUser() { UserName = dictuser["PassWord"].ToString() == repo.ContestAdminPW ? repo.IdentityAdmin : repo.IdentityUser };
        //    IdentityResult result = manager.Create(createuser, repo.IdentityPW);

        //    if (result.Succeeded)
        //    {
        //        //Assign Roles  

        //        var userManager = new UserManager<IdentityUser>(userStore);
        //        var user = userManager.Find(dictuser["PassWord"].ToString() == repo.ContestAdminPW ? repo.IdentityAdmin : repo.IdentityUser,
        //            repo.IdentityPW);

        //        if (user != null)
        //        {
        //            if (dictuser["PassWord"].ToString().ToUpper() == repo.ContestAdminPW)
        //            {
        //                userManager.AddToRole(user.Id, "Superusers");
        //            }
        //            else
        //            {
        //                userManager.AddToRole(user.Id, "Users");
        //            }

        //            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
        //            var userIdentity = userManager.CreateIdentity(createuser, DefaultAuthenticationTypes.ApplicationCookie);
        //            authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userIdentity);
        //            Response.Redirect("~/UI/Settings.aspx");
        //            //return dictuser["PassWord"].ToString().ToUpper() == SArepo.ContestAdminPW ? "Server has been setup, User : " + SArepo.IdentityAdmin + " has successfully Logged In"
        //            //: "Server has been setup, User : " + SArepo.ContestUser + " has successfully Logged In";
        //        }
        //        else
        //        {
        //            lblModal.Text = "Login failed!";
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        var userManager = new UserManager<IdentityUser>(userStore);
        //        var user = userManager.Find(dictuser["PassWord"].ToString().ToUpper() == repo.ContestAdminPW.ToUpper() ? repo.IdentityAdmin : repo.IdentityUser,
        //             repo.IdentityPW);


        //        if (user != null)
        //        {
        //            userManager.AddToRole(user.Id, dictuser["PassWord"].ToString().ToUpper() == repo.ContestAdminPW ? "Superusers" : "Users");

        //            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
        //            var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
        //            authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userIdentity);

        //            Response.Redirect("~/UI/AdminUsers.aspx");

        //            //Response.StatusCode = 200;
        //            //return dictuser["PassWord"].ToString().ToUpper() == SArepo.ContestAdminPW ? "User : " + SArepo.IdentityAdmin + " has successfully Logged In"
        //            //    : "User : " + SArepo.ContestUser + " has successfully Logged In";
        //        }
        //        else
        //        {
        //            lblModal.Text = "Login failed!";
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
        //            return;
        //            //Response.StatusCode = 500;
        //            //return "Server Login has failed! Please contact admin!";
        //        }
        //    }



        //}

        //protected void btnCancel2_Click(object sender, EventArgs e)
        //{
        //    lblModal.Text = "";
        //       ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
        //}

        //protected void btnCancel_Click(object sender, EventArgs e)
        //{
        //    lblModal.Text = "";
        //       ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
        //}
    }
}