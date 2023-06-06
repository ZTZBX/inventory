using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;

namespace inventory.Client
{
    public class ClientMain : BaseScript
    {
        public ClientMain()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
            EventHandlers["updateItemsMetaData"] += new Action<string>(UpdateItemsMetaData);

        }

        private void OnClientResourceStart(string resourceName)
        {
            OpenNuiEvent();
            GetItemsMetaData();
        }

        private void UpdateItemsMetaData(string itemsMetaData)
        {
            if (itemsMetaData != null && itemsMetaData.Length > 0)
            {
                Inventory.ItemsMetaData = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(itemsMetaData);
            }
        }

        async private void GetItemsMetaData()
        {
            while (true)
            {
                await Delay(0);
                if (!Inventory.playerHasToken) { continue; }
                TriggerServerEvent("getItemsMetaData");
                break;
            }
        }

        static public void InventoryNui()
        {
            string jsonString = "{\"showIn\": true }";
            SetNuiFocus(true, true);
            SendNuiMessage(jsonString);
        }

        private async void OpenNuiEvent()
        {
            // importing prop for ground objects
            uint backpack = (uint)GetHashKey("p_michael_backpack_s");
            RequestModel(backpack);

            while (HasModelLoaded(backpack) == false)
            {
                RequestModel(backpack);
                await Delay(100);
            }


            while (true)
            {
                await Delay(0);
                if (!Inventory.playerHasToken) { continue; }
                // G Key
                if (IsControlJustReleased(0, 47))
                {
                    if (!Inventory.inventoryOpen)
                    {

                        TriggerServerEvent("getInventory", Exports["core-ztzbx"].playerToken());
                        await Delay(200);
                        InventoryNui();
                        Inventory.inventoryOpen = true;
                    }

                }
            }
        }

    }
}