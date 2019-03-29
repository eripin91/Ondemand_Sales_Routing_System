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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.VisualBasic.FileIO;
using System.Threading;
using System.Data.Entity.Validation;
using iSchedule.Models;
using System.Globalization;

namespace iSchedule.BLL
{
    public sealed class Repository : GenericFunctions
    {
        #region Singleton Setup

        static readonly Repository _instance = new Repository();
        public static Repository Instance
        {
            get
            {
                return _instance;
            }
        }

        Repository()
        {
        }

        #endregion


        #region MMS OR SMS

        //public FunctionResult_Models SubmitSchedule(Parameters_Models param)
        //{
        //    using (var db = new BaseEntities())
        //    {
        //        //try
        //        //{


        //        var FunctRes = new FunctionResult_Models(true);
        //        FunctRes.IsSendSMS = false;
        //        FunctRes.Schedule.IsValid = true;


        //        var CleanedMessage = CleanMessage(param);

        //        var CreatedOn = param.createdon != null && param.createdon != "" ? DateTime.Parse(param.createdon.ToString()).ToUniversalTime() : System.DateTime.UtcNow;//jfield.ContainsKey("createdOn") ? JsonConvert.DeserializeObject<DateTime>(StandardContest.Models.Helpers.SerializerHelper(jfield["createdOn"].ToString()))  : System.DateTime.UtcNow;

        //        //ReturnFail only when message is totally not savable

        //        //Validation before matchfields
        //        var ValidationResult_Modelss = PreMatchFieldValidations(CreatedOn);
        //        if (!ValidationResult_Modelss.IsSuccessful)
        //        {
        //            FunctRes.Schedule.IsValid = false;
        //            FunctRes.IsSendSMS = false;

        //            FunctRes.ListOfReasonsForPossibleFailures.Add(ValidationResult_Modelss.ReasonForFailure);
        //            FunctRes.ListOfResponsesForPossibleFailures.Add(ValidationResult_Modelss.ResponseForFailure);

        //        }

        //        #region SpecificRegexMatching
        //        Regex regex = new Regex(ValidationRegexFull, RegexOptions.IgnoreCase);
        //        var MatchedMessage = regex.Match(CleanedMessage.Trim());
        //        // Set Fields which dont require validation
        //        FunctRes.Schedule.CreatedOn = CreatedOn;
        //        //FunctRes.Schedule.MobileNo = param.MobileNo;

        //        if (MatchedMessage.Success)
        //        {

        //            //Matched fields will now have a space infront of them because the space is now inside the regex of each field.
        //            var MatchedResultList = MatchedMessage.Groups.Cast<Group>().Select(match => match.Value).Skip(1).ToList().Select(s => s.Trim()).ToList();

        //            var FieldsL = FieldNames.Split(',').ToList();
        //            for (int k = 0; k < FieldsL.Count; k++)
        //            {
        //                if (FieldsL[k] == "EventDate")
        //                {
        //                    DateTime dt;
        //                    DateTime.TryParse(MatchedResultList[k], out dt);
        //                    FunctRes.Schedule.EventDate = dt;
        //                }
        //                else
        //                {
        //                    //MatchedFields.Add(FieldsL[k], MatchedResultList[k]);

        //                    var prop = FunctRes.Schedule.GetType().GetProperty(FieldsL[k]);
        //                    prop.SetValue(FunctRes.Schedule, MatchedResultList[k]);
        //                }
        //            }

        //            //Edit Schedule Logics

        //            FunctRes.Schedule = EditSchedule(FunctRes.Schedule);



        //            //Validate LOGIC using MatchedFields
        //            ValidationResult_Modelss = null;
        //            if (!ValidationResult_Modelss.IsSuccessful)
        //            {
        //                FunctRes.Schedule.IsValid = false;

        //                FunctRes.ListOfReasonsForPossibleFailures.Add(ValidationResult_Modelss.ReasonForFailure);
        //                FunctRes.ListOfResponsesForPossibleFailures.Add(ValidationResult_Modelss.ResponseForFailure);

