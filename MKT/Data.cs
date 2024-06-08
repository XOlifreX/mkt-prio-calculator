using static MKTCoverage.Data.CourseLists;

namespace MKTCoverage.MKT
{
    public struct InfoWithLevel
    {
        public string Name;
        public int Level;
    };

    public class Data
    {
        public List<string> Drivers { get; set; }
        public List<string> Karts { get; set; }
        public List<string> Gliders { get; set; }
        // **********
        public Dictionary<string, Course> Courses {  get; set; }
        public Dictionary<string, DKG> Drivables {  get; set; }

        // **********

        public Data()
        {
            this.Drivers = new List<string>();
            this.Karts = new List<string>();
            this.Gliders = new List<string>();

            this.Courses = new Dictionary<string, Course>();
            this.Drivables = new Dictionary<string, DKG>();
        }


        public void InitDkgData()
        {
            string line;
            try
            {
                StreamReader sr = new StreamReader("Data/dkg.csv");
                line = sr.ReadLine();
                int i = 1;
                while (line != null)
                {
                    line = sr.ReadLine();
                    i++;

                    if (i <= 2) continue;

                    // **********

                    if (string.IsNullOrEmpty(line)) break;

                    var items = line.Split(',');
                    var name = items[1];
                    var type = items[2];

                    if (type == "Driver")
                    {
                        this.Drivers.Add(name);
                    } 
                    else if (type == "Kart")
                    {
                        this.Karts.Add(name);
                    } 
                    else if (type == "Glider")
                    {
                        this.Gliders.Add(name);
                    }
                }

                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Done!");
            }
        }

        private DkgType GetDkgType(string name)
        {
            if (this.Drivers.Contains(name)) return DkgType.Driver;
            if (this.Karts.Contains(name)) return DkgType.Kart;
            
            return DkgType.Glider;
        }

        public void InitCoverage()
        {
            string line;
            try
            {
                StreamReader sr = new StreamReader("Data/coverage.csv");
                line = sr.ReadLine();
                int i = 1;
                while (line != null)
                {
                    line = sr.ReadLine();
                    i++;

                    if (i <= 2) continue;

                    // **********

                    if (string.IsNullOrEmpty(line)) break;

                    var items = line.Split(',');
                    var isTop = items[2] == "Top Shelf";
                    // var level = int.Parse(items[3]);

                    // **********

                    DKG drivable;
                    if (this.Drivables.ContainsKey(items[0]))
                    {
                        drivable = this.Drivables[items[0]];
                    }
                    else
                    {
                        drivable = new DKG();
                        drivable.Name = items[0];
                        drivable.Type = GetDkgType(items[0]);
                        drivable.CoursesTop = new List<InfoWithLevel>();
                        drivable.CoursesMiddle = new List<InfoWithLevel>();

                        this.Drivables.Add(items[0], drivable);
                    }

                    if (isTop)
                    {
                        drivable.CoursesTop.Add(new InfoWithLevel() {
                            Name = items[1],
                            Level = 1
                        });
                    }
                    else
                    {
                        drivable.CoursesMiddle.Add(new InfoWithLevel()
                        {
                            Name = items[1],
                            Level = 1
                        });
                    }

                    // **********

                    Course course;
                    if (this.Courses.ContainsKey(items[1]))
                    {
                        course = this.Courses[items[1]];
                    }
                    else
                    {
                        course = new Course();
                        course.Name = items[1];
                        course.DrivablesTop = new List<InfoWithLevel>();
                        course.DrivablesMiddle = new List<InfoWithLevel>();

                        this.Courses.Add(items[1], course);
                    }

                    if (isTop)
                    {
                        course.DrivablesTop.Add(new InfoWithLevel()
                        {
                            Name = items[0],
                            Level = 1
                        });
                    }
                    else
                    {
                        course.DrivablesMiddle.Add(new InfoWithLevel()
                        {
                            Name = items[0],
                            Level = 1
                        });
                    }
                }

                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Done!");
            }
        }

        public void PrintCoverageListOfCourseList(List<string> list)
        {
            var ranking = new Dictionary<string, int>();

            foreach (string item in list)
            {
                if (!this.Courses.ContainsKey(item)) continue;

                var course = this.Courses[item];

                foreach (InfoWithLevel info in course.DrivablesTop)
                {
                    if (ranking.ContainsKey(info.Name))
                    {
                        ranking[info.Name]++;
                    }
                    else
                    {
                        ranking[info.Name] = 1;
                    }
                }
            }

            ranking = ranking.OrderByDescending(x => x.Value).ToDictionary();

            Console.WriteLine("Ranking: ");
            int i = 1;

            foreach (KeyValuePair <string, int> entry in ranking)
            {
                Console.WriteLine($"#{i} - {entry.Key}: {entry.Value} appearances.");
                i++;
            }
        }
            
