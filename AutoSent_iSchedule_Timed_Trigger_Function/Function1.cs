using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using iSchedule.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;
using System.Data;
using iSchedule.BLL;
using Microsoft.AspNetCore.Mvc;

namespace AutoSent_iSchedule_Timed_Trigger_Function
{
    public static class Function1
    {
        //[FunctionName("Function1")]
        //public static void Run([TimerTrigger("0 * * * * *")]TimerInfo myTimer, ILogger log)
        //{
        //    log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        //}
        [FunctionName("Timed_Trigger_AutoSentMessage")]
        public static void Run([TimerTrigger("0 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            try
            {
                //Settings_BLL settings_BLL = new Settings_BLL();
                //Schedules_BLL schedules_BLL = new Schedules_BLL();

                var config = new ConfigurationBuilder()
                 .SetBasePath(Environment.CurrentDirectory)
                 .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                 .AddEnvironmentVariables()
                 .Build();
                string connString = config["DefaultConnection"];
                int AddLocalTimeZone = Convert.ToInt32(config["AddLocalTimeZone"]);
                int totalSent = 0;
                string createdon = ""; //req.Query["createdon"];

                DateTime localNow = (createdon == "" || createdon == null) ? Convert.ToDateTime(DateTime.UtcNow.ToString("hh:mm:ss")) : Convert.ToDateTime(createdon);
                DataTable table = new DataTable();
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    List<Settings> runningSettings = new List<Settings>();

                    string query = "select * from settings where cast(scheduletime as time) = @_selectedTime";
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@_selectedTime", localNow.TimeOfDay);
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            Settings objSettings = new Settings();

                            objSettings.SettingsID = reader["SettingsID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["SettingsID"]);
                            objSettings.CreatedOn = reader["CreatedOn"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedOn"]);
                            objSettings.AppId = reader["AppId"].ToString() ?? reader["AppId"].ToString();
                            objSettings.AppSecret = reader["AppSecret"].ToString() ?? reader["AppSecret"].ToString();
                            objSettings.Scheduletime = reader["Scheduletime"] == DBNull.Value ? TimeSpan.MinValue : TimeSpan.Parse(reader["Scheduletime"].ToString());
                            objSettings.MessageTemplate = reader["MessageTemplate"].ToString() ?? reader["MessageTemplate"].ToString();
                            objSettings.IsActive = reader["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsActive"]);
                            objSettings.LastModified = reader["LastModified"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["LastModified"].ToString());
                            objSettings.UserId = reader["UserId"].ToString() ?? reader["AppId"].ToString();

                            runningSettings.Add(objSettings);
                        }
                        foreach (var item in runningSettings)
                        {
                            // get all schedules where isSent = 0
                            List<Schedules> upcomingSchedules = new List<Schedules>();
                            query = "select * from schedules where isSent = 0 and isvalid = 1 and appid = @_appid";
                            command = new SqlCommand(query, conn);
                            command.Parameters.AddWithValue("@_appid", item.AppId);
                            reader = command.ExecuteReader();
                            try
                            {
                                while (reader.Read())
                                {
                                    Schedules objSchedule = new Schedules();
                                    objSchedule.SchedulesId = reader["SchedulesId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["SchedulesId"]);
                                    objSchedule.AppId = reader["AppId"].ToString() ?? reader["AppId"].ToString();
                                    objSchedule.CreatedOn = reader["CreatedOn"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedOn"]);
                                    objSchedule.MobileNo = reader["MobileNo"].ToString() ?? reader["MobileNo"].ToString();
                                    objSchedule.IsValid = reader["IsValid"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsValid"]);
                                    objSchedule.Reason = reader["Reason"].ToString() ?? reader["Reason"].ToString();
                                    objSchedule.Response = reader["Response"].ToString() ?? reader["Response"].ToString();
                                    objSchedule.EventDate = reader["EventDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["EventDate"]);
                                    objSchedule.Custom1 = reader["Custom1"].ToString() ?? reader["Custom1"].ToString();
                                    objSchedule.Custom2 = reader["Custom2"].ToString() ?? reader["Custom2"].ToString();
                                    objSchedule.Custom3 = reader["Custom3"].ToString() ?? reader["Custom3"].ToString();
                                    objSchedule.IsSent = reader["IsSent"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsSent"]);
                                    objSchedule.SentOn = reader["SentOn"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["SentOn"]);
                                    upcomingSchedules.Add(objSchedule);
                                }
                                //foreach schedules , send SMS and set isSent = 0, SentOn = localNow
                                foreach (var it in upcomingSchedules)
                                {
                                    if (it.EventDate.AddHours(AddLocalTimeZone).Date == DateTime.UtcNow.Date)
                                    {
                                        bool successSent = GeneralFunctions.SendSms(Convert.ToInt32(item.AppId), new Guid(item.AppSecret), it.MobileNo,
                                            item.MessageTemplate.Replace("{custom1}", it.Custom1).Replace("{custom2}", it.Custom2).Replace("{custom3}", it.Custom3));

                                        //update isSent and sentOn
                                        query = "update schedules set IsSent = @_IsSent, SentOn = @_SentOn where [SchedulesId] = @_SchedulesId";
                                        command = new SqlCommand(query, conn);
                                        command.Parameters.AddWithValue("@_IsSent", successSent);
                                        if (successSent)
                                        {
                                            command.Parameters.AddWithValue("@_SentOn", localNow);
                                            totalSent++;
                                        }
                                        command.Parameters.AddWithValue("@_SchedulesId", it.SchedulesId);
                                        command.ExecuteNonQuery();

                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
                log.LogInformation($"Success trigger at " + localNow + ", " + totalSent + " SMS sent");
            }
            catch (Exception ex)
            {
            }
        }
    }
}
