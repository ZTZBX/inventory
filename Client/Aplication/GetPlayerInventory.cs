using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System.Linq;


namespace inventory.Client
{
    public class GetPlayerInventory : BaseScript
    {
        

        public GetPlayerInventory()
        {
            RegisterNuiCallbackType("get_inventory");
            EventHandlers["__cfx_nui:get_inventory"] += new Action<IDictionary<string, object>, CallbackDelegate>(GetInventory);
            EventHandlers["setInventory"] += new Action<string>(SetInventory);
        }

        private void SetInventory(string inventory)
        {

            Inventory.content = inventory;
        }

        private void GetInventory(IDictionary<string, object> data, CallbackDelegate cb)
        {       
            
            if (Inventory.playerHasToken)
            {
                cb(new
                {
                data = Inventory.content,
                });
            }
        }

    }
}