        //            }

        //            FunctRes.IsSavable = true;

        //        }
        //        else
        //        {

        //            //If Invalid, for loop form regex on the fly to determine which field have error(use Key) RegexKeyPairValue,

        //            var FieldsL = FieldNames.Split(',').ToList();
        //            var BuildingRegex = "";
        //            for (int k = 0; k < FieldsL.Count; k++)
        //            {
        //                var CurrentFieldRegex = @System.Configuration.ConfigurationManager.AppSettings["ValidationRegex" + FieldsL[k]];

        //                if (BuildingRegex == "")
        //                {
        //                    BuildingRegex = "(" + CurrentFieldRegex.Remove(CurrentFieldRegex.Length - 1).Remove(0, 1) + ")";
        //                }
        //                else
        //                {
        //                    //check if optional field to omit space
        //                    //example of UOB regex ^(?: (?i)uob)?$
        //                    if (ValidationRegexOptionalFields.Contains(FieldsL[k]) /*CurrentFieldRegex.StartsWith("^(?:") && CurrentFieldRegex.EndsWith(")?$")*/)
        //                    {
        //                        //if optional field, then the space is already within the field itself
        //                        BuildingRegex = BuildingRegex + "(" + CurrentFieldRegex.Remove(CurrentFieldRegex.Length - 1).Remove(0, 1) + ")";
        //                    }
        //                    else
        //                    {

        //                        BuildingRegex = BuildingRegex + "(" + " " + CurrentFieldRegex.Remove(CurrentFieldRegex.Length - 1).Remove(0, 1) + ")";
        //                    }

        //                }

        //                var TestingRegex = k == FieldsL.Count - 1 ? "^" + BuildingRegex + "$" : "^" + BuildingRegex + "( ?)";

        //                Regex InvalidedRegex = new Regex(TestingRegex, RegexOptions.IgnoreCase);
        //                var MatchNowOrNot = InvalidedRegex.Match(CleanedMessage.Trim());

        //                if (!MatchNowOrNot.Success)
        //                {
        //                    //Validation has failed at this field, therefore we assume this field to be the one causing the error.
        //                    FunctRes.ListOfReasonsForPossibleFailures.Add(FieldsL[k] + " Is Not Valid!");
        //                }

        //            }

        //            FunctRes.Schedule.IsValid = false;
        //            FunctRes.IsSavable = false;

        //            //FunctRes.ListOfResponsesForPossibleFailures.Add(ErrorMessageGeneric);
        //        }

        //        #endregion

        //        //Validation has completed, if IsValid survives, add it as true

        //        // CompleteReturnObject.Add("SuccessMessage", WarningList.Count > 0 ? "Matching has not been completed succesfully. " : "Matched Successfully. Please Submit.");

        //        //Decide whether to save Schedulefields based on validity
        //        string AppId;

        //        FunctRes.Schedule.Reason = FunctRes.ListOfReasonsForPossibleFailures.Count == 0 ? "" : FunctRes.ListOfReasonsForPossibleFailures[0].ToString();
        //        //FunctRes.Schedule.Response = FunctRes.ListOfResponsesForPossibleFailures.Count == 0 ? ValidMessage : FunctRes.ListOfResponsesForPossibleFailures[0].ToString();

        //        FunctRes.Schedule = SaveSchedule(FunctRes.Schedule, FunctRes.IsSavable);
        //        AppId = FunctRes.Schedule.AppId;

        //        FunctRes.message = "Successfully Saved with Schedule ID : " + AppId;
        //        return FunctRes;

        //        //Call Custom Logic Loop of After_Submit

        //        //}
        //        //catch (Exception ex)
        //        //{
        //        //    return new ReturnFail() { message = ex.ToString(), SendSMS = false };
        //        //}

        //    }
        //}

        #endregion


        //#region Online

