using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static F1.ViewModel;

namespace F1
{
    partial class ViewModel
    {
        private void SaveSettings()
        {
            var filename = "Names\\HighlightedDrivers.txt";
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine(HighLights[0].Name + "," + HighLights[0].Team + "," + HighLights[0].Country + "," + HighLights[0].ShowName + "," + HighLights[0].Color);
                writer.WriteLine(HighLights[1].Name + "," + HighLights[1].Team + "," + HighLights[1].Country + "," + HighLights[1].ShowName + "," + HighLights[1].Color);
                writer.WriteLine(HighLights[2].Name + "," + HighLights[2].Team + "," + HighLights[2].Country + "," + HighLights[2].ShowName + "," + HighLights[2].Color);
                writer.WriteLine(HighLights[3].Name + "," + HighLights[3].Team + "," + HighLights[3].Country + "," + HighLights[3].ShowName + "," + HighLights[3].Color);
            }

            filename = "Names\\Settings.txt";
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine(PortNumber.ToString() + ";" + SwitchingEnabled.ToString() + ";" + SwitchInterval + ";" + SeasonName);
            }

        }
        //private void WriteData()
        //{
        //var filename = Track + " " + DateTime.Now.Date.ToLongDateString() + ".txt";
        //using (StreamWriter writer = new StreamWriter(filename))
        //{
        //    foreach (var highLight in HighLights)
        //    {
        //        var user = Users.FirstOrDefault(x => x.Name == highLight.Key);
        //        if (user != null)
        //        {
        //            writer.WriteLine(user.Name);
        //            foreach (var userLap in user.Laps)
        //            {
        //                TimeSpan span = TimeSpan.FromSeconds(userLap.Value.Time);
        //                var time = span.ToString(@"mm\:ss\:fff");
        //                writer.WriteLine(userLap.Key + ": " + time);
        //            }
        //        }
        //    }
        //}
        private void WriteFinalClassification(List<User> sortedList)
        {
            var path = "Result\\" + SeasonName;
            var filename = Track + " " + DateTime.Now.Date.ToShortDateString() + "_" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + " " + Session + ".txt";
            //var filename = Track + " " + DateTime.Now.Date.ToShortDateString() + " " + Session + ".txt";
            var fullpath = Path.Combine(path, filename);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (StreamWriter writer = new StreamWriter(fullpath))
            {
                if (_session == "4" || _session == "5" || _session == "6" || _session == "7" || _session == "8" || _session == "9")
                {
                    writer.WriteLine("Position, Driver, Team, Best, Gap");
                }
                if (_session == "10" || _session == "11")
                {
                    writer.WriteLine("Position, Driver, Team, Grid, Stops, Best, Time, Points, Penalties, Penalty Time");
                }

                if (_session == "4" || _session == "5" || _session == "6" || _session == "7" || _session == "8" || _session == "9")
                {
                    var Leader = sortedList[0];
                    foreach (var driver in sortedList)
                    {
                        var Position = driver.m_position;
                        var Driver = driver.ViewName;
                        var Team = driver.Team;
                        var Best = TimeSpan.FromMilliseconds(driver.m_bestLapTime).ToString(@"mm\:ss\:fff");
                        TimeSpan span = TimeSpan.FromMilliseconds(Leader.m_bestLapTime - driver.m_bestLapTime);
                        var Gap = span.ToString(@"ss\:fff");

                        string[] vars = { Position.ToString(), Driver, Team, Best.ToString(), Gap.ToString() };
                        writer.WriteLine(string.Join(",", vars));
                    }
                    var player = sortedList.FirstOrDefault(x => x.Name == "Player");
                    if (player != null)
                    {
                        var opponent = sortedList.FirstOrDefault(x => x.Name != "Player");
                        TimeSpan diffspan = TimeSpan.FromMilliseconds(player.m_bestLapTime - opponent.m_bestLapTime);
                        var diff = diffspan.ToString(@"mm\:ss\:fff");
                        writer.WriteLine("Gap between players and AI: " + diff);
                    }
                }
                if (_session == "10" || _session == "11")
                {
                    var Leader = sortedList[0];
                    foreach (var driver in sortedList)
                    {
                        var Position = driver.m_position;
                        var Driver = driver.ViewName;
                        var Team = driver.Team;
                        var Grid = driver.m_gridPosition;
                        var Stops = driver.m_numPitStops;
                        var Best = TimeSpan.FromMilliseconds(driver.m_bestLapTime).ToString(@"mm\:ss\:fff");
                        TimeSpan span = TimeSpan.FromSeconds(Leader.m_totalRaceTime - (driver.m_totalRaceTime + driver.m_penaltiesTime));
                        TimeSpan leaderspan = TimeSpan.FromSeconds(driver.m_totalRaceTime);
                        var Gap = Leader == driver ? leaderspan.ToString(@"mm\:ss\:fff") : span.ToString(@"mm\:ss\:fff");
                        var Points = driver.m_points;
                        var Penalties = driver.m_numPenalties;
                        var PenTime = driver.m_penaltiesTime;

                        string[] vars = { Position.ToString(), Driver, Team, Grid.ToString(), Stops.ToString(),
                            Best.ToString(), Gap.ToString(), Points.ToString(), Penalties.ToString(), PenTime.ToString()  };
                        writer.WriteLine(string.Join(",", vars));
                    }
                    var player = sortedList.FirstOrDefault(x => x.Name == "Player");
                    if (player != null)
                    {
                        var opponent = sortedList.FirstOrDefault(x => x.Name != "Player");
                        TimeSpan diffspan = TimeSpan.FromSeconds(player.m_totalRaceTime - opponent.m_totalRaceTime);
                        var diff = diffspan.ToString(@"mm\:ss\:fff");
                        writer.WriteLine("Gap between players and AI: " + diff);
                    }
                }
            }
        }

        private void ReadData()
        {
            FileInfo info = new FileInfo("Names\\Drivers.txt");
            using (StreamReader reader = info.OpenText())
            {
                Drivers.Clear();
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                        break;
                    var lines = line.Split(',');
                    Drivers.Add(Convert.ToInt32(lines[1]), lines[0].ToString());
                }
            }
            info = new FileInfo("Names\\Teams.txt");
            using (StreamReader reader = info.OpenText())
            {
                Teams.Clear();
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                        break;
                    var lines = line.Split(',');
                    Teams.Add(new DataItem { Id = Convert.ToInt32(lines[1]), Name = lines[0].ToString() });
                }
            }
            info = new FileInfo("Names\\Tracks.txt");
            using (StreamReader reader = info.OpenText())
            {
                Tracks.Clear();
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                        break;
                    var lines = line.Split(',');
                    Tracks.Add(Convert.ToInt32(lines[1]), lines[0].ToString());
                }
            }

            info = new FileInfo("Names\\Nationality.txt");
            using (StreamReader reader = info.OpenText())
            {
                Nationality.Clear();
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                        break;
                    var lines = line.Split(',');
                    Nationality.Add(new DataItem { Id = Convert.ToInt32(lines[0]), Name = lines[1].ToString() });
                }
            }

            info = new FileInfo("Names\\HighlightedDrivers.txt");
            using (StreamReader reader = info.OpenText())
            {
                HighLights.Clear();
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                        break;
                    var lines = line.Split(',');
                    HighLights.Add(new HighLight { Name = lines[0], Team = Convert.ToInt32(lines[1]), Country = Convert.ToInt32(lines[2]), ShowName = lines[3], Color = lines[4].ToString() });
                }
            }

            info = new FileInfo("Names\\Settings.txt");
            using (StreamReader reader = info.OpenText())
            {
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                        break;
                    var lines = line.Split(';');
                    PortNumber = Convert.ToInt32(lines[0]);
                    SwitchingEnabled = Convert.ToBoolean(lines[1]);
                    SwitchInterval = Convert.ToInt32(lines[2]);
                    if (lines.Count() > 3)
                        SeasonName = lines[3];
                }
            }
            Seasons = new ObservableCollection<string>();
            if (Directory.Exists("Result"))
            {
                foreach (string dir in Directory.GetDirectories("Result").Select(Path.GetFileName).ToList())
                {
                    Seasons.Add(dir);
                }
                SelectedSeason = SeasonName;
            }
        }

        private void ReadStandings(bool reloadPosition)
        {
            var standings = new Standings();
            standings.Driver = new List<DriverStanding> { };
            standings.Header = new HeaderStanding();
            standings.Header.Track = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("Driver", "0"),
                new Tuple<string, string>("Total", "1")
            };
            var numberOfRaces = 0;
            var di = new DirectoryInfo(Path.Combine("Result", SelectedSeason));
            if (!Directory.Exists(Path.Combine("Result", SelectedSeason)))
            {
                Directory.CreateDirectory(Path.Combine("Result", SelectedSeason));
            }
            var files = di.GetFiles("*Race*.txt");
            foreach (var file in files.OrderBy(x => x.Name.Split()[1]))
            {
                numberOfRaces++;
                var filePart = file.Name.Split(' ');
                standings.Header.Track.Add(new Tuple<string, string>(filePart[0], filePart[1]));
                using (StreamReader reader = file.OpenText())
                {
                    while (true)
                    {
                        string line = reader.ReadLine();
                        if (line == null)
                            break;
                        var lines = line.Split(',');
                        if (lines[0] == "Position" || lines[0].StartsWith("Gap"))
                            continue;
                        var driver = standings.Driver.FirstOrDefault(x => x.Name == lines[1]);
                        if (driver == null)
                        {
                            if (reloadPosition)
                            {
                                var pr = new List<Tuple<string, int>>();
                                for (int index = 1; index < numberOfRaces; index++)
                                {
                                    pr.Add(new Tuple<string, int>(DateTime.MinValue.ToString(), 0));
                                }
                                pr.Add(new Tuple<string, int>(filePart[1], Convert.ToInt32(lines[0])));
                                standings.Driver.Add(new DriverStanding { Name = lines[1], Total = Convert.ToInt32(lines[7]), PerRace = pr });
                            }
                            else
                            {
                                var pr = new List<Tuple<string, int>>();
                                for (int index = 1; index < numberOfRaces; index++)
                                {
                                    pr.Add(new Tuple<string, int>(DateTime.MinValue.ToString(), 0));
                                }
                                pr.Add(new Tuple<string, int>(filePart[1], Convert.ToInt32(lines[7])));
                                standings.Driver.Add(new DriverStanding { Name = lines[1], Total = Convert.ToInt32(lines[7]), PerRace = pr });
                            }
                        }
                        else
                        {
                            if (reloadPosition)
                            {
                                driver.PerRace.Add(new Tuple<string, int>(filePart[1], Convert.ToInt32(lines[0])));
                            }
                            else
                            {
                                driver.PerRace.Add(new Tuple<string, int>(filePart[1], Convert.ToInt32(lines[7])));
                            }
                            driver.Total += Convert.ToInt32(lines[7]);
                        }
                    }
                }

            }
            var dt = new DataTable();
            foreach (string w in standings.Header.Track.OrderBy(x => x.Item2).Select(y => y.Item1.ToString()).ToArray())
            {
                if (dt.Columns.Contains(w))
                {
                    if (dt.Columns.Contains(w + "2"))
                    {
                        dt.Columns.Add(w + "3");
                    }
                    else
                    {
                        dt.Columns.Add(w + "2");
                    }
                }
                else
                {
                    dt.Columns.Add(w);

                }
            }
            foreach (var driver in standings.Driver.OrderByDescending(x => x.Total))
            {
                var l1 = new List<string>
                {
                    driver.Name,
                    driver.Total.ToString()
                };
                var q = driver.PerRace.OrderBy(x => x.Item1).Select(y => y.Item2.ToString()).ToArray();
                l1.AddRange(q);
                dt.Rows.Add(l1.ToArray());
            }
            ChamionshipStandings = dt;
        }
    }
}
