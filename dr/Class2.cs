using System;
using CommandSystem;
using dr;
using InventorySystem.Items;
using LabApi.Features.Wrappers;

namespace ladapi
{
   
     
    
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public  class class2 : ICommand
    {
        
        public static class2 Instance = new class2();
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var citem=  Player.Get(sender).AddItem(ItemType.SCP1344, ItemAddReason.AdminCommand).Serial;
            Class1.Instance.customitem.Add(Convert.ToString(citem),2);
            response = "hiiii";
            return true;
        }

        public string Command { get; } = "Scp-963";
        public string[] Aliases { get; } = Array.Empty<string>(); 
        public string Description { get; } = "";
        
    }
}