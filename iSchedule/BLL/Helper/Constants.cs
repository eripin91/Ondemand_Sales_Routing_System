using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Collections;
using System.Reflection;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.File; //Files
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Data;


namespace iSchedule.BLL
{
    public class Constants
    {
        //public readonly JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = int.MaxValue };

        //Email Settings
        public readonly string EmailFrom = @System.Configuration.ConfigurationManager.AppSettings["EmailFrom"];
        public readonly string EmailSenderName = @System.Configuration.ConfigurationManager.AppSettings["EmailSenderName"];
        public readonly string EmailSubject = @System.Configuration.ConfigurationManager.AppSettings["EmailSubject"];
        public readonly string EmailHost = @System.Configuration.ConfigurationManager.AppSettings["EmailHost"];
        public readonly int EmailPort = Convert.ToInt32(@System.Configuration.ConfigurationManager.AppSettings["EmailPort"]);
        public readonly string SMTPUserName = @System.Configuration.ConfigurationManager.AppSettings["SMTPUserName"];
        public readonly string SMTPPassword = @System.Configuration.ConfigurationManager.AppSettings["SMTPPassword"];
        public readonly string EmailBodyPath = @System.Configuration.ConfigurationManager.AppSettings["EmailBodyPath"];
        //<!--Contest Settings -->

        public readonly string Keyword = @System.Configuration.ConfigurationManager.AppSettings["Keyword"];

        public readonly DateTime TestDate = Convert.ToDateTime(@System.Configuration.ConfigurationManager.AppSettings["TestDate"]).ToUniversalTime();
        public readonly DateTime StartDate = Convert.ToDateTime(@System.Configuration.ConfigurationManager.AppSettings["StartDate"]).ToUniversalTime();
        public readonly DateTime EndDate = Convert.ToDateTime(@System.Configuration.ConfigurationManager.AppSettings["EndDate"]).ToUniversalTime();

        public readonly string UploadLink = @System.Configuration.ConfigurationManager.AppSettings["UploadLink"];
        public readonly string ImageLink = @System.Configuration.ConfigurationManager.AppSettings["ImageLink"];

        public readonly IEnumerable<string> ScheduleExclusionFields = @System.Configuration.ConfigurationManager.AppSettings["ScheduleExclusionFields"].ToString().Split(',').ToList();
        public readonly IEnumerable<string> WinnerExclusionFields = @System.Configuration.ConfigurationManager.AppSettings["WinnerExclusionFields"].ToString().Split(',').ToList();

        public readonly string IP = @System.Configuration.ConfigurationManager.AppSettings["AuthorizedIP"];

        //       <!--Validations -->

        public readonly string ValidAmount = @System.Configuration.ConfigurationManager.AppSettings["ValidAmount"];
        public readonly decimal TierAmount = Convert.ToDecimal(@System.Configuration.ConfigurationManager.AppSettings["TierAmount"]);
        public readonly decimal TierChance = Convert.ToDecimal(@System.Configuration.ConfigurationManager.AppSettings["TierChance"]);
        
        public readonly string ValidationRegexMobileNo = @System.Configuration.ConfigurationManager.AppSettings["ValidationRegexMobileNo"];
        public readonly string ValidationRegexMultipleWords = @System.Configuration.ConfigurationManager.AppSettings["ValidationRegexMultipleWords"];

        public readonly string ValidMessageOnline = @System.Configuration.ConfigurationManager.AppSettings["ValidMessageOnline"];
        //    <!--Login Information -->

        public readonly string ContestUser = @System.Configuration.ConfigurationManager.AppSettings["ContestUser"];
        public readonly string ContestPW = @System.Configuration.ConfigurationManager.AppSettings["ContestPW"];
        public readonly string ContestAdminPW = @System.Configuration.ConfigurationManager.AppSettings["ContestAdminPW"];
        public readonly string IdentityUser = @System.Configuration.ConfigurationManager.AppSettings["IdentityUser"];
        public readonly string IdentityAdmin = @System.Configuration.ConfigurationManager.AppSettings["IdentityAdmin"];
        public readonly string IdentityPW = @System.Configuration.ConfigurationManager.AppSettings["IdentityPW"];


        public readonly byte[] DecryptAESKey = Convert.FromBase64String(@System.Configuration.ConfigurationManager.AppSettings["DecryptAESKey"]);
        public readonly byte[] DecryptAESinitVector = Convert.FromBase64String(@System.Configuration.ConfigurationManager.AppSettings["DecryptAESinitVector"]);
        //     <!--Azure Storage Info-->

