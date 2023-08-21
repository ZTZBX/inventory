using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;

namespace inventory.Client
{
    public class ChangeItemPosition : BaseScript
    {
        public ChangeItemPosition()
        {
            RegisterNuiCallbackType("change_item_position");
            EventHandlers["__cfx_nui:change_item_position"] += new Action<IDictionary<string, object>, CallbackDelegate>(ChangeItemPositionNui);
        }

        private void ChangeItemPositionNui(IDictionary<string, object> data, CallbackDelegate cb)
        {
            object itemName;
            object position;


            if (!data.TryGetValue("itemname", out itemName)) { return; }
            if (!data.TryGetValue("position", out position)) { return; }

            string currentItemName = itemName.ToString();
            string currentPosition = position.ToString();

            Debug.WriteLine(itemName.ToString());

            if (itemName.ToString() == "shoesCharacter")
            {

                string shoes = Exports["player"].getShoes();
                Exports["player"].updateShoes("no-shoes", Inventory.temporalPlayerPed, Exports["core-ztzbx"].playerToken());
                List<Dictionary<string, string>> tempContent = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(Inventory.content);

                // now lets add the item in the inventory
                TriggerServerEvent("addItemInventoryS", Exports["core-ztzbx"].playerToken(), shoes, 1);
                TriggerServerEvent("getCurrentBackPackMaxSize", Exports["core-ztzbx"].playerToken());

                bool item_exists = false;

                for (int i = 0; i < tempContent.Count; i++)
                {
                    if (tempContent[i]["name"] == shoes)
                    {
                        tempContent[i]["quantity"] = (Int32.Parse(tempContent[i]["quantity"]) + 1).ToString();
                        item_exists = true;
                        break;
                    }
                }

                if (!item_exists)
                {
                    for (int i = 0; i < tempContent.Count; i++)
                    {
                        if (tempContent[i]["name"] == "empty")
                        {
                            Dictionary<string, string> newitem = new Dictionary<string, string>();
                            tempContent[i]["name"] = shoes;
                            tempContent[i].Add("quantity", "1");
                            tempContent[i].Add("unit", Inventory.ItemsMetaData[shoes][1]);
                            tempContent[i].Add("image", Inventory.ItemsMetaData[shoes][1]);
                            tempContent[i].Add("descriptiontitle", Inventory.ItemsMetaData[shoes][2]);
                            tempContent[i].Add("description", Inventory.ItemsMetaData[shoes][3]);
                            tempContent[i].Add("type", Inventory.ItemsMetaData[shoes][5]);
                            break;
                        }
                    }
                }

                Inventory.content = JsonConvert.SerializeObject(tempContent);

                cb(new { data = "ok" });

                string jsonString = "{\"showIn\": false }";
                SetNuiFocus(false, false);
                SendNuiMessage(jsonString);
                Inventory.inventoryOpen = false;

                cb(new { data = "ok" });

                ClientMain.InventoryNui(false);
                Inventory.inventoryOpen = true;
            }
            else
            {
                TriggerServerEvent("changeItemPosition", Exports["core-ztzbx"].playerToken(), currentItemName, currentPosition);
                TriggerServerEvent("getInventory", Exports["core-ztzbx"].playerToken());
                cb(new { data = "ok" });

            }

            

        }

    }
}