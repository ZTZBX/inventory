using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using System.Collections.Generic;
using Newtonsoft.Json;

using static CitizenFX.Core.Native.API;

namespace inventory.Server
{
    public class ItemsDropInteraction : BaseScript
    {

        InventoryData invData = new InventoryData();

        public ItemsDropInteraction()
        {
            EventHandlers["getItemsDropOnGround"] += new Action<Player>(GetItemsDropOnGround);
            EventHandlers["getCurrentBackPack"] += new Action<Player, int>(GetCurrentBackPack);
            EventHandlers["addItemsInInentoryFromGround"] += new Action<Player, string, int, string, int>(AddItemsInInentoryFromGround);
        }
        
        private void GetCurrentBackPack([FromSource] Player user, int backpackId)
        {
            if (!ItemDrop.stackItemsGround.ContainsKey(backpackId)) { return; }
            TriggerClientEvent(user, "setBackPackOnGround", JsonConvert.SerializeObject(ItemDrop.stackItemsGround[backpackId]));
        }


        private void GetItemsDropOnGround([FromSource] Player user)
        {
            // generating readonly 

            TriggerClientEvent(user, "setDropItems", JsonConvert.SerializeObject(ItemDrop.backpackLocations));
        }


        private void AddItemsInInentoryFromGround([FromSource] Player user, string token, int backpackId, string itemName, int itemQuantityToAddInInventory)
        {

            if (!ItemDrop.stackItemsGround.ContainsKey(backpackId)) { return; }
            if (!ItemDrop.stackItemsGround[backpackId].ContainsKey(itemName)) { return; }

            if (ItemDrop.stackItemsGround[backpackId][itemName] < itemQuantityToAddInInventory) { return; }

            // check if item exists in inventory
            bool inInventoryI = false;

            foreach (var itemI in Items.inInventory[token])
            {
                if (itemI["name"] == itemName)
                {
                    inInventoryI = true;
                    break;
                }
            }


            if (ItemDrop.stackItemsGround[backpackId][itemName] == itemQuantityToAddInInventory)
            {
                if (ItemDrop.stackItemsGround[backpackId].Count == 1)
                {
                    ItemDrop.backpackLocations.Remove(backpackId);
                    ItemDrop.stackItemsGround.Remove(backpackId);
                    TriggerClientEvent(user, "deleteClosestItemBox", "1234");
                }
                else
                {
                    ItemDrop.stackItemsGround[backpackId].Remove(itemName);
                }

            }
            else
            {
                ItemDrop.stackItemsGround[backpackId][itemName] = ItemDrop.stackItemsGround[backpackId][itemName] - itemQuantityToAddInInventory;
            }
            
            if (!inInventoryI)
            {
                invData.AddItemToInventory(
                token,
                Items.data[itemName][0],
                itemQuantityToAddInInventory
            );
            }
            else
            {
                invData.AddItemQuantityInInventory(token, itemName, itemQuantityToAddInInventory);
            }


            TriggerClientEvent(user, "setDropItems", JsonConvert.SerializeObject(ItemDrop.backpackLocations));
            TriggerClientEvent(user, "updateInventory");

        }
    }
}