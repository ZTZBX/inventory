using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;



namespace inventory.Client
{
    public class ItemsDropInteraction : BaseScript
    {

        public ItemsDropInteraction()
        {
            EventHandlers["setDropItems"] += new Action<string>(setDropItems);
            EventHandlers["setBackPackOnGround"] += new Action<string>(SetBackPackOnGround);
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
            RegisterNuiCallbackType("get_on_ground_items");
            EventHandlers["__cfx_nui:get_on_ground_items"] += new Action<IDictionary<string, object>, CallbackDelegate>(GetOnGroundItems);
        }

        private void GetOnGroundItems(IDictionary<string, object> data, CallbackDelegate cb)
        {

            if (ItemsDroped.CurrentBackPack.Count == 0)
            {
                cb(new { data = "[]" });
                return;
            }

            List<Dictionary<string, string>> listOfGroundObjects = new List<Dictionary<string, string>>();

            foreach (var item in ItemsDroped.CurrentBackPack)
            {
                Dictionary<string, string> currentItem = new Dictionary<string, string>();

                currentItem.Add("name", Inventory.ItemsMetaData[item.Key][0]);
                currentItem.Add("image", Inventory.ItemsMetaData[item.Key][1]);
                currentItem.Add("quantity", item.Value.ToString());
                currentItem.Add("unit", Inventory.ItemsMetaData[item.Key][4]);

                listOfGroundObjects.Add(currentItem);
            }

            cb(new { data = JsonConvert.SerializeObject(listOfGroundObjects) });


        }

        private void OnClientResourceStart(string resourceName)
        {
            LoadDropedItems();
        }

        async public void LoadDropedItems()
        {
            Vector3 currectCoords;

            while (true)
            {
                await Delay(1000);
                if (!Inventory.playerhaslogged) { continue; }
                TriggerServerEvent("getItemsDropOnGround");

                while (!Inventory.getItemsDropOnGroundLoaded) {await Delay(0);}

                if (ItemsDroped.dropedItems.Count > 0)
                {
                    foreach (var backpacks in ItemsDroped.dropedItems)
                    {
                        currectCoords = GetEntityCoords(PlayerPedId(), false);
                        if (MathU.CheckIfCoordsAreInRadeo(
                            currectCoords.X,
                            currectCoords.Y,
                            currectCoords.Z - 1.0f,
                            backpacks.Value[0],
                            backpacks.Value[1],
                            backpacks.Value[2],
                            2.0f
                        ))
                        {
                            TriggerServerEvent("getCurrentBackPack", backpacks.Key);
                            while (!Inventory.getCurrentBackPackLoaded) {await Delay(0);}
                            ItemsDroped.CurrectBackPackIdObject = backpacks.Key;
                        }
                        else
                        {
                            ItemsDroped.CurrentBackPack.Clear();
                            ItemsDroped.CurrectBackPackIdObject = -1;
                        }

                    }

                }
                else
                {
                    ItemsDroped.CurrentBackPack.Clear();
                    ItemsDroped.CurrectBackPackIdObject = -1;
                }

                Inventory.getItemsDropOnGroundLoaded = false;
                Inventory.getCurrentBackPackLoaded = false;

            }

        }

        private void setDropItems(string items)
        {
            if (items != null && items.Length > 0)
            {
                ItemsDroped.dropedItems = JsonConvert.DeserializeObject<Dictionary<int, List<float>>>(items);
                Inventory.getItemsDropOnGroundLoaded = true;
            }
        }

        private void SetBackPackOnGround(string backpack)
        {
            if (backpack != null && backpack.Length > 0)
            {
                ItemsDroped.CurrentBackPack = JsonConvert.DeserializeObject<Dictionary<string, int>>(backpack);
                Inventory.getCurrentBackPackLoaded = true;
            }
        }

    }
}