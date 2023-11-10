using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.Windows.Threading;
using System.Data;

namespace F1
{
    public partial class ViewModel : ViewModelBase
    {
        #region "Private Fields"

        private ObservableCollection<User> _users;
        private DataTable _championshipStanding;
        private SeriesCollection _seriesCollection;
        private ObservableCollection<DriverGraph> _showDriversGraph;
        private ICommand _startCommand;
        private ICommand _stopCommand;
        private ICommand _selectDriversCommand;
        private ICommand _saveSettingsCommand;
        private ICommand _reloadCommand;
        private ICommand _reloadPosCommand;
        private int _numberOfDrivers;

        #endregion

        #region "Public Fileds"

        public static bool UdpAction1Pressed = false;
        public ObservableCollection<DataItem> _Nationality = new ObservableCollection<DataItem>();
        public static Dictionary<int, string> Drivers = new Dictionary<int, string>();
        public static ObservableCollection<DataItem> _Teams = new ObservableCollection<DataItem>();
        public static Dictionary<int, string> Tracks = new Dictionary<int, string>();
        public static Dictionary<int, string> ClassicDrivers = new Dictionary<int, string>();
        public static Dictionary<int, string> ClassicTeams = new Dictionary<int, string>();
        public static ObservableCollection<HighLight> _HighLights = new ObservableCollection<HighLight>();
        public ObservableCollection<string> _Colors = new ObservableCollection<string> { "Blue", "Yellow", "Orange", "Green" };
        public static User Leader;
        public static float TrackLength;
        public bool Run = true;

        public int PortNumber { get; set; }
        public bool SwitchingEnabled { get; set; }
        public int SwitchInterval { get; set; }
        public string SeasonName { get; set; }
        public ObservableCollection<string> Seasons { get; set; }
        public string SelectedSeason { get; set; }

        private string twlf;
        public string TyreWearLF
        {
            get
            {
                return twlf;
            }
            set
            {
                twlf = value;
                NotifyPropertyChanged("TyreWearLF");
            }
        }
        private string twrf;
        public string TyreWearRF
        {
            get
            {
                return twrf;
            }
            set
            {
                twrf = value;
                NotifyPropertyChanged("TyreWearRF");
            }
        }
        private string twlr;
        public string TyreWearLR
        {
            get
            {
                return twlr;
            }
            set
            {
                twlr = value;
                NotifyPropertyChanged("TyreWearLR");
            }
        }
        private string twrr;
        public string TyreWearRR
        {
            get
            {
                return twrr;
            }
            set
            {
                twrr = value;
                NotifyPropertyChanged("TyreWearRR");
            }
        }
        private string ttlr;
        public string TyreTempLR
        {
            get
            {
                return ttlr + "°";
            }
            set
            {
                ttlr = value;
                NotifyPropertyChanged("TyreTempLR");
            }
        }
        private string ttrf;
        public string TyreTempRF
        {
            get
            {
                return ttrf + "°";
            }
            set
            {
                ttrf = value;
                NotifyPropertyChanged("TyreTempRF");
            }
        }
        private string ttlf;
        public string TyreTempLF
        {
            get
            {
                return ttlf + "°";
            }
            set
            {
                ttlf = value;
                NotifyPropertyChanged("TyreTempLF");
            }
        }
        private string ttrr;
        public string TyreTempRR
        {
            get
            {
                return ttrr + "°";
            }
            set
            {
                ttrr = value;
                NotifyPropertyChanged("TyreTempRR");
            }
        }
        private int lfwd;
        public int LeftFrontWingDamage
        {
            get
            {
                return lfwd;
            }
            set
            {
                lfwd = value;
                NotifyPropertyChanged("LeftFrontWingDamage");
            }
        }
        private int rfwd;
        public int RightFrontWingDamage
        {
            get
            {
                return rfwd;
            }
            set
            {
                rfwd = value;
                NotifyPropertyChanged("RightFrontWingDamage");
            }
        }
        private int rwd;
        public int RearWingDamage
        {
            get
            {
                return rwd;
            }
            set
            {
                rwd = value;
                NotifyPropertyChanged("RearWingDamage");
            }
        }

        public static ObservableCollection<HighLight> HighLights
        {
            get { return _HighLights; }
            set { _HighLights = value; }
        }
        public ObservableCollection<string> Colors
        {
            get { return _Colors; }
            set { _Colors = value; }
        }

