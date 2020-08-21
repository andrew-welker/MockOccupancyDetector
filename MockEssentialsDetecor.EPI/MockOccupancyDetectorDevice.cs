using Crestron.SimplSharpPro.DeviceSupport;
using Newtonsoft.Json;
using MockOccupancyDetector;
using PepperDash.Essentials.Core;
using PepperDash.Essentials.Core.Bridges;
using PepperDash.Core;

namespace MockOccupancyDetector
{
    /// <summary>
    /// Example of a plugin device
    /// </summary>
    public class MockOccupancyDetectorDevice : EssentialsDevice, IBridgeAdvanced
    {
        /// <summary>
        /// Device Constructor.  Called by BuildDevice
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="config"></param>
        public MockOccupancyDetectorDevice(string key, string name, MockOccupancyDetectorPropertiesConfig config)
            : base(key, name)
        {

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