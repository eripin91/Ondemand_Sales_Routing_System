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
using iSchedule.Models;
using System.Threading;

namespace iSchedule.Views
{
    public partial class Entries : System.Web.UI.Page
    {
        Repository repo = Repository.Instance;
        //Becoz u donno if some pages will need to have a different PageSize
        static readonly int PageSize = 2;
        string appId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            appId = repo.Cookies_Get("uAppId");

            if (string.IsNullOrEmpty(appId))
            {
                Response.Redirect("~/UI/ErrorPage.aspx");
            }

            if (!Page.IsPostBack)
            {
                PurgeDiv.Visible = false;
                ExportDiv.Visible = false;
                PurgeSel.Visible = false;
                PagingDiv.Visible = false;
                LoadedDiv.Visible = false;
                ShowEntries(radEvents.SelectedValue);
            }            
        }

        //protected void Filter_Click(object sender, EventArgs e)
        //{
        //    //Validate Input
        //    int integer;

        //    //Needs to Test
        //    if (Int32.TryParse(CurrentPage.Text, out integer) == false)
        //    {
        //        lblModal.Text = "Please select a proper page!";
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
        //        return;
        //    }

        //    if (lblTotal.Text != "" &&
        //        (Int32.TryParse(CurrentPage.Text, out integer) == false ||
        //        Convert.ToInt32(CurrentPage.Text) < 1 ||
        //        Convert.ToInt32(CurrentPage.Text) > repo.calculateLastPage(Convert.ToInt32(lblTotal.Text), PageSize)))
        //    {
        //        lblModal.Text = "Please select a proper page!";
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
        //        return;
        //    }

        //    var Options = new Options_Models()
        //    {
        //        isSent = ddlIsSent.SelectedValue,
        //        Page = Convert.ToInt32(CurrentPage.Text),
        //        PageSize = PageSize,
        //        AppId = appId
        //    };

        //    var Result = repo.GetEntries(Options);

        //    if (!Result.Valid)
        //    {
        //        lblModal.Text = Result.message;
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
        //        return;
        //    }

        //    //json = SARepository.SerializerHelper(new Dictionary<string, object>() {
        //    //        { "Count" , Query.Count() },
        //    //        { "Entries" , retblock},
        //    //        { "EntriesHeader" , headers}
        //    //    });

        //    lblTotal.Text = Result.TotalCount.ToString();
        //    lblTotalPages.Text = repo.calculateLastPage(Convert.ToInt32(lblTotal.Text), PageSize).ToString();

        //    var dt = repo.ListOfDictionaryToDataTable(Result.DataAsDictionary);
            
        //    EntriesGV.DataSource = dt; // Paging?
        //    EntriesGV.DataKeyNames = Result.DataHeaders.ToArray();
        //    EntriesGV.DataBind();
            
        //    if (User.Identity.IsAuthenticated && User.IsInRole("Superusers"))
        //    {
        //        PurgeDiv.Visible = true;
        //        //PurgeSel.Visible = true;
        //    }
        //    else
        //    {
        //        PurgeDiv.Visible = false;
        //        PurgeSel.Visible = true;
        //    }
        //    ExportDiv.Visible = true;
        //    PagingDiv.Visible = true;
        //    LoadedDiv.Visible = true;
        //}

