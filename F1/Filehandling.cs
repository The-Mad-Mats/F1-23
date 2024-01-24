using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;

namespace F1
{
    partial class ViewModel
    {
        private void SaveSettings()
        {
            var filename = "Names\\HighlightedDrivers.txt";
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine(HighLights[0].Name + "," + HighLights[0].Color + "," + HighLights[0].Human);
                writer.WriteLine(HighLights[1].Name + "," + HighLights[1].Color + "," + HighLights[1].Human);
                writer.WriteLine(HighLights[2].Name + "," + HighLights[2].Color + "," + HighLights[2].Human);
                writer.WriteLine(HighLights[3].Name + "," + HighLights[3].Color + "," + HighLights[3].Human);
            }

            filename = "Names\\Settings.txt";
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine(PortNumber.ToString() + ";" + SwitchingEnabled.ToString() + ";" + SwitchInterval + ";" + SeasonName);
            }
            var path = "Result\\" + SeasonName;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
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
                    writer.WriteLine("Position, Driver, Team, Grid, Stops, Best, Time, Points, Penalties, Penalty Time, Warnings");
                }

                if (_session == "4" || _session == "5" || _session == "6" || _session == "7" || _session == "8" || _session == "9")
                {
                    var Leader = sortedList[0];
                    foreach (var driver in sortedList)
                    {
                        var Position = driver.m_position;
                        var Driver = driver.Name;
                        var Team = driver.Team;
                        var Best = TimeSpan.FromMilliseconds(driver.m_bestLapTime).ToString(@"mm\:ss\:fff");
                        TimeSpan span = TimeSpan.FromMilliseconds(Leader.m_bestLapTime - driver.m_bestLapTime);
                        var Gap = span.ToString(@"ss\:fff");

                        string[] vars = { Position.ToString(), Driver, Team, Best.ToString(), Gap.ToString() };
                        writer.WriteLine(string.Join(",", vars));
                    }
                    var player = sortedList.FirstOrDefault(x => HighLights.Where(y => y.Human).Select(z => z.Name).Contains(x.Name));
                    if (player != null)
                    {
                        var opponent = sortedList.FirstOrDefault(x => !HighLights.Where(y => y.Human).Select(z => z.Name).Contains(x.Name));
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
                        var Driver = driver.Name;
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
                        var Warnings = driver.Warnings;

                        string[] vars = { Position.ToString(), Driver, Team, Grid.ToString(), Stops.ToString(),
                            Best.ToString(), Gap.ToString(), Points.ToString(), Penalties.ToString(), PenTime.ToString(), Warnings.ToString()  };
                        writer.WriteLine(string.Join(",", vars));
                    }
                    var player = sortedList.FirstOrDefault(x => HighLights.Where(y => y.Human).Select(z => z.Name).Contains(x.Name));
                    if (player != null)
                    {
                        var opponent = sortedList.FirstOrDefault(x => !HighLights.Where(y => y.Human).Select(z => z.Name).Contains(x.Name));
                        TimeSpan diffspan = TimeSpan.FromSeconds(player.m_totalRaceTime - opponent.m_totalRaceTime);
                        var diff = diffspan.ToString(@"mm\:ss\:fff");
                        writer.WriteLine("Gap between players and AI: " + diff);
                    }
                }
                //Check best laps
                var humanList = sortedList.Where(x => HighLights.Where(y => y.Human).Select(z => z.Name).Contains(x.Name));
                var bestHuman = humanList.FirstOrDefault(y => y.m_bestLapTime == humanList.Min(x => x.m_bestLapTime));
                var bestLapCollection = ReadBestLaps();
                var bestLap = bestLapCollection.FirstOrDefault(x => x.Track == Track);
                if (_session == "4" || _session == "5" || _session == "6" || _session == "7" || _session == "8" || _session == "9")
                {
                    var blTimeSpan = TimeSpan.ParseExact(bestLap.QTime, @"m\:ss\:fff", CultureInfo.InvariantCulture);
                    if (bestHuman.m_bestLapTime < blTimeSpan.TotalMilliseconds)
                    {
                        bestLap.QTime = TimeSpan.FromMilliseconds(bestHuman.m_bestLapTime).ToString(@"m\:ss\:fff");
                        bestLap.QDriver = bestHuman.Name;
                        bestLap.QSeason = SeasonName;
                        SaveBestLaps(bestLapCollection);
                    }
                }
                if (_session == "10" || _session == "11")
                {
                    var blTimeSpan = TimeSpan.ParseExact(bestLap.RTime, @"m\:ss\:fff", CultureInfo.InvariantCulture);
                    if (bestHuman.m_bestLapTime < blTimeSpan.TotalMilliseconds)
                    {
                        bestLap.RTime = TimeSpan.FromMilliseconds(bestHuman.m_bestLapTime).ToString(@"m\:ss\:fff");
                        bestLap.RDriver = bestHuman.Name;
                        bestLap.RSeason = SeasonName;
                        SaveBestLaps(bestLapCollection);
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
                    if (lines.Length > 3)
                    {
                        HighLights.Add(new HighLight { Name = lines[0], Color = lines[4].ToString(), Human = false });
                    }
                    else if (lines.Length == 2)
                    {
                        HighLights.Add(new HighLight { Name = lines[0], Color = lines[1].ToString(), Human = false });
                    }
                    else
                    {
                        HighLights.Add(new HighLight { Name = lines[0], Color = lines[1].ToString(), Human = Convert.ToBoolean(lines[2]) });
                    }
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
            ReadRaces();
            ReadQualis();
        }

        private void ReadRaces()
        {
            Races = new ObservableCollection<string>();
            if (Directory.Exists("Result"))
            {
                var di = new DirectoryInfo(Path.Combine("Result", SelectedSeason));
                var files = di.GetFiles("*Race*.txt");
                foreach (var file in files.OrderBy(x => x.Name.Split()[1]))
                {
                    Races.Add(file.Name);
                }
            }

        }
        private void ReadQualis()
        {
            Qualifications = new ObservableCollection<string>();
            if (Directory.Exists("Result"))
            {
                var di = new DirectoryInfo(Path.Combine("Result", SelectedSeason));
                var files = di.GetFiles("*Qualifying*.txt");
                foreach (var file in files.OrderBy(x => x.Name.Split()[1]))
                {
                    Qualifications.Add(file.Name);
                }
            }

        }
        private List<BestLaps> ReadBestLaps()
        {
            var bestLapsCollection = new List<BestLaps>();
            FileInfo info = new FileInfo($"Result\\{SeasonName}\\BestLaps.txt");
            if(!info.Exists)
            {
                File.Copy("BestLaps.txt", $"Result\\{SeasonName}\\BestLaps.txt");

            }
            using (StreamReader reader = info.OpenText())
            {
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                        break;
                    var lines = line.Split(',');
                    var bl = new BestLaps();
                    bl.Track = lines[0];
                    bl.QTime = lines[1];
                    bl.QDriver = lines[2];
                    bl.QSeason = lines[3];
                    bl.RTime = lines[4];
                    bl.RDriver = lines[5];
                    bl.RSeason = lines[6];
                    bestLapsCollection.Add(bl);
                    var blTimeSpan = TimeSpan.ParseExact(bl.QTime, @"m\:ss\:fff", CultureInfo.InvariantCulture);

                }
            }
            var dt = new DataTable();
            dt.Columns.Add("Track");
            dt.Columns.Add("Q Time");
            dt.Columns.Add("Q Driver");
            dt.Columns.Add("Q Season");
            dt.Columns.Add("R Time");
            dt.Columns.Add("R Driver");
            dt.Columns.Add("R Season");
            foreach (var b in bestLapsCollection)
            {
                dt.Rows.Add(b.Track, b.QTime, b.QDriver, b.QSeason, b.RTime, b.RDriver, b.RSeason);
            }
            BestLaps = dt;
            return bestLapsCollection;
        }
        private void SaveBestLaps(List<BestLaps> bestLapsCollection)
        {
            using (StreamWriter writer = new StreamWriter($"Result\\{SeasonName}\\BestLaps.txt"))
            {
                foreach (BestLaps bl in bestLapsCollection)
                {
                    writer.WriteLine($"{bl.Track},{bl.QTime},{bl.QDriver},{bl.QSeason},{bl.RTime},{bl.RDriver},{bl.RSeason}");
                }
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
        private void ReadRaceResult()
        {
            if (SelectedRace == null)
                return;
            var di = new DirectoryInfo(Path.Combine("Result", SelectedSeason));
            if (!Directory.Exists(Path.Combine("Result", SelectedSeason)))
            {
                Directory.CreateDirectory(Path.Combine("Result", SelectedSeason));
            }
            var file = di.GetFiles(SelectedRace).FirstOrDefault(); 
            var headers = new List<string>();
            var rows = new List<List<string>>();
            using (StreamReader reader = file.OpenText())
            {

                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                        break;
                    var lines = line.Split(',').ToList();
                    if (lines[0] == "Position")
                    {
                        foreach (string linePart in lines)
                        {
                            headers.Add(linePart);
                        }
                    }
                    else if (lines[0].StartsWith("Gap"))
                    {
                        DiffInRace = lines[0];
                    }

                    else
                    {
                        rows.Add(lines.ToList());
                    }

                }
            }
            var dt = new DataTable();
            dt.Columns.Add("Position");
            dt.Columns.Add("Driver");
            dt.Columns.Add("Team");
            dt.Columns.Add("Grid");
            dt.Columns.Add("Stops");
            dt.Columns.Add("Best");
            dt.Columns.Add("Time");
            dt.Columns.Add("Points");
            dt.Columns.Add("Penalties");
            dt.Columns.Add("Penalty Time");
            if(headers.Count > 10)
                dt.Columns.Add("Warnings");

            foreach (var row in rows)
            {
                dt.Rows.Add(row.ToArray());
            }
            RaceResult = dt;

        }
        private void ReadQualiResult()
        {
            if (SelectedQuali == null)
                return;
            var di = new DirectoryInfo(Path.Combine("Result", SelectedSeason));
            if (!Directory.Exists(Path.Combine("Result", SelectedSeason)))
            {
                Directory.CreateDirectory(Path.Combine("Result", SelectedSeason));
            }
            var file = di.GetFiles(SelectedQuali).FirstOrDefault();
            var headers = new List<string>();
            var rows = new List<List<string>>();
            using (StreamReader reader = file.OpenText())
            {

                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                        break;
                    var lines = line.Split(',').ToList();
                    if (lines[0] == "Position")
                    {
                        foreach (string linePart in lines)
                        {
                            headers.Add(linePart);
                        }
                    }
                    else if (lines[0].StartsWith("Gap"))
                    {
                        DiffInQuali = lines[0];
                    }

                    else
                    {
                        rows.Add(lines.ToList());
                    }

                }
            }
            var dt = new DataTable();
            dt.Columns.Add("Position");
            dt.Columns.Add("Driver");
            dt.Columns.Add("Team");
            dt.Columns.Add("Best");
            dt.Columns.Add("Gap");

            foreach (var row in rows)
            {
                dt.Rows.Add(row.ToArray());
            }
            QualiResult = dt;

        }

    }
}
