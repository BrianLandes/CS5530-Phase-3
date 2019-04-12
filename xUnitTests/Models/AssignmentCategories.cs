using System;
using System.Collections.Generic;

namespace xUnitTests.Models
{
    public partial class AssignmentCategories
    {
        public AssignmentCategories()
        {
            Assignments = new HashSet<Assignments>();
        }

        public uint AcId { get; set; }
        public string Name { get; set; }
        public uint GradingWeight { get; set; }
        public uint CId { get; set; }

        public virtual Classes C { get; set; }
        public virtual ICollection<Assignments> Assignments { get; set; }
    }
}
