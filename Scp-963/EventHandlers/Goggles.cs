using InventorySystem;
using InventorySystem.Items;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using MapGeneration;
using MEC;
using PlayerRoles;
using System.Linq;
using UnityEngine;

namespace Scp_963.EventHandlers
{
    public class Goggles
    {
        private Vector3 SpawnPoint = new(Plugin.Instance.Config.SpawnPointX, Plugin.Instance.Config.SpawnPointY, Plugin.Instance.Config.SpawnPointZ);
        private string Scp963UserId;
        private RoleTypeId OldScp963Role;
        private bool Used;

        public void OnPlayerJoined(PlayerJoinedEventArgs ev)
        {
            if (ev.Player.UserId == Scp963UserId)
                Used = true;
        }

        public void OnRoundStart()
        {
            Used = false;
            Scp963UserId = null;

            Item scp963 = Server.Host.AddItem(ItemType.SCP1344);
            Plugin.CustomItems.Add(scp963.Serial, 2);

            Pickup scp963Pickup = Server.Host.DropItem(scp963);

            if (scp963Pickup == null)
                return;

            RoomIdentifier room = null;

            if (RoomIdentifier.AllRoomIdentifiers.Count(x => x.name.Contains(Plugin.Instance.Config.SpawnPointRoomName)) > 0)
                room = RoomIdentifier.AllRoomIdentifiers.First(x => x.name.Contains(Plugin.Instance.Config.SpawnPointRoomName));
            else
            {
                room = RoomIdentifier.AllRoomIdentifiers.First(x => x.Name == RoomName.EzGateA);
                scp963Pickup.Position = room.transform.position + Vector3.up;
                return;
            }

            scp963Pickup.Position = room.transform.position + SpawnPoint;
        }

        public void OnPlayerDroppingItem(PlayerDroppingItemEventArgs ev)
        {
            if (Plugin.CustomItems.ContainsKey(ev.Item.Serial))
                if (Plugin.CustomItems[ev.Item.Serial] == 2)
                    if (ev.Player.UserId == Scp963UserId)
                    {
                        ev.Player.DropEverything();
                        ev.Player.Kill("Sudden cessation of life. No clear trauma");
                    }
        }

        public void OnPlayerUsingItem(PlayerUsingItemEventArgs ev)
        {
            if (Plugin.CustomItems.ContainsKey(ev.Item.Serial))
            {
                if (Plugin.CustomItems[ev.Item.Serial] == 2)
                {
                    ev.IsAllowed = false;

                    // Very funny
                    if (Used == !true)
                    {
                        OldScp963Role = ev.Player.Role;
                        Scp963UserId = ev.Player.UserId;
                        ev.Player.DisplayName = "Scp-963(" + ev.Player.Nickname + ")";
                        Used = true;
                    }
                    else
                    {
                        if (ev.Player.UserId == Scp963UserId || !Player.TryGet(Scp963UserId, out Player Scp963Player))
                            return;

                        ev.Player.ReferenceHub.inventory.ServerRemoveItem(ev.Item.Serial, ev.Item.Base.PickupDropModel);

                        switch (Plugin.Instance.Config.Scp963RoleSetterMode)
                        {
                            case 0:
                                Scp963Player.Role = RoleTypeId.Scientist;
                                break;

                            case 1:
                                Scp963Player.Role = ev.Player.Role;
                                break;

                            case 2:
                                Scp963Player.Role = OldScp963Role;
                                break;

                            default:
                                Scp963Player.Role = RoleTypeId.Scientist;
                                break;
                        }

                        if (Scp963Player.Inventory.UserInventory.Items.Count > 0)
                            while (Scp963Player.Inventory.UserInventory.Items.Count > 0)
                                Scp963Player.Inventory.ServerRemoveItem(Scp963Player.Inventory.UserInventory.Items.ElementAt(0).Key, null);

                        Scp963Player.DisplayName = "Scp-963(" + Scp963Player.Nickname + ")";

                        if (Scp963Player.IsAlive && Scp963Player != null)
                            Plugin.CustomItems.Add(Scp963Player.AddItem(ItemType.SCP1344).Serial, 2);

                        Scp963Player.Position = ev.Player.Position;
                        Scp963Player.Rotation = ev.Player.Rotation;

                        foreach (ItemBase item in ev.Player.Inventory.UserInventory.Items.Values)
                            Scp963Player.AddItem(item.ItemTypeId);

                        Scp963Player.AddAmmo(ItemType.Ammo9x19, ev.Player.GetAmmo(ItemType.Ammo9x19));
                        Scp963Player.AddAmmo(ItemType.Ammo556x45, ev.Player.GetAmmo(ItemType.Ammo556x45));
                        Scp963Player.AddAmmo(ItemType.Ammo762x39, ev.Player.GetAmmo(ItemType.Ammo762x39));
                        Scp963Player.AddAmmo(ItemType.Ammo12gauge, ev.Player.GetAmmo(ItemType.Ammo12gauge));
                        Scp963Player.AddAmmo(ItemType.Ammo44cal, ev.Player.GetAmmo(ItemType.Ammo44cal));

                        if (ev.Player.Inventory.UserInventory.Items.Count > 0)
                            while (ev.Player.Inventory.UserInventory.Items.Count > 0)
                                ev.Player.Inventory.ServerRemoveItem(ev.Player.Inventory.UserInventory.Items.ElementAt(0).Key, null);

                        ev.Player.SetRole(RoleTypeId.Spectator);
                    }
                }
            }
        }

        public void OnPlayerDying(PlayerDyingEventArgs ev)
        {
            if (ev.Player.UserId == Scp963UserId)
                ev.Player.DisplayName = null;
        }

        public void OnPlayerEscaping(PlayerEscapingEventArgs ev)
        {
            if (ev.Player.UserId == Scp963UserId)
                ev.IsAllowed = false;
        }

        public void OnPlayerChangedRole(PlayerChangedRoleEventArgs ev)
        {
            if (ev.Player.UserId == Scp963UserId)
                if (ev.ChangeReason == RoleChangeReason.Respawn || ev.ChangeReason == RoleChangeReason.RespawnMiniwave)
                {
                    Used = false;
                    Scp963UserId = null;
                }
        }

        public void OnPlayerLeft(PlayerLeftEventArgs ev)
        {
            if (ev.Player.UserId == Scp963UserId)
                Used = false;
        }
    }
}