using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Submissions
    {
        public string UId { get; set; }
        public uint AId { get; set; }
        public DateTime Time { get; set; }
        public string Content { get; set; }
        public uint Score { get; set; }

        public virtual Assignments A { get; set; }
        public virtual Students U { get; set; }
    }
}
