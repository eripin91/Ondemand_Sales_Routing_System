using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Globalization;
using System.Data;
using iSchedule.BLL;
using System.Security.Cryptography;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace iSchedule.Views
{
    public partial class AdminUsersView : System.Web.UI.Page
    {
        Repository repo = Repository.Instance;
        Users_BLL usersBLL = new Users_BLL();
        Settings_BLL settingsBLL = new Settings_BLL();
        //Becoz u donno if some pages will need to have a different PageSize
        static readonly int PageSize = 50;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (User.Identity.IsAuthenticated && User.IsInRole("Superusers"))
                {

                }
                else
                {
                    Response.Redirect("/UI/Login.aspx");
                }
                UsersGV.DataSource = usersBLL.getAllUsers();
                UsersGV.DataBind();

                ddlAppId.DataSource = settingsBLL.getAllUnUsedSettings();
                ddlAppId.DataBind();
            }
            
        }

        //protected void UpdatePopUp_Click(object sender, EventArgs e)
        //{
        //    //Get the button that raised the event
        //    Button btn = (Button)sender;

        //    //Get the row that contains this button
        //    GridViewRow gvr = (GridViewRow)btn.NamingContainer;

        //    int SettingsId = Convert.ToInt32(gvr.Cells[0].Text);

        //    //populate item name
        //    Settings setting = settingsBLL.getSettingsBySettingsId(SettingsId);
        //    txtAppId.Text = setting.AppId;
        //    txtAppSecret.Text = setting.AppSecret;

        //    hdnEntryID.Value = SettingsId.ToString();

        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divUserPopUp').modal('show');", true);
        //}

        //protected void Update_Click(object sender, EventArgs e)
        //{
        //    int SettingsID = Convert.ToInt32(hdnEntryID.Value);

        //    Settings objSettings = new Settings
        //    {
        //        SettingsID = SettingsID,
        //        AppId = txtAppId.Text,
        //        AppSecret = txtAppSecret.Text
        //    };

        //    Settings newSettings = settingsBLL.update(objSettings);
        //    if (newSettings != null)
        //    {
        //        Response.Redirect(Request.RawUrl);
        //    }
        //    else
        //    {
        //        lblError.Text = "Failed to update";
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divUserPopUp').modal('show');", true);
        //    }            
        //}

        protected void Email_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            Button btn = (Button)sender;

            //Get the row that contains this button
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;

            int SettingsID = Convert.ToInt32(gvr.Cells[0].Text);

            //if (settingsBLL.delete(SettingsID) != null)
            //{
            //    lblModal.Text = "Delete success";

            //    Response.Redirect(Request.RawUrl);
            //}
            //else
            //{
            //    lblModal.Text = "Failed to delete";
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
            //}
        }

        protected void Reset_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            Button btn = (Button)sender;

            //Get the row that contains this button
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;

            string id = gvr.Cells[0].Text;

            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            var user = userManager.FindById(id);

            if (user != null)
            {
                string newPass = RandomString(12); 
                user.PasswordHash = userManager.PasswordHasher.HashPassword(newPass);
                userStore.UpdateAsync(user);
                
                string HtmlBody = GeneralFunctions.CreateEmailBody(repo.SuccessRegisterEmailBodyPath, user.Email, newPass);
                GeneralFunctions.SendEmail(repo.EmailFrom, user.Email, repo.EmailFrom, repo.EmailSubject, repo.EmailHost, repo.EmailPort, repo.SMTPUserName, repo.SMTPPassword, HtmlBody);

                lblModal.Text = "Success sending new account information to " + user.Email;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
            }
            else
            {
                lblModal.Text = "Failed to reset password";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            Button btn = (Button)sender;

            //Get the row that contains this button
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;

            string id = gvr.Cells[0].Text;

            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            var user = userManager.FindById(id);

            if (user != null)
            {
                string email = user.Email;

                userStore.DeleteAsync(user);

                Response.Redirect(Request.RawUrl);
                UsersGV.DataSource = usersBLL.getAllUsers();
                UsersGV.DataBind();
            }
            else
            {
                lblModal.Text = "Failed to delete account";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
            }
        }

        protected void TagPopUp_Click(object sender, EventArgs e)
        {
            //Get the button that raised the event
            Button btn = (Button)sender;

            //Get the row that contains this button
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;

            string userId = gvr.Cells[0].Text; ;
            hdnUserID.Value = userId;

            var setting = settingsBLL.getSettingsByUserId(userId);

            if (setting != null)
                curAppId.Text = setting.AppId;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divTagPopUp').modal('show');", true);
        }
        protected void Tag_Click(object sender, EventArgs e)
        {
            var oldSettings = settingsBLL.getSettingsByUserId(hdnUserID.Value);
            
            if(oldSettings != null)
                oldSettings.UserId = null;

            var oldResult = settingsBLL.update(oldSettings);

            if (oldResult != null)
            {
                var setting = settingsBLL.getSettingsByAppId(ddlAppId.SelectedValue);
                setting.UserId = hdnUserID.Value;

                var newSetting = settingsBLL.update(setting);
                if (newSetting != null)
                {
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    lblModal.Text = "Failed to update";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
                }
            }
            else
            {
                lblModal.Text = "Failed to update";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
            }
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);

            string email = Email.Text;
            string pass = RandomString(12);

            var user = new IdentityUser() { UserName = email,Email=email };
                        
            IdentityResult result = manager.Create(user, pass);

            //send to email with new pass
            if (result.Succeeded)
            {
                string HtmlBody = GeneralFunctions.CreateEmailBody(repo.SuccessRegisterEmailBodyPath,email,pass);
                GeneralFunctions.SendEmail(repo.EmailFrom, email, repo.EmailFrom, repo.EmailSubject, repo.EmailHost, repo.EmailPort, repo.SMTPUserName, repo.SMTPPassword, HtmlBody);
                Response.Redirect(Request.RawUrl);
                UsersGV.DataSource = usersBLL.getAllUsers();
                UsersGV.DataBind();
            }
            else
            {
                lblModal.Text = result.Errors.FirstOrDefault();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
            }
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}