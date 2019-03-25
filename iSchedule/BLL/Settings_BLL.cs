using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using iSchedule.edmx;
using System.IO;
using System.Data.Entity.Validation;

namespace iSchedule.BLL
{
    public class Settings_BLL : GenericFunctions
    {
        public List<Settings> getAllSettings()
        {
            using (var db = new BaseEntities())
            {
                return db.Settings.ToList();
            }
        }
        public Settings getSettingsBySettingsId(int settingsId)
        {
            using (var db = new BaseEntities())
            {
                return db.Settings.FirstOrDefault(s => s.SettingsID == settingsId);
            }
        }
        public Settings getSettingsByAppId(string appId)
        {
            using (var db = new BaseEntities())
            {
                return db.Settings.FirstOrDefault(s=>s.AppId==appId);
            }
        }

        public List<Settings> getSettingsByDateTime(DateTime dt)
        {
            using (var db = new BaseEntities())
            {
                return db.Settings.Where(s => s.Scheduletime == dt.TimeOfDay).ToList();
            }
        }
        public Settings create(Settings _settings)
        {
            try
            {
                using (var db = new BaseEntities())
                {
                    Settings newSetting = db.Settings.Add(_settings);

                    if (db.SaveChanges() > 0)
                        return newSetting;
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

        public Settings update(Settings _settings)
        {
            try
            {
                using (var db = new BaseEntities())
                {
                    Settings setting = getSettingsBySettingsId(_settings.SettingsID);

                    setting.LastModified = DateTime.UtcNow;
                    setting.AppId = _settings.AppId;
                    setting.AppSecret = _settings.AppSecret;
                    setting.Scheduletime = _settings.Scheduletime;
                    setting.MessageTemplate = _settings.MessageTemplate;
                    setting.IsActive = _settings.IsActive;

                    db.Entry(setting).State = System.Data.Entity.EntityState.Modified;

                    if (db.SaveChanges() > 0)
                        return setting;
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

        public Settings delete(int SettingsId)
        {
            try
            {
                using (var db = new BaseEntities())
                {
                    Settings setting = getSettingsBySettingsId(SettingsId);
                    
                    db.Entry(setting).State = System.Data.Entity.EntityState.Deleted;

                    if (db.SaveChanges() > 0)
                        return setting;
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