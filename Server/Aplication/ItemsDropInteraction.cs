using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using System.Collections.Generic;
using Newtonsoft.Json;

using static CitizenFX.Core.Native.API;

namespace inventory.Server
{
    public class ItemsDropInteraction : BaseScript
    {
       

        public ItemsDropInteraction()
        {
            EventHandlers["getItemsDropOnGround"] += new Action<Player>(GetItemsDropOnGround);
            EventHandlers["getCurrentBackPack"] += new Action<Player, int>(GetCurrentBackPack);
        }

        private void GetCurrentBackPack([FromSource] Player user, int backpackId)
        {
            TriggerClientEvent(user, "setBackPackOnGround", JsonConvert.SerializeObject(ItemDrop.stackItemsGround[backpackId]));
        }


        private void GetItemsDropOnGround([FromSource] Player user)
        {
            // generating readonly 

            TriggerClientEvent(user, "setDropItems", JsonConvert.SerializeObject(ItemDrop.backpackLocations));
        }
    }
}