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
            EventHandlers["updateInventory"] += new Action<string>(UpdateInventory);

        }

        private void UpdateInventory(string info)
        {
            TriggerServerEvent("getInventory", Exports["core-ztzbx"].playerToken());
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
            DisplayRadar(false);
        }

        private async void OpenNuiEvent()
        {
            // importing prop for ground objects
            uint box = (uint)GetHashKey("prop_cs_cardbox_01");
            RequestModel(box);

            while (HasModelLoaded(box) == false)
            {
                RequestModel(box);
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

                        Vector3 pedCoords = GetEntityCoords(PlayerPedId(), false);

                        

                        int playerClone = ClonePed(
                        PlayerPedId(), 
                        0.0f, 
                        false, 
                        false
                        );
                        SetEntityCoords(playerClone, pedCoords.X-2.0f, pedCoords.Y, pedCoords.Z + 3.0f, false, false, false, false);

                        FreezeEntityPosition(playerClone, true);
                        SetEntityInvincible(playerClone, true);

                        SetEntityHeading(playerClone, 180.0f);


                        Vector3 pedClone = GetEntityCoords(playerClone, false);

                        int cam_zoom = CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", pedClone.X - 1.0f, pedClone.Y - 3.0f, pedClone.Z, 0, 0, 0, GetGameplayCamFov(), true, 0);
                        ClearFocus();
                        SetCamActive(cam_zoom, true);
                        RenderScriptCams(true, true, 1000, true, false);

                    }

                }
            }
        }

    }
}