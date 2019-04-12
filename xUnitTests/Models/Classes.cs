using System;
using System.Collections.Generic;

namespace xUnitTests.Models
{
    public partial class Classes
    {
        public Classes()
        {
            AssignmentCategories = new HashSet<AssignmentCategories>();
            Enrolled = new HashSet<Enrolled>();
        }

        public uint CId { get; set; }
        public string CatalogId { get; set; }
        public ushort SemesterYear { get; set; }
        public string SemesterSeason { get; set; }
        public string Teacher { get; set; }
        public string Location { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public virtual Courses Catalog { get; set; }
        public virtual Professors TeacherNavigation { get; set; }
        public virtual ICollection<AssignmentCategories> AssignmentCategories { get; set; }
        public virtual ICollection<Enrolled> Enrolled { get; set; }
    }
}
