using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace F1
{
    public class User : ViewModelBase
    {
        #region Private variables
        private int _team;
        private int _country;
        private string _resulStatus;
        private string _tyreWear;
        private string _drs;
        private string _fuelMix;
        private string _ersMode;

        #endregion
        #region Public variables
        public bool StartPosSet;
        public int DriverId;
        int _carPosition = 0;
        public string Sector;
        public double LastLap;
        public string Session;
        public Dictionary<int, Lap> Laps = new Dictionary<int, Lap>();
        public double _bestLap;
        public double _currentLap;
        private string _tyre;
        public string _pits;
        #endregion
        public string Name { get; set; }
        public string ViewName
        {
            get
            {
                var show = ViewModel.HighLights.FirstOrDefault(x => x.Name == Name && x.Team == _team && x.Country == _country);
                return show?.ShowName ?? Name;
            }
        }


        public int PassPlus { get; set; }
        public int PassMinus { get; set; }
        public int CarPosition
        {
            get { return _carPosition; }
            set
            {
                if (_carPosition != 0)
                {
                    if (_carPosition > value)
                        PassPlus++;
                    if (_carPosition < value)
                        PassMinus++;
                }
                _carPosition = value;
            }
        }

        public string Color
        {
            get
            {
                var color = ViewModel.HighLights.FirstOrDefault(x => x.Name == Name);
                return color?.Color ?? "";
            }
        }

        public string Gap
        {
            get
            {
                if (ViewModel.Leader == null || Session == null || Laps.Count == 0)
                    return "";
                if (Session == "1" || Session == "2" || Session == "3" || Session == "4" || Session == "5" || Session == "6" || Session == "7" || Session == "8" || Session == "9" || Session == "12")
                {
                    TimeSpan span = TimeSpan.FromMilliseconds(_bestLap - ViewModel.Leader._bestLap);
                    return span.ToString(@"mm\:ss\:fff");
                }
                else
                {
                    double userTime = Laps.Sum((x => x.Value.Time / 1000));
                    double leaderTime = ViewModel.Leader.Laps.Where(y => y.Key < LapNum).Sum(x => x.Value.Time / 1000);
                    if (Laps[Laps.Count].Section2 != 0)
                    {
                        userTime += Laps[Laps.Count].Section2 / 1000;
                        leaderTime += ViewModel.Leader.Laps[Laps.Count].Section2 / 1000;
                    }
                    if (Laps[Laps.Count].Section1 != 0)
                    {
                        userTime += Laps[Laps.Count].Section1 / 1000;
                        leaderTime += ViewModel.Leader.Laps[Laps.Count].Section1 / 1000;
                    }
                    TimeSpan span = TimeSpan.FromSeconds(userTime - leaderTime);
                    return span.ToString(@"mm\:ss\:fff");
                }
            }
        }
        public string GapInMeters
        {
            get
            {
                if (Session == "1" || Session == "2" || Session == "3" || Session == "4" || Session == "5" || Session == "6" || Session == "7" || Session == "8" || Session == "9")
                {
                    return string.Empty;
                }
                else
                {
                    if (ViewModel.Leader.LapNum == LapNum)
                    {
                        return (ViewModel.Leader.LapDistance - LapDistance).ToString();
                    }
                    else if (ViewModel.Leader.LapNum == LapNum + 1)
                    {
                        return (ViewModel.Leader.LapDistance + (ViewModel.TrackLength - LapDistance)).ToString();
                    }
                    else
                    {
                        var laps = ViewModel.Leader.LapNum - LapNum - 1;
                        var length = ViewModel.TrackLength * laps;
                        return (length + ViewModel.Leader.LapDistance + (ViewModel.TrackLength - LapDistance)).ToString();
                    }
                }
            }
        }
        public string Driver
        {
            get { return ViewModel.Drivers.FirstOrDefault(x => x.Key == DriverId).Value; }
        }

        public string Team
        {
            get { return ViewModel.Teams.FirstOrDefault(x => x.Id == _team)?.Name; }
            set
            {
                _team = Convert.ToInt32(value);
            }
        }
        public int Country
        {
            get { return _country; }
            set
            {
                _country = Convert.ToInt32(value);
            }
        }

        public string BestLap
        {
            get
            {
                TimeSpan span = TimeSpan.FromMilliseconds(_bestLap);
                return span.ToString(@"mm\:ss\:fff");
            }
            set
            {
                _bestLap = Convert.ToDouble(value);
            }
        }
        public string CurrentLap
        {
            get
            {
                TimeSpan span = TimeSpan.FromSeconds(_currentLap);
                return span.ToString(@"mm\:ss\:fff");
            }
            set
            {
                _currentLap = Convert.ToDouble(value);
            }
        }

        public int LapNum { get; set; }
        public string Tyre
        {
            get { return _tyre; }
            set
            {
                if (value != _tyre)
                {
                    _tyre = value;
                }
            }
        }


        public BitmapImage InPits
        {
            get
            {
                switch (_pits)
                {
                    case "0":
                        var onTrack = new BitmapImage();
                        using (var stream = new FileStream("Images\\OnTrack.png", FileMode.Open))
                        {
                            onTrack.BeginInit();
                            onTrack.CacheOption = BitmapCacheOption.OnLoad;
                            onTrack.StreamSource = stream;
                            onTrack.EndInit();
                        }
                        return onTrack;
                    case "1":
                    case "2":
                        var inPit = new BitmapImage();
                        using (var stream = new FileStream("Images\\InPit.png", FileMode.Open))
                        {
                            inPit.BeginInit();
                            inPit.CacheOption = BitmapCacheOption.OnLoad;
                            inPit.StreamSource = stream;
                            inPit.EndInit();
                        }
                        return inPit;

                    default:
                        var Track = new BitmapImage();
                        using (var stream = new FileStream("Images\\OnTrack.png", FileMode.Open))
                        {
                            Track.BeginInit();
                            Track.CacheOption = BitmapCacheOption.OnLoad;
                            Track.StreamSource = stream;
                            Track.EndInit();
                        }
                        return Track;
                }
            }
        }
        public string Pits
        {
            get { return _pits; }
            set
            {
                if (value != _pits)
                {
                    _pits = value;
                }
            }
        }
        public string Penalties { get; set; }

        public string TyreLaps { get; set; }

        public string TyreWear {
            get { return _tyreWear + "/" + TyreLaps; }
            set
            {
                if (value != _tyreWear)
                {
                    _tyreWear = value;
                }
            }
        }

        public int LeftFrontWingDamage { get; set; }
        public int RightFrontWingDamage { get; set; }
        public int RearWingDamage { get; set; }

        public BitmapImage WingDamage
        {
            get
            {
                var wing = new BitmapImage();
                string image = string.Empty;
                if (LeftFrontWingDamage == 0)
                {
                    if (RightFrontWingDamage == 0)
                    {
                        if (RearWingDamage == 0)
                        {
                            //Left 0, Right 0, Rear 0
                            image = "Images\\0.png";
                        }
                        else if (RearWingDamage < 50 && RearWingDamage > 0)
                        {
                            //Left 0, Right 0, Rear 50
                            image = "Images\\1.png";
                        }
                        else if (RearWingDamage >= 50)
                        {
                            //Left 0, Right 0, Rear 100
                            image = "Images\\2.png";
                        }
                    }
                    if (RightFrontWingDamage < 50 && RightFrontWingDamage > 0)
                    {
                        if (RearWingDamage == 0)
                        {
                            //Left 0, Right 50, Rear 0
                            image = "Images\\3.png";
                        }
                        else if (RearWingDamage < 50 && RearWingDamage > 0)
                        {
                            //Left 0, Right 50, Rear 50
                            image = "Images\\4.png";
                        }
                        else if (RearWingDamage >= 50)
                        {
                            //Left 0, Right 50, Rear 100
                            image = "Images\\5.png";
                        }
                    }
                    else if (RightFrontWingDamage >= 50)
                    {
                        if (RearWingDamage == 0)
                        {
                            //Left 0, Right 100, Rear 0
                            image = "Images\\6.png";
                        }
                        else if (RearWingDamage < 50 && RearWingDamage > 0)
                        {
                            //Left 0, Right 100, Rear 50
                            image = "Images\\7.png";
                        }
                        else if (RearWingDamage >= 50)
                        {
                            //Left 0, Right 100, Rear 100
                            image = "Images\\8.png";
                        }
                    }
                }
                else if (LeftFrontWingDamage < 50 && LeftFrontWingDamage > 0)
                {
                    if (RightFrontWingDamage == 0)
                    {
                        if (RearWingDamage == 0)
                        {
                            //Left 50, Right 0, Rear 0
                            image = "Images\\9.png";
                        }
                        else if (RearWingDamage < 50 && RearWingDamage > 0)
                        {
                            //Left 50, Right 0, Rear 50
                            image = "Images\\10.png";
                        }
                        else if (RearWingDamage >= 50)
                        {
                            //Left 50, Right 0, Rear 100
                            image = "Images\\11.png";
                        }
                    }
                    else if (RightFrontWingDamage < 50 && RightFrontWingDamage > 0)
                    {
                        if (RearWingDamage == 0)
                        {
                            //Left 50, Right 50, Rear 0
                            image = "Images\\12.png";
                        }
                        else if (RearWingDamage < 50 && RearWingDamage > 0)
                        {
                            //Left 50, Right 50, Rear 50
                            image = "Images\\13.png";
                        }
                        else if (RearWingDamage >= 50)
                        {
                            //Left 50, Right 50, Rear 100
                            image = "Images\\14.png";
                        }
                    }
                    else if (RightFrontWingDamage >= 50)
                    {
                        if (RearWingDamage == 0)
                        {
                            //Left 50, Right 100, Rear 0
                            image = "Images\\15.png";
                        }
                        else if (RearWingDamage < 50 && RearWingDamage > 0)
                        {
                            //Left 50, Right 100, Rear 50
                            image = "Images\\16.png";
                        }
                        else if (RearWingDamage >= 50)
                        {
                            //Left 50, Right 100, Rear 100
                            image = "Images\\17.png";
                        }
                    }
                }
                else if (LeftFrontWingDamage >= 50)
                {
                    if (RightFrontWingDamage == 0)
                    {
                        if (RearWingDamage == 0)
                        {
                            //Left 100, Right 0, Rear 0
                            image = "Images\\18.png";
                        }
                        else if (RearWingDamage < 50 && RearWingDamage > 0)
                        {
                            //Left 100, Right 0, Rear 50
                            image = "Images\\19.png";
                        }
                        else if (RearWingDamage >= 50)
                        {
                            //Left 100, Right 0, Rear 100
                            image = "Images\\20.png";
                        }
                    }
                    else if (RightFrontWingDamage < 50 && RightFrontWingDamage > 0)
                    {
                        if (RearWingDamage == 0)
                        {
                            //Left 100, Right 50, Rear 0
                            image = "Images\\21.png";
                        }
                        else if (RearWingDamage < 50 && RearWingDamage > 0)
                        {
                            //Left 100, Right 50, Rear 50
                            image = "Images\\22.png";
                        }
                        else if (RearWingDamage >= 50)
                        {
                            //Left 100, Right 50, Rear 100
                            image = "Images\\23.png";
                        }
                    }
                    else if (RightFrontWingDamage >= 50)
                    {
                        if (RearWingDamage == 0)
                        {
                            //Left 100, Right 100, Rear 0
                            image = "Images\\24.png";
                        }
                        else if (RearWingDamage < 50 && RearWingDamage > 0)
                        {
                            //Left 100, Right 100, Rear 50
                            image = "Images\\25.png";
                        }
                        else if (RearWingDamage >= 50)
                        {
                            //Left 100, Right 100, Rear 100
                            image = "Images\\26.png";
                        }
                    }
                }
                using (var stream = new FileStream(image, FileMode.Open))
                {
                    wing.BeginInit();
                    wing.CacheOption = BitmapCacheOption.OnLoad;
                    wing.StreamSource = stream;
                    wing.EndInit();
                }
                return wing;
            }
        }

        public string WingDamageString
        {
            get
            {
                if (LeftFrontWingDamage == 0)
                {
                    if (RightFrontWingDamage == 0)
                    {
                        if (RearWingDamage == 0)
                        {
                            //Left 0, Right 0, Rear 0
                            return "0";
                        }
                        if (RearWingDamage < 50)
                        {
                            //Left 0, Right 0, Rear 50
                            return "1";
                        }
                        else
                        {
                            //Left 0, Right 0, Rear 100
                            return "2";
                        }
                    }
                    if (RightFrontWingDamage < 50 && RightFrontWingDamage > 0)
                    {
                        if (RearWingDamage == 0)
                        {
                            //Left 0, Right 50, Rear 0
                            return "3";
                        }
                        if (RearWingDamage < 50)
                        {
                            //Left 0, Right 50, Rear 50
                            return "4";
                        }
                        else
                        {
                            //Left 0, Right 50, Rear 100
                            return "5";
                        }
                    }
                    else
                    {
                        if (RearWingDamage == 0)
                        {
                            //Left 0, Right 100, Rear 0
                            return "6";
                        }
                        else if (RearWingDamage < 50)
                        {
                            //Left 0, Right 100, Rear 50
                            return "7";
                        }
                        else
                        {
                            //Left 0, Right 100, Rear 100
                            return "8";
                        }
                    }
                }
                else if (LeftFrontWingDamage < 50)
                {
                    if (RightFrontWingDamage == 0)
                    {
                        if (RearWingDamage == 0)
                        {
                            //Left 50, Right 0, Rear 0
                            return "9";
                        }
                        else if (RearWingDamage < 50)
                        {
                            //Left 50, Right 0, Rear 50
                            return "10";
                        }
                        else
                        {
                            //Left 50, Right 0, Rear 100
                            return "11";
                        }
                    }
                    else if (RightFrontWingDamage < 50 && RightFrontWingDamage > 0)
                    {
                        if (RearWingDamage == 0)
                        {
                            //Left 50, Right 50, Rear 0
                            return "12";
                        }
                        else if (RearWingDamage < 50)
                        {
                            //Left 50, Right 50, Rear 50
                            return "13";
                        }
                        else
                        {
                            //Left 50, Right 50, Rear 100
                            return "14";
                        }
                    }
                    else
                    {
                        if (RearWingDamage == 0)
                        {
                            //Left 50, Right 100, Rear 0
                            return "15";
                        }
                        else if (RearWingDamage < 50)
                        {
                            //Left 50, Right 100, Rear 50
                            return "16";
                        }
                        else
                        {
                            //Left 50, Right 100, Rear 100
                            return "17";
                        }
                    }
                }
                else
                {
                    if (RightFrontWingDamage == 0)
                    {
                        if (RearWingDamage == 0)
                        {
                            //Left 100, Right 0, Rear 0
                            return "18";
                        }
                        else if (RearWingDamage < 50)
                        {
                            //Left 100, Right 0, Rear 50
                            return "19";
                        }
                        else
                        {
                            //Left 100, Right 0, Rear 100
                            return "20";
                        }
                    }
                    else if (RightFrontWingDamage < 50 && RightFrontWingDamage > 0)
                    {
                        if (RearWingDamage == 0)
                        {
                            //Left 100, Right 50, Rear 0
                            return "21";
                        }
                        else if (RearWingDamage < 50)
                        {
                            //Left 100, Right 50, Rear 50
                            return "22";
                        }
                        else
                        {
                            //Left 100, Right 50, Rear 100
                            return "23";
                        }
                    }
                    else
                    {
                        if (RearWingDamage == 0)
                        {
                            //Left 100, Right 100, Rear 0
                            return "24";
                        }
                        else if (RearWingDamage < 50)
                        {
                            //Left 100, Right 100, Rear 50
                            return "25";
                        }
                        else
                        {
                            //Left 100, Right 100, Rear 100
                            return "26";
                        }
                    }
                }
            }
        }

        public string ResultStatus
        {
            get
            {
                switch (_resulStatus)
                {
                    case "0": return "Invalid";
                    case "1": return "Inactive";
                    case "2": return "Active";
                    case "3": return "Finished";
                    case "4": return "Disq";
                    case "5": return "Not classified";
                    case "6": return "Retired";
                    default: return _resulStatus;
                }
            }
            set
            {
                _resulStatus = value;
            }
        }


        public string DRS
        {
            get
            {
                switch (_drs)
                {
                    case "0": return "Off";
                    case "1": return "On";
                    default: return _drs;
                }
            }
            set
            {
                _drs = value;
            }

        }


        public string FuelMix
        {
            get
            {
                switch (_fuelMix)
                {
                    case "0": return "Lean";
                    case "1": return "Standard";
                    case "2": return "Rich";
                    case "3": return "Max";
                    default: return _fuelMix;
                }
            }
            set
            {
                _fuelMix = value;
            }

        }

        public string ERSMode
        {
            get
            {
                switch (_ersMode)
                {
                    case "0": return "None";
                    case "1": return "Medium";
                    case "2": return "Hotlap";
                    case "3": return "Overtake";
                    default: return _fuelMix;
                }
            }
            set
            {
                _ersMode = value;
            }
        }
        public int LapDistance { get; set; }

        public string TractionControl { get; set; }
        public string AntiLockBrakes { get; set; }

        public string Speed { get; set; }
        public string BrakesTemp { get; set; }
        public string TyreSurfTemp { get; set; }
        public string TyreInnerTemp { get; set; }
        public string TyreAlert
        {
            get
            {
                if (LeftFrontInnerTemp > 110 ||
                    RightFrontInnerTemp > 110 ||
                    LeftRearInnerTemp > 110 ||
                    RightRearInnerTemp > 110)
                {
                    return "Red";
                }
                if (LeftFrontInnerTemp > 100 && LeftFrontInnerTemp <= 110 ||
                    RightFrontInnerTemp > 100 && RightFrontInnerTemp <= 110 ||
                    LeftRearInnerTemp > 100 && LeftRearInnerTemp <= 110 ||
                    RightRearInnerTemp > 100 && RightRearInnerTemp <= 110)
                { 
                    return "Yellow"; 
                }
                if (LeftFrontInnerTemp > 90 && LeftFrontInnerTemp <= 100 ||
                    RightFrontInnerTemp > 90 && RightFrontInnerTemp <= 100 ||
                    LeftRearInnerTemp > 90 && LeftRearInnerTemp <= 100 ||
                    RightRearInnerTemp > 90 && RightRearInnerTemp <= 100)
                { 
                    return "Green"; 
                }
                return "Blue";
            }
        }
        public int LeftFrontInnerTemp { get; set; }
        public int RightFrontInnerTemp { get; set; }
        public int LeftRearInnerTemp { get; set; }
        public int RightRearInnerTemp { get; set; }
        public string EngineTemp { get; set; }
        public string Fuel { get; set; }
        public string ERSStore { get; set; }

        public int m_position { get; set; }             // Finishing position
        public int m_numLaps { get; set; }               // Number of laps completed
        public int m_gridPosition { get; set; }          // Grid position of the car
        public int m_points { get; set; }                // Number of points scored
        public int m_numPitStops { get; set; }           // Number of pit stops made
        public int m_resultStatus { get; set; }          // Result status - 0 = invalid, 1 = inactive, 2 = active
                                                          // 3 = finished, 4 = disqualified, 5 = not classified
                                                          // 6 = retired
        public double m_bestLapTime { get; set; }           // Best lap time of the session in seconds
        public string BestLapTime
        {
            get
            {
                TimeSpan span = TimeSpan.FromMilliseconds(m_bestLapTime);
                return span.ToString(@"mm\:ss\:fff");
            }
            set
            {
                m_bestLapTime = Convert.ToDouble(value);
            }
        }
        public double m_totalRaceTime { get; set; }         // Total race time in seconds without penalties
        public string TotalRaceTime
        {
            get
            {
                TimeSpan span = TimeSpan.FromSeconds(m_totalRaceTime);
                return span.ToString(@"mm\:ss\:fff");
            }
            set
            {
                m_totalRaceTime = Convert.ToDouble(value);
            }
        }
        public int m_penaltiesTime { get; set; }        // Total penalties accumulated in seconds
        public int m_numPenalties { get; set; }         // Number of penalties applied to this driver
        public int m_numTyreStints { get; set; }         // Number of tyres stints up to maximum
        public byte[] m_tyreStintsActual { get; set; }   // Actual tyres used by this driver
        public byte[] m_tyreStintsVisual { get; set; }   // Visual tyres used by this driver
        public string FCGap
        {
            get
            {
                if (ViewModel.Leader == null || Session == null || Laps.Count == 0)
                    return "";
                if (Session == "1" || Session == "2" || Session == "3" || Session == "4" || Session == "5" || Session == "6" || Session == "7" || Session == "8" || Session == "9" || Session == "12")
                {
                    TimeSpan span = TimeSpan.FromMilliseconds(m_bestLapTime - ViewModel.Leader.m_bestLapTime);
                    return span.ToString(@"mm\:ss\:fff");
                }
                else
                {
                    //TimeSpan span = TimeSpan.FromSeconds(m_totalRaceTime - ViewModel.Leader.m_totalRaceTime);
                    //return span.ToString(@"mm\:ss\:fff");

                    TimeSpan span = TimeSpan.FromSeconds(ViewModel.Leader.m_totalRaceTime - (m_totalRaceTime + m_penaltiesTime));
                    TimeSpan leaderspan = TimeSpan.FromSeconds(m_totalRaceTime);
                    return ViewModel.Leader == this ? leaderspan.ToString(@"mm\:ss\:fff") : span.ToString(@"mm\:ss\:fff");

                }
            }
        }
    }
}
