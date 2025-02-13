using System.Collections.Generic;
using LabApi.Events.Handlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using Version = System.Version;

namespace dr
{
    
    public class Class1 : Plugin<Config>
    {

        
        public static Class1 Instance { get; private set; }

        
        public override string Name { get; } = "Scp-963";
        public override string Description { get; } = "";
        public override string Author { get; } = "Choco";

        public override Version Version { get; } = new Version(1, 0, 0, 0);

        public override Version RequiredApiVersion { get; } = new(LabApiProperties.CompiledVersion);

        public override void Enable()
        {
            Instance = this;
            ServerEvents.RoundStarted += EventHandlers.Googles.ResetVars;
            PlayerEvents.Dying += EventHandlers.Googles.Dead;
            PlayerEvents.Joined += EventHandlers.Googles.BeforeRoundCheck;
            PlayerEvents.UsingItem += EventHandlers.Googles.UsingItem;
            PlayerEvents.DroppingItem += EventHandlers.Googles.OnDrop;
            PlayerEvents.Escaping += EventHandlers.Googles.noescape;
            PlayerEvents.ChangedRole += EventHandlers.Googles.nospawnwave;
            PlayerEvents.Left += EventHandlers.Googles.Drleft;
        }

        public override void Disable()
        {
            ServerEvents.RoundStarted -= EventHandlers.Googles.ResetVars;
            PlayerEvents.Dying -= EventHandlers.Googles.Dead;
            PlayerEvents.Joined -= EventHandlers.Googles.BeforeRoundCheck;
            PlayerEvents.UsingItem -= EventHandlers.Googles.UsingItem;
            PlayerEvents.DroppingItem -= EventHandlers.Googles.OnDrop;
            PlayerEvents.Escaping -= EventHandlers.Googles.noescape;
           PlayerEvents.ChangedRole -= EventHandlers.Googles.nospawnwave;
            PlayerEvents.Left -= EventHandlers.Googles.Drleft;
           
        }
        public  Dictionary<string, int> customitem = new();











    }
        
        
        
}
