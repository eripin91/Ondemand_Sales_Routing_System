using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.VisualBasic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml;
using System.Reflection;
using System.Drawing;
using ImageResizer;
using System.Text.RegularExpressions;
using iSchedule.Models;
using System.Security.Cryptography;

namespace iSchedule.BLL
{
    public class GenericFunctions : Constants
    {

        public string CleanMessage(Parameters_Models param)
        {

            //Remove Keyword and Append Number to String.
            var json = param.Message.ToString();
            json = json.Replace("\r\n", " ").Replace("\n\r", " ").Replace("\n", " ").Replace("\r", " ");
            json = GeneralFunctions.NormalizeSpaces(json).Trim();

            if (json.Trim().ToUpper().StartsWith(Keyword.ToUpper() + " "))
            {
                var KeywordNFirstSpace = json.Split(' ')[0];
                json = json.Remove(0, KeywordNFirstSpace.Length);
            }
            json = json.Trim();

            json = param.MobileNo.ToString() + " " + json;

            return json;


        }

        //public string GetSASToken()
        //{

        //    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

        //    // Retrieve a reference to a container.
        //    CloudBlobContainer container = blobClient.GetContainerReference(AzureStorageContainer);

        //    //// Create the container if it doesn't already exist.
        //    //container.CreateIfNotExists();

        //    //To set public
        //    container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

        //    if (container.Exists())
        //    {
        //        // SAS is for the container level, else have to generate at individual level
        //        var sasToken = container.GetSharedAccessSignature(new Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPolicy()
        //        {
        //            Permissions = Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPermissions.Read,
        //            SharedAccessExpiryTime = new DateTimeOffset(DateTime.UtcNow.AddDays(1))
        //        });

        //        // Return the SAS token.
        //        return sasToken;
        //    }
        //    throw new Exception("Token Generation for Link Failed!");
        //}


        //public FunctionResult_Models SendBlobToAzureStorage(HttpPostedFileBase file, string filename, out string AzureLink)
        //{
        //    try
        //    {
              
        //        byte[] imageBytes = null;
        //        BinaryReader rdr = new BinaryReader(file.InputStream);
        //        imageBytes = rdr.ReadBytes((int)file.ContentLength);

        //        //var b64s = base64String.ToString().Substring(base64String.ToString().IndexOf(',') + 1);
        //        //byte[] imageBytes = Convert.FromBase64String(b64s);
        //        var mime = MimeDetective.GetMimeType(imageBytes, filename);

        //        //Check if file is image or pdf
        //        if (!mime.StartsWith(CheckType.Image) && mime != CheckType.PDF)
        //        {
        //            AzureLink = "";
        //            return new FunctionResult_Models(false) { message = "File is not an image or PDF" };
        //        }

        //        DateTime dt = DateTime.UtcNow;
        //        string fileNamePrefix = dt.ToString("yyMMdd_HHmmssfff");// + '_';
        //        string finalizedfilename = fileNamePrefix + Path.GetExtension(filename) /* + UniqueID.ToString() + '_' + */;
        //        Guid UniqueID = Guid.NewGuid();


        //        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

        //        // Retrieve a reference to a container.
        //        CloudBlobContainer container = blobClient.GetContainerReference(AzureStorageContainer);

        //        //// Create the container if it doesn't already exist.
        //        container.CreateIfNotExists();

        //        container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

        //        // Retrieve reference to a blob named "myblob".
        //        CloudBlockBlob blockBlob = container.GetBlockBlobReference(finalizedfilename);

        //        // Create or overwrite the "myblob" blob with contents from a local file.
        //        blockBlob.Properties.ContentType = mime;

        //        //Resize image
        //        var resizedImage = ResizeImage(imageBytes);

        //        blockBlob.UploadFromByteArray(resizedImage, 0, resizedImage.Length);

        //        blockBlob.SetProperties();

        //        AzureLink = AzureStorageLink + AzureStorageContainer + "/" + finalizedfilename;
        //        return new FunctionResult_Models(true);

