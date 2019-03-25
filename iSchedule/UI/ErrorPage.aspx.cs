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

namespace iSchedule.Views
{
    public partial class ErrorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (User.Identity.IsAuthenticated)
                {
                    Response.Redirect("Schedules.aspx");
                }
                else
                {
                    //Version.InnerText = typeof(ErrorPage).Assembly.GetName().Version.ToString();
                    //lblDT.Text = @System.DateTime.Now.Year.ToString();
                }
            }
        }
        
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