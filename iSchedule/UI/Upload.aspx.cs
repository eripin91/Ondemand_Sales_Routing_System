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
using System.Net.Http;

namespace iSchedule.Views
{
    public partial class UploadView : System.Web.UI.Page
    {
        Repository repo = Repository.Instance;
        //Becoz u donno if some pages will need to have a different PageSize
        static readonly int PageSize = 50;
        string appId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                
            }
            appId = repo.Session_Get("uAppId");
            if (string.IsNullOrEmpty(appId))
            {
                Response.Redirect("~/UI/ErrorPage.aspx");
            }
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            var JsonInfo = new Dictionary<string, object>();

            #region Validation

            //Validation

            if (UploadF.HasFile)
                try
                {
                    //Upload.SaveAs("C:\\Uploads\\" + Upload.FileName);
                    FileSpecs.InnerHtml = "File name: " +
                          UploadF.PostedFile.FileName + "<br>" +
                          UploadF.PostedFile.ContentLength + " kb<br>" +
                          "Content type: " +
                          UploadF.PostedFile.ContentType;

                    JsonInfo.Add("File", new { filename = UploadF.PostedFile.FileName });
                }
                catch (Exception ex)
                {
                    lblModal.Text = "ERROR: " + ex.Message.ToString();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
                    return;
                }
            else
            {
                lblModal.Text = "You have not specified a file.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
                return; ;
            }


            //End of Validation

            #endregion



            var rtn = repo.UploadEntries(new HttpPostedFileWrapper(UploadF.PostedFile));

            lblModal.Text = rtn.message;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
            return;

        }

    }
}