        //    }
        //    catch (Exception ex)
        //    {
        //        var x = ex;
        //        AzureLink = "";
        //        return new FunctionResult_Models(false) { message = "File Saving Failed!" };
        //    }


        //}



        #region OverLoadedMethods for Excluding Headers


        public List<string> ExcludeHeaders(Schedules query, List<string> Excludes)
        {
            var qu = query.GetType().GetProperties().Where(p => p.GetMethod.IsVirtual == false).AsEnumerable();
            qu = qu.Where(c => !Excludes.Contains(c.Name));

            return qu.Select(s => s.Name).ToList();
        }

        public List<string> ExcludeHeaders(Settings query, List<string> Excludes)
        {
            //Possible to merge 2 list of porperties if u dont intend to create a dictionary/object from it
            var qu = query.GetType().GetProperties().Where(p => p.GetMethod.IsVirtual == false).Select(s=>s.Name)
                                                      //.Union(query.Schedules.GetType().GetProperties().Where(p => p.GetMethod.IsVirtual == false).Select(s=>s.Name))
                                                      .Where(y => !Excludes.Contains(y))
                                                      .AsEnumerable();
            return qu.Select(s => s).ToList();
        }
        public List<string> ExcludeHeaders(IEnumerable<KeyValuePair<string, object>> query, List<string> Excludes)
        {
            var qu = query.AsEnumerable();
            qu = qu.Where(c => !Excludes.Contains(c.Key));

            return qu.Select(s => s.Key).ToList();
        }
        public IEnumerable<KeyValuePair<string, object>> ExcludeHeaders(IEnumerable<KeyValuePair<string, object>> query, List<string> Excludes, bool returnKeyPair)
        {
            var qu = query.AsEnumerable();
            qu = qu.Where(c => !Excludes.Contains(c.Key));

            return qu;
        }
        public IEnumerable<KeyValuePair<string, object>> ExcludeHeaders(Dictionary<string, object> query, List<string> Excludes, bool returnKeyPair)
        {
            var qu = query.AsEnumerable();
            qu = qu.Where(c => !Excludes.Contains(c.Key));

            return qu;
        }

        public List<string> ExcludeHeaders(Dictionary<string, object> query, List<string> Excludes)
        {
            var qu = query.AsEnumerable();
            qu = qu.Where(c => !Excludes.Contains(c.Key));

            return qu.Select(s => s.Key).ToList();
        }


        #endregion


        public DateTime UTCNowAsLocal()
        {
            //Get Local Datetime of Now.
            return System.DateTime.UtcNow.AddHours(AddLocalTimeZone);
        }

        public DateTime LocalToday0000AsUTC()
        {
            var LocalToday0000 = UTCNowAsLocal().Date;  //Get 0000 of Today in Local Time.
            return LocalToday0000.AddHours(SubtractLocalTimeZone); //Convert  0000 of Today in Local Time back to UTC
        }

        public DateTime LocalToday2359AsUTC()
        {
            var LocalToday0000 = UTCNowAsLocal().Date.AddDays(1);  //Get 2359 of Today in Local Time.
            return LocalToday0000.AddHours(SubtractLocalTimeZone); //Convert  259 of Today in Local Time back to UTC
        }


        public DateTime FromLocalToUTC(DateTime dt)
        {
            return dt.AddHours(SubtractLocalTimeZone);
        }

        public DateTime FromUTCToLocal(DateTime dt)
        {
            return dt.AddHours(AddLocalTimeZone);
        }


        public string SerializerHelper(object objecttoserialize)
        {
            var x = JsonConvert.SerializeObject(objecttoserialize, Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings()
                    {
                        DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                    });
            //    var y = Regex.Replace(x, @"\""\\/Date\((-?\d+)\)\\/\""", "$1");
            return x;

        }

        public T DeserializerHelper<T>(string stringtodeserialize)
        {
            return JsonConvert.DeserializeObject<T>(stringtodeserialize, new JsonSerializerSettings()
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
            });
        }

        public int calculateLastPage(int totalcount, int pagesize)
        {
            var c = (int)Math.Ceiling((decimal)totalcount / (decimal)pagesize);
            return c <= 0 ? 1 : c;
        }

