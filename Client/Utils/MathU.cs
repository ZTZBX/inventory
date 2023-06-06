using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using System.Collections.Generic;


using static CitizenFX.Core.Native.API;

namespace inventory.Client
{
    static public class MathU
    {
        static public bool CheckIfCoordsAreInRadeo(float px, float py, float pz, float cx, float cy, float cz, float radeo)
        {
            float current_range_x_min = px - radeo;
            float current_range_y_min = py - radeo;
            float current_range_x_max = px + radeo;
            float current_range_y_max = py + radeo;

            // height
            float current_range_z_max = pz + radeo;
            float current_range_z_min = pz - radeo;

            if ((cx >= current_range_x_min && cx <= current_range_x_max && cy >= current_range_y_min && cy <= current_range_y_max) || (cx == px && cy == py))
            {
                if ((cz >= current_range_z_min && cz <= current_range_z_max) || (pz == cz))
                {
                    return true;
                }
            }

            return false;

        }
    }
}