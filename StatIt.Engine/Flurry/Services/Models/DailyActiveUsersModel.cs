﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatIt.Engine.Flurry.Services.Models
{
    public class DailyActiveUsersModel
    {
        public string DAUDate { get; set; }
        public int iOSUsers { get; set; }
        public int AndroidUsers { get; set; }
    }
}
