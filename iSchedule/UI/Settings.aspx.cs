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

namespace iSchedule.Views
{
    public partial class SettingsView : System.Web.UI.Page
    {
        Repository repo = Repository.Instance;
        Settings_BLL settingsBLL = new Settings_BLL();
        //Becoz u donno if some pages will need to have a different PageSize
        static readonly int PageSize = 50;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                 {
                    string appId = string.Empty;
                    string appSecret = string.Empty;
                    DateTime expiredDT;

                    if (string.IsNullOrEmpty(repo.Session_Get("uAppId")))
                    {
                        byte[] EncryptedToken = Convert.FromBase64String(Request.QueryString["token"]);

                        string DecryptedToken = repo.DecryptStringFromBytes_Aes(EncryptedToken, repo.DecryptAESKey, repo.DecryptAESinitVector);

                        appId = DecryptedToken.Split('|')[0];
                        appSecret = DecryptedToken.Split('|')[1];
                        DateTime.TryParse(DecryptedToken.Split('|')[2], out expiredDT);

                        DateTime currentDT;
                        DateTime.TryParse(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"), out currentDT);
                        if (DateTime.Compare(expiredDT, currentDT) < 0) Response.Redirect("~/UI/ErrorPage.aspx", false);

                        repo.Session_Set("uAppId", appId);
                    }
                    else
                    {
                        appId = repo.Session_Get("uAppId");
                        Settings objSettings = settingsBLL.getSettingsByAppId(appId);
                        if (objSettings != null)
                            appSecret = objSettings.AppSecret;
                    }                    

                    Settings setting = settingsBLL.getSettingsByAppId(appId);
                    if (setting == null)
                    {
                        //insert because appId not exist
                        Settings objSetting = new Settings
                        {
                            AppId = appId,
                            AppSecret = appSecret,
                            CreatedOn = DateTime.UtcNow
                        };

                        Settings newSetting = settingsBLL.create(objSetting);
                        if (newSetting == null)
                        {
                            lblModal.Text = "failed to save this AppId!";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
                            return;
                        }
                    }
                    else
                    {
                        
                        TimeSpan addTimeZone = TimeSpan.FromHours(repo.AddLocalTimeZone);
                        if (setting.Scheduletime != null) sendTime.Text = Convert.ToDateTime(((TimeSpan)setting.Scheduletime).Add(addTimeZone).ToString()).ToString("HH:mm");
                        areaMsgTemplate.Text = setting.MessageTemplate;
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