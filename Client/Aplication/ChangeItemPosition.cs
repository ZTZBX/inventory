using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace inventory.Client
{
    public class ChangeItemPosition : BaseScript
    {
        public ChangeItemPosition()
        {
            RegisterNuiCallbackType("change_item_position");
            EventHandlers["__cfx_nui:change_item_position"] += new Action<IDictionary<string, object>, CallbackDelegate>(ChangeItemPositionNui);
        }

        private void ChangeItemPositionNui(IDictionary<string, object> data, CallbackDelegate cb)
        {
            object itemName;
            object position;


            if (!data.TryGetValue("itemname", out itemName)) { return; }
            if (!data.TryGetValue("position", out position)) { return; }

            string currentItemName = itemName.ToString();
            string currentPosition = position.ToString();

            TriggerServerEvent("changeItemPosition", Exports["core-ztzbx"].playerToken(), currentItemName, currentPosition);
            TriggerServerEvent("getInventory", Exports["core-ztzbx"].playerToken());

            cb(new{data = "ok"});
            
        }

    }
} 