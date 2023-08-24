using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using System.Collections.Generic;

using static CitizenFX.Core.Native.API;

// mp_m_freemode_01

namespace inventory.Client
{
    public class PlayerHashLogged : BaseScript
    {
        public PlayerHashLogged()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
        }

        private void OnClientResourceStart(string resourceName)
        {
            Check();
        }

        async private void Check()
        {

            string current_t;

            while (true)
            {
                await Delay(1000);

                try
                {
                    current_t = Exports["core-ztzbx"].playerToken();
                }
                catch
                {
                    continue;
                }

                if (current_t != null)
                {
                    Inventory.currentToken = current_t;
                    Inventory.playerhaslogged = true;
                    break;
                }
                
            }
        }

    }
}