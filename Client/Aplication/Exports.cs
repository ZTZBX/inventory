using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using System.Collections.Generic;
using Newtonsoft.Json;

using static CitizenFX.Core.Native.API;

namespace inventory.Client
{
    public class Exports : BaseScript
    {

        public Exports()
        {
            Exports.Add("getItemMeta", new Func<string>(GetItemMeta)); 
            Exports.Add("addItemInventory", new Action<string, int>(AddItemInventory)); 
        }

        private void AddItemInventory(string item, int quantity)
        {
            TriggerServerEvent("addItemInventoryS", Inventory.currentToken, item, quantity);
            TriggerServerEvent("getCurrentBackPackMaxSize", Inventory.currentToken);
        }


        private string GetItemMeta()
        {
            return JsonConvert.SerializeObject(Inventory.ItemsMetaData);
        }


    }
}