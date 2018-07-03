﻿using System;
using System.ComponentModel.DataAnnotations;

namespace PerfMonitor
{
    public class Jit  // contains the percentage of total CPU usage and DateTime of instant
    {
        public String method { get; set; }
        [Key]
        public DateTime timestamp { get; set; }
    }
}