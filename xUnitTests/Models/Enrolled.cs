﻿using System;
using System.Collections.Generic;

namespace xUnitTests.Models
{
    public partial class Enrolled
    {
        public string UId { get; set; }
        public uint CId { get; set; }
        public string Grade { get; set; }

        public virtual Classes C { get; set; }
        public virtual Students U { get; set; }
    }
}
