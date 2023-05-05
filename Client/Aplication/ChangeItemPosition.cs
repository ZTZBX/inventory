using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace inventory.Client
{
    public class ExitNui : BaseScript
    {
        public ChangeItemPosition()
        {
            RegisterNuiCallbackType("change_item_position");
            EventHandlers["__cfx_nui:change_item_position"] += new Action<IDictionary<string, object>, CallbackDelegate>(ChangeItemPositionNui);
        }

        private void ChangeItemPositionNui(IDictionary<string, object> data, CallbackDelegate cb)
        {
            // TODO: DEBUG THIS

            object itemName;
            object position;


            if (!data.TryGetValue("item_name", out itemName)) { return; }
            if (!data.TryGetValue("position", out position)) { return; }

            string currentItemName = itemName.ToString();
            string currentPosition = position.ToString();

            TriggerServerEvent("changeItemPosition", currentItemName, currentPosition);

            // TODO: CHECK IF THIS SOLVS THE PROBLEM WITH THE CALLS IN JV
            cb(new{data = "ok"});
        }

    }
} 