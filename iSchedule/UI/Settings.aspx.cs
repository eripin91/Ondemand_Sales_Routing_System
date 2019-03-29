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

                    if (string.IsNullOrEmpty(repo.Cookies_Get("uAppId")))
                    {
                        byte[] et = repo.EncryptStringToBytes_Aes("2700|800f7db9-67a3-4f35-af2d-f14badb62311|1893456000", repo.DecryptAESKey, repo.DecryptAESinitVector);

                        string urlEncode = HttpUtility.UrlEncode(Request.QueryString["token"]);
                        string tk = HttpUtility.UrlEncode(Convert.ToBase64String(et.ToArray()));

                        byte[] EncryptedToken = Convert.FromBase64String(Request.QueryString["token"]);

                        string DecryptedToken = repo.DecryptStringFromBytes_Aes(EncryptedToken, repo.DecryptAESKey, repo.DecryptAESinitVector);

                        appId = DecryptedToken.Split('|')[0];
                        appSecret = DecryptedToken.Split('|')[1];
                        DateTime.TryParse(DecryptedToken.Split('|')[2], out expiredDT);

                        repo.Cookies_Set("uAppId", appId, DateTime.Now.AddDays(1));
                        repo.Cookies_Set("uAppSecret", appSecret, DateTime.Now.AddDays(1));
                        repo.Cookies_Set("uExpiredTick", expiredDT.ToString(), DateTime.Now.AddDays(1));
                    }
                    else
                    {
                        appId = repo.Cookies_Get("uAppId");
                        appSecret = repo.Cookies_Get("uAppSecret");
                        DateTime.TryParse(repo.Cookies_Get("uExpiredTick"), out expiredDT); 
                    }
                    //check tick is not expired
                    //Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    //long tickTimeStamp = DateTime.UtcNow.Ticks;

                    DateTime currentDT;
                    DateTime.TryParse(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"),out currentDT);

                    //if (expiredTick < tickTimeStamp) Response.Redirect("~/UI/ErrorPage.aspx",false);
                    if(DateTime.Compare(expiredDT, currentDT)<0) Response.Redirect("~/UI/ErrorPage.aspx", false);

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
                        if (setting.Scheduletime != null) sendTime.Text =  ((TimeSpan)setting.Scheduletime).Add(addTimeZone).ToString();
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
            string appId = repo.Cookies_Get("uAppId");

            Settings setting = settingsBLL.getSettingsByAppId(appId);

            TimeSpan minusTimeZone = TimeSpan.FromHours(repo.SubtractLocalTimeZone);
            setting.Scheduletime = TimeSpan.Parse(sendTime.Text).Add(minusTimeZone);
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