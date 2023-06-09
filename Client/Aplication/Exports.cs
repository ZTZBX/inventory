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
            TriggerServerEvent("addItemInventoryS", Exports["core-ztzbx"].playerToken(), item, quantity);
            TriggerServerEvent("getCurrentBackPackMaxSize", Exports["core-ztzbx"].playerToken());
        }


        private string GetItemMeta()
        {
            return JsonConvert.SerializeObject(Inventory.ItemsMetaData);
        }


    }
}