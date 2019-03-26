using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using iSchedule.Models;
using iSchedule.BLL;
using System.Text.RegularExpressions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Collections.Specialized;
using System.Net.Http.Formatting;

namespace iSchedule.Controllers
{
    [AllowAnonymous]
    public class RESTController : ApiController
    {
        Repository Repo = Repository.Instance;

        //[HttpPost]
        //[AllowAnonymous]
        //[Route("REST/Add/")]
        //public string Add([FromBody] FormDataCollection body)
        //{
        //    return GetAndPostFunction(new Parameters
        //    {
        //        createdon = body.Get("createdon") == null ? null : body["createdon"].ToString(),
        //        MobileNo = body.Get("MobileNo") == null ? null : body["MobileNo"].ToString(),
        //        Message = body.Get("Message") == null ? null : body["Message"].ToString(),
        //        FileLink = body.Get("FileLink") == null ? null : body["FileLink"].ToString(),
        //        EntrySource = (body["FileLink"] != null && body["FileLink"].ToString() != "") ? "MMS" : "SMS",
        //        SendResponse = true,
        //    });
        //}

        //public string GetAndPostFunction(Parameters_Models body)
        //{
        //    try
        //    {
        //        //AuthorizedIP
        //        string SourceIP = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();

        //        if (Repo.IP != "" && SourceIP != Repo.IP)
        //            throw new ArgumentException("Unauthorized access from " + SourceIP);
        //        //AuthorizedIP

        //        //UserID and Keyword
        //        DateTime dt = DateTime.UtcNow;

        //        var Result = Repo.SubmitEntry(body);

        //        if (body.SendResponse && (Result.IsSendSMS && Repo.AppID != "" && Repo.AppSecret != ""))/* && body.EntrySource !="API" */ //UnComment this to prevent API Entries from sending responses. 
        //        {
        //            GeneralFunctions.SendSms(Convert.ToInt32(Repo.AppID), new Guid(Repo.AppSecret), body.MobileNo.ToString(),
        //                Result.Entry.Response);
        //        }

        //        return "OK";

        //    }
        //    catch (Exception ex)
        //    {
        //        var ErrMsg = "Error : " + ex.Message + ex.StackTrace.ToString();
        //        Repo.ErrorLog(ErrMsg);
        //        return ErrMsg;
        //    }
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //[Route("REST/Submit/")]
        //public string Submit(string Createdon, string MobileNo, string MessagePart1, string MessagePart2, string FileLink = "")
        //{
        //    return GetAndPostFunction(new Parameters
        //    {
        //        createdon = DateTime.Parse(Createdon).AddHours(Repo.SubtractLocalTimeZone).ToString(),
        //        MobileNo = MobileNo,
        //        Message = MessagePart1 + " " + MessagePart2, //form Message from parameters
        //        FileLink = FileLink,
        //        EntrySource = "API",
        //        SendResponse = true,
        //    });
        //}

        //[HttpPost]
        [HttpGet]
        [AllowAnonymous]
        [Route("REST/AutoSentMessage/")]
        public string AutoSentMessage(string createdon = "")
        {
            try
            {
                Settings_BLL settings_BLL = new Settings_BLL();
                Schedules_BLL schedules_BLL = new Schedules_BLL();

                //Determine Start and End Date based on today's date?

                //var StartDate = body.Get("startdate") == null ? DateTime.MinValue : Convert.ToDateTime(body["startdate"].ToString()).AddHours(Repo.SubtractLocalTimeZone);
                //var EndDate = body.Get("enddate") == null ? DateTime.MinValue : Convert.ToDateTime(body["enddate"].ToString()).AddHours(Repo.SubtractLocalTimeZone);

                ////Determine how many winners to pick?

                //var NoOfWinners = body.Get("noofrecords") == null ? 0 : Convert.ToInt32(body["noofrecords"].ToString());
                DateTime localNow = (createdon == "") ? DateTime.UtcNow : Convert.ToDateTime(createdon);

                //get all setting where local now == set time
                List<Settings> runningSettings = settings_BLL.getSettingsByDateTime(localNow);

                //foreach list of setting, get all schedules where isSent = 0
                foreach (var item in runningSettings)
                {
                    List<Schedules> upcomingSchedules = schedules_BLL.getAllUpcomingSchedulesByAppId(item.AppId);

                    //foreach schedules , send SMS and set isSent = 0, SentOn = localNow
                    foreach (var it in upcomingSchedules)
                    {
                        if (it.EventDate.AddHours(Repo.AddLocalTimeZone).Date == DateTime.UtcNow.Date)
                        {
                            GeneralFunctions.SendSms(Convert.ToInt32(item.AppId), new Guid(item.AppSecret), it.MobileNo,
                                item.MessageTemplate.Replace("{custom1}",it.Custom1).Replace("{custom2}", it.Custom2).Replace("{custom3}", it.Custom3));
                            //update isSent and sentOn
                            it.IsSent = true;
                            it.SentOn = localNow;

                            schedules_BLL.update(it);
                        }
                    }

                }
                return "OK";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
                
        }



        //[HttpGet]
        //[AllowAnonymous]
        //[Route("REST/SetupSettings/")]
        //public string SetupSettings()
        //{
        //    return Repo.SetupSettings().message;
        //}

    }
}