        public DataTable ListOfDictionaryToDataTable(object json)
        {
            var Entries = (List<Dictionary<string, object>>)json;

            var dt = new DataTable();
            foreach (KeyValuePair<string, object> kpv in Entries.FirstOrDefault())
            {
                if (kpv.Key == "DateEntry" || kpv.Key == "DateWon" || kpv.Key == "CreatedOn" || kpv.Key == "EventDate" || kpv.Key == "SentOn")
                {
                    dt.Columns.Add(kpv.Key, typeof(DateTime));
                }
                else
                {
                    dt.Columns.Add(kpv.Key);
                }

            }


            foreach (Dictionary<string, object> dictionary in Entries)
            {
                DataRow dataRow = dt.NewRow();

                foreach (string column in dictionary.Keys)
                {
                    if (column == "DateEntry" || column == "DateWon" || column=="CreatedOn" || column=="EventDate" || column=="SentOn")
                    {
                        if (dictionary[column] != null)
                        {
                            var LDate = Convert.ToDateTime(dictionary[column]).AddHours(AddLocalTimeZone);

                            dataRow[column] = LDate;//.ToString("dd-MM-yyyy HH:mm:ss");
                        }

                    }
                    else
                    {
                        dataRow[column] = dictionary[column];
                    }

                }

                dt.Rows.Add(dataRow);
            }
            return dt;
        }


        public List<Dictionary<string, object>> ConvertEntriesToDictionary(IEnumerable<Schedules> ListOfEntries, List<string> ExcludedFields)
        {
            using (var db = new BaseEntities())
            {

                //var WinnerNRICs = db.Settings.Select(s => s.NRIC_NoPrefix).ToList();
                //var WinnerMobileNos = db.Settings.Select(s => s.MobileNo).ToList();

                return ListOfEntries.Select(s => s.GetType().GetProperties().Where(p => p.GetMethod.IsVirtual == false)
                                        .ToDictionary(prop => prop.Name, prop => prop.GetValue(s, null))
                                        .Union(new List<KeyValuePair<string, object>>() { new KeyValuePair<string, object>("Checked", false) }.AsEnumerable())
                                        //If entry already has at least one winner tagged to it, then the button will not be visible to convert this entry to winner.
                                        //ischedule.Union(new List<KeyValuePair<string, object>>() { new KeyValuePair<string, object>("ExcludePastWinner", s.Settings.Count > 0 ? false : true) }.AsEnumerable())
                                        //.Union(new List<KeyValuePair<string, object>>() { new KeyValuePair<string, object>("ExcludePastWinner", s.Settings.Count > 0 ? false : WinnerNRICs.Contains(s.NRIC_NoPrefix) ? false : true) }.AsEnumerable())
                                        //.Union(new List<KeyValuePair<string, object>>() { new KeyValuePair<string, object>("ExcludePastWinner", s.Settings.Count > 0 ? false : WinnerMobileNos.Contains(s.MobileNo) ? false : true) }.AsEnumerable())
                                        .Where(y => !ExcludedFields.Contains(y.Key))
                                        //.Union(new List<KeyValuePair<string, object>>() { new KeyValuePair<string, object>("SAS", SAS) }.AsEnumerable())
                                        .ToDictionary(prop => prop.Key, prop => prop.Value)).ToList();

            }
        }


        public List<Dictionary<string, object>> ConvertWinnersToDictionary(IEnumerable<Settings> ListOfWinners, List<string> ExcludedFields)
        {
            return ListOfWinners.Select(s =>
                                            /*Convert Winner Properties to IEnum KPV excluding Entry*/
                                            s.GetType().GetProperties().Where(p => p.GetMethod.IsVirtual == false)
                                            .ToDictionary(prop => prop.Name, val => val.GetValue(s, null))
                                            /*Convert Entry Properties to IEnum KPV*/
                                            //.Union(s.Schedules.GetType().GetProperties().Where(p => p.GetMethod.IsVirtual == false).ToDictionary(prop => prop.Name, val => val.GetValue(s.Schedules, null)))
                                            /*Exclude list of fields to be excluded defined in web.config*/
                                            .Where(y => !ExcludedFields.Contains(y.Key))
                                            .ToDictionary(x => x.Key, y => y.Value)).ToList(); //
        }


