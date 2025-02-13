
using System;
using System.Collections.Generic;
using GameCore;
using InventorySystem;
using InventorySystem.Items;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using MEC;
using PlayerRoles;
using UnityEngine;
using Logger = LabApi.Features.Console.Logger;

namespace dr.EventHandlers
{
    
    public class Googles 
    {
        private static Vector3 spawn{get;set;} = new (Config.Instance.spawnpointx, Config.Instance.spawnpointy, Config.Instance.spawnpointz);
        private static int itemspawned {get; set;}
        private static RoleTypeId StartRole {get;set;}
        private static bool Used { get; set; }
        private static Player DrBat1 {get; set;}
         private static string DrBat2 {get; set;}

         public static void BeforeRoundCheck(PlayerJoinedEventArgs ev)
         {
            



              if (RoundStart.RoundStarted == false)
              {
                  Timing.CallDelayed(4f, () =>
                  {
                      if (itemspawned < 1)
                      {

                          itemspawned++;
                          ev.Player.Role = RoleTypeId.Scientist;
                          ev.Player.Position = spawn;
                          ev.Player.Inventory.UserInventory.Items.Clear();
                          
                          var citem = ev.Player.AddItem(ItemType.SCP1344).Serial;
                          Logger.Info(citem);
                          Class1.Instance.customitem.Add(Convert.ToString(citem), 2);
                         
                          Timing.CallDelayed(1f, () =>
                          {
                              ev.Player.DropItem(citem);
                             

                              ev.Player.Role = RoleTypeId.Spectator;
                          });

                      }
                  });
              }
              else if (ev.Player.UserId == DrBat2)
              { 
                  Used = true;
                  DrBat1 = ev.Player;
              }

         }

        public static void ResetVars()
        {
            Used = false;
            itemspawned = 0;
            DrBat1 = null;
            DrBat2 = null;
        }


        public static void OnDrop(PlayerDroppingItemEventArgs ev)
        {
            if (Class1.Instance.customitem.ContainsKey(Convert.ToString(ev.Item.Serial)))
            {
                if (Class1.Instance.customitem[Convert.ToString(ev.Item.Serial)] == 2)
                {
                    if (ev.Player == DrBat1)
                    {
                        ev.Player.DropEverything();
                        ev.Player.Kill("Sudden cessation of life. No clear trauma") ;
                        
                    }
                }
            } 
        }

        public static void UsingItem(PlayerUsingItemEventArgs ev)
        {
            if (Class1.Instance.customitem.ContainsKey(Convert.ToString(ev.Item.Serial)))
            {
                if (Class1.Instance.customitem[Convert.ToString(ev.Item.Serial)] == 2)
                {



                   
                    ev.IsAllowed = false;
                   

                    if (Used == !true)
                    {
                        StartRole = ev.Player.Role;
                        DrBat1 = ev.Player;
                        DrBat2 = ev.Player.UserId;
                        ev.Player.DisplayName = "Scp-963(" + DrBat1.Nickname + ")";
                        Used = true;
                    }
                    else
                    {


                        if (ev.Player == DrBat1) return;
                        ev.Player.Inventory.UserInventory.Items.Remove(ev.Item.Serial);
                        
                        List<ItemBase> inventory = new List<ItemBase>(ev.Player.Inventory.UserInventory.Items.Values);
                       
                       
                        switch (Config.Instance.OldRole_Setstheroletotheoldplayerrole)
                        {
                            case 0:
                                
                                DrBat1.Role = RoleTypeId.Scientist;
                                break;

                            case 1:
                          
                                DrBat1.Role = ev.Player.Role;
                                break;

                            case 2:
                               
                                DrBat1.Role = StartRole;
                                break;

                            default:
                                
                                DrBat1.Role = RoleTypeId.Scientist;
                                break;
                        }
                       
                        DrBat1.Inventory.UserInventory.Items.Clear();
                        DrBat1.DisplayName = "Scp-963(" + DrBat1.Nickname + ")";





                        
                        
                        if (DrBat1.IsAlive)
                        {
                            if (DrBat1 != null)
                            { 
                               
                                var citem = DrBat1.AddItem(ItemType.SCP1344).Serial;
                                Class1.Instance.customitem.Add(Convert.ToString(citem), 2);
                                
                            }
                        }

                         
                        /*  if (Config.Instance.showasoldrole)
                          {
                            
                              ev.Player.ChangeAppearance(ev.Player.Role.Type);
                             
                         */ // }
                       
                       
                        DrBat1.Position = ev.Player.Position;
                        DrBat1.Rotation = ev.Player.Rotation;
                        foreach (var item in inventory)
                        {
                            DrBat1.AddItem(item.ItemTypeId);
                            ev.Player.RemoveItem(item);
                            //DrBat1.Inventory.UserInventory.Items.Add(item.ItemSerial, item);
                            //ev.Player.Inventory.UserInventory.Items.Remove(item.ItemSerial);
                            //change owner here


                          






                        }
                        
                        DrBat1.AddAmmo(ItemType.Ammo9x19, ev.Player.Inventory.GetCurAmmo(ItemType.Ammo9x19));
                        DrBat1.AddAmmo(ItemType.Ammo556x45, ev.Player.Inventory.GetCurAmmo(ItemType.Ammo556x45));
                        DrBat1.AddAmmo(ItemType.Ammo762x39, ev.Player.Inventory.GetCurAmmo(ItemType.Ammo762x39));
                        DrBat1.AddAmmo(ItemType.Ammo12gauge, ev.Player.Inventory.GetCurAmmo(ItemType.Ammo12gauge));
                        DrBat1.AddAmmo(ItemType.Ammo44cal, ev.Player.Inventory.GetCurAmmo(ItemType.Ammo44cal));

                        ev.Player.Inventory.UserInventory.Items.Clear();
                        ev.Player.Role = RoleTypeId.Spectator;


                    }
                }
               
                
            }
           
            

        }


        
        
    

         public static void Dead(PlayerDyingEventArgs ev)
        {
           if (ev.Player == DrBat1)
           {
               ev.Player.DisplayName = null;
           }
        }

        public static void noescape(PlayerEscapingEventArgs ev)
        {
                if (ev.Player == DrBat1)
                {
                    ev.IsAllowed = false;
                }
        }

        public static void nospawnwave(PlayerChangedRoleEventArgs ev)
        {
          
              if (ev.Player == DrBat1)
              {
                   if (ev.ChangeReason == RoleChangeReason.Respawn || ev.ChangeReason == RoleChangeReason.RespawnMiniwave)
                   {
                        Logger.Info("here" + ev.Player);
                        DrBat1 = null;
                        DrBat2 = null;
                        Used = false; 
                   }
              }
        }
            
        

        public static void Drleft(PlayerLeftEventArgs ev)
        {
            if (ev.Player == DrBat1)
            {
                Used = false;
            }
        }


        
                
                      
                    


                    

    }
}