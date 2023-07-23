using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;

namespace inventory.Client
{
    public class GetItemsOnCharacter : BaseScript
    {
        public GetItemsOnCharacter()
        {
            RegisterNuiCallbackType("get_player_items_on_character");
            EventHandlers["__cfx_nui:get_player_items_on_character"] += new Action<IDictionary<string, object>, CallbackDelegate>(GetItemsOnCharacterNui);
        }

        private void GetItemsOnCharacterNui(IDictionary<string, object> data, CallbackDelegate cb)
        {
            Dictionary<string, string> playerCharacterists = new Dictionary<string, string>();
            playerCharacterists.Add("shoes", Exports["player"].getShoes());
            cb(new{data = JsonConvert.SerializeObject(playerCharacterists)});   
        }

    }
} 