        public int CheckAccountCreditBalance()
        {

            if (AppID != null && AppSecret != null && AppID != "" && AppSecret != "")
            {
                try
                {
                    var result = SendSms(Convert.ToInt32(AppID), new Guid(AppSecret), "", "");
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(result);

                    XmlNode content = doc.SelectSingleNode("/root/credit/balance");
                    if (content != null)
                    {
                        return Convert.ToInt32(content.FirstChild.Value);
                    }
                    else
                    {
                        throw new Exception("Unable to load element");
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog(ex.ToString());
                    return 0;
                }

            }
            else
            {
                return 0;
            }

        }

        public int CheckPartsCountOfMessage(string Message)
        {

            if (AppID != null && AppSecret != null && AppID != "" && AppSecret != "")
            {
                try
                {
                    var result = SendSms(Convert.ToInt32(AppID), new Guid(AppSecret), "", Message);
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(result);

                    XmlNode content = doc.SelectSingleNode("/root/content");
                    if (content != null)
                    {
                        return Convert.ToInt32(content.Attributes["parts"].Value);
                    }
                    else
                    {
                        throw new Exception("Unable to load element");
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog(ex.ToString());
                    return 0;
                }

            }
            else
            {
                return 0;
            }

        }

        /// <summary>
        /// Function to send SMS based on Contest App Specs
        /// </summary>
        public FunctionResult_Models SendSms(Schedules Entry, string respmessage)
        {
            if (AppID != null && AppSecret != null && AppID != "" && AppSecret != "" &&
                //Either Entry.Response or respmessage has to contain something
                ((Entry.Response != null && Entry.Response != "") || (respmessage != null && respmessage != "")))
            {
                try
                {
                    return new FunctionResult_Models(true)
                    {
                        message = SendSms(Convert.ToInt32(AppID), new Guid(AppSecret), Entry.MobileNo, /* Take respmessage when available */
                                                                                                        (respmessage != null && respmessage != "") ? respmessage
                                                                                                         : Entry.Response)
                    };
                }
                catch (Exception ex)
                {
                    ErrorLog(ex.ToString());
                    return new FunctionResult_Models(false) { message = "Sending SMS Failed!" };
                }

            }
            else
            {
                return new FunctionResult_Models(false) { message = "Contest spec validation failed!" };
            }

        }

        /// <summary>
        /// Worker method to send sms
        /// </summary>
        public string SendSms(int AppID, Guid AppSecret, string receivers, string content)
        {

            ASCIIEncoding encoding = new ASCIIEncoding();

            byte[] postData = encoding.GetBytes("AppID=" + AppID + "&AppSecret=" + AppSecret + "&receivers=" + receivers + "&content=" + HttpUtility.UrlEncode(content) + "&responseformat=XML");

            Stream stream = new MemoryStream(postData);

            HttpContent httpcontent = new StreamContent(stream);
            httpcontent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var uri = "http://www.smsdome.com/api/http/sendsms.aspx";

            using (var client = new HttpClient())
            {
                var response = client.PostAsync(uri, httpcontent).Result;
                return response.Content.ReadAsStringAsync().Result;
            }

        }



        public void ErrorLog(string ErrorMsg)
        {
            //Log to a file within a month
            var path = System.Web.HttpContext.Current.Server.MapPath("~/Logs");
            var boole = System.IO.Directory.Exists(path);
            if (!boole)
            {
                System.IO.Directory.CreateDirectory(path);
            }
            else
            {
                //Determine if File Exist if note create, then append to the csv
                var dtL = DateTime.UtcNow;
                var filename = dtL.ToString("yyyy") + dtL.ToString("MM") + ".csv";

                using (StreamWriter writer = new StreamWriter(path + "/" + filename, true))
                {
                    string RowDelimiter = Environment.NewLine;
                    char TextQualifier = Convert.ToChar("\"");
                    char ColumnDelimiter = '\0';
                    ColumnDelimiter = Convert.ToChar(",");
                    StringBuilder CurrentLine = new StringBuilder();
                    CurrentLine.Length = 0;

                    // Store DateTime	datetime(yyyymmdd-hhMMsstt)
                    CurrentLine.AppendFormat("{0}", dtL.ToString("yyyyMMdd-hhmmsstt"));
                    CurrentLine.AppendFormat("{0}", ColumnDelimiter);

                    // Store ContestID if applicable	int
                    CurrentLine.AppendFormat("{0}", ContestPW + "_" + ContestUser);
                    CurrentLine.AppendFormat("{0}", ColumnDelimiter);

                    // Store error message
                    // Escape field if it contains field separator, double quote and line break
                    if (ErrorMsg.Contains(ColumnDelimiter) || ErrorMsg.Contains("\"") || ErrorMsg.Contains(Microsoft.VisualBasic.Constants.vbCrLf) || ErrorMsg.Contains(Microsoft.VisualBasic.Constants.vbCr) ||
                                                               ErrorMsg.Contains(Microsoft.VisualBasic.Constants.vbLf))
                    {
                        // Add escape code for each double quote
                        if (ErrorMsg.Contains("\""))
                            ErrorMsg = ErrorMsg.Replace("\"", "\"\"");

                        // Add text qualifier for the field
                        CurrentLine.AppendFormat("\"{0}\"", ErrorMsg);
                    }
                    else
                    {
                        CurrentLine.AppendFormat("{0}", ErrorMsg);
                    }

                    CurrentLine.AppendFormat("{0}", ColumnDelimiter);


                    CurrentLine.AppendFormat("{0}", RowDelimiter);


                    writer.Write(CurrentLine);
                }


            }

        }


        public string GetConstantValues()
        {
            var Message = "";
            var ListOfConstants = base.GetType().GetFields(BindingFlags.Public| BindingFlags.Instance).ToList();
            for (int i = 0; i < ListOfConstants.Count; i++)
            {
                var Value = ListOfConstants[i].GetValue(this);

                if (Value.GetType().IsGenericType &&
           Value.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>))) {
                    Value = String.Join(",", ((List<string>)Value));
                }



                Message = Message == "" ? ListOfConstants[i].Name + " : " + Value :
                          Message + "<br />" + ListOfConstants[i].Name + " : " + Value;
            }
            return Message;

        }

