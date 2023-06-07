using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;


namespace inventory.Client
{
    public class GetItemsFromGround : BaseScript
    {

        public GetItemsFromGround()
        {
            RegisterNuiCallbackType("get_item");
            EventHandlers["__cfx_nui:get_item"] += new Action<IDictionary<string, object>, CallbackDelegate>(GetItem);
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

                Debug.WriteLine($"{currentItemName} {currentQuantity}");
                TriggerServerEvent("addItemsInInentoryFromGround",
                    Exports["core-ztzbx"].playerToken(),
                    ItemsDroped.CurrectBackPackIdObject,
                    currentItemName,
                    currentQuantity
                   );
            }

            string jsonString = "{\"showIn\": false }";
            SetNuiFocus(false, false);
            SendNuiMessage(jsonString);
            Inventory.inventoryOpen = false;
        }

    }
}