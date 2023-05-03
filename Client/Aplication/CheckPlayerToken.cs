using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace inventory.Client
{
    public class CheckPlayerToken : BaseScript
    {
        public CheckPlayerToken()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
        }

        private void OnClientResourceStart(string resourceName)
        {
            ChangePlayerTokenState();
        }

        async private void ChangePlayerTokenState()
        {
            while(true)
            {
                await Delay(100);
                if (Exports["core-ztzbx"].playerToken() != null)
                {
                    Inventory.playerHasToken = true;
                    TriggerServerEvent("getInventory", Exports["core-ztzbx"].playerToken());
                    break;
                }
            }
        } 
    }
}