        public static ObservableCollection<DataItem> Teams
        {
            get { return _Teams; }
            set { _Teams = value; }
        }
        public ObservableCollection<DataItem> Nationality
        {
            get { return _Nationality; }
            set { _Nationality = value; }
        }
        public ObservableCollection<DriverGraph> ShowDriversGraphs
        {
            get { return _showDriversGraph; }
            set
            {
                _showDriversGraph = value;
                NotifyPropertyChanged("ShowDriverGraphs");
            }
        }
        public SeriesCollection SeriesCollection
        {
            get { return _seriesCollection; }
            set
            {
                _seriesCollection = value;
                NotifyPropertyChanged("SeriesCollection");
            }
        }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                NotifyPropertyChanged("Users");
            }
        }

        public DataTable ChamionshipStandings
        {
            get { return _championshipStanding; }
            set
            {
                _championshipStanding = value;
                NotifyPropertyChanged("ChamionshipStandings");
            }
        }


        private string _session;
        public string CurrentSession = "0";

        public int PlayerCarIndex;
        public string Session
        {
            get
            {
                switch (_session)
                {
                    case "1": return "Practice 1";
                    case "2": return "Practice 2";
                    case "3": return "Practice 3";
                    case "4": return "Practice";
                    case "5": return "Qualifying 1";
                    case "6": return "Qualifying 2";
                    case "7": return "Qualifying 3";
                    case "8": return "Qualifying";
                    case "9": return "One-Shot Qualifying";
                    case "10": return "Race1";
                    case "11": return "Race2";
                    case "12": return "Race3";
                    case "13": return "Time Trial";
                    default: return "Session";
                }
            }
            set
            {
                _session = value;
                NotifyPropertyChanged("Session");
            }
        }

        private string _weather;

        public string Weather
        {
            get
            {
                switch (_weather)
                {
                    case "0": return "Clear";
                    case "1": return "Light Cloud";
                    case "2": return "Overcast";
                    case "3": return "Light Rain";
                    case "4": return "Heavy Rain";
                    case "5": return "Storm";
                    default: return "Weather";
                }
            }
            set
            {
                _weather = value;
                NotifyPropertyChanged("Weather");
            }
        }

        public string AirTemp { get; set; }
        public string TrackTemp { get; set; }

        private int _track;

        public string Track
        {
            get
            {
                var value = "Track";
                if (Tracks.Count != 0 && _track != 100)
                    value = ViewModel.Tracks.FirstOrDefault(x => x.Key == _track).Value;
                return value;
            }
            set
            {
                _track = Convert.ToInt32(value);
                NotifyPropertyChanged("Track");
            }
        }

        private double _timeLeft;

        public string TimeLeft
        {
            get
            {
                TimeSpan span = TimeSpan.FromSeconds(_timeLeft);
                return span.ToString(@"mm\:ss\:fff");
            }
            set
            {
                _timeLeft = Convert.ToDouble(value);
                NotifyPropertyChanged("TimeLeft");
            }
        }

        public ICommand StartCommand
        {
            get
            {
                if (_startCommand == null)
                {
                    _startCommand = new RelayCommand(param => this.Start(),
                        null);
                }
                return _startCommand;
            }
        }

        public ICommand StopCommand
        {
            get
            {
                if (_stopCommand == null)
                {
                    _stopCommand = new RelayCommand(param => this.Stop(),
                        null);
                }
                return _stopCommand;
            }
        }

        public ICommand SelectDriversCommand
        {
            get
            {
                if (_selectDriversCommand == null)
                {
                    _selectDriversCommand = new RelayCommand(param => this.SelectDrivers(),
                        null);
                }
                return _selectDriversCommand;
            }
        }

        public ICommand SaveSettingsCommand
        {
            get
            {
                if (_saveSettingsCommand == null)
                {
                    _saveSettingsCommand = new RelayCommand(param => this.SaveSettings(),
                        null);
                }
                return _saveSettingsCommand;
            }
        }
        public ICommand ReloadCommand
        {
            get
            {
                if (_reloadCommand == null)
                {
                    _reloadCommand = new RelayCommand(param => this.ReloadStandings(false),
                        null);
                }
                return _reloadCommand;
            }
        }
        public ICommand ReloadPosCommand
        {
            get
            {
                if (_reloadPosCommand == null)
                {
                    _reloadPosCommand = new RelayCommand(param => this.ReloadStandings(true),
                        null);
                }
                return _reloadPosCommand;
            }
        }

        #endregion

        #region Constructor"

        public ViewModel()
        {
            //User = new User();
            Track = "100";
            Users = new ObservableCollection<User>();
            Users.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Users_CollectionChanged);
            SeriesCollection = new SeriesCollection();
            SeriesCollection.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Series_CollectionChanged);
            YFormatter = value => (value * -1).ToString();
            ReadData();
            ReadStandings(false);
            Run = true;
            Task.Run(() => UDPReader());

        }

        #endregion

        void Users_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("Users");
        }
        void Series_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("SeriesCollection");
        }
        private void Start()
        {
            Run = true;
        }

        private void Stop()
        {
            //Run = false;
            //WriteData();
        }

        public void UDPReader()
        {
            using (var cli = new UdpClient(PortNumber))
            {
                var ep = new IPEndPoint(IPAddress.Any, PortNumber);
                cli.EnableBroadcast = true;
                var userList = new List<User>();
                var showDrivers = new List<DriverGraph>();
                while (Run)
                {
                    Thread.Sleep(0);
                    var task = cli.ReceiveAsync();
                    task.Wait();
                    var buffer = task.Result;
                    var data = buffer.Buffer;
                    var packetType = UDPConverter.GetPacketHeader(data);
                    PlayerCarIndex = Convert.ToInt32(packetType.m_playerCarIndex);
                    switch (packetType.m_packetId)
                    {
                        case 0:
                            //Handle Motion data
                            //var motionData = ConvertToMotionData(data);
                            break;
                        case 1:
                            //Handle Session data
                            try
                            {
                                var sessionData = UDPConverter.ConvertToSessionData(data);
                                Session = sessionData.m_sessionType.ToString();
                                TimeLeft = sessionData.m_sessionTimeLeft.ToString();
                                Track = sessionData.m_trackId.ToString();
                                TrackTemp = sessionData.m_trackTemperature.ToString();
                                AirTemp = sessionData.m_airTemperature.ToString();
                                Weather = sessionData.m_weather.ToString();
                                TrackLength = sessionData.m_trackLength;
                                if (_session != CurrentSession)
                                {
                                    CurrentSession = _session;
                                    userList = new List<User>();
                                    SeriesCollection = new SeriesCollection();
                                }
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("Session Data" + e.Message);
                                throw;
                            }
                            break;
                        case 2:
                            //Handle Lap data
                            if (userList.Count > 0)
                            {
                                try
                                {
                                    var lapData = UDPConverter.ConvertToLapData(data);
                                    int index = 0;
                                    foreach (var ld in lapData.m_lapData)
                                    {
                                        if (index >= _numberOfDrivers)
                                            break;
                                        //userList[index]._bestLap = ld.m_bestLapTime;
                                        //userList[index]._currentLap = ld.m_currentLapTime;
                                        //TODO:
                                        //userList[index].BestLap = ld.m_bestLapTime.ToString();
                                        userList[index].CurrentLap = ld.m_currentLapTimeInMS.ToString();
                                        userList[index].CarPosition = ld.m_carPosition;
                                        userList[index].LapNum = ld.m_currentLapNum;
                                        userList[index].Pits = ld.m_pitStatus.ToString();
                                        userList[index].Penalties = ld.m_penalties.ToString();
                                        userList[index].ResultStatus = ld.m_resultStatus.ToString();
                                        userList[index].Session = _session;
                                        userList[index].LapDistance = Convert.ToInt32(ld.m_lapDistance);
                                        if (ld.m_currentLapNum == 1)
                                        {
                                            //Only on first lap, check if Start position is saved
                                            if (userList[index].StartPosSet == false)
                                            {
                                                //var lap = new Lap();
                                                //lap.Position = ld.m_carPosition;
                                                //userList[index].Laps.Add(ld.m_currentLapNum - 1, lap);
                                                AddLap(userList[index].ViewName, ld.m_carPosition);
                                                userList[index].StartPosSet = true;
                                            }
                                        }
                                        if (!userList[index].Laps.ContainsKey(ld.m_currentLapNum))
                                        {
                                            var lap = new Lap();
                                            lap.Section1 = ld.m_sector1TimeInMS;
                                            lap.Section2 = ld.m_sector2TimeInMS;
                                            lap.Time = 0;
                                            lap.Position = 0;
                                            userList[index].Laps.Add(ld.m_currentLapNum, lap);
                                        }
                                        else
                                        {
                                            userList[index].Laps[ld.m_currentLapNum].Section1 = ld.m_sector1TimeInMS;
                                            userList[index].Laps[ld.m_currentLapNum].Section2 = ld.m_sector2TimeInMS;
                                            if (ld.m_currentLapNum > 1)
                                            {
                                                if (!userList[index].Laps.ContainsKey(ld.m_currentLapNum - 1))
                                                {
                                                    var lap = new Lap();
                                                    lap.Time = ld.m_lastLapTimeInMS;
                                                    userList[index].Laps.Add(ld.m_currentLapNum - 1, lap);
                                                }
                                                else if (userList[index].Laps[ld.m_currentLapNum - 1].Position == 0)
                                                {
                                                    userList[index].Laps[ld.m_currentLapNum - 1].Time = ld.m_lastLapTimeInMS;
                                                    userList[index].Laps[ld.m_currentLapNum - 1].Position = ld.m_carPosition;
                                                    AddLap(userList[index].ViewName, ld.m_carPosition);
                                                }
                                            }
                                        }

                                        index++;
                                    }
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Lap Data" + e.Message);
                                    throw;
                                }
                            }
                            break;
                        case 3:
                            //Handle Event data
                            var eventData = UDPConverter.ConvertToEventData(data);
                            var sess = System.Text.Encoding.UTF8.GetString(eventData.m_eventStringCode);
                            if (sess == "SSTA")
                            {
                                userList = new List<User>();
                            }
                            else if (sess == "BUTN")
                            {
                                var buttonStatus = (int)eventData.m_eventDetails.butn.m_buttonStatus;
                                int udpAction1 = 1048576;
                                if ((buttonStatus & udpAction1) == 1048576)
                                {
                                    UdpAction1Pressed = true;
                                }
                                else
                                {
                                    //                                    UdpAction1Pressed = false;
                                }
                            }
                            break;
                        case 4:
                            //Handle Participants data
                            try
                            {
                                if (userList.Count == 0)
                                {
                                    var participantData = UDPConverter.ConvertToParticipantData(data);
                                    _numberOfDrivers = participantData.m_numActiveCars;
                                    userList = new List<User>();
                                    showDrivers = new List<DriverGraph>();
                                    foreach (var participant in participantData.m_participants)
                                    {
                                        //var s = new string(participant.m_name);
                                        var s = System.Text.Encoding.UTF8.GetString(participant.m_name);
                                        var user = new User
                                        {
                                            DriverId = participant.m_driverId,
                                            Name = s.Trim('\0'),
                                            Team = participant.m_teamId.ToString(),
                                            Country = participant.m_nationality,
                                            CarPosition = 0,
                                            PassMinus = 0,
                                            PassPlus = 0,
                                            BestLap = "0"
                                        };
                                        userList.Add(user);
                                        showDrivers.Add(new DriverGraph() { Name = user.ViewName, Checked = true });
                                        AddGraph(user.ViewName);
                                        if (userList.Count >= _numberOfDrivers)
                                            break;
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("Session Data" + e.Message);
                                throw;
                            }
                            break;
                        case 5:
                            //Handle Car setup data
                            //var setupData = ConvertToCarSetupData(data);
                            break;
                        case 6:
                            //Handle Car telemetry data
                            if (userList.Count > 0)
                            {
                                try
                                {
                                    var telemetryData = UDPConverter.ConvertToCarTelemetryData(data);
                                    int index = 0;
                                    foreach (var td in telemetryData.m_carTelemetryData)
                                    {
                                        if (index >= _numberOfDrivers)
                                            break;
                                        userList[index].Speed = td.m_speed.ToString();
                                        userList[index].DRS = td.m_drs.ToString();
                                        var a = td.m_brakesTemperature[0] + td.m_brakesTemperature[1] + td.m_brakesTemperature[2] + td.m_brakesTemperature[3];
                                        var b = a / 4;
                                        userList[index].BrakesTemp = b.ToString();
                                        var c = td.m_tyresSurfaceTemperature[0] + td.m_tyresSurfaceTemperature[1] + td.m_tyresSurfaceTemperature[2] + td.m_tyresSurfaceTemperature[3];
                                        var d = c / 4;
                                        userList[index].TyreSurfTemp = d.ToString();
                                        var e = td.m_tyresInnerTemperature[0] + td.m_tyresInnerTemperature[1] + td.m_tyresInnerTemperature[2] + td.m_tyresInnerTemperature[3];
                                        var f = e / 4;
                                        userList[index].TyreInnerTemp = f.ToString();
                                        userList[index].LeftFrontInnerTemp = td.m_tyresInnerTemperature[0];
                                        userList[index].RightFrontInnerTemp = td.m_tyresInnerTemperature[1];
                                        userList[index].LeftRearInnerTemp = td.m_tyresInnerTemperature[2];
                                        userList[index].RightRearInnerTemp = td.m_tyresInnerTemperature[3];
                                        userList[index].EngineTemp = td.m_engineTemperature.ToString();
                                        if (index == PlayerCarIndex)
                                        {
                                            TyreTempLF = td.m_tyresInnerTemperature[0].ToString();
                                            TyreTempRF = td.m_tyresInnerTemperature[1].ToString();
                                            TyreTempLR = td.m_tyresInnerTemperature[2].ToString();
                                            TyreTempRR = td.m_tyresInnerTemperature[3].ToString();

                                        }

                                        index++;
                                    }
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Telmetry Data" + e.Message);
                                    throw;
                                }
                            }
                            break;
                        case 7:
                            //Handle Car status data
                            if (userList.Count > 0)
                            {
                                try
                                {
                                    var statusData = UDPConverter.ConvertToCarStatusData(data);
                                    int index = 0;
                                    foreach (var sd in statusData.m_carStatusData)
                                    {
                                        if (index >= _numberOfDrivers)
                                            break;
                                        userList[index].TractionControl = sd.m_tractionControl.ToString();
                                        userList[index].AntiLockBrakes = sd.m_antiLockBrakes.ToString();
                                        userList[index].FuelMix = sd.m_fuelMix.ToString();
                                        userList[index].Fuel = sd.m_fuelRemainingLaps.ToString("0.##");
                                        //TODO:
                                        //var a = sd.m_tyresWear[0] + sd.m_tyresWear[1] + sd.m_tyresWear[2] + sd.m_tyresWear[3];
                                        //var b = a / 4;
                                        //var c = 100 - b;
                                        userList[index].TyreLaps = sd.m_tyresAgeLaps.ToString();
                                        userList[index].Tyre = sd.m_tyreVisualCompound.ToString();
                                        userList[index].ERSStore = sd.m_ersStoreEnergy.ToString();
                                        userList[index].ERSMode = sd.m_ersDeployMode.ToString();
                                        //userList[index].LeftFrontWingDamage = sd.m_frontLeftWingDamage;
                                        //userList[index].RightFrontWingDamage = sd.m_frontRightWingDamage;
                                        //userList[index].RearWingDamage = sd.m_rearWingDamage;
                                        index++;
                                    }
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Car Status Data" + e.Message);
                                    throw;
                                }
                            }
                            break;
                        case 8:
                            //Handle Final classification data
                            if (userList.Count > 0)
                            {
                                try
                                {
                                    var classificationData = UDPConverter.ConvertToFinalClassificationData(data);
                                    int index = 0;
                                    foreach (var sd in classificationData.m_classificationData)
                                    {
                                        if (index >= classificationData.m_numCars || index >= userList.Count())
                                            break;
                                        userList[index].m_position = sd.m_position;
                                        userList[index].m_points = sd.m_points;
                                        userList[index].m_numLaps = sd.m_numLaps;
                                        userList[index].m_gridPosition = sd.m_gridPosition;
                                        userList[index].m_numPitStops = sd.m_numPitStops;
                                        userList[index].m_resultStatus = sd.m_resultStatus;
                                        userList[index].BestLapTime = sd.m_bestLapTime.ToString();
                                        userList[index].TotalRaceTime = sd.m_totalRaceTime.ToString();
                                        userList[index].m_penaltiesTime = sd.m_penaltiesTime;
                                        userList[index].m_numPenalties = sd.m_numPenalties;
                                        userList[index].m_numTyreStints = sd.m_numTyreStints;
                                        index++;
                                    }
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Final Classification Data" + e.Message);
                                    throw;
                                }
                            }
                            break;
                        case 10:
                            //Handle Car damage data
                            if (userList.Count > 0)
                            {
                                try
                                {
                                    var statusData = UDPConverter.ConvertToCarDamageData(data);
                                    int index = 0;
                                    foreach (var sd in statusData.m_carDamageData)
                                    {
                                        if (index >= _numberOfDrivers)
                                            break;
                                        //TODO:
                                        var a = sd.m_tyresWear[0] + sd.m_tyresWear[1] + sd.m_tyresWear[2] + sd.m_tyresWear[3];
                                        var b = a / 4;
                                        var c = 100 - b;
                                        userList[index].TyreWear = Math.Round(c).ToString();
                                        userList[index].LeftFrontWingDamage = sd.m_frontLeftWingDamage;
                                        userList[index].RightFrontWingDamage = sd.m_frontRightWingDamage;
                                        userList[index].RearWingDamage = sd.m_rearWingDamage;
                                        if (index == PlayerCarIndex)
                                        {
                                            TyreWearLF = Math.Round(100 - sd.m_tyresWear[0]).ToString();
                                            TyreWearRF = Math.Round(100 - sd.m_tyresWear[1]).ToString();
                                            TyreWearLR = Math.Round(100 - sd.m_tyresWear[2]).ToString();
                                            TyreWearRR = Math.Round(100 - sd.m_tyresWear[3]).ToString();
                                            LeftFrontWingDamage = 100 - sd.m_frontLeftWingDamage;
                                            RightFrontWingDamage = 100 - sd.m_frontRightWingDamage;
                                            RearWingDamage = 100 - sd.m_rearWingDamage;

                                        }
                                        index++;
                                    }
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Car Damage Data" + e.Message);
                                    throw;
                                }
                            }
                            break;
                        case 11:
                            //Session history data
                            if (userList.Count > 0)
                            {
                                try
                                {
                                    var statusData = UDPConverter.ConvertToSessionHistoryData(data);
                                    int index = Convert.ToInt32(statusData.m_carIdx);
                                    if (statusData.m_bestLapTimeLapNum > 0 && index < userList.Count())
                                        userList[index].BestLap = statusData.m_lapHistoryData[statusData.m_bestLapTimeLapNum - 1].m_lapTimeInMS.ToString();

                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Session History Data" + e.Message);
                                    throw;
                                }
                            }
                            break;
                    }

                    if (packetType.m_packetId == 8)
                    {
                        List<User> sortedList = userList.OrderBy(y => y.m_position).ToList();
                        sortedList = sortedList.OrderBy(x => x.m_position == 0).ToList();
                        Users = new ObservableCollection<User>(sortedList);
                        WriteFinalClassification(sortedList);
                    }
                    else
                    {
                        List<User> sortedList = userList.OrderBy(y => y.CarPosition).ToList();
                        sortedList = sortedList.OrderBy(x => x.CarPosition == 0).ToList();
                        Users = new ObservableCollection<User>(sortedList);
                        ShowDriversGraphs = new ObservableCollection<DriverGraph>(showDrivers);
                        if (Users.Count > 0)
                            Leader = Users[0];
                    }
                }
            }
        }

        private void AddLap(string name, int position)
        {
            Application.Current.Dispatcher?.Invoke((Action)delegate
            {
                var serie = SeriesCollection.First(x => x.Title == name);
                serie.Values.Add(-position);

                Labels = new string[Users[0].Laps.Count];
                for (int i = 0; i < Users[0].Laps.Count; i++)
                {
                    Labels[i] = (i + 1).ToString();
                }

                //YFormatter = value => (value * -1).ToString();
            });
        }

        private void AddGraph(string Name)
        {
            Application.Current.Dispatcher?.Invoke((Action)delegate
            {
                var series = new LineSeries
                {
                    Title = Name,
                    Values = new ChartValues<int>(),
                    LineSmoothness = 0,
                    Fill = Brushes.Transparent
                };
                SeriesCollection.Add(series);
            });
        }

        private void RePrintGrapgh()
        {
            foreach (var driverGraph in ShowDriversGraphs)
            {
                if (driverGraph.Checked)
                {
                    var serie = (LineSeries)SeriesCollection.First(x => x.Title == driverGraph.Name);
                    serie.Visibility = Visibility.Visible;
                }
                else
                {
                    var serie = (LineSeries)SeriesCollection.First(x => x.Title == driverGraph.Name);
                    serie.Visibility = Visibility.Hidden;
                }
            }
        }

        private void SelectDrivers()
        {
            var sd = new SelectDrivers();
            //var showDrivers = new List<DriverGraph>();
            //showDrivers.Add(new DriverGraph() {Name = "Mats",Checked = true });
            //showDrivers.Add(new DriverGraph() { Name = "Juha", Checked = true });
            //ShowDriversGraphs = new ObservableCollection<DriverGraph>(showDrivers);
            sd.DataContext = this;
            sd.ShowDialog();
            RePrintGrapgh();
        }



        private void ReloadStandings(bool reloadPosition)
        {
            ReadStandings(reloadPosition);
        }

    }
}
