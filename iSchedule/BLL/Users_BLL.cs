using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iSchedule.Models;
using System.IO;
using System.Data.Entity.Validation;

namespace iSchedule.BLL
{
    public class Users_BLL : GenericFunctions
    {
        public List<AspNetUsers> getAllUsers()
        {
            using (var db = new BaseEntities())
            {
                return db.AspNetUsers.ToList();
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