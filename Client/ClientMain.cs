using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace inventory.Client
{
    public class ClientMain : BaseScript
    {
        public ClientMain()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
        }

        private void OnClientResourceStart(string resourceName)
        {
            OpenNuiEvent();
        }

        static public void InventoryNui()
        {
            string jsonString = "{\"showIn\": true }";
            SetNuiFocus(true, true);
            SendNuiMessage(jsonString);
        }

        private async void OpenNuiEvent()
        {

            while (true)
            {
                await Delay(0);
                if (!Inventory.playerHasToken) {continue;}
                // G Key
                if (IsControlJustReleased(0, 47))
                {
                    if (!Inventory.inventoryOpen)
                    {
                        
                        TriggerServerEvent("getInventory", Exports["core-ztzbx"].playerToken());
                        InventoryNui();
                        Inventory.inventoryOpen = true;
                        await Delay(100);
                    }
                    
                }
            }
        }
        
    }
}