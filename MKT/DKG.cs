using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKTCoverage.MKT
{
    public enum DkgType
    {
        Driver, Kart, Glider
    };

    public class DKG
    {
        public string Name { get; set; }
        public DkgType Type { get; set; }
        public List<InfoWithLevel> CoursesTop { get; set; }
        public List<InfoWithLevel> CoursesMiddle { get; set; }
    }
}