        public FunctionResult_Models InsertSchedule(Schedules Schedule)
        {
            try
            {
                bool __lockWasTaken = false;
                //lock object                    
                try
                {
                    Monitor.Enter(Globals.balanceLock, ref __lockWasTaken);
                    //Validation before matchfields
                    var ValidationResult_Modelss = PreMatchFieldValidations(DateTime.Now);
                    if (!ValidationResult_Modelss.IsSuccessful)
                    {
                        return new FunctionResult_Models(false) { message = ValidationResult_Modelss.ReasonForFailure };
                    }

                    var FunctRes = new FunctionResult_Models(true);

                    FunctRes.Schedule.IsValid = true;
                    var regex = new Regex("");
                    var Match = regex.Match("");

                    regex = new Regex(ValidationRegexMobileNo, RegexOptions.IgnoreCase);
                    Match = regex.Match(Schedule.MobileNo.Trim());

                    string appId = Cookies_Get("uAppId");

                    if (string.IsNullOrEmpty(appId))
                    {
                        FunctRes.Schedule.IsValid = false;

                        FunctRes.ListOfReasonsForPossibleFailures.Add("App Id is Invalid!");
                    }
                    if (!Match.Success)
                    {
                        FunctRes.Schedule.IsValid = false;

                        FunctRes.ListOfReasonsForPossibleFailures.Add("Mobile is Invalid!");
                    }
                    
                    if (Schedule.EventDate.Equals(DateTime.MinValue))
                    {
                        FunctRes.Schedule.IsValid = false;

                        FunctRes.ListOfReasonsForPossibleFailures.Add("Event Date is Invalid!");
                    }                    

                    FunctRes.Schedule.Reason = FunctRes.ListOfReasonsForPossibleFailures.Count == 0 ? "" : FunctRes.ListOfReasonsForPossibleFailures[0].ToString();
                    FunctRes.Schedule.Response = FunctRes.ListOfResponsesForPossibleFailures.Count == 0 ? "" : FunctRes.ListOfResponsesForPossibleFailures[0].ToString();
                    FunctRes.Schedule.MobileNo = Schedule.MobileNo;
                    FunctRes.Schedule.EventDate = Schedule.EventDate;
                    FunctRes.Schedule.Custom1 = Schedule.Custom1;
                    FunctRes.Schedule.Custom2 = Schedule.Custom2;
                    FunctRes.Schedule.Custom3 = Schedule.Custom3;
                    FunctRes.Schedule.AppId = appId;

                    //Validation Success
                    //Run Additional logics , duplication etc.


                    using (var db = new BaseEntities())
                    {

                        //Validate LOGIC using MatchedFields
                        //ValidationResult_Models = ValidationLogics(FunctRes.Schedule, "WEB");
                        //if (!ValidationResult_Modelss.IsSuccessful)
                        //{
                        //    return new FunctionResult_Models(false) { message = ValidationResult_Modelss.ReasonForFailure };
                        //}

                        //Should make sure Schedule is savable before sending to AZS

                        //string fl;
                        //var fileresult = SendBlobToAzureStorage(Pfile, Schedule.FileName, out fl);

                        //if (!fileresult.Valid)
                        //{
                        //    return fileresult;
                        //}

                        //FunctRes.Schedule.FileLink = fl;
                        //FunctRes.Schedule.Response = ValidMessageOnline;
                        //Save Schedule after passing all checks
                        FunctRes.Schedule = SaveSchedule(FunctRes.Schedule, true);

                        FunctRes.message = ValidMessageOnline.Replace("{mobileNo}", FunctRes.Schedule.MobileNo); // "Successfully saved with ID : " + FunctRes.Schedule.AppId.ToString();

                        return FunctRes;
                    }
                }
                finally
                {
                    if (__lockWasTaken) System.Threading.Monitor.Exit(Globals.balanceLock);
                }
            }
            catch (Exception ex)
            {
                return new FunctionResult_Models(false) { message = ex.ToString() };
            }


        }


        //public FunctionResult_Models CompleteSchedule(OnlineSchedule OSchedule, HttpPostedFileBase Pfile, out string responsestring)
        //{
        //    try
        //    {

