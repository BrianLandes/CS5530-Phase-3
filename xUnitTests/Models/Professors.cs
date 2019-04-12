using System;
using System.Collections.Generic;

namespace xUnitTests.Models
{
    public partial class Professors
    {
        public Professors()
        {
            Classes = new HashSet<Classes>();
        }

        public string UId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public string WorksIn { get; set; }

        public virtual Departments WorksInNavigation { get; set; }
        public virtual ICollection<Classes> Classes { get; set; }
    }
}
