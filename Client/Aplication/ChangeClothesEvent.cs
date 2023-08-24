using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;

namespace inventory.Client
{
    public class ChangeClothesEvent : BaseScript
    {
        public ChangeClothesEvent()
        {
            RegisterNuiCallbackType("change_clothes");
            EventHandlers["__cfx_nui:change_clothes"] += new Action<IDictionary<string, object>, CallbackDelegate>(ChangeClothesNui);
        }

        private void ChangeClothesNui(IDictionary<string, object> data, CallbackDelegate cb)
        {
            object itemType;
            object itemName;


            if (!data.TryGetValue("itemtype", out itemType)) { return; }
            if (!data.TryGetValue("itemname", out itemName)) { return; }

            string currentitemType = itemType.ToString();
            string currentitemName = itemName.ToString();

            if (currentitemType == "shoesCharacter")
            {

                List<Dictionary<string, string>> tempContent = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(Inventory.content);

                // replace the item in the inventory

                // check if are already shoes 
                string current_item_to_add_in_inventory = Exports["player"].getShoes(); // first position existing

                // adding item in inventory
                if (current_item_to_add_in_inventory != "no-shoes")
                {
                    
                    TriggerServerEvent("addItemInventoryS", Inventory.currentToken, current_item_to_add_in_inventory, 1);
                    TriggerServerEvent("getCurrentBackPackMaxSize", Inventory.currentToken);

                    bool item_exists = false;

                    for (int i = 0; i < tempContent.Count; i++)
                    {
                        if (tempContent[i]["name"] == current_item_to_add_in_inventory)
                        {
                            tempContent[i]["quantity"] = (Int32.Parse(tempContent[i]["quantity"]) + 1).ToString();
                            item_exists = true;
                            break;
                        }
                    }

                    
                    //  adding new item in inventory

                    if (!item_exists)
                    {
                         for (int i = 0; i < tempContent.Count; i++)
                        {
                            if (tempContent[i]["name"] == "empty")
                            {
                                Dictionary<string, string> newitem = new Dictionary<string, string>();
                                tempContent[i]["name"] = current_item_to_add_in_inventory;
                                tempContent[i].Add("quantity", "1");
                                tempContent[i].Add("unit", Inventory.ItemsMetaData[current_item_to_add_in_inventory][1]);
                                tempContent[i].Add("image", Inventory.ItemsMetaData[current_item_to_add_in_inventory][1]);
                                tempContent[i].Add("descriptiontitle", Inventory.ItemsMetaData[current_item_to_add_in_inventory][2]);
                                tempContent[i].Add("description", Inventory.ItemsMetaData[current_item_to_add_in_inventory][3]);
                                tempContent[i].Add("type", Inventory.ItemsMetaData[current_item_to_add_in_inventory][5]);
                                break;
                            }
                        }
                    }

                    Inventory.currentItemsWeight = Inventory.currentItemsWeight + (float)((float)Int32.Parse(Inventory.ItemsMetaData[current_item_to_add_in_inventory][6]) / 1000.0f);

                    
                }

                Exports["player"].updateShoes(currentitemName, Inventory.temporalPlayerPed, Inventory.currentToken);
                // removing the item from the inventory

                for (int i = 0; i < tempContent.Count; i++)
                {
                    if (tempContent[i]["name"] == currentitemName)
                    {
                        if (Int32.Parse(tempContent[i]["quantity"]) == 1)
                        {
                            tempContent.RemoveAt(i);
                        }
                        else
                        {
                            tempContent[i]["quantity"] = (Int32.Parse(tempContent[i]["quantity"]) - 1).ToString();
                        }
                        break;
                    }
                }

                Inventory.content = JsonConvert.SerializeObject(tempContent);

                Inventory.currentItemsWeight = Inventory.currentItemsWeight - (float)((float)Int32.Parse(Inventory.ItemsMetaData[currentitemName][6]) / 1000.0f);
                TriggerServerEvent("removeItemInventoryS", Inventory.currentToken, currentitemName, 1);
            }


            cb(new { data = "ok" });

            string jsonString = "{\"showIn\": false }";
            SetNuiFocus(false, false);
            SendNuiMessage(jsonString);
            Inventory.inventoryOpen = false;

            cb(new { data = "ok" });

            ClientMain.InventoryNui(false);
            Inventory.inventoryOpen = true;
        }

    }
}