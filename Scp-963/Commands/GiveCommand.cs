using System;
using CommandSystem;
using InventorySystem.Items;
using LabApi.Features.Wrappers;

namespace Scp_963.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class GiveCommand : ICommand
    {
        public string Command { get; } = "scp963";
        public string[] Aliases { get; } = { "963" }; 
        public string Description { get; } = "Gives you an SCP-963";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Plugin.CustomItems.Add(Player.Get(sender).AddItem(ItemType.SCP1344, ItemAddReason.AdminCommand).Serial, 2);
            response = "You have got SCP-963";
            return true;
        }
    }
}