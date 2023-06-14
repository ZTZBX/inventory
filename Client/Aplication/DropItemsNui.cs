using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;

namespace inventory.Client
{
    public class DropItemsNui : BaseScript
    {
        public DropItemsNui()
        {
            RegisterNuiCallbackType("drop_items");
            EventHandlers["__cfx_nui:drop_items"] += new Action<IDictionary<string, object>, CallbackDelegate>(DropItems);
        }

        private void DropItems(IDictionary<string, object> data, CallbackDelegate cb)
        {

            object item;
            object quantity;


            if (!data.TryGetValue("item", out item)) { return; }
            if (!data.TryGetValue("quantity", out quantity)) { return; }

            string currentItemName = item.ToString();
            string currentQuantity = quantity.ToString();

            TriggerServerEvent("dropItems", Exports["core-ztzbx"].playerToken(), currentItemName, currentQuantity, Exports["core-ztzbx"].playerUsername());

            cb(new { data = "ok" });


            TriggerServerEvent("getCurrentBackPack", ItemsDroped.CurrectBackPackIdObject);
            TriggerServerEvent("getInventory", Exports["core-ztzbx"].playerToken());

            if (ItemsDroped.CurrentBackPack.ContainsKey(currentItemName))
            {
                ItemsDroped.CurrentBackPack[currentItemName] = ItemsDroped.CurrentBackPack[currentItemName] + Int32.Parse(currentQuantity);
            }
            else
            {
                ItemsDroped.CurrentBackPack.Add(currentItemName, Int32.Parse(currentQuantity));
            }

            List<Dictionary<string, string>> tempContent = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(Inventory.content);

            for (int i = 0; i < tempContent.Count; i++)
            {

                if (tempContent[i]["name"] == currentItemName)
                {

                    int currentQ = Int32.Parse(tempContent[i]["quantity"]) - Int32.Parse(currentQuantity);
                    if (currentQ <= 0)
                    {
                        tempContent[i]["name"] = "empty";
                    }
                    else
                    {
                        tempContent[i]["quantity"] = (Int32.Parse(tempContent[i]["quantity"]) - Int32.Parse(currentQuantity)).ToString(); 
                    }

                    Inventory.currentItemsWeight = Inventory.currentItemsWeight - (float)((float)Int32.Parse(currentQuantity) * (float)Int32.Parse(tempContent[i]["weight"])) / 1000.0f;

                    break;
                }

            }

            Inventory.content = JsonConvert.SerializeObject(tempContent);

            string jsonString = "{\"showIn\": false }";
            SetNuiFocus(false, false);
            SendNuiMessage(jsonString);
            Inventory.inventoryOpen = false;

            ClientMain.InventoryNui(false);
            Inventory.inventoryOpen = true;

        }






    }
}