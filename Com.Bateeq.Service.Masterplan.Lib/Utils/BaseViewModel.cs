﻿using System;

namespace Com.Bateeq.Service.Masterplan.Lib.Utils
{
    public class BaseViewModel
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedUtc { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedAgent { get; set; }
        public DateTime LastModifiedUtc { get; set; }
        public string LastModifiedBy { get; set; }
        public string LastModifiedAgent { get; set; }
    }
}
