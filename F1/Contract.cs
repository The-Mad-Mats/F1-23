using System.Collections.Generic;

namespace F1
{
    public class Contract
    {
        public int PacketId;
        //Session
        public string Track;
        public string Session;
        public string TimeLeft;
        public string Weather;
        public string AirTemp;
        public string TrackTemp;

        public List<User> Users;
    }
}
