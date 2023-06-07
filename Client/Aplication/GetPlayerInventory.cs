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
            RegisterNuiCallbackType("get_item_meta_data");
            EventHandlers["__cfx_nui:get_item_meta_data"] += new Action<IDictionary<string, object>, CallbackDelegate>(GetItemMetaData);
            EventHandlers["setInventory"] += new Action<string>(SetInventory);
        }

        private void GetItemMetaData(IDictionary<string, object> data, CallbackDelegate cb)
        {
            object item;

            if (!data.TryGetValue("item", out item)) { return; }

            cb(new
            {
                data = "{\"image\":\""+Inventory.ItemsMetaData[item.ToString()][1]+"\", \"unit\":\""+Inventory.ItemsMetaData[item.ToString()][4]+"\", \"name\":\""+Inventory.ItemsMetaData[item.ToString()][0]+"\"}",
            });

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