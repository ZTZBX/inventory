using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using System.Collections.Generic;
using Newtonsoft.Json;

using static CitizenFX.Core.Native.API;

namespace inventory.Server
{
    public class InventoryLogic : BaseScript
    {
        InventoryData invData = new InventoryData();

        public InventoryLogic()
        {
            EventHandlers["getInventory"] += new Action<Player, string>(GetInventory);
            EventHandlers["changeItemPosition"] += new Action<Player, string, string, string>(ChangeItemPosition);
            EventHandlers["dropItems"] += new Action<Player, string, string, string, string>(DropItems);
        }

        private void DropItems([FromSource] Player user, string token, string itemname, string quantity, string username)
        {
            int quantityi = Int32.Parse(quantity);
            bool newBackPack = true;

            if (quantityi > 0)
            {
                invData.DropItems(token, itemname, quantityi);

                int PlayerTargetNetworkId = Exports["core-ztzbx"].getPlayerNetworkIdFromUsername(username);
                Vector3 playerCoords = GetEntityCoords(NetworkGetEntityFromNetworkId(PlayerTargetNetworkId));

                uint backpack = (uint)GetHashKey("p_michael_backpack_s");

                // checking if already backpack on zone
                if (ItemDrop.backpackLocations.Count != 0)
                {
                    // check if a backpack is in the radeo
                    bool itemInStack = false;

                    foreach (var backpackOnGround in ItemDrop.backpackLocations)
                    {

                        bool onZone = MathU.CheckIfCoordsAreInRadeo(
                        playerCoords.X,
                        playerCoords.Y,
                        playerCoords.Z - 1.0f,
                        backpackOnGround.Value[0],
                        backpackOnGround.Value[1],
                        backpackOnGround.Value[2],
                        2.0f
                        );

                        if (onZone)
                        {   
                            newBackPack = false;

                            if (ItemDrop.stackItemsGround[backpackOnGround.Key].Count > 0)
                            {
                                // checking if is already that item in the inventory and stack  it

                                if (ItemDrop.stackItemsGround[backpackOnGround.Key].ContainsKey(itemname))
                                {
                                    ItemDrop.stackItemsGround[backpackOnGround.Key][itemname] += quantityi;
                                    itemInStack = true;
                                    break;
                                }
                            }

                            if (itemInStack == false)
                            {
                                Dictionary<string, int> currentItemOnGround = new Dictionary<string, int>();
                                currentItemOnGround.Add(itemname, quantityi);
                                ItemDrop.stackItemsGround[backpackOnGround.Key] = currentItemOnGround;
                            }
                            break;
                        };

                    }

                }

                if (newBackPack)
                {
                    int id_ob = CreateObject(
                            (int)backpack,
                            playerCoords.X,
                            playerCoords.Y,
                            playerCoords.Z - 1.0f,
                            true,
                            false,
                            false
                    );

                    Dictionary<string, int> currentItemOnGround = new Dictionary<string, int>();
                    currentItemOnGround.Add(itemname, quantityi);

                    ItemDrop.stackItemsGround.Add(id_ob, currentItemOnGround);
                    ItemDrop.backpackLocations.Add(id_ob, new List<float> { playerCoords.X, playerCoords.Y, playerCoords.Z - 1.0f });
                }
            }

        }



        private void ChangeItemPosition([FromSource] Player user, string token, string itemname, string position)
        {
            invData.ChangeItemPosition(token, itemname, position);
        }

        private void GetInventory([FromSource] Player user, string token)
        {
            bool itemInSlot;
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            int inventorySize = invData.GetInventorySize(token);
            List<Dictionary<string, string>> currentInventory = invData.GetItemsMetadata(token);

            if (currentInventory.Count > 0)
            {
                for (int i = 1; i < inventorySize + 1; i++)
                {
                    itemInSlot = false;
                    foreach (Dictionary<string, string> item in currentInventory)
                    {
                        if (Int32.Parse(item["slotposition"]) == i)
                        {
                            result.Add(item);
                            itemInSlot = true;
                            break;
                        }
                    }

                    if (itemInSlot) { continue; }
                    Dictionary<string, string> sub_result = new Dictionary<string, string>();
                    sub_result.Add("name", "empty");
                    sub_result.Add("slotposition", i.ToString());
                    result.Add(sub_result);

                }
            }
            else
            {
                for (int i = 1; i < inventorySize + 1; i++)
                {
                    Dictionary<string, string> sub_result = new Dictionary<string, string>();
                    sub_result.Add("name", "empty");
                    sub_result.Add("slotposition", i.ToString());
                    result.Add(sub_result);
                }
            }

            TriggerClientEvent(user, "setInventory", JsonConvert.SerializeObject(result));

        }

    }
}