using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace inventory.Client
{
    public class ExitNui : BaseScript
    {
        public ExitNui()
        {
            RegisterNuiCallbackType("exit");
            EventHandlers["__cfx_nui:exit"] += new Action<IDictionary<string, object>, CallbackDelegate>(Exit);
        }

        private void Exit(IDictionary<string, object> data, CallbackDelegate cb)
        {
            string jsonString = "{\"showIn\": false }";
            SetNuiFocus(false, false);
            SendNuiMessage(jsonString);
            Inventory.inventoryOpen = false;

            DisplayRadar(true);

            DeletePed(ref Inventory.temporalPlayerPed);

            ClearFocus();
            RenderScriptCams(false, false, 0, true, false);

            TriggerServerEvent("getCurrentBackPackMaxSize", Inventory.currentToken);

            cb(new{data = "ok"});
            
        }

    }
}