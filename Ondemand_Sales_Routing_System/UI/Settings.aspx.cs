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

namespace iSchedule.Views
{
    public partial class SettingsView : System.Web.UI.Page
    {
        Repository repo = Repository.Instance;
        Settings_BLL settingsBLL = new Settings_BLL();
        //Becoz u donno if some pages will need to have a different PageSize
        static readonly int PageSize = 50;
        public Settings _settings = new Settings();

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
                    _settings = settingsBLL.getSettingsByUserId(User.Identity.GetUserId());

                    //DateTime expiredDT;

                    //if (string.IsNullOrEmpty(repo.Session_Get("uAppId")))
                    //{   
                        //byte[] EncryptedToken = Convert.FromBase64String(Request.QueryString["token"]);

                        //get appId|appSecret|tick
                        //string userId = User.Identity.GetUserId();
                        //var _settings = settingsBLL.getSettingsByUserId(userId);

                        //appId = _settings.AppId;
                        //appSecret = _settings.AppSecret;

                        //string DecryptedToken = repo.DecryptStringFromBytes_Aes(EncryptedToken, repo.DecryptAESKey, repo.DecryptAESinitVector);

                        //appId = DecryptedToken.Split('|')[0];
                        //appSecret = DecryptedToken.Split('|')[1];
                        //DateTime.TryParse(DecryptedToken.Split('|')[2], out expiredDT);
                        //int unixTimeStamp = 0;
                        //int.TryParse(DecryptedToken.Split('|')[2], out unixTimeStamp);
                        //expiredDT = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddSeconds(unixTimeStamp);

                        //DateTime currentDT;
                        //DateTime.TryParse(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"), out currentDT);
                        //if (DateTime.Compare(expiredDT, currentDT) < 0)
                        //{
                        //    Response.Redirect("~/UI/ErrorPage.aspx", false);
                        //}
                        //else {
                            //repo.Session_Set("uAppId", appId);
                        //}
                        
                    //}
                    //else
                    //{
                    //    appId = repo.Session_Get("uAppId");
                    //    Settings objSettings = settingsBLL.getSettingsByAppId(appId);
                    //    if (objSettings != null)
                    //        appSecret = objSettings.AppSecret;
                    //}                    

                    //Settings setting = settingsBLL.getSettingsByUserId(User.Identity.GetUserId());
                    if (_settings == null)
                    {
                        //insert because appId not exist
                        //Settings objSetting = new Settings
                        //{
                        //    AppId = appId,
                        //    AppSecret = appSecret,
                        //    CreatedOn = DateTime.UtcNow
                        //};

                        //Settings newSetting = settingsBLL.create(objSetting);
                        //if (newSetting == null)
                        //{
                        //    lblModal.Text = "failed to save this AppId!";
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
                        //    return;
                        //}
                        settingDiv.Visible = false;
                        lblModal.Text = "No AppId tagged to this user";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
                        
                    }
                    else
                    {
                        settingDiv.Visible = true;
                        repo.Session_Set("uAppId", _settings.AppId);
                        TimeSpan addTimeZone = TimeSpan.FromHours(repo.AddLocalTimeZone);
                        if (_settings.Scheduletime != null) sendTime.Text = Convert.ToDateTime(((TimeSpan)_settings.Scheduletime).Add(addTimeZone).ToString()).ToString("HH:mm");
                        areaMsgTemplate.Text = _settings.MessageTemplate;
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
            string appId = repo.Session_Get("uAppId");

            Settings setting = settingsBLL.getSettingsByAppId(appId);

            if (string.IsNullOrEmpty(sendTime.Text))
            {
                lblModal.Text = "Time to send is required";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
                return;
            }
            if (string.IsNullOrEmpty(areaMsgTemplate.Text))
            {
                lblModal.Text = "Message is required";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
                return;
            }

            TimeSpan minusTimeZone = TimeSpan.FromHours(repo.SubtractLocalTimeZone);
            TimeSpan cycleTimeZone = TimeSpan.FromHours(repo.CycleLocalTimeZone);
            setting.Scheduletime = TimeSpan.Parse(sendTime.Text).Hours < (repo.SubtractLocalTimeZone * -1) ? TimeSpan.Parse(sendTime.Text).Add(cycleTimeZone) : TimeSpan.Parse(sendTime.Text).Add(minusTimeZone);
            setting.MessageTemplate = areaMsgTemplate.Text;

            Settings newSettings = settingsBLL.update(setting);
            if (newSettings != null)
            {
                lblModal.Text = "Update success";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
            }
            else
            {
                lblModal.Text = "Failed to update";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
            }
        }
    }
}