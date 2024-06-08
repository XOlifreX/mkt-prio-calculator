using MKTCoverage.Data;

namespace MKTCoverage
{
    internal static class Program
    {
        static void Main()
        {
            var data = new MKT.Data();
            data.InitDkgData();
            data.InitCoverage();

            data.PrintCoverageListOfCourseListModular(CourseLists.RankedCoursesWithSeperateDkgInfo);
        }
    }
}