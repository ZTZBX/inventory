using System;
using CitizenFX.Core;

namespace inventory.Server
{
    public class AddItemInventory : BaseScript
    {
        InventoryData invData = new InventoryData();

        public AddItemInventory()
        {
            EventHandlers["addItemInventoryS"] += new Action<Player, string, string, int>(AddItemInventoryI);
        }

        private void AddItemInventoryI([FromSource] Player user, string token, string item, int quantity)
        {
            // check if item exists in inventory
            bool inInventoryI = false;

            foreach (var itemI in Items.inInventory)
            {
                if (itemI["name"] == item)
                {
                    inInventoryI = true;
                    break;
                }
            }

            if (inInventoryI)
            {
                invData.AddItemQuantityInInventory(token, item, quantity);
                return;
            }


            
            invData.AddItemToInventory(token, Items.data[item][0], quantity, Items.data[item][4], Items.data[item][1], Items.data[item][2], Items.data[item][3] );
    

            TriggerClientEvent(user, "updateInventory");
        }

    }
}