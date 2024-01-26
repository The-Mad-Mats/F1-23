using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F1
{
    public class Standings
    {
        public HeaderStanding Header { get; set; }
        public List<DriverStanding> Driver { get; set; }

    }

    public class HeaderStanding
    {
        public List<Tuple<string, string>> Track { get; set; }
    }
    public class DriverStanding
    {
        public string Name { get; set; }
        public int Total { get; set; }
        public List<Tuple<string, int>> PerRace { get; set; }
    }
    public class TeamStandings
    {
        public HeaderStanding Header { get; set; }
        public List<TeamStanding> Team { get; set; }

    }
    public class TeamStanding
    {
        public string Name { get; set; }
        public int Total { get; set; }
        public List<Tuple<string, string, int>> PerRace { get; set; }
    }

}
