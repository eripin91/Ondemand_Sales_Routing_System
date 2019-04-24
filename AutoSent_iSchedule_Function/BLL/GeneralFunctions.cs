using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Data;
using System.Net;


namespace iSchedule.BLL
{
    public static class GeneralFunctions
    {
        public static bool SendSms(int AppID, Guid AppSecret, string receivers, string content)
        {
            try
            {
                // create the web request with the url to the web
                // service with the method name added to the end
                HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create("http://www.smsdome.com/api/http/sendsms.aspx");

                // add the parameters as key valued pairs making
                // sure they are URL encoded where needed
                ASCIIEncoding encoding = new ASCIIEncoding();
                //byte[] postData = encoding.GetBytes("createdon=" + dt + "&MobileNo=" + MobileNo + "&Message=" + Message);
                byte[] postData = encoding.GetBytes("AppID=" + AppID + "&AppSecret=" + AppSecret + "&receivers=" + receivers + "&content=" + HttpUtility.UrlEncode(content) + "&responseformat=XML");
                httpReq.ContentType = "application/x-www-form-urlencoded";
                httpReq.Method = "POST";
                httpReq.ContentLength = postData.Length;

                // convert the request to a steeam object and send it on its way
                Stream ReqStrm = httpReq.GetRequestStream();
                ReqStrm.Write(postData, 0, postData.Length);
                ReqStrm.Close();

                // get the response from the web server and
                // read it all back into a string variable
                HttpWebResponse httpResp = (HttpWebResponse)httpReq.GetResponse();
                StreamReader respStrm = new StreamReader(httpResp.GetResponseStream(), Encoding.ASCII);
                string result = respStrm.ReadToEnd();
                httpResp.Close();
                respStrm.Close();

                // show the result the test box for testing purposes
                //string result2 = result;
                return true;
                ////////////////////////////////////////////////////////
            }
            catch (Exception ex)
            {
                var x = ex.ToString();
                return false;
                //WriteToLogFile("Campaign Error: " + ex.Message);
            }
        }
    }
}
