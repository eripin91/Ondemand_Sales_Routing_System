﻿using System;
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

namespace iSchedule.Views
{
    public partial class Layout : System.Web.UI.MasterPage
    {
        Repository repo = Repository.Instance;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Buffer = true;
            //Response.CacheControl = "no-cache";
            //Response.AddHeader("Pragma", "no-cache");
            //Response.AppendHeader("pragma", "no-cache");
            //Response.Expires = -1441;
            //Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            //Response.Cache.SetNoStore();

            if (!Page.IsPostBack)
            {
                //if (!HttpContext.Current.User.Identity.IsAuthenticated)
                //{
                //    Response.Redirect("~/UI/Login.aspx");
                //}

                AdminUser.Visible = false;
                AdminSettings.Visible = false;

                UserSettings.Visible = false;
                UserUpload.Visible = false;
                UserSchedules.Visible = false;
                ChangePassword.Visible = false;
                lblAppId.Text = repo.Session_Get("uAppId");
                //AdminManageSettings.Visible = false;

                if (HttpContext.Current.User.IsInRole("Superusers"))
                {
                    AdminUser.Visible = true;
                    AdminSettings.Visible = true;
                    UserSettings.Visible = false;
                    UserUpload.Visible = false;
                    UserSchedules.Visible = false;
                    ChangePassword.Visible = false;

                    // AdminManageSettings.Visible = true;
                }
                else
                {
                    UserSettings.Visible = true;
                    UserUpload.Visible = true;
                    UserSchedules.Visible = true;
                    ChangePassword.Visible = true;
                }
            }
        }

        protected void SignOut_Click(object sender, EventArgs e)
        {
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            Session.Abandon();


            Response.Redirect("~/UI/Login.aspx");
        }
    }
}