        protected void ShowEntries(string _isSent)
        {
            //Validate Input
            int integer;

            //Needs to Test
            if (Int32.TryParse(CurrentPage.Text, out integer) == false)
            {
                lblModal.Text = "Please select a proper page!";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
                return;
            }

            

            var Options = new Options_Models()
            {
                isSent = _isSent,
                Page = Convert.ToInt32(CurrentPage.Text),
                PageSize = PageSize,
                AppId = appId
            };

            var Result = repo.GetEntries(Options);

            lblTotal.Text = Result.TotalCount.ToString();

            if (lblTotal.Text != "" &&
                (Int32.TryParse(CurrentPage.Text, out integer) == false ||
                Convert.ToInt32(CurrentPage.Text) < 1 ||
                Convert.ToInt32(CurrentPage.Text) > repo.calculateLastPage(Convert.ToInt32(lblTotal.Text), PageSize)))
            {
                lblModal.Text = "Please select a proper page!";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
                return;
            }

            if (!Result.Valid)
            {
                //lblModal.Text = Result.message;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
                return;
            }

            //json = SARepository.SerializerHelper(new Dictionary<string, object>() {
            //        { "Count" , Query.Count() },
            //        { "Entries" , retblock},
            //        { "EntriesHeader" , headers}
            //    });

            
            lblTotalPages.Text = repo.calculateLastPage(Convert.ToInt32(lblTotal.Text), PageSize).ToString();

            var dt = repo.ListOfDictionaryToDataTable(Result.DataAsDictionary);

            EntriesGV.DataSource = dt; // Paging?
            EntriesGV.DataKeyNames = Result.DataHeaders.ToArray();
            EntriesGV.DataBind();

            if (User.Identity.IsAuthenticated && User.IsInRole("Superusers"))
            {
                PurgeDiv.Visible = true;
                //PurgeSel.Visible = true;
            }
            else
            {
                PurgeDiv.Visible = false;
                PurgeSel.Visible = true;
            }
            ExportDiv.Visible = true;
            PagingDiv.Visible = true;
            LoadedDiv.Visible = true;
        }
        public void ExportToCsv_click(object sender, EventArgs e)
        {
            //Validate Input
            int integer;

            //Needs to Test
            if (Int32.TryParse(CurrentPage.Text, out integer) == false)
            {
                lblModal.Text = "Please select a proper page!";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
                return;
            }

            if (lblTotal.Text != "" &&
                (Int32.TryParse(CurrentPage.Text, out integer) == false ||
                Convert.ToInt32(CurrentPage.Text) < 1 ||
                Convert.ToInt32(CurrentPage.Text) > repo.calculateLastPage(Convert.ToInt32(lblTotal.Text), PageSize)))
            {
                lblModal.Text = "Please select a proper page!";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
                return;
            }
            
            var Options = new Options_Models() {
                AppId = appId
            };

            var Result = repo.GetEntriesCSV(Options);

            if (!Result.Valid)
            {
                lblModal.Text = Result.message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUp').modal('show');", true);
                return;
            }

            //json = SARepository.SerializerHelper(new Dictionary<string, object>() {
            //        { "Count" , Query.Count() },
            //        { "Entries" , retblock},
            //        { "EntriesHeader" , headers}
            //    });

            var dt = repo.ListOfDictionaryToDataTable(Result.DataAsDictionary);

            string headers = string.Join(",", Result.DataHeaders.ToArray());

            GeneralFunctions.ExportToCsv(dt, headers, "entries");

        }

        protected void FirstPage_Click(object sender, EventArgs e)
        {
            CurrentPage.Text = (1).ToString();
            //Filter_Click(sender, e);
            ShowEntries(radEvents.SelectedValue);
        }

        protected void PreviousPage_Click(object sender, EventArgs e)
        {
            int integer;
            if (Int32.TryParse(CurrentPage.Text, out integer) == true && Convert.ToInt32(CurrentPage.Text) > 1)
            {
                CurrentPage.Text = (Convert.ToInt32(CurrentPage.Text) - 1).ToString();
                //Filter_Click(sender, e);
                ShowEntries(radEvents.SelectedValue);
            }
        }

        protected void NextPage_Click(object sender, EventArgs e)
        {
            int integer;
            if (Int32.TryParse(CurrentPage.Text, out integer) == true && Convert.ToInt32(CurrentPage.Text) < repo.calculateLastPage(Convert.ToInt32(lblTotal.Text), PageSize))
            {
                CurrentPage.Text = (Convert.ToInt32(CurrentPage.Text) + 1).ToString();
                //Filter_Click(sender, e);
                ShowEntries(radEvents.SelectedValue);
            }
        }

        protected void LastPage_Click(object sender, EventArgs e)
        {
            CurrentPage.Text = repo.calculateLastPage(Convert.ToInt32(lblTotal.Text), PageSize).ToString();
            //Filter_Click(sender, e);
            ShowEntries(radEvents.SelectedValue);
        }

        protected void PurgeSelected_Click(object sender, EventArgs e)
        {
            var DelIDs = new List<int>();
            foreach (GridViewRow grow in EntriesGV.Rows)
            {
                //Searching CheckBox("chkDel") in an individual row of Grid  
                CheckBox chkdel = (CheckBox)grow.FindControl("Tick");
                //If CheckBox is checked than delete the record with particular empid  
                if (chkdel.Checked)
                {

                    int empid = Convert.ToInt32(grow.Cells[1].Text.ToString());
                    DelIDs.Add(empid);
                }
            }
            
            var result = repo.PurgeSelectedEntries(DelIDs);

            Response.Redirect(Request.RawUrl);
        }

        protected void Purge_Click(object sender, EventArgs e)
        {
            var result = repo.PurgeEntries();

            Response.Redirect(Request.RawUrl);
        }

