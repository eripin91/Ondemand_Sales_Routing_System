using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iSchedule.Models;
using System.IO;
using System.Data.Entity.Validation;
using System.Data.Entity;
using Microsoft.VisualBasic.ApplicationServices;

namespace iSchedule.BLL
{
    public class Users_BLL : GenericFunctions
    {
        public List<User_ViewModel> getAllUsers()
        {
            using (var db = new BaseEntities())
            {
                var users = db.AspNetUsers.AsQueryable();
                List<User_ViewModel> list_user_viewModel = new List<User_ViewModel>();
                foreach (var item in users)
                {
                    var setting = db.Settings.FirstOrDefault(s=>s.UserId==item.Id);
                    string appId = string.Empty;
                    if (setting != null)
                        appId = setting.AppId;

                    User_ViewModel user_viewModel = new User_ViewModel{
                        Email = item.Email,
                        Id = item.Id,
                        UserName = item.UserName,
                        AppId = appId
                    };
                    list_user_viewModel.Add(user_viewModel);
                }


                return list_user_viewModel;
                
            }
        }
        public AspNetUsers getUsersByUsersId(string userId)
        {
            using (var db = new BaseEntities())
            {
                return db.AspNetUsers.FirstOrDefault(s => s.Id == userId);
            }
        }

    }
}