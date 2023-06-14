using System;
using CitizenFX.Core;
using System.Collections.Generic;

namespace inventory.Server
{
    public class Backpack : BaseScript
    {
        InventoryData invData = new InventoryData();

        public Backpack()
        {
            EventHandlers["getCurrentBackPackMaxSize"] += new Action<Player, string>(GetCurrentBackPackMaxSize);
        }

        private void GetCurrentBackPackMaxSize([FromSource] Player user, string token)
        {
            int resultbackpackmax = 0;
            int resultitemsweight = 0;

            int backpackLevel = invData.GetCurrentBackPackLevel(token);

            if (backpackLevel == 0)
            {
                resultbackpackmax = InventoryMeta.backpackLevel0;
            }
            if (backpackLevel == 1)
            {
                resultbackpackmax = InventoryMeta.backpackLevel1;
            }
            if (backpackLevel == 2)
            {
                resultbackpackmax = InventoryMeta.backpackLevel2;
            }
            if (backpackLevel == 3)
            {
                resultbackpackmax = InventoryMeta.backpackLevel3;
            }

            foreach (Dictionary<string, string> items in Items.inInventory[token])
            {
                if (items["name"] != "empty")
                {
                    resultitemsweight += Int32.Parse(items["weight"]) * Int32.Parse(items["quantity"]);
                }

            }

            TriggerClientEvent(user, "setCurrentBackPack", resultbackpackmax);

            TriggerClientEvent(user, "setCurrentItemsWeight", resultitemsweight);

        }

    }
}