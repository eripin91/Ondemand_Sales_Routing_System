using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using iSchedule.edmx;
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
                    byte[] et = repo.EncryptStringToBytes_Aes("2700|800f7db9-67a3-4f35-af2d-f14badb62311|1893456000", repo.DecryptAESKey, repo.DecryptAESinitVector);

                    string urlEncode = HttpUtility.UrlEncode(Request.QueryString["token"]);
                    string tk = HttpUtility.UrlEncode(Convert.ToBase64String(et.ToArray()));

                    byte[] EncryptedToken = Convert.FromBase64String(Request.QueryString["token"]);                    

                    string DecryptedToken = repo.DecryptStringFromBytes_Aes(EncryptedToken, repo.DecryptAESKey, repo.DecryptAESinitVector);

                    string appId = DecryptedToken.Split('|')[0];
                    string appSecret = DecryptedToken.Split('|')[1];
                    int expiredTick = Convert.ToInt32(DecryptedToken.Split('|')[2]);

                    repo.Cookies_Set("uAppId", appId, DateTime.Now.AddDays(1));
                    repo.Cookies_Set("uAppSecret", appSecret, DateTime.Now.AddDays(1));
                    //check tick is not expired
                    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                    if (expiredTick < unixTimestamp) Response.Redirect("~/UI/ErrorPage.aspx");

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
                        sendTime.Text = setting.Scheduletime.ToString();
                        areaMsgTemplate.Text = setting.MessageTemplate;
                    }
                }
                catch (Exception ex)
                {
                    Response.Redirect("~/UI/ErrorPage.aspx");
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