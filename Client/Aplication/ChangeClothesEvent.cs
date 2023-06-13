using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace inventory.Client
{
    public class ChangeClothesEvent : BaseScript
    {
        public ChangeClothesEvent()
        {
            RegisterNuiCallbackType("change_clothes");
            EventHandlers["__cfx_nui:change_clothes"] += new Action<IDictionary<string, object>, CallbackDelegate>(ChangeClothesNui);
        }

        private void ChangeClothesNui(IDictionary<string, object> data, CallbackDelegate cb)
        {
            object itemType;
            object itemName;


            if (!data.TryGetValue("itemtype", out itemType)) { return; }
            if (!data.TryGetValue("itemname", out itemName)) { return; }

            string currentitemType = itemType.ToString();
            string currentitemName = itemName.ToString();

            if (currentitemType == "shoesCharacter")
            {
                Exports["player"].updateShoes(currentitemName, Inventory.temporalPlayerPed);
            }

            cb(new { data = "ok" });
        }

    }
}