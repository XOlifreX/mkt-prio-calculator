using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKTCoverage.MKT
{
    public class Course
    {
        public string Name { get; set; }
        public List<InfoWithLevel> DrivablesTop { get; set; }
        public List<InfoWithLevel> DrivablesMiddle { get; set; }
    }
}
