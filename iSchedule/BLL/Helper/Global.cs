using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iSchedule.BLL
{
    public static class Globals
    {
        public static readonly object balanceLock = new object();
    }
}