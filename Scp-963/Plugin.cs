using System.Collections.Generic;
using LabApi.Events.Handlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using Scp_963.EventHandlers;
using Version = System.Version;

namespace Scp_963
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance { get; private set; }
        
        public override string Name { get; } = "Scp-963";
        public override string Description { get; } = "SCP-963";
        public override string Author { get; } = "Choco";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredApiVersion { get; } = new(LabApiProperties.CompiledVersion);

        public static Dictionary<ushort, int> CustomItems;

        public Goggles Goggles;

        public override void Enable()
        {
            Instance = this;
            CustomItems = new();
            Goggles = new();

            ServerEvents.RoundStarted += Goggles.OnRoundStart;
            PlayerEvents.Dying += Goggles.OnPlayerDying;
            PlayerEvents.Joined += Goggles.OnPlayerJoined;
            PlayerEvents.UsingItem += Goggles.OnPlayerUsingItem;
            PlayerEvents.DroppingItem += Goggles.OnPlayerDroppingItem;
            PlayerEvents.Escaping += Goggles.OnPlayerEscaping;
            PlayerEvents.ChangedRole += Goggles.OnPlayerChangedRole;
            PlayerEvents.Left += Goggles.OnPlayerLeft;
        }

        public override void Disable()
        {
            ServerEvents.RoundStarted -= Goggles.OnRoundStart;
            PlayerEvents.Dying -= Goggles.OnPlayerDying;
            PlayerEvents.Joined -= Goggles.OnPlayerJoined;
            PlayerEvents.UsingItem -= Goggles.OnPlayerUsingItem;
            PlayerEvents.DroppingItem -= Goggles.OnPlayerDroppingItem;
            PlayerEvents.Escaping -= Goggles.OnPlayerEscaping;
            PlayerEvents.ChangedRole -= Goggles.OnPlayerChangedRole;
            PlayerEvents.Left -= Goggles.OnPlayerLeft;

            CustomItems = null;
            Goggles = null;
            Instance = null;
        }
    }
}
