﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iSchedule.BLL;

namespace iSchedule.UI
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        Repository repo = Repository.Instance;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["token"]))
            {
                byte[] EncryptedToken = Convert.FromBase64String(Request.QueryString["token"]);

                string DecryptedToken = repo.DecryptStringFromBytes_Aes(EncryptedToken, repo.DecryptAESKey, repo.DecryptAESinitVector);

                
                lblDecrypted.Text = DecryptedToken;
            }
            lblKey.Text = Convert.ToBase64String(repo.DecryptAESKey).ToString();
            lblIV.Text = Convert.ToBase64String(repo.DecryptAESinitVector).ToString();

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            byte[] et = repo.EncryptStringToBytes_Aes(txtEncrypted.Text, repo.DecryptAESKey, repo.DecryptAESinitVector);

            string urlEncode = HttpUtility.UrlEncode(Request.QueryString["token"]);
            string tk = HttpUtility.UrlEncode(Convert.ToBase64String(et.ToArray()));

            //Unix Timestamp is: 1595989821, GMT / UTC Date Time is: 2020-07-29 02:30:21

            lblEncryptString.Text = tk;
        }
    }
}