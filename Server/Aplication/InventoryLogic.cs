using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace inventory.Server
{
    public class InventoryLogic : BaseScript
    {
        InventoryData invData = new InventoryData();
        public InventoryLogic()
        {
            EventHandlers["getInventory"] += new Action<Player, string>(GetInventory);
            EventHandlers["changeItemPosition"] += new Action<Player, string, string>(ChangeItemPosition);
        }

        private void ChangeItemPosition([FromSource] Player user, string itemname, string position)
        {
            invData.ChangeItemPosition(itemname, position);
        }

        private void GetInventory([FromSource] Player user, string token)
        {
            bool itemInSlot;
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            int inventorySize = invData.GetInventorySize(token);
            List<Dictionary<string, string>> currentInventory = invData.GetItemsMetadata(token);

            if (currentInventory.Count > 0)
            {
                for (int i = 1; i < inventorySize+1; i++)
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

                    if (itemInSlot){continue;}
                    Dictionary<string, string> sub_result = new Dictionary<string, string>();
                    sub_result.Add("name", "empty");
                    sub_result.Add("slotposition", i.ToString());
                    result.Add(sub_result);
                    
                }
            }
            else
            {
                for (int i = 1; i < inventorySize+1; i++)
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