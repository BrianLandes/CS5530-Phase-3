using System;
using System.Collections.Generic;

namespace xUnitTests.Models
{
    public partial class Assignments
    {
        public Assignments()
        {
            Submissions = new HashSet<Submissions>();
        }

        public uint AId { get; set; }
        public uint AcId { get; set; }
        public string Name { get; set; }
        public uint MaxPointValue { get; set; }
        public DateTime DueDate { get; set; }
        public string Content { get; set; }

        public virtual AssignmentCategories Ac { get; set; }
        public virtual ICollection<Submissions> Submissions { get; set; }
    }
}
