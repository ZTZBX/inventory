using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;

namespace inventory.Client
{
    public class GetItemsFromGround : BaseScript
    {

        public GetItemsFromGround()
        {
            RegisterNuiCallbackType("get_item");
            EventHandlers["__cfx_nui:get_item"] += new Action<IDictionary<string, object>, CallbackDelegate>(GetItem);
            EventHandlers["deleteClosestItemBox"] += new Action<string>(DeleteClosestItemBox);
        }

        private void DeleteClosestItemBox(string info)
        {
            uint box = (uint)GetHashKey("prop_cs_cardbox_01");
            Vector3 pedCoords = GetEntityCoords(PlayerPedId(), false);
            var obj = GetClosestObjectOfType(pedCoords.X, pedCoords.Y, pedCoords.Z, 5.0f, box, false, false, false);

            if (DoesEntityExist(obj))
            {
                DeleteObject(ref obj);
            }

        }

        private void GetItem(IDictionary<string, object> data, CallbackDelegate cb)
        {
            object item;
            object quantity;

            if (!data.TryGetValue("item", out item)) { return; }
            if (!data.TryGetValue("quantity", out quantity)) { return; }

            string currentItemName = item.ToString();
            int currentQuantity = Int32.Parse(quantity.ToString());





            if (ItemsDroped.CurrentBackPack[currentItemName] >= currentQuantity && currentQuantity != 0)
            {

                TriggerServerEvent("addItemsInInentoryFromGround",
                    Inventory.currentToken,
                    ItemsDroped.CurrectBackPackIdObject,
                    currentItemName,
                    currentQuantity
                   );

                

                if (ItemsDroped.CurrentBackPack.ContainsKey(currentItemName))
                {
                    ItemsDroped.CurrentBackPack[currentItemName] = ItemsDroped.CurrentBackPack[currentItemName] - currentQuantity;

                    if (ItemsDroped.CurrentBackPack[currentItemName] <= 0)
                    {
                        ItemsDroped.CurrentBackPack.Remove(currentItemName);
                    }


                }


                List<Dictionary<string, string>> tempContent = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(Inventory.content);

                bool itemDontExistsInInventory = true;
                int littlePositionId;

                littlePositionId = 999999;

                for (int i = 0; i < tempContent.Count; i++)
                {
                    
                    if (Int32.Parse(tempContent[i]["slotposition"]) < littlePositionId && tempContent[i]["name"] == "empty")
                    {
                        littlePositionId = Int32.Parse(tempContent[i]["slotposition"]);
                    }

                    if (tempContent[i]["name"] == currentItemName)
                    {
                        itemDontExistsInInventory = false;
                        tempContent[i]["quantity"] = (Int32.Parse(tempContent[i]["quantity"]) + currentQuantity).ToString();

                        Inventory.currentItemsWeight = Inventory.currentItemsWeight + (float)((float)currentQuantity * (float)Int32.Parse(tempContent[i]["weight"]) / 1000.0f);
                        break;
                    }
 
                }

                if (itemDontExistsInInventory && littlePositionId != 999999)
                {
                    Dictionary<string, string> newitem = new Dictionary<string, string>();

                    newitem.Add("name", Inventory.ItemsMetaData[currentItemName][0]);
                    newitem.Add("quantity", currentQuantity.ToString());
                    newitem.Add("unit", Inventory.ItemsMetaData[currentItemName][4]);
                    newitem.Add("image", Inventory.ItemsMetaData[currentItemName][1]);
                    newitem.Add("descriptiontitle", Inventory.ItemsMetaData[currentItemName][2]);
                    newitem.Add("description", Inventory.ItemsMetaData[currentItemName][3]);
                    newitem.Add("slotposition", littlePositionId.ToString());
                    newitem.Add("type", Inventory.ItemsMetaData[currentItemName][5]);

                    tempContent[littlePositionId-1] = newitem;
                    
                    Inventory.currentItemsWeight = Inventory.currentItemsWeight + (float)((float)currentQuantity * (float)Int32.Parse(Inventory.ItemsMetaData[currentItemName][6]) / 1000.0f);
                }

                Inventory.content = JsonConvert.SerializeObject(tempContent);

                string jsonString = "{\"showIn\": false }";
                SetNuiFocus(false, false);
                SendNuiMessage(jsonString);
                Inventory.inventoryOpen = true;

                ClientMain.InventoryNui(false);
                Inventory.inventoryOpen = true;
            }


        }

    }
}