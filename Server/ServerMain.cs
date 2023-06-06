using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using Newtonsoft.Json;

namespace inventory.Server
{
    public class ServerMain : BaseScript
    {

         ItemMetaData invData = new ItemMetaData();

        public ServerMain()
        {
             EventHandlers["onResourceStart"] += new Action<string>(onResourceStart);
             EventHandlers["getItemsMetaData"] += new Action<Player>(GetItemsMetaData);
        }

        private void onResourceStart(string resourceName)
        {
            LoadItemsMetadata();
        }

        private void LoadItemsMetadata()
        {
            Items.data = invData.GetItemMeta();
        }

        private void GetItemsMetaData([FromSource] Player user)
        {
           TriggerClientEvent(user, "updateItemsMetaData", JsonConvert.SerializeObject(Items.data));
        }
        
    }
}