using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace inventory.Client
{
    public class DropItemsNui : BaseScript
    {
        public DropItemsNui()
        {
            RegisterNuiCallbackType("drop_items");
            EventHandlers["__cfx_nui:drop_items"] += new Action<IDictionary<string, object>, CallbackDelegate>(DropItems);
        }

        private void DropItems(IDictionary<string, object> data, CallbackDelegate cb)
        {
            // TODO: DEBUG THIS

            object item;
            object quantity;


            if (!data.TryGetValue("item", out item)) { return; }
            if (!data.TryGetValue("quantity", out quantity)) { return; }

            string currentItemName = item.ToString();
            string currentQuantity = quantity.ToString();

            TriggerServerEvent("dropItems", Exports["core-ztzbx"].playerToken(), currentItemName, currentQuantity, Exports["core-ztzbx"].playerUsername());

            string jsonString = "{\"showIn\": false }";
            SetNuiFocus(false, false);
            SendNuiMessage(jsonString);
            Inventory.inventoryOpen = false;
            
            cb(new { data = "ok" });
        }

    }
}