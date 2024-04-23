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
            TriggerServerEvent("getInventory", Inventory.currentToken);
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
                await Delay(1000);
                if (!Inventory.playerhaslogged) { continue; }
                TriggerServerEvent("getItemsMetaData");
                break;
            }
        }

        static public void InventoryNui(bool createNewPreviewPed)
        {
            /*
            This function handle the construction of inventory environment and call the NUI 
            */

            if (!Inventory.inventoryOpen)
            {
                if (createNewPreviewPed)
                {
                    Vector3 pedCoords = GetEntityCoords(PlayerPedId(), false);
                    int playerClone = ClonePed(
                    PlayerPedId(),
                    0.0f,
                    false,
                    false
                    );
                    SetEntityCoords(playerClone, pedCoords.X - 2.0f, pedCoords.Y, pedCoords.Z + 1000.0f, false, false, false, false);

                    FreezeEntityPosition(playerClone, true);
                    SetEntityInvincible(playerClone, true);

                    SetEntityHeading(playerClone, 180.0f);


                    Vector3 pedClone = GetEntityCoords(playerClone, false);

                    int cam_zoom = CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", pedClone.X - 1.0f, pedClone.Y - 3.0f, pedClone.Z, 0, 0, 0, GetGameplayCamFov(), true, 0);
                    ClearFocus();
                    SetCamActive(cam_zoom, true);
                    RenderScriptCams(true, true, 1000, true, false);
                    Inventory.temporalPlayerPed = playerClone;
                }

                string jsonString = "{\"showIn\": true }";
                SetNuiFocus(true, true);
                SendNuiMessage(jsonString);
                DisplayRadar(false);

                Inventory.inventoryOpen = true;

            }
        }

        private async void OpenNuiEvent()
        {

            /*
            Entry point of the inventory
            */

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

                if (!Inventory.playerhaslogged) { continue; }

                // G Key
                if (IsControlJustReleased(0, 47))
                {
                    if (!Inventory.inventoryOpen)
                    {
                        TriggerServerEvent("getInventory", Inventory.currentToken);
                        TriggerServerEvent("getCurrentBackPackMaxSize", Inventory.currentToken);

                        while (!Inventory.inventoryLoaded || !Inventory.currentBackPackSizeLoaded || !Inventory.setCurrentItemsWeightLoaded) { await Delay(100); }

                        InventoryNui(true);
                        Inventory.inventoryLoaded = false;
                        Inventory.currentBackPackSizeLoaded = false;
                        Inventory.setCurrentItemsWeightLoaded = false;    
                    }

                    await Delay(500);
                }
            }
        }

    }
}