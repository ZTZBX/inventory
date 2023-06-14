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
                Exports["player"].updateShoes(currentitemName, Inventory.temporalPlayerPed);

                List<Dictionary<string, string>> tempContent = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(Inventory.content);

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

                TriggerServerEvent("removeItemInventoryS", Exports["core-ztzbx"].playerToken(), currentitemName, 1);
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