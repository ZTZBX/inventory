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
        }

        static public void InventoryNui()
        {
            string jsonString = "{\"showIn\": true }";
            SetNuiFocus(true, true);
            SendNuiMessage(jsonString);
        }


        [Command("inventory")]
        public void HelloServer()
        {
            InventoryNui();
        }
    }
}