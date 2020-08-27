using Crestron.SimplSharpPro.DeviceSupport;
using Newtonsoft.Json;
using MockOccupancyDetector;
using PepperDash.Essentials.Core;
using PepperDash.Essentials.Core.Bridges;
using PepperDash.Core;

using Crestron.SimplSharp;

namespace MockOccupancyDetector
{
    /// <summary>
    /// Example of a plugin device
    /// </summary>
    public class MockOccupancyDetectorDevice : EssentialsDevice, IBridgeAdvanced, IOccupancyStatusProvider
    {
        private bool _isOccupied;

        public bool IsOccupied
        {
            get
            {
                return _isOccupied;
            }
            set
            {
                if (value != _isOccupied)
                {
                    _isOccupied = value;
                    Debug.Console(1, this, "Mock Occupancy Detector State: {0}", _isOccupied);
                    RoomIsOccupiedFeedback.FireUpdate();
                }
            }
        }

        #region IOccupancyStatusProvider Members

        public BoolFeedback RoomIsOccupiedFeedback { get; private set; }

        #endregion

        /// <summary>
        /// Device Constructor.  Called by BuildDevice
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="config"></param>
        public MockOccupancyDetectorDevice(string key, string name, MockOccupancyDetectorPropertiesConfig config)
            : base(key, name)
        {
            RoomIsOccupiedFeedback = new BoolFeedback(() => IsOccupied);

            CrestronConsole.AddNewConsoleCommand((s) => SetOccupiedStatus(s), "setoccupiedstate", 
                "Sets the state of the mock occupancy detector [true/false]", ConsoleAccessLevelEnum.AccessOperator);
        }

        public void SetOccupiedStatus(string state)
        {
            bool s;
            if (state == "true")
                s = true;
            else if (state == "false")
                s = false;
            else
                return;

            IsOccupied = s;
        }

        /// <summary>
        /// Add items to be executed during the Activaction phase
        /// </summary>
        /// <returns></returns>
        public override bool CustomActivate()
        {

            return base.CustomActivate();
        }

        /// <summary>
        /// This method gets called by the EiscApi bridge and calls your bridge extension method
        /// </summary>
        /// <param name="trilist"></param>
        /// <param name="joinStart"></param>
        /// <param name="joinMapKey"></param>
        public void LinkToApi(BasicTriList trilist, uint joinStart, string joinMapKey, EiscApiAdvanced bridge)
        {
            // Construct the default join map
            var joinMap = new MockOccupancyDetectorBridgeJoinMap(joinStart);

            // Attempt to get a custom join map if specified in config
            var joinMapSerialized = JoinMapHelper.GetJoinMapForDevice(joinMapKey);

            // If we find a custom join map, deserialize it
            if (!string.IsNullOrEmpty(joinMapSerialized))
                joinMap = JsonConvert.DeserializeObject<MockOccupancyDetectorBridgeJoinMap>(joinMapSerialized);

            //Checking if the bridge is null allows for backwards compatability with configurations that use EiscApi instead of EiscApiAdvanced
            if (bridge != null)
            {
                bridge.AddJoinMap(Key, joinMap);
            }

            // Set all your join actions here


            // Link all your feedbacks to joins here
        }


    }
}