        //        ////Validation before matchfields
        //        //var ValidationResult_Modelss = PreMatchFieldValidations(DateTime.Now);
        //        //if (!ValidationResult_Modelss.IsSuccessful)
        //        //{
        //        //    return new FunctionResult_Models(false) { message = ValidationResult_Modelss.ReasonForFailure };
        //        //}
        //        ////Validation before matchfields

        //        var results = new Dictionary<string, object>();

        //        var AppId = Convert.ToInt32(OSchedule.AppId);
        //        var MobileNo = OSchedule.MobileNo;


        //        using (var db = new BaseEntities())
        //        {
        //            //Validate MobileNumber as well
        //            var ListOfEntries = db.Schedules.Where(s => s.AppId == AppId && s.MobileNo == MobileNo && s.IsValid).ToList();
        //            if (ListOfEntries.Count == 0)
        //            {
        //                responsestring = "";
        //                return new FunctionResult_Models(false) { message = "Unable to find Schedule." };
        //            }

        //            var Schedule = ListOfEntries[0];

        //            //Should make sure Schedule is savable before sending to AZS

        //            string fl;
        //            var fileresult = SendBlobToAzureStorage(Pfile, OSchedule.FileName, out fl);

        //            if (!fileresult.Valid)
        //            {
        //                responsestring = "";
        //                return fileresult;
        //            }

        //            Schedule.FileLink = fl;

        //            db.SaveChanges();
        //            //Schedule.Response = ValidMessageOnline;
        //            //Save Schedule after passing all checks
        //            responsestring = ValidMessageOnlineCompletion;
        //            return new FunctionResult_Models(true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        responsestring = "";
        //        return new FunctionResult_Models(false) { message = ex.ToString() };
        //    }
        //}