        public byte[] ResizeImage(byte[] ImageInBytes) {
          
            //first MS for reading Image from byte[] ImageInBytes
            using (var ms = new MemoryStream(ImageInBytes))
            {
                //Run Resizer
                var settings = new ResizeSettings("width=500;format=jpg;mode=max");

                //second MS for the ImageResizer to workon
                using (var returnms = new MemoryStream())
                {
                    ImageResizer.ImageBuilder.Current.Build(Image.FromStream(ms), returnms, settings);

                    //return byte[]
                    return returnms.ToArray();

                }
                
            }            
        }

        public string ModifyValueByRegex(string word, string regex)
        {
            Regex rgx = new Regex(@regex);

            return rgx.Replace(word, "");
        }

        public byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }

        public string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }

        //public void Session_Set(string key, string value, DateTime expires)
        //{
        //    HttpCookie cookie = new HttpCookie(key, value);
        //    cookie.Expires = expires;
        //    HttpContext.Current.Response.SetCookie(cookie);
        //}

        //public string Session_Get(string key)
        //{
        //    string value = "";
        //    if (HttpContext.Current.Request.Cookies.AllKeys.Contains(key))
        //    {
        //        value = HttpContext.Current.Request.Cookies[key].Value;
        //    }

        //    return value;
        //}

        public void Session_Set(string key, string value)
        {
            HttpContext.Current.Session[key] = value;
        }

        public string Session_Get(string key)
        {
            string value = "";
            if (HttpContext.Current.Session[key]!=null)
            {
                value = HttpContext.Current.Session[key].ToString();
            }

            return value;
        }
    }
}