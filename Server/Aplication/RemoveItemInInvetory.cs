using System;
using CitizenFX.Core;

namespace inventory.Server
{
    public class RemoveItemInInvetory : BaseScript
    {
        InventoryData invData = new InventoryData();

        public RemoveItemInInvetory()
        {
            EventHandlers["removeItemInventoryS"] += new Action<Player, string, string, int>(RemoveItemInInvetoryI);
        }

        private void RemoveItemInInvetoryI([FromSource] Player user, string token, string item, int quantity)
        {
            // check if item exists in inventory
            bool inInventoryI = false;

            foreach (var itemI in Items.inInventory[token])
            {
                if (itemI["name"] == item)
                {
                    inInventoryI = true;
                    break;
                }
            }

            if (inInventoryI)
            {
                invData.RemoveItemQuantityInInventory(token, item, quantity);
                TriggerClientEvent(user, "updateInventory");
                return;
            }

            
        }

    }
}