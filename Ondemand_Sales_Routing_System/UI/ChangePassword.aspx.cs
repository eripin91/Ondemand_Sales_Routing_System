using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using iSchedule.Models;
using System.Globalization;
using System.Data;
using iSchedule.BLL;
using System.Security.Cryptography;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace iSchedule.Views
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        Repository repo = Repository.Instance;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                 {
                    if (!User.Identity.IsAuthenticated)
                    {
                        Response.Redirect("~/UI/ErrorPage.aspx");
                    }
                    
                }
                catch (Exception ex)
                {
                    Response.Redirect("~/UI/ErrorPage.aspx",false);
                }
            }
        }
        protected void Save_Click(object sender, EventArgs e)
        {
            if (OldPass.Text == "")
            {
                lblModal.Text = "Old password is required";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
                return;
            }
            if (NewPass.Text == "")
            {
                lblModal.Text = "New password is required";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
                return;
            }
            if (ConfirmNewPass.Text == "")
            {
                lblModal.Text = "Confirm new password is required";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
                return;
            }
            if (NewPass.Text!=ConfirmNewPass.Text)
            {
                lblModal.Text = "New password and confirm new password must same";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
                return;
            }
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            var result = userManager.ChangePassword(User.Identity.GetUserId(), OldPass.Text, NewPass.Text);
            if (result.Succeeded) {
                lblModal.Text = "Success change password";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
            }
            else
            {
                lblModal.Text = result.Errors.FirstOrDefault();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
            }

    }
    }
}