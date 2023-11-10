using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace F1
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketHeader
    {
        public ushort m_packetFormat; // 2023
        public byte m_gameYear;                // Game year - last two digits e.g. 23
        public byte m_gameMajorVersion;     // Game major version - "X.00"
        public byte m_gameMinorVersion;     // Game minor version - "1.XX"
        public byte m_packetVersion; // Version of this packet type, all start from 1   
        public byte m_packetId; // Identifier for the packet type, see below
        public ulong m_sessionUID; // Unique identifier for the session
        public float m_sessionTime; // Session timestamp
        public uint m_frameIdentifier; // Identifier for the frame the data was retrieved on
        public uint m_overallFrameIdentifier;  // Overall identifier for the frame the data was retrieved
                                          // on, doesn't go back after flashbacks

        public byte m_playerCarIndex; // Index of player's car in the array
        public byte m_secondaryPlayerCarIndex;  // Index of secondary player's car in the array (splitscreen)
                                          // 255 if no second player
    };
    // 0 = Motion
    // 1 = Session
    // 2 = LapData
    // 3 = Event
    // 4 = Participants
    // 5 = Car setup
    // 6 = Car telemetry
    // 7 = Car status
    // 8 = Final classification
    // 9 = Lobby info
    // 10 = Car Damage
    // 11 = Session History

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CarMotionData
    {
        public float m_worldPositionX; // World space X position
        public float m_worldPositionY; // World space Y position
        public float m_worldPositionZ; // World space Z position
        public float m_worldVelocityX; // Velocity in world space X
        public float m_worldVelocityY; // Velocity in world space Y
        public float m_worldVelocityZ; // Velocity in world space Z
        public short m_worldForwardDirX; // World space forward X direction (normalised)
        public short m_worldForwardDirY; // World space forward Y direction (normalised)
        public short m_worldForwardDirZ; // World space forward Z direction (normalised)
        public short m_worldRightDirX; // World space right X direction (normalised)
        public short m_worldRightDirY; // World space right Y direction (normalised)
        public short m_worldRightDirZ; // World space right Z direction (normalised)
        public float m_gForceLateral; // Lateral G-Force component
        public float m_gForceLongitudinal; // Longitudinal G-Force component
        public float m_gForceVertical; // Vertical G-Force component
        public float m_yaw; // Yaw angle in radians
        public float m_pitch; // Pitch angle in radians
        public float m_roll; // Roll angle in radians
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketMotionData
    {
        public PacketHeader m_header; // Header
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public CarMotionData[] m_carMotionData; // Data for all cars on track
        // Extra player car ONLY data
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        //public float[] m_suspensionPosition; // Note: All wheel arrays have the following order:
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        //public float[] m_suspensionVelocity; // RL, RR, FL, FR
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        //public float[] m_suspensionAcceleration; // RL, RR, FL, FR
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        //public float[] m_wheelSpeed; // Speed of each wheel
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        //public float m_wheelSlip; // Slip ratio for each wheel
        //public float m_localVelocityX; // Velocity in local space
        //public float m_localVelocityY; // Velocity in local space
        //public float m_localVelocityZ; // Velocity in local space
        //public float m_angularVelocityX; // Angular velocity x-component
        //public float m_angularVelocityY; // Angular velocity y-component
        //public float m_angularVelocityZ; // Angular velocity z-component
        //public float m_angularAccelerationX; // Angular velocity x-component
        //public float m_angularAccelerationY; // Angular velocity y-component
        //public float m_angularAccelerationZ; // Angular velocity z-component
        //public float m_frontWheelsAngle; // Current front wheels angle in radians
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MarshalZone
    {
        public float m_zoneStart; // Fraction (0..1) of way through the lap the marshal zone starts
        public sbyte m_zoneFlag; // -1 = invalid/unknown, 0 = none, 1 = green, 2 = blue, 3 = yellow, 4 = red
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct WeatherForecastSample
    {
        public byte m_sessionType;                     // 0 = unknown, 1 = P1, 2 = P2, 3 = P3, 4 = Short P, 5 = Q1
                                                       // 6 = Q2, 7 = Q3, 8 = Short Q, 9 = OSQ, 10 = R, 11 = R2
                                                       // 12 = Time Trial
        public byte m_timeOffset;                      // Time in minutes the forecast is for
        public byte m_weather;                         // Weather - 0 = clear, 1 = light cloud, 2 = overcast
                                                       // 3 = light rain, 4 = heavy rain, 5 = storm
        public sbyte m_trackTemperature;                // Track temp. in degrees celsius
        public sbyte m_trackTemperatureChange;          // 0 = Up, 1 = Down, 2 = No change
        public sbyte m_airTemperature;                  // Air temp. in degrees celsius    };
        public sbyte m_airTemperatureChange;          // 0 = Up, 1 = Down, 2 = No change
        public byte m_rainPercentage;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketSessionData
    {
        public PacketHeader m_header; // Header

        public byte m_weather; // Weather - 0 = clear, 1 = light cloud, 2 = overcast 3 = light rain, 4 = heavy rain, 5 = storm
        public sbyte m_trackTemperature; // Track temp. in degrees celsius
        public sbyte m_airTemperature; // Air temp. in degrees celsius
        public byte m_totalLaps; // Total number of laps in this race
        public ushort m_trackLength; // Track length in metres
        public byte m_sessionType; // 0 = unknown, 1 = P1, 2 = P2, 3 = P3, 4 = Short P5 = Q1, 6 = Q2, 7 = Q3, 8 = Short Q, 9 = OSQ 10 = R, 11 = R2, 12 = Time Trial
        public sbyte m_trackId; // -1 for unknown, 0-21 for tracks, see appendix
        public byte m_formula; // Era, 0 = modern, 1 = classic 2=F2 3=Generic
        public ushort m_sessionTimeLeft; // Time left in session in seconds
        public ushort m_sessionDuration; // Session duration in seconds
        public byte m_pitSpeedLimit; // Pit speed limit in kilometres per hour
        public byte m_gamePaused; // Whether the game is paused
        public byte m_isSpectating; // Whether the player is spectating
        public byte m_spectatorCarIndex; // Index of the car being spectated
        public byte m_sliProNativeSupport; // SLI Pro support, 0 = inactive, 1 = active
        public byte m_numMarshalZones; // Number of marshal zones to follow
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
        public MarshalZone[] m_marshalZones; // List of marshal zones – max 21
        public byte m_safetyCarStatus; // 0 = no safety car, 1 = full safety car 2 = virtual safety car
        public byte m_networkGame; // 0 = offline, 1 = online
        public byte m_numWeatherForecastSamples; // Number of weather samples to follow
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 56)]
        WeatherForecastSample[] m_weatherForecastSamples;   // Array of weather forecast samples
        public byte m_forecastAccuracy;          // 0 = Perfect, 1 = Approximate
        public byte m_aiDifficulty;              // AI Difficulty rating – 0-110
        public uint m_seasonLinkIdentifier;      // Identifier for season - persists across saves
        public uint m_weekendLinkIdentifier;     // Identifier for weekend - persists across saves
        public uint m_sessionLinkIdentifier;     // Identifier for session - persists across saves
        public byte m_pitStopWindowIdealLap;     // Ideal lap to pit on for current strategy (player)
        public byte m_pitStopWindowLatestLap;    // Latest lap to pit on for current strategy (player)
        public byte m_pitStopRejoinPosition;     // Predicted position to rejoin at (player)
        public byte m_steeringAssist;            // 0 = off, 1 = on
        public byte m_brakingAssist;             // 0 = off, 1 = low, 2 = medium, 3 = high
        public byte m_gearboxAssist;             // 1 = manual, 2 = manual & suggested gear, 3 = auto
        public byte m_pitAssist;                 // 0 = off, 1 = on
        public byte m_pitReleaseAssist;          // 0 = off, 1 = on
        public byte m_ERSAssist;                 // 0 = off, 1 = on
        public byte m_DRSAssist;                 // 0 = off, 1 = on
        public byte m_dynamicRacingLine;         // 0 = off, 1 = corners only, 2 = full
        public byte m_dynamicRacingLineType;     // 0 = 2D, 1 = 3D
        public byte m_gameMode;                  // Game mode id -see appendix
        public byte m_ruleSet;                   // Ruleset -see appendix
        public uint m_timeOfDay;                 // Local time of day -minutes since midnight
        public byte m_sessionLength;             // 0 = None, 2 = Very Short, 3 = Short, 4 = Medium
                                                 //5 = Medium Long, 6 = Long, 7 = Full
        public byte m_speedUnitsLeadPlayer;             // 0 = MPH, 1 = KPH
        public byte m_temperatureUnitsLeadPlayer;       // 0 = Celsius, 1 = Fahrenheit
        public byte m_speedUnitsSecondaryPlayer;        // 0 = MPH, 1 = KPH
        public byte m_temperatureUnitsSecondaryPlayer;  // 0 = Celsius, 1 = Fahrenheit
        public byte m_numSafetyCarPeriods;              // Number of safety cars called during session
        public byte m_numVirtualSafetyCarPeriods;       // Number of virtual safety cars called
        public byte m_numRedFlagPeriods;                // Number of red flags called during session

    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LapData
    {
        public uint m_lastLapTimeInMS;            // Last lap time in milliseconds
        public uint m_currentLapTimeInMS;     // Current time around the lap in milliseconds
        public ushort m_sector1TimeInMS;           // Sector 1 time in milliseconds
        public byte m_sector1TimeMinutes;        // Sector 1 whole minute part
        public ushort m_sector2TimeInMS;           // Sector 2 time in milliseconds
        public byte m_sector2TimeMinutes;        // Sector 2 whole minute part
        public ushort m_deltaToCarInFrontInMS;     // Time delta to car in front in milliseconds
        public ushort m_deltaToRaceLeaderInMS;     // Time delta to race leader in milliseconds
        public float m_lapDistance;         // Distance vehicle is around current lap in metres – could
                                           // be negative if line hasn’t been crossed yet
        public float m_totalDistance;       // Total distance travelled in session in metres – could
                                            // be negative if line hasn’t been crossed yet
        public float m_safetyCarDelta;            // Delta in seconds for safety car
        public byte m_carPosition;             // Car race position
        public byte m_currentLapNum;       // Current lap number
        public byte m_pitStatus;               // 0 = none, 1 = pitting, 2 = in pit area
        public byte m_numPitStops;                 // Number of pit stops taken in this race
        public byte m_sector;                  // 0 = sector1, 1 = sector2, 2 = sector3
        public byte m_currentLapInvalid;       // Current lap invalid - 0 = valid, 1 = invalid
        public byte m_penalties;               // Accumulated time penalties in seconds to be added
        public byte m_totalWarnings;                  // Accumulated number of warnings issued
        public byte m_cornerCuttingWarnings;     // Accumulated number of corner cutting warnings issued
        public byte m_numUnservedDriveThroughPens;  // Num drive through pens left to serve
        public byte m_numUnservedStopGoPens;        // Num stop go pens left to serve
        public byte m_gridPosition;            // Grid position the vehicle started the race in
        public byte m_driverStatus;            // Status of driver - 0 = in garage, 1 = flying lap
                                         // 2 = in lap, 3 = out lap, 4 = on track
        public byte m_resultStatus;              // Result status - 0 = invalid, 1 = inactive, 2 = active
                                           // 3 = finished, 4 = didnotfinish, 5 = disqualified
                                           // 6 = not classified, 7 = retired
        public byte m_pitLaneTimerActive;          // Pit lane timing, 0 = inactive, 1 = active
        public ushort m_pitLaneTimeInLaneInMS;      // If active, the current time spent in the pit lane in ms
        public ushort m_pitStopTimerInMS;           // Time of the actual pit stop in ms
        public byte m_pitStopShouldServePen;   	 // Whether the car should serve a penalty at this stop
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketLapData
    {
        public PacketHeader m_header; // Header
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public LapData[] m_lapData; // Lap data for all cars on track
        public byte m_timeTrialPBCarIdx;  // Index of Personal Best car in time trial (255 if invalid)
        public byte m_timeTrialRivalCarIdx; 	// Index of Rival car in time trial (255 if invalid)

    };

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct EventDataDetails
    {
        [FieldOffset(0)]
        public FastestLap ftlp;

        [FieldOffset(0)]
        public Retirement rtmt;

        [FieldOffset(0)]
        public TeamMateInPits tmpt;

        [FieldOffset(0)]
        public RaceWinner rcwn;

        [FieldOffset(0)]
        public Penalty pnlt;

        [FieldOffset(0)]
        public SpeedTrap spt;

        [FieldOffset(0)]
        public StartLIghts stlg;

        [FieldOffset(0)]
        public DriveThroughPenaltyServed dtsv;

        [FieldOffset(0)]
        public StopGoPenaltyServed sgsv;

        [FieldOffset(0)]
        public Flashback flbk;

        [FieldOffset(0)]
        public Buttons butn;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FastestLap
    {
        public byte vehicleIdx; // Vehicle index of car achieving fastest lap
        public float lapTime;    // Lap time is in seconds
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Retirement
    {
        public byte vehicleIdx; // Vehicle index of car retiring
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TeamMateInPits
    {
        public byte vehicleIdx; // Vehicle index of team mate
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RaceWinner
    {
        public byte vehicleIdx; // Vehicle index of the race winner
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Penalty
    {
        public byte penaltyType;          // Penalty type – see Appendices
        public byte infringementType;     // Infringement type – see Appendices
        public byte vehicleIdx;           // Vehicle index of the car the penalty is applied to
        public byte otherVehicleIdx;      // Vehicle index of the other car involved
        public byte time;                 // Time gained, or time spent doing action in seconds
        public byte lapNum;               // Lap the penalty occurred on
        public byte placesGained;         // Number of places gained by this   
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SpeedTrap
    {
        public byte vehicleIdx; // Vehicle index of the race winner
        public float speed;      // Top speed achieved in kilometres per hour
        public byte overallFastestInSession;   // Overall fastest speed in session = 1, otherwise 0
        public byte driverFastestInSession;    // Fastest speed for driver in session = 1, otherwise 0
        public byte fastestVehicleIdxInSession;// Vehicle index of the vehicle that is the fastest in this session
        public float fastestSpeedInSession;      // Speed of the vehicle that is the fastest in this session
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct StartLIghts
    {
        public byte numLights;        // Number of lights showing
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DriveThroughPenaltyServed
    {
        public byte vehicleIdx;                 // Vehicle index of the vehicle serving drive through
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct StopGoPenaltyServed
    {
        public byte vehicleIdx;                 // Vehicle index of the vehicle serving stop go
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Flashback
    {
        public uint flashbackFrameIdentifier;  // Frame identifier flashed back to
        float flashbackSessionTime;       // Session time flashed back to
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Buttons
    {
        public uint m_buttonStatus;    // Bit flags specifying which buttons are being pressed
                                       // currently - see appendices
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketEventData
    {
        public PacketHeader m_header; // Header

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] m_eventStringCode; // Event string code, see above
        public EventDataDetails m_eventDetails;       // Event details - should be interpreted differently
                                               // for each type
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ParticipantData
    {
        public byte m_aiControlled; // Whether the vehicle is AI (1) or Human (0) controlled
        public byte m_driverId; // Driver id - see appendix
        public byte m_networkId;      // Network id – unique identifier for network players
        public byte m_teamId; // Team id - see appendix
        public byte m_myTeam;       // My team flag – 1 = My Team, 0 = otherwise
        public byte m_raceNumber; // Race number of the car
        public byte m_nationality; // Nationality of the driver
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        public byte[] m_name; // Name of participant in UTF-8 format – null terminated. Will be truncated with … (U+2026) if too long
        public byte m_yourTelemetry;          // The player's UDP setting, 0 = restricted, 1 = public
        public byte m_showOnlineNames;   // The player's show online names setting, 0 = off, 1 = on
        public byte m_platform;          // 1 = Steam, 3 = PlayStation, 4 = Xbox, 6 = Origin, 255 = unknown

    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketParticipantsData
    {
        public PacketHeader m_header; // Header

        public byte m_numActiveCars; // Number of cars in the data
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public ParticipantData[] m_participants;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CarSetupData
    {
        public byte m_frontWing; // Front wing aero
        public byte m_rearWing; // Rear wing aero
        public byte m_onThrottle; // Differential adjustment on throttle (percentage)
        public byte m_offThrottle; // Differential adjustment off throttle (percentage)
        public float m_frontCamber; // Front camber angle (suspension geometry)
        public float m_rearCamber; // Rear camber angle (suspension geometry)
        public float m_frontToe; // Front toe angle (suspension geometry)
        public float m_rearToe; // Rear toe angle (suspension geometry)
        public byte m_frontSuspension; // Front suspension
        public byte m_rearSuspension; // Rear suspension
        public byte m_frontAntiRollBar; // Front anti-roll bar
        public byte m_rearAntiRollBar; // Front anti-roll bar
        public byte m_frontSuspensionHeight; // Front ride height
        public byte m_rearSuspensionHeight; // Rear ride height
        public byte m_brakePressure; // Brake pressure (percentage)
        public byte m_brakeBias; // Brake bias (percentage)
        public float m_rearLeftTyrePressure; // Rear tyre pressure (PSI)
        public float m_rearRightTyrePressure; // Rear tyre pressure (PSI)
        public float m_frontLeftTyrePressure; // Front tyre pressure (PSI)
        public float m_frontRightTyrePressure; // Front tyre pressure (PSI)
        public byte m_ballast; // Ballast
        public float m_fuelLoad; // Fuel load
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketCarSetupData
    {
        public PacketHeader m_header; // Header
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public CarSetupData[] m_carSetups;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CarTelemetryData
    {
        public ushort m_speed; // Speed of car in kilometres per hour
        public float m_throttle; // Amount of throttle applied (0 to 100)
        public float m_steer; // Steering (-100 (full lock left) to 100 (full lock right))
        public float m_brake; // Amount of brake applied (0 to 100)
        public byte m_clutch; // Amount of clutch applied (0 to 100)
        public sbyte m_gear; // Gear selected (1-8, N=0, R=-1)
        public ushort m_engineRPM; // Engine RPM
        public byte m_drs; // 0 = off, 1 = on
        public byte m_revLightsPercent; // Rev lights indicator (percentage)
        public ushort m_revLightsBitValue;        // Rev lights (bit 0 = leftmost LED, bit 14 = rightmost LED)        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public ushort[] m_brakesTemperature; // Brakes temperature (celsius)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] m_tyresSurfaceTemperature; // Tyres surface temperature (celsius)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] m_tyresInnerTemperature; // Tyres inner temperature (celsius)
        public ushort m_engineTemperature; // Engine temperature (celsius)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] m_tyresPressure; // Tyres pressure (PSI)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] m_surfaceType;           // Driving surface, see appendices
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketCarTelemetryData
    {
        public PacketHeader m_header; // Header
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public CarTelemetryData[] m_carTelemetryData;
        //public uint m_buttonStatus; // Bit flags specifying which buttons are being pressed currently - see appendices
        public byte m_mfdPanelIndex;       // Index of MFD panel open - 255 = MFD closed
                                     // Single player, race – 0 = Car setup, 1 = Pits
                                     // 2 = Damage, 3 =  Engine, 4 = Temperatures
                                     // May vary depending on game mode
        public byte m_mfdPanelIndexSecondaryPlayer;   // See above
        public sbyte m_suggestedGear;       // Suggested gear for the player (1-8)
                                    // 0 if no gear suggested
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CarStatusData
    {
        public byte m_tractionControl; // 0 (off) - 2 (high)
        public byte m_antiLockBrakes; // 0 (off) - 1 (on)
        public byte m_fuelMix; // Fuel mix - 0 = lean, 1 = standard, 2 = rich, 3 = max
        public byte m_frontBrakeBias; // Front brake bias (percentage)
        public byte m_pitLimiterStatus; // Pit limiter status - 0 = off, 1 = on
        public float m_fuelInTank; // Current fuel mass
        public float m_fuelCapacity; // Fuel capacity
        public float m_fuelRemainingLaps;        // Fuel remaining in terms of laps (value on MFD)        
        public ushort m_maxRPM; // Cars max RPM, point of rev limiter
        public ushort m_idleRPM; // Cars idle RPM
        public byte m_maxGears; // Maximum number of gears
        public byte m_drsAllowed; // 0 = not allowed, 1 = allowed, -1 = unknown
        public ushort m_drsActivationDistance;    // 0 = DRS not available, non-zero - DRS will be available
                                           // in [X] metres
        public byte m_actualTyreCompound; // F1 Modern - 16 = C5, 17 = C4, 18 = C3, 19 = C2, 20 = C1 7 = inter, 8 = wet
        public byte m_tyreVisualCompound;       // F1 visual (can be different from actual compound)  16 = soft, 17 = medium, 18 = hard, 7 = inter, 8 = wet
        public byte m_tyresAgeLaps;             // Age in laps of the current set of tyres
        public sbyte m_vehicleFiaFlags; // -1 = invalid/unknown, 0 = none, 1 = green // 2 = blue, 3 = yellow, 4 = red
        public float m_enginePowerICE;           // Engine power output of ICE (W)
        public float m_enginePowerMGUK;          // Engine power output of MGU-K (W)
        public float m_ersStoreEnergy; // ERS energy store in Joules
        public byte m_ersDeployMode; // ERS deployment mode, 0 = none, 1 = medium  2 = overtake, 3 = hotlap
        public float m_ersHarvestedThisLapMGUK; // ERS energy harvested this lap by MGU-K
        public float m_ersHarvestedThisLapMGUH; // ERS energy harvested this lap by MGU-H
        public float m_ersDeployedThisLap; // ERS energy deployed this lap
        public byte m_networkPaused;            // Whether the car is paused in a network game    
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketCarStatusData
    {
        public PacketHeader m_header; // Header
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public CarStatusData[] m_carStatusData;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FinalClassificationData
    {
        public byte m_position;              // Finishing position
        public byte m_numLaps;               // Number of laps completed
        public byte m_gridPosition;          // Grid position of the car
        public byte m_points;                // Number of points scored
        public byte m_numPitStops;           // Number of pit stops made
        public byte m_resultStatus;          // Result status - 0 = invalid, 1 = inactive, 2 = active
                                             // 3 = finished, 4 = disqualified, 5 = not classified
                                             // 6 = retired
        public uint m_bestLapTime;           // Best lap time of the session in seconds
        public double m_totalRaceTime;         // Total race time in seconds without penalties
        public byte m_penaltiesTime;         // Total penalties accumulated in seconds
        public byte m_numPenalties;          // Number of penalties applied to this driver
        public byte m_numTyreStints;         // Number of tyres stints up to maximum
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] m_tyreStintsActual;   // Actual tyres used by this driver
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] m_tyreStintsVisual;   // Visual tyres used by this driver
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] m_tyreStintsEndLaps;  // The lap number stintsend on
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketFinalClassificationData
    {
        PacketHeader m_header;                             // Header

        public byte m_numCars;                 // Number of cars in the final classification
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public FinalClassificationData[] m_classificationData;
    };

    //LOBBY INFO Packet
    //TODO:

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CarDamageData
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] m_tyresWear; // Tyre wear percentage
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] m_tyresDamage; // Tyre damage (percentage)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] m_brakesDamage;                  // Brakes damage (percentage)
        public byte m_frontLeftWingDamage; // Front left wing damage (percentage)
        public byte m_frontRightWingDamage; // Front right wing damage (percentage)
        public byte m_rearWingDamage; // Rear wing damage (percentage)
        public byte m_floorDamage;                      // Floor damage (percentage)
        public byte m_diffuserDamage;                   // Diffuser damage (percentage)
        public byte m_sidepodDamage;                    // Sidepod damage (percentage)
        public byte m_drsFault;                 // Indicator for DRS fault, 0 = OK, 1 = fault
        public byte m_ersFault;                         // Indicator for ERS fault, 0 = OK, 1 = fault
        public byte m_gearBoxDamage;                    // Gear box damage (percentage)
        public byte m_engineDamage; // Engine damage (percentage)
        public byte m_engineMGUHWear;                   // Engine wear MGU-H (percentage)
        public byte m_engineESWear;                     // Engine wear ES (percentage)
        public byte m_engineCEWear;                     // Engine wear CE (percentage)
        public byte m_engineICEWear;                    // Engine wear ICE (percentage)
        public byte m_engineMGUKWear;                   // Engine wear MGU-K (percentage)
        public byte m_engineTCWear;                     // Engine wear TC (percentage)
        public byte m_engineBlown;                     // Engine blown, 0 = OK, 1 = fault
        public byte m_engineSeized;                     // Engine seized, 0 = OK, 1 = fault
    };
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketCarDamageData
    {
        public PacketHeader m_header; // Header
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public CarDamageData[] m_carDamageData;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LapHistoryData
    {
        public uint m_lapTimeInMS;           // Lap time in milliseconds
        public ushort m_sector1TimeInMS;       // Sector 1 time in milliseconds
        public byte m_sector1TimeMinutes;    // Sector 1 whole minute part
        public ushort m_sector2TimeInMS;       // Sector 2 time in milliseconds
        public byte m_sector2TimeMinutes;    // Sector 2 whole minute part
        public ushort m_sector3TimeInMS;       // Sector 3 time in milliseconds
        public byte m_sector3TimeMinutes;    // Sector 3 whole minute part
        public byte m_lapValidBitFlags;      // 0x01 bit set-lap valid,      0x02 bit set-sector 1 valid
                                       // 0x04 bit set-sector 2 valid, 0x08 bit set-sector 3 valid
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TyreStintHistoryData
    {
        public byte m_endLap;                // Lap the tyre usage ends on (255 of current tyre)
        public byte m_tyreActualCompound;    // Actual tyres used by this driver
        public byte m_tyreVisualCompound;    // Visual tyres used by this driver
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketSessionHistoryData
    {
        PacketHeader m_header;                   // Header

        public byte m_carIdx;                   // Index of the car this lap data relates to
        public byte m_numLaps;                  // Num laps in the data (including current partial lap)
        public byte m_numTyreStints;            // Number of tyre stints in the data

        public byte m_bestLapTimeLapNum;        // Lap the best lap time was achieved on
        public byte m_bestSector1LapNum;        // Lap the best Sector 1 time was achieved on
        public byte m_bestSector2LapNum;        // Lap the best Sector 2 time was achieved on
        public byte m_bestSector3LapNum;        // Lap the best Sector 3 time was achieved on

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public LapHistoryData[] m_lapHistoryData;   // 100 laps of data max
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public TyreStintHistoryData[] m_tyreStintsHistoryData;
    };










    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //struct UdpPacketData
    //{
    //    public float m_time; // Total seconds driven from start line
    //    public float m_lapTime; // Total seconds of current lap
    //    public float m_lapDistance; // Total distance through lap in meters
    //    public float m_totalDistance; // Total distance driven from start line
    //    public float m_x; // World space position
    //    public float m_y; // World space position
    //    public float m_z; // World space position
    //    public float m_speed; // Meters/sec
    //    public float m_xv; // Velocity in world space
    //    public float m_yv; // Velocity in world space
    //    public float m_zv; // Velocity in world space
    //    public float m_xr; // World space right direction
    //    public float m_yr; // World space right direction
    //    public float m_zr; // World space right direction
    //    public float m_xd; // World space forward direction
    //    public float m_yd; // World space forward direction
    //    public float m_zd; // World space forward direction

    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public float[] m_susp_pos; //

    //    //public float m_susp_pos_bl;             //
    //    //public float m_susp_pos_br;             //
    //    //public float m_susp_pos_fl;             //
    //    //public float m_susp_pos_fr;             //
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public float[] m_susp_vel; //

    //    //public float m_susp_vel_bl;             //
    //    //public float m_susp_vel_br;             //
    //    //public float m_susp_vel_fl;             //
    //    //public float m_susp_vel_fr;             //
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public float[] m_wheel_speed; //

    //    //public float m_wheel_speed_bl;          //
    //    //public float m_wheel_speed_br;          //
    //    //public float m_wheel_speed_fl;          //
    //    //public float m_wheel_speed_fr;          // 
    //    public float m_throttle; // Throttle input

    //    public float m_steer; // Steering input (-1 left to +1 right)
    //    public float m_brake; // Brake input
    //    public float m_clutch; // Clutch input
    //    public float m_gear; // 0 - R | 1 - N | 2-9 - 1-8
    //    public float m_gforce_lat; // Lateral G's
    //    public float m_gforce_lon; // Longiitude G's
    //    public float m_lap; // Current lap number
    //    public float m_engineRate; // Engine RPM
    //    public float m_sli_pro_native_support; // SLI Pro support
    //    public float m_car_position; // car race position
    //    public float m_kers_level; // kers energy left
    //    public float m_kers_max_level; // kers maximum energy
    //    public float m_drs; // 0 = off, 1 = on
    //    public float m_traction_control; // 0 (off) - 2 (high)
    //    public float m_anti_lock_brakes; // 0 (off) - 1 (on)
    //    public float m_fuel_in_tank; // current fuel mass
    //    public float m_fuel_capacity; // fuel capacity
    //    public float m_in_pits; // 0 = none, 1 = pitting, 2 = in pit area
    //    public float m_sector; // 0 = sector1, 1 = sector2; 2 = sector3
    //    public float m_sector1_time; // time of sector1 (or 0)
    //    public float m_sector2_time; // time of sector2 (or 0)

    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    //    public float[] m_brakes_temp; // brakes temperature (centigrade)

    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public float[] m_wheels_pressure; // wheels pressure PSI
    //    public float m_team_info; // team ID 
    //    public float m_total_laps; // total number of laps in this race
    //    public float m_track_size; // track size meters
    //    public float m_last_lap_time; // last lap time
    //    public float m_max_rpm; // cars max RPM, at which point the rev limiter will kick in
    //    public float m_idle_rpm; // cars idle RPM
    //    public float m_max_gears; // maximum number of gears
    //    public float m_sessionType; // 0 = unknown, 1 = practice, 2 = qualifying, 3 = race
    //    public float m_drsAllowed; // 0 = not allowed, 1 = allowed, -1 = invalid / unknown
    //    public float m_track_number; // -1 for unknown, 0-21 for tracks
    //    public float m_vehicleFIAFlags; // -1 = invalid/unknown, 0 = none, 1 = green, 2 = blue, 3 = yellow, 4 = red
    //    public float m_era; // era, 2017 (modern) or 1980 (classic)
    //    public float m_engine_temperature; // engine temperature (centigrade)
    //    public float m_gforce_vert; // vertical g-force component
    //    public float m_ang_vel_x; // angular velocity x-component
    //    public float m_ang_vel_y; // angular velocity y-component
    //    public float m_ang_vel_z; // angular velocity z-component

    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    //    public byte[] m_tyres_temperature; // tyres temperature (centigrade)

    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public byte[] m_tyres_wear; // tyre wear percentage

    //    public byte m_tyre_compound; // compound of tyre – 0 = ultra soft, 1 = super soft, 2 = soft, 3 = medium, 4 = hard, 5 = inter, 6 = wet

    //    public byte m_front_brake_bias; // front brake bias (percentage)
    //    public byte m_fuel_mix; // fuel mix - 0 = lean, 1 = standard, 2 = rich, 3 = max
    //    public byte m_currentLapInvalid; // current lap invalid - 0 = valid, 1 = invalid
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public byte[] m_tyres_damage; // tyre damage (percentage)
    //    public byte m_front_left_wing_damage; // front left wing damage (percentage)
    //    public byte m_front_right_wing_damage; // front right wing damage (percentage)
    //    public byte m_rear_wing_damage; // rear wing damage (percentage)
    //    public byte m_engine_damage; // engine damage (percentage)
    //    public byte m_gear_box_damage; // gear box damage (percentage)
    //    public byte m_exhaust_damage; // exhaust damage (percentage)
    //    public byte m_pit_limiter_status; // pit limiter status – 0 = off, 1 = on
    //    public byte m_pit_speed_limit; // pit speed limit in mph
    //    public float m_session_time_left; // NEW: time left in session in seconds 
    //    public byte m_rev_lights_percent; // NEW: rev lights indicator (percentage)
    //    public byte m_is_spectating; // NEW: whether the player is spectating
    //    public byte m_spectator_car_index; // NEW: index of the car being spectated

    //    // Car data
    //    public byte m_num_cars; // number of cars in data

    //    public byte m_player_car_index; // index of player's car in the array

    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
    //    public CarUDPData[] m_car_data; // data for all cars on track

    //    public float m_yaw; // NEW (v1.8)
    //    public float m_pitch; // NEW (v1.8)
    //    public float m_roll; // NEW (v1.8)
    //    public float m_x_local_velocity; // NEW (v1.8) Velocity in local space
    //    public float m_y_local_velocity; // NEW (v1.8) Velocity in local space
    //    public float m_z_local_velocity; // NEW (v1.8) Velocity in local space

    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    //    public float[] m_susp_acceleration; // NEW (v1.8) RL, RR, FL, FR

    //    public float m_ang_acc_x; // NEW (v1.8) angular acceleration x-component
    //    public float m_ang_acc_y; // NEW (v1.8) angular acceleration y-component
    //    public float m_ang_acc_z; // NEW (v1.8) angular acceleration z-component
    //}

    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct CarUDPData
    //{
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    //    public float[] m_worldPosition; // world co-ordinates of vehicle

    //    public float m_lastLapTime;
    //    public float m_currentLapTime;
    //    public float m_bestLapTime;
    //    public float m_sector1Time;
    //    public float m_sector2Time;
    //    public float m_lapDistance;
    //    public byte m_driverId;
    //    public byte m_teamId;
    //    public byte m_carPosition; // UPDATED: track positions of vehicle
    //    public byte m_currentLapNum;

    //    public byte m_tyreCompound
    //        ; // compound of tyre – 0 = ultra soft, 1 = super soft, 2 = soft, 3 = medium, 4 = hard, 5 = inter, 6 = wet

    //    public byte m_inPits; // 0 = none, 1 = pitting, 2 = in pit area
    //    public byte m_sector; // 0 = sector1, 1 = sector2, 2 = sector3
    //    public byte m_currentLapInvalid; // current lap invalid - 0 = valid, 1 = invalid
    //    public byte m_penalties; // NEW: accumulated time penalties in seconds to be added
    //};

}
