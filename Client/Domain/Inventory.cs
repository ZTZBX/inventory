using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace inventory.Client
{
    static public class Inventory
    {

        static public string currentToken;

        static public bool inventoryLoaded = false;
        static public bool currentBackPackSizeLoaded = false; 
        static public bool setCurrentItemsWeightLoaded = false;

        static public bool getCurrentBackPackLoaded = false;
        static public bool getItemsDropOnGroundLoaded = false;

        static public bool playerhaslogged = false;
        static public bool inventoryOpen = false;
        static public string content;

        static public int temporalPlayerPed = -1; 

        public static Dictionary<string, List<string>> ItemsMetaData = new Dictionary<string, List<string>>();

        public static float currentBackPackSize = -1.0f;

        public static float currentItemsWeight = -1.0f;
    }
}