        private struct DkgTracksRankingInfo
        {
            public DKG dkg;
            public List<string> courses;
        }

        public void PrintCoverageListOfCourseListModular(List<TracksDkgInfo> list)
        {
            var ranking = new Dictionary<string, int>();

            foreach (TracksDkgInfo item in list)
            {
                if ((item.drivers || item.karts || item.gliders) == false) continue;

                if (!this.Courses.ContainsKey(item.name)) continue;

                var course = this.Courses[item.name];

                foreach (InfoWithLevel info in course.DrivablesTop)
                {
                    if (info.Level > 1) continue;

                    if (!this.Drivables.ContainsKey(info.Name)) continue;
                    DKG dkg = Drivables[info.Name];

                    switch (dkg.Type)
                    {
                        case DkgType.Driver:
                            if (item.drivers)
                            {
                                if (ranking.ContainsKey(info.Name))
                                {
                                    ranking[info.Name]++;
                                }
                                else
                                {
                                    ranking[info.Name] = 1;
                                }
                            }

                            break;
                        case DkgType.Kart:
                            if (item.karts)
                            {
                                if (ranking.ContainsKey(info.Name))
                                {
                                    ranking[info.Name]++;
                                }
                                else
                                {
                                    ranking[info.Name] = 1;
                                }
                            }

                            break;
                        default:
                            if (item.gliders)
                            {
                                if (ranking.ContainsKey(info.Name))
                                {
                                    ranking[info.Name]++;
                                }
                                else
                                {
                                    ranking[info.Name] = 1;
                                }
                            }

                            break;
                    }
                }
            }

            ranking = ranking.OrderByDescending(x => x.Value).ToDictionary();

            Console.WriteLine("\nALL DKG - Ranking: ");
            int i = 1;

            foreach (KeyValuePair<string, int> entry in ranking)
            {
                Console.WriteLine($"#{i} - {entry.Key}: {entry.Value} appearances.");
                i++;
            }

            // Highest prio DKG's:
            var temp = new List<TracksDkgInfo>(list);
            var highPrioRanking = new Dictionary<string, DkgTracksRankingInfo>();

            // Add whatever DKG you want to include at the top to change the prio list
            // In case there's a DKG you Really want to get regardless of relevance
            List<KeyValuePair<string, int>> changedRanking= ranking.ToList();
            changedRanking.Sort(
                delegate (KeyValuePair<string, int> pair1,
                KeyValuePair<string, int> pair2)
                {
                    if (pair1.Key == "Cream B Dasher Mk. 2") return -1;
                    if (pair2.Key == "Cream B Dasher Mk. 2") return 1;

                    return pair2.Value.CompareTo(pair1.Value);
                }
            );

            foreach (KeyValuePair<string, int> entry in changedRanking)
            {
                if (!this.Drivables.ContainsKey(entry.Key)) continue;

                var dkg = this.Drivables[entry.Key];

                for (int j = 0; j < temp.Count; j++)
                {
                    var course = temp[j];
                    if ((course.drivers || course.karts || course.gliders) == false) continue;

                    if (!dkg.CoursesTop.Any(x => x.Name == course.name)) continue;

                    switch (dkg.Type)
                    {
                        case DkgType.Driver:
                            if (!course.drivers) continue;

                            course.drivers = false;

                            break;
                        case DkgType.Kart:
                            if (!course.karts) continue;

                            course.karts = false;

                            break;
                        default:
                            if (!course.gliders) continue;

                            course.gliders = false;

                            break;
                    }

                    if (!highPrioRanking.ContainsKey(dkg.Name))
                    {
                        var info = new DkgTracksRankingInfo()
                        {
                            dkg = dkg,
                            courses = new List<string>() { course.name }
                        };

                        highPrioRanking[dkg.Name] = info;
                    }
                    else
                    {
                        highPrioRanking[dkg.Name].courses.Add(course.name);
                    }
                }
            }

            Console.WriteLine("\nHighest Prio - Ranking: ");
            i = 1;

            foreach (KeyValuePair<string, DkgTracksRankingInfo> entry in highPrioRanking)
            {
                Console.WriteLine($"{entry.Key} ({entry.Value.dkg.Type}): {entry.Value.courses.Count} courses covered.");
                foreach (string course in entry.Value.courses)
                {
                    Console.WriteLine($"    - {course}");
                }

                i++;
            }
        }
    }
}
