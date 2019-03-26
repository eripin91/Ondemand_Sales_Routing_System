using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using iSchedule.Models;
using System.IO;
using System.Data.Entity.Validation;

namespace iSchedule.BLL
{
    public class Schedules_BLL : GenericFunctions
    {
        public Schedules getScheduleBySchedulesId(int schedulesId)
        {
            using (var db = new BaseEntities())
            {
                return db.Schedules.FirstOrDefault(s => s.SchedulesId == schedulesId);
            }
        }
        public List<Schedules> getSchedulesByAppId(string appId)
        {
            using (var db = new BaseEntities())
            {
                return db.Schedules.Where(s=>s.AppId==appId).ToList();
            }
        }

        public List<Schedules> getAllUpcomingSchedulesByAppId(string appId)
        {
            using (var db = new BaseEntities())
            {
                return db.Schedules.Where(s => !s.IsSent & s.AppId==appId).ToList();
            }
        }
        public Schedules update(Schedules _schedules)
        {
            try
            {
                using (var db = new BaseEntities())
                {
                    Schedules schedules = getScheduleBySchedulesId(_schedules.SchedulesId);
                    schedules.MobileNo = _schedules.MobileNo;
                    schedules.EventDate = _schedules.EventDate;
                    schedules.Custom1 = _schedules.Custom1;
                    schedules.Custom2 = _schedules.Custom2;
                    schedules.Custom3 = _schedules.Custom3;
                    schedules.Reason = _schedules.Reason;
                    schedules.Response = _schedules.Response;
                    schedules.IsSent = _schedules.IsSent;
                    schedules.SentOn = _schedules.SentOn;
                    db.Entry(schedules).State = System.Data.Entity.EntityState.Modified;

                    if (db.SaveChanges() > 0)
                        return schedules;
                    else return null;
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
    }
}