        protected void radEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            EntriesGV.DataSource = null;
            EntriesGV.DataBind();
            lblTotal.Text = "0";
            CurrentPage.Text = "1";
            ShowEntries(radEvents.SelectedValue);
        }

        //protected void ConvertWinner_Click(object sender, EventArgs e)
        //{
        //    //Get the button that raised the event
        //    Button btn = (Button)sender;

        //    //Get the row that contains this button
        //    GridViewRow gvr = (GridViewRow)btn.NamingContainer;

        //    int AppId = Convert.ToInt32(gvr.Cells[1].Text);

        //    //Get Entry and Call ConvertWinners

        //    using (var db = new BaseEntities()) {

        //        var Entry = db.Schedules.Where(s => s.AppId == AppId).FirstOrDefault();

        //        db.Schedules_Winners.Add(new Schedules_Winners()
        //        {
        //            Schedules = Entry,
        //            DateWon = System.DateTime.UtcNow,
        //            WinnerListName = "Converted on " + System.DateTime.UtcNow
        //               .AddHours(repo.AddLocalTimeZone).ToString("dd MMM yyyy"),
        //            MobileNo = Entry.MobileNo,
        //            NRIC_NoPrefix = Entry.NRIC_NoPrefix,
        //        });
        //        db.SaveChanges();


        //        lblModalNoCancel.Text = "Entry picked as Winner with ID : "+ Entry.AppId.ToString() + "!";

        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUpNoCancel').modal('show');", true);

        //        //This simulates the button click from within your code.
        //        Filter_Click(Filter, EventArgs.Empty);


        //        return;


        //    }

        //}



        //protected void Resend_Click(object sender, EventArgs e)
        //{
        //    //Get the button that raised the event
        //    Button btn = (Button)sender;

        //    //Get the row that contains this button
        //    GridViewRow gvr = (GridViewRow)btn.NamingContainer;

        //    int AppId = Convert.ToInt32(gvr.Cells[1].Text);

        //    //Get Entry and Call ConvertWinners

        //    using (var db = new BaseEntities())
        //    {

        //        var Entry = db.Schedules.Where(s => s.AppId == AppId).FirstOrDefault();

        //        //Send Message

        //        GeneralFunctions.SendSms(Convert.ToInt32(repo.AppID), new Guid(repo.AppSecret), Entry.MobileNo.ToString(),
        //           repo.ResentMessage.Replace("{uploadlink}", repo.UploadLink + "?i=" + Entry.VerificationCode));

        //        lblModalNoCancel.Text = "Upload Link has been resent to customer's mobile : " + Entry.MobileNo;

        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUpNoCancel').modal('show');", true);

        //        //This simulates the button click from within your code.
        //        Filter_Click(Filter, EventArgs.Empty);


        //        return;

        //    }
        //}

        //protected void InEligible_Click(object sender, EventArgs e)
        //{
        //    //Get the button that raised the event
        //    Button btn = (Button)sender;

        //    //Get the row that contains this button
        //    GridViewRow gvr = (GridViewRow)btn.NamingContainer;

        //    int AppId = Convert.ToInt32(gvr.Cells[1].Text);

        //    //Get Entry and Call ConvertWinners

        //    using (var db = new BaseEntities())
        //    {

        //        var Entry = db.Schedules.Where(s => s.AppId == AppId).FirstOrDefault();

        //        Entry.IsValid = false;
        //        Entry.Reason = "Upload is not valid!";

        //        //Send Message

        //        GeneralFunctions.SendSms(Convert.ToInt32(repo.AppID), new Guid(repo.AppSecret), Entry.MobileNo.ToString(),
        //           repo.RejectedMessage.Replace("{receiptno}", Entry.ReceiptNo));

        //        //SaveChanges
        //        db.SaveChanges();


        //        lblModalNoCancel.Text = "Rejection has been sent to customer's mobile : " + Entry.MobileNo;

        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUpNoCancel').modal('show');", true);

        //        //This simulates the button click from within your code.
        //        Filter_Click(Filter, EventArgs.Empty);


        //        return;

        //    }
        //}

        //protected void Verified_Click(object sender, EventArgs e)
        //{
        //    //Get the button that raised the event
        //    Button btn = (Button)sender;

        //    //Get the row that contains this button
        //    GridViewRow gvr = (GridViewRow)btn.NamingContainer;

        //    int AppId = Convert.ToInt32(gvr.Cells[1].Text);

        //    //Get Entry and Call ConvertWinners

        //    using (var db = new BaseEntities())
        //    {

        //        var Entry = db.Schedules.Where(s => s.AppId == AppId).FirstOrDefault();

        //        Entry.IsVerified = true;


        //        //Send Message

        //        GeneralFunctions.SendSms(Convert.ToInt32(repo.AppID), new Guid(repo.AppSecret), Entry.MobileNo.ToString(),
        //           repo.VerifiedMessage.Replace("{receiptno}", Entry.ReceiptNo));

        //        //SaveChanges
        //        db.SaveChanges();


        //        lblModalNoCancel.Text = "Verification has been sent to customer's mobile : " + Entry.MobileNo;

        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "$('#divPopUpNoCancel').modal('show');", true);

        //        //This simulates the button click from within your code.
        //        Filter_Click(Filter, EventArgs.Empty);


        //        return;

        //    }
        //}

    }
}