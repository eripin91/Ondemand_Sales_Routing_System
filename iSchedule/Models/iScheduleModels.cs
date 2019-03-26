using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iSchedule.Models
{    
    public class Parameters_Models
    {
        public string EntrySource { get; set; }
        public bool SendResponse { get; set; }

        //Standard Params for SMS/MMS
        public string createdon { get; set; }
        public string MobileNo { get; set; }
        public DateTime EventDate { get; set; }
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public string Custom3 { get; set; }
        public string Message { get; set; }
        public string FileLink { get; set; }
    }

    public class Options_Models
    {
        public DateTime StartDate;
        public DateTime EndDate;
        public string ValidOnly;
        public string AppId;
        public int Page;
        public int PageSize;
        public string WinnerGroupName;
        public string EntryType;
        public bool ExcludePastNRIC;
        public bool ExcludePastMob;
        public int QuotaToSelect;
        public bool UploadStatus;
        public string isSent;
    }

    public class ValidationResult_Models
    {
        public ValidationResult_Models(bool valid)
        {
            IsSuccessful = valid;
        }
        public bool IsSuccessful;
        public string ReasonForFailure;
        public string ResponseForFailure;
    }

    [Serializable]
    public class FunctionResult_Models
    {
        public Schedules Schedule = new Schedules();
        public Settings Winner = new Settings();
        public Parameters_Models param;
        public bool IsSendSMS = true;
        public bool IsSavable;
        public List<string> ListOfReasonsForPossibleFailures = new List<string>();
        public List<string> ListOfResponsesForPossibleFailures = new List<string>();
        public int TotalCount;
        public List<Dictionary<string, object>> DataAsDictionary;
        public List<string> DataHeaders;
        public string message;
        public bool Valid;

        public FunctionResult_Models(bool valid)
        {
            Valid = valid;
        }
    }    
    public class OnlineEntries_Models : Schedules
    {
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public string FileName { get; set; }
    }

}