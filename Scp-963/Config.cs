using System.ComponentModel;

namespace Scp_963
{
    public class Config
    {
        [Description("Chaged the SCP-963 role mode. (0 = Always become Scientist; 1 = Keep the wearer`s role; 2 = Keep the old role; other = Always become Scientist)")]
        public  int Scp963RoleSetterMode { get; set; } = 0;

        [Description("Spawn point room")]
        public string SpawnPointRoomName { get; set; } = "Outside";

        [Description("Spawn position x")]
        public float SpawnPointX { get; set; } = 123.703f;

        [Description("Spawn position y")]
        public float SpawnPointY { get; set; } = -11f;

        [Description("Spawn position z")]
        public float SpawnPointZ { get; set; } = 18.218f;
    }
}