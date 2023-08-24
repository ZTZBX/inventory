using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System.Linq;


namespace inventory.Client
{
    public class InventoryWeight : BaseScript
    {


        public InventoryWeight()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
            RegisterNuiCallbackType("get_inventory_meta_weight");
            EventHandlers["__cfx_nui:get_inventory_meta_weight"] += new Action<IDictionary<string, object>, CallbackDelegate>(GetInventoryMetaWeight);
        }

        private void OnClientResourceStart(string resourceName)
        {
            CheckCarry();
        }

        async private void CheckCarry()
        {
            bool status = false;

            RequestAnimSet("MOVE_M@DRUNK@VERYDRUNK");
            while (!HasAnimSetLoaded("MOVE_M@DRUNK@VERYDRUNK"))
            {
                await Delay(100);
            }

            while (true)
            {
                await Delay(1000);
                if (!Inventory.playerhaslogged) { continue; }

                if (Inventory.currentItemsWeight > Inventory.currentBackPackSize)
                {
                    SetPedMoveRateOverride(PlayerPedId(), 0.6f);

                    if (IsControlJustReleased(0, 55))
                    {
                        Exports["notification"].send("Dear " + Exports["core-ztzbx"].playerUsername(), "BackPack", "You are carrying to much you cant jump!!");
                    } 

                    DisableControlAction(2, 22, true);
                }

                if (Inventory.currentItemsWeight > Inventory.currentBackPackSize && status == false)
                {

                    SetPedMovementClipset(PlayerPedId(), "MOVE_M@DRUNK@VERYDRUNK", 1.0f);
                    SetPedIsDrunk(PlayerPedId(), true);

                    

                    Exports["notification"].send("Dear " + Exports["core-ztzbx"].playerUsername(), "BackPack", "You are carrying to much.");
                    status = true;
                }
                if (Inventory.currentItemsWeight <= Inventory.currentBackPackSize && status == true)
                {
                    ResetPedMovementClipset(PlayerPedId(), 1.0f);
                    SetPedIsDrunk(PlayerPedId(), false);
                    status = false;
                }
            }

        }


        private void GetInventoryMetaWeight(IDictionary<string, object> data, CallbackDelegate cb)
        {
            cb(new
            {
                data = "{\"curret_weight\": " + Inventory.currentItemsWeight + ", \"max_weight\": " + Inventory.currentBackPackSize + "}",
            });

        }
    }
}