        //public readonly string AzureStorageLink = @System.Configuration.ConfigurationManager.AppSettings["AzureStorageLink"].ToString(); // @Properties.Settings.Default.AzureStorageLink.ToString(); 
        //public readonly string AzureStorageContainer = @System.Configuration.ConfigurationManager.AppSettings["AzureStorageContainer"].ToString();   //@Properties.Settings.Default.AzureStorageContainer.ToString();
        //public readonly CloudStorageAccount storageAccount = CloudStorageAccount.Parse(@System.Configuration.ConfigurationManager.ConnectionStrings["StorageConnectionString"].ToString());


        //    <!-- SMS Account Settings -->

        public readonly string AppID = @System.Configuration.ConfigurationManager.AppSettings["AppID"];
        public readonly string AppSecret = @System.Configuration.ConfigurationManager.AppSettings["AppSecret"];


        //    <!-- Other Settings -->

        public readonly int AddLocalTimeZone = 8;
        public readonly int SubtractLocalTimeZone = -8;
        public readonly int CycleLocalTimeZone = 16;
        public readonly string DateTimeFormat = "dd MMM yyyy HH:mm:ss";


        public readonly string DefaultMobileNo = @System.Configuration.ConfigurationManager.AppSettings["DefaultMobileNo"];
        public readonly string DefaultTestMessage = @System.Configuration.ConfigurationManager.AppSettings["DefaultTestMessage"];

        //Read from DB?

            /*
        public Schedules_Settings GetSettings() {
            using (var db = new BaseEntities()) {
                return db.Schedules_Settings.FirstOrDefault();
            }
        }

        public FunctionResult SetupSettings()
        {
            using (var db = new BaseEntities()) {
                db.Schedules_Settings.Add(new Schedules_Settings()
                {
                    Keyword = "TEST",
                    TestDate = Convert.ToDateTime("2016-01-01 16:00:00"),
                    StartDate = Convert.ToDateTime("2017-01-01 16:00:00"),
                    EndDate = Convert.ToDateTime("2017-12-31 15:59:00"),
                    UploadLink = "http://smsdome-contest-base-sea-web-prd-development.azurewebsites.net/Views/OnlineCompletion",
                    ImageLink = "https://www.smsdome.com/wp-content/uploads/2017/01/SmsDome_Official_Logo_2017-500px.png",
                    RepeatedMessageOnline = "Repeated entry detected. We have received your previous entry well. Please input a new receipt number.",
                    RepeatedMessageSMS = "Repeated entry detected. Note that you can only SMS in each receipt number once.",
                    GenericErrorMessage = "Incomplete fields detected. Kindly SMS in all fields again. BASE <space> Name <space> NRIC <space> Receipt Number <space> Amount Spent (Min $25)",
                    ValidMessage = "Thank you for your participation. If you wish to be kept updated about our future promotions via SMS , kindly SMS Y1 to 90933211 to opt in. Please visit {uploadlink} to complete your entry.",
                    ValidMessageOnline = "Thank you for your participation. Your entry is well received.",
                    
                    EntryExclusionFields = "Response,NRIC_NoPrefix,Response,VerificationCode",
                    WinnerExclusionFields = "Response,NRIC_NoPrefix,Response,VerificationCode",


                    TierAmount = (decimal)25.00,
                    TierChance = 1,

                    Fields = "MSISDN,Name,NRIC,ReceiptNo,Amount",
                    ValidationRegexFull = @"^(\+*\d+)( [\S ]+)( (?:S|T|s|t)?\d{7}[a-zA-Z])( \S+)( (?:[Ss]?\$)?(?:\d{1,10}(?:\,\d{3})*|(?:\d+))(?:\.\d{0,2})?)$",
                    ValidationRegexMSISDN = @"^\+*\d+$",
                    ValidationRegexNRIC = @"^(?:S|T|s|t)?\d{7}[a-zA-Z]$",
                    ValidationRegexName = @"^[\S ]+$",
                    ValidationRegexAmount = @"^(?:[Ss]?\$)?(?:\d{1,10}(?:\,\d{3})*|(?:\d+))(?:\.\d{0,2})?$",
                    ValidationRegexAmount2 = @"([\d,.]+)",
                    ValidationRegexReceiptNo = @"^\S+$",
                    ValidationRegexEmail = "^$",

                    ContestUser = "TEST",
                    ContestAdminPW = "SMSDOME",
                    ContestPW = "170101",
                    IdentityUser = "BaseContestUser",
                    IdentityAdmin = "BaseContestAdmin",
                    IdentityPW = "f3b396a7-a95e-4a84-ade1-f475f42fa893",
                    AzureStorageLink = "https://smsdomedevseasa.blob.core.windows.net/",
                    AzureStorageContainer = "basecontestcontainer",
                    AppID = 670,
                    AppSecret = "822910db-cc3b-4649-b7ce-aba3fec73eb6",
                    
                });

                db.SaveChanges();
            }

            return new FunctionResult(true) {message = "Successfully saved!" };
            
        }*/
    }
}