        public FunctionResult_Models UploadEntries(HttpPostedFileBase Pfile)
        {

            try
            {
                //Validation Success
                //Run Additional logics , duplication etc.

                var MessageList = new List<string>();

                using (var db = new BaseEntities())
                {
                    if (Pfile != null && Pfile.ContentLength > 0 && Path.GetExtension(Pfile.FileName) == ".csv")
                    {
                        //Current set of data :
                        var CsvData = new List<Schedules>();

                        using (TextFieldParser parser = new TextFieldParser(Pfile.InputStream))
                        {
                            parser.CommentTokens = new string[] { "#" };
                            parser.SetDelimiters(new string[] { "," });
                            parser.HasFieldsEnclosedInQuotes = true;



                            //  parser.ReadLine();
                            bool firstLine = true;
                            int EventDateIndex=0, MobileNoIndex=0, Custom1Index=0, Custom2Index=0, Custom3Index=0; 
                            while (!parser.EndOfData)
                            {
                                string[] fields = parser.ReadFields();

                                // get the column headers
                                if (firstLine)
                                {
                                    firstLine = false;
                                    EventDateIndex = Array.IndexOf(fields, "EventDate");
                                    MobileNoIndex = Array.IndexOf(fields, "MobileNo");
                                    Custom1Index = Array.IndexOf(fields, "Custom1");
                                    Custom2Index = Array.IndexOf(fields, "Custom2");
                                    Custom3Index = Array.IndexOf(fields, "Custom3");
                                    continue;
                                }

                                

                                //Assume Column 1 = UniqueCode, Column 2 = StartDate, Column 3 = End Date
                                
                                ////Mini validation

                                //var IsValid = true;
                                //var Reason = "";

                                
                                DateTime s;
                                if (!DateTime.TryParseExact(fields[EventDateIndex], "dd MMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out s))
                                {
                                    s = FromUTCToLocal(DateTime.MinValue);
                                }

                                CsvData.Add(new Schedules()
                                {
                                    //MobileNo,Name,NRIC,ReceiptNo,Amount

                                    //CreatedOn = FromLocalToUTC(s),
                                    CreatedOn = FromLocalToUTC(DateTime.Now),
                                    MobileNo = fields[MobileNoIndex],
                                    EventDate = FromLocalToUTC(s),
                                    Custom1 = Custom1Index>0?fields[Custom1Index]:string.Empty,
                                    Custom2 = Custom2Index>0?fields[Custom2Index]:string.Empty,
                                    Custom3 = Custom3Index > 0?fields[Custom3Index]:string.Empty,
                                    /*
                                    //If Online Submission
                                    EntryText = "",
                                    Name = fields[1],
                                    NRIC = fields[2],
                                    ReceiptNo = fields[3],
                                    Amount = Convert.ToDecimal(fields[4]),
                                    //Chances = s,
                                    //Group = fields[5].ToUpper(),
                                    //Reason = Reason,
                                    //IsValid = IsValid,
                                    ScheduleSource = "WEB",
                                    */

                                    //If SMS Route
                                    //EntryText = fields[2], //fields[0] + " " + fields[1] + " " + fields[2] + " " + fields[3] + " " + fields[4]

                                });

                                if (fields == null)
                                {
                                    break;
                                }

                            }
                        }

                        string appId = Cookies_Get("uAppId");

                        if (!string.IsNullOrEmpty(appId))
                        {
                            //purge existing
                            PurgeEntriesByAppId(appId);
                            for (var i = 0; i < CsvData.Count; i++)
                            {
                                //string msg = CsvData[i].EventDate.ToString();

                                //if (!string.IsNullOrEmpty(CsvData[i].Custom1)) msg += " " + CsvData[i].Custom1;
                                //if (!string.IsNullOrEmpty(CsvData[i].Custom2)) msg += " " + CsvData[i].Custom2;
                                //if (!string.IsNullOrEmpty(CsvData[i].Custom3)) msg += " " + CsvData[i].Custom3;

                                MessageList.Add(InsertSchedule(new Schedules()
                                {
                                    CreatedOn = DateTime.Parse(CsvData[i].CreatedOn.ToString("yyyy-MM-dd HH:mm:ss")).ToUniversalTime(),
                                    EventDate = CsvData[i].EventDate,
                                    Custom1 = CsvData[i].Custom1 ?? string.Empty,
                                    Custom2 = CsvData[i].Custom2 ?? string.Empty,
                                    Custom3 = CsvData[i].Custom3 ?? string.Empty,
                                    MobileNo = CsvData[i].MobileNo ?? string.Empty
                                }).message);
                            }
                        }
                    }
                    else
                    {
                        return new FunctionResult_Models(false) { message = "File is invalid! Only CSV Files are accepted!" };
                    }

                    return new FunctionResult_Models(true) { message = "CSV Uploaded!"};
                }
            }
            catch (Exception ex)
            {
                return new FunctionResult_Models(false) { message = ex.ToString() };
            }

        }

        //public FunctionResult_Models UploadGroupEntries(HttpPostedFileBase Pfile)
        //{

        //    try
        //    {
        //        //Validation Success
        //        //Run Additional logics , duplication etc.

        //        var MessageList = new List<string>();

        //        using (var db = new BaseEntities())
        //        {
        //            if (Pfile != null && Pfile.ContentLength > 0 && Path.GetExtension(Pfile.FileName) == ".csv")
        //            {
        //                //Current set of data :
        //                var CsvData = new List<Schedules>();

        //                using (TextFieldParser parser = new TextFieldParser(Pfile.InputStream))
        //                {
        //                    parser.CommentTokens = new string[] { "#" };
        //                    parser.SetDelimiters(new string[] { "," });
        //                    parser.HasFieldsEnclosedInQuotes = true;



        //                    //  parser.ReadLine();

        //                    while (!parser.EndOfData)
        //                    {
        //                        string[] fields = parser.ReadFields();

        //                        ////Mini validation

        //                        //var IsValid = true;
        //                        //var Reason = "";




        //                        CsvData.Add(new Schedules()
        //                        {
        //                            //MobileNo,Name,NRIC,ReceiptNo,Amount

        //                            DateSchedule = DateTime.UtcNow,
        //                            //MobileNo = fields[1],
        //                            /*
        //                            //If Online Submission
        //                            EntryText = "",
        //                            Name = fields[1],
        //                            NRIC = fields[2],
        //                            ReceiptNo = fields[3],
        //                            Amount = Convert.ToDecimal(fields[4]),
        //                            //Chances = s,
        //                            //Group = fields[5].ToUpper(),
        //                            //Reason = Reason,
        //                            //IsValid = IsValid,
        //                            ScheduleSource = "WEB",
        //                            */

        //                            //If SMS Route
        //                            EntryText = fields[0] + " " + fields[1] + " " + fields[2] + " " + fields[3] + " " + fields[4] + " " + fields[5] //fields[2], 

        //                        });

        //                        if (fields == null)
        //                        {
        //                            break;
        //                        }

        //                    }
        //                }


        //                for (var i = 0; i < CsvData.Count; i++)
        //                {
        //                    MessageList.Add(SubmitSchedule(new Parameters_Models()
        //                    {
        //                        createdon = CsvData[i].DateSchedule.ToString("yyyy-MM-dd HH:mm:ss"),
        //                        ScheduleSource = "WEB",
        //                        FileLink = "",
        //                        Message = CsvData[i].EntryText,
        //                        MobileNo = "0" //CsvData[i].MobileNo
        //                    }).message);
        //                }

        //            }
        //            else
        //            {
        //                return new FunctionResult_Models(false) { message = "File is invalid! Only CSV Files are accepted!" };
        //            }

        //            return new FunctionResult_Models(true) { message = "CSV Uploaded!" + String.Join("<br/>", MessageList)/**/ };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new FunctionResult_Models(false) { message = ex.ToString() };
        //    }

        //}
        //#endregion



        #region Logics

        public Schedules EditSchedule(Schedules Schedule)
        {
            //Edit Schedule Logics



            return Schedule;
        }


        public ValidationResult_Models PreMatchFieldValidations(DateTime dt)
        {
            //Campaign Date Checking
            if (dt < TestDate || dt > EndDate)
            {

                return new ValidationResult_Models(false)
                {
                    ReasonForFailure = "Not In Campaign Period",
                    ResponseForFailure = "Error : Not within campaign period (From " + TestDate + " to " + EndDate + ")"
                };


            }
            //Campaign Date Checking

            return new ValidationResult_Models(true);

        }

        //public ValidationResult_Models ValidationLogics(Schedules Entry, string type)
        //{
        //    using (var db = new BaseEntities())
        //    {

        //        //Validation

        //        #region Validation

        //        var regex = new Regex("");
        //        var Match = regex.Match("");



        //        if (type == "WEB") //Only online Entries need to validate Field By Field.
        //        {
        //            regex = new Regex(ValidationRegexMobileNo, RegexOptions.IgnoreCase);
        //            Match = regex.Match(Entry.MobileNo.Trim());

        //            if (!Match.Success)
        //            {
        //                return new ValidationResult_Models(false) { ReasonForFailure = "Mobile is Invalid!", ResponseForFailure = ErrorMessageGeneric };
        //            }



        //            regex = new Regex(ValidationRegexName, RegexOptions.IgnoreCase);
        //            Match = regex.Match(Entry.Name.Trim());

        //            if (!Match.Success)
        //            {
        //                return new ValidationResult_Models(false) { ReasonForFailure = "Name is Invalid!", ResponseForFailure = ErrorMessageGeneric };
        //            }


        //            regex = new Regex(ValidationRegexNRIC, RegexOptions.IgnoreCase);
        //            Match = regex.Match(Entry.NRIC.Trim());

        //            if (!Match.Success)
        //            {
        //                return new ValidationResult_Models(false) { ReasonForFailure = "NRIC is Invalid!", ResponseForFailure = ErrorMessageGeneric };
        //            }


        //            regex = new Regex(ValidationRegexReceiptNo, RegexOptions.IgnoreCase);
        //            Match = regex.Match(Entry.ReceiptNo.Trim());

        //            if (!Match.Success)
        //            {
        //                return new ValidationResult_Models(false) { ReasonForFailure = "ReceiptNo is Invalid!", ResponseForFailure = ErrorMessageGeneric };
        //            }


        //            regex = new Regex(ValidationRegexEmail, RegexOptions.IgnoreCase);
        //            Match = regex.Match(Entry.Email.Trim());

        //            if (!Match.Success)
        //            {
        //                return new ValidationResult_Models(false) { ReasonForFailure = "Email is Invalid!", ResponseForFailure = ErrorMessageGeneric };
        //            }

        //            regex = new Regex(ValidationRegexDOB, RegexOptions.IgnoreCase);
        //            Match = regex.Match(Entry.DOB.Trim());

        //            if (!Match.Success)
        //            {
        //                return new ValidationResult_Models(false) { ReasonForFailure = "DOB is Invalid!", ResponseForFailure = ErrorMessageGeneric };
        //            }


        //        }

        //        DateTime dt;
        //        if (DateTime.TryParseExact(Entry.DOB, "ddMMyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt) == false
        //           && DateTime.TryParseExact(Entry.DOB, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt) == false)
        //        {

        //            return new ValidationResult_Models(false) { ReasonForFailure = "DOB is Invalid!", ResponseForFailure = ErrorMessageGeneric };
        //        }

        //        if (Convert.ToDecimal(Entry.Amount) < TierAmount)
        //        {
        //            return new ValidationResult_Models(false) { ReasonForFailure = "Amount is below " + TierAmount.ToString() + "!", ResponseForFailure = ErrorMessageGeneric };
        //        }

        //        #endregion
        //        var EntryMobile = (Entry.MobileNo.Substring(0, 1) == "+" ?/*2*/ Entry.MobileNo.Substring(1, Entry.MobileNo.Length - 1) :/*2*/ Entry.MobileNo);
                
        //        //if managed to pass all logics then return true
        //        return new ValidationResult_Models(true);
        //    }

        //}
        #endregion



        #region Saving

        public Schedules SaveSchedule(Schedules Schedule, bool savable)
        {
            try
            {
                //DefaultValues Overriding
                
                //If dont have value then set def value
                Schedule.CreatedOn = Schedule.CreatedOn == DateTime.MinValue ? DateTime.UtcNow : Schedule.CreatedOn;
                Schedule.EventDate = Schedule.EventDate == DateTime.MinValue ? DateTime.UtcNow : Schedule.EventDate;
                Schedule.Response = Schedule.Response == null || Schedule.Response == "" ? "" : Schedule.Response;
                Schedule.Reason = Schedule.Reason == null || Schedule.Reason == "" ? "" : Schedule.Reason;
                Schedule.MobileNo = Schedule.MobileNo == null | Schedule.MobileNo == "" ? "" : Schedule.MobileNo;

                using (var db = new BaseEntities())
                {

                    //Calc Chances

                    //if (savable)
                    //{
                    //    //Schedule.Chances = Schedule.IsValid ? 1 : 0; /*ChancesCalcs(Schedule); //For Contests with Tier Logic*/
                    //                                           //Schedule.NRIC_NoPrefix = Char.IsLetter(Schedule.NRIC.FirstOrDefault()) ? Schedule.NRIC.Substring(1, Schedule.NRIC.Length - 1) : Schedule.NRIC;
                    //}
                    //else
                    //{
                    //    Schedule.Chances = 0;
                    //    Schedule.NRIC = "";
                    //    Schedule.Name = "";
                    //    Schedule.NRIC_NoPrefix = "";
                    //    Schedule.ReceiptNo = "";
                    //    Schedule.Amount = 0;
                    //    Schedule.FileLink = "";
                    //    Schedule.GroupName = "";
                    //    Schedule.NoOfDays = 0;
                    //    Schedule.IsInExcelB = "No";
                    //    Schedule.IsInExcelC = "No";                        
                    //}

                    db.Schedules.Add(Schedule);
                    db.SaveChanges();

                    /* Replacer for Valid Message */
                    //if (savable && Schedule.IsValid)
                    //{
                    //    //Schedule.Response = ValidMessage.Replace("{uploadlink}", UploadLink);
                    //    db.SaveChanges();
                    //}

                    return Schedule;
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }


        }

        #endregion



        #region Views

        public FunctionResult_Models GetEntries(Options_Models Options)
        {
            using (var db = new BaseEntities())
            {

                var FunctRes = new FunctionResult_Models(true);


                var Query = db.Schedules.Where(s => s.AppId == Options.AppId);


                //Filter Validity

                Query = Options.isSent.ToUpper() == "True".ToUpper() ? Query.Where(s => (bool)s.IsSent == true) :
                    Options.isSent.ToUpper() == "False".ToUpper() ? Query.Where(s => (bool)s.IsSent == false) :
                    Query;


                //Other Filters?

                var query = Query.OrderByDescending(s => s.AppId).Skip((Options.Page - 1) * Options.PageSize).Take(Options.PageSize).AsEnumerable();

                //var SAS = GetSASToken();

                var count = query.Count();

                FunctRes.TotalCount = Query.Count();

                if (count == 0)
                {
                    return new FunctionResult_Models(false) { message = "No Entries Found!" };
                }

                

                FunctRes.DataHeaders = ExcludeHeaders(query.FirstOrDefault(), ScheduleExclusionFields.ToList());

                FunctRes.DataAsDictionary = ConvertEntriesToDictionary(query, ScheduleExclusionFields.ToList());

                return FunctRes;

            }
        }

        public FunctionResult_Models GetEntriesCSV(Options_Models Options)
        {
            using (var db = new BaseEntities())
            {
                var FunctRes = new FunctionResult_Models(true);

                var Query = db.Schedules.Where(s => s.AppId == Options.AppId);

                //Filter Validity                

                var query = Query.AsEnumerable();

                var Excludes = ScheduleExclusionFields.ToList();


                //Hide these 2
                Excludes.Add("SchedulesId");

                if (query.Count() == 0)
                {
                    return new FunctionResult_Models(false) { message = "No Entries Found!" };
                }

                FunctRes.DataHeaders = ExcludeHeaders(query.FirstOrDefault(), Excludes);

                FunctRes.DataAsDictionary = query.OrderByDescending(s => s.CreatedOn).ThenByDescending(s => s.AppId).Select(s =>
                    ExcludeHeaders(s.GetType().GetProperties().Where(p => p.GetMethod.IsVirtual == false).ToDictionary(prop => prop.Name, prop => prop.GetValue(s, null)), Excludes, true).
                    ToDictionary(prop => prop.Key, prop => prop.Value)).ToList();

                return FunctRes;

            }
        }

        #endregion

        #region Purge



        public FunctionResult_Models PurgeEntries()
        {
            using (var db = new BaseEntities())
            {
                db.Schedules.RemoveRange(db.Schedules);
                db.SaveChanges();

                return new FunctionResult_Models(true) { message = "Successfully Purged!" };
            }

        }

        public FunctionResult_Models PurgeEntriesByAppId(string appId)
        {
            using (var db = new BaseEntities())
            {
                IQueryable<Schedules> schedules = db.Schedules.Where(s => s.AppId == appId);
                db.Schedules.RemoveRange(schedules);
                db.SaveChanges();

                return new FunctionResult_Models(true) { message = "Successfully Purged!" };
            }

        }

        public FunctionResult_Models PurgeSelectedEntries(List<int> SchedulesId)
        {
            using (var db = new BaseEntities())
            {
                var Schedules = db.Schedules.Where(s => SchedulesId.Contains(s.SchedulesId));
                db.Schedules.RemoveRange(Schedules);

                db.SaveChanges();
                return new FunctionResult_Models(true) { message = "Successfully Purged!" };

            }

        }
        #endregion




        public Dictionary<string, object> ContestInfo()
        {
            //create new object
            //manually define object perperties

            return typeof(Repository)
              .GetFields(BindingFlags.Public | BindingFlags.Static)
              .ToDictionary(f => f.Name,
                            f => f.GetValue(null));
        }







    }
}