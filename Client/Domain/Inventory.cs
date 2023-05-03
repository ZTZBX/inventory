using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace inventory.Client
{
    static public class Inventory
    {
        static public bool playerHasToken = false;
        static public bool inventoryOpen = false;

        static public string content;
    }
}