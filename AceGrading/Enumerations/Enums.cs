using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AceGrading
{
    public enum Online_Status { Offline, Online, Finished };
    public enum Wifi_Status { UsingWifi, AbstainingWifi, NotMonitored };
    public enum Cheating_Role { Perpetrator, Victim, Uncertain };
    public enum Test_Status { NotStarted, Started, Paused, CollectingData, Ended };
}
