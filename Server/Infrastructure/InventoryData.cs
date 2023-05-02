using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CitizenFX.Core;

namespace inventory.Server
{
    public class InventoryData : BaseScript
    {
        public InventoryData()
        {
        }

        public bool CheckItemIn(string token, string itemname)
        {

            string query = $"select name from inventory as pinv RIGHT join players as player on player.username = pinv.username where player.token ='{token}' and pinv.name='{itemname}'";
            dynamic result = Exports["fivem-mysql"].raw(query);

            if (result[0][0] == "") { return false; }

            return true;
        }

        public void AddItemToInventory(string token, string itemname, int quantity, string unit, string imageName, string descriptiontitle, string description)
        {
            if (CheckItemIn(token, itemname)) { return; }
            string usernameQuery = $"SELECT username from player where token='{token}'";
            dynamic username = Exports["fivem-mysql"].raw(usernameQuery);
            string query = $"INSERT INTO `inventory` (`username`, `name`, `quantity`, `unit`, `image`, `descriptiontitle`, `description`) VALUES ('{username[0][0]}', '{itemname}', '{quantity.ToString()}', '{unit}', '{imageName}', '{descriptiontitle}', '{description}')";
            Exports["fivem-mysql"].raw(query);
        }

        public void ChangeQuantityItem(string token, string itemname, int quantity)
        {
            if (!CheckItemIn(token, itemname)) { return; }
            string usernameQuery = $"SELECT username from player where token='{token}'";
            dynamic username = Exports["fivem-mysql"].raw(usernameQuery);
            string query = $"UPDATE `inventory` SET `quantity` = '{quantity.ToString()}' WHERE `inventory`.`username` = '{username[0][0]}' AND `inventory`.`name` = '{itemname}'";
            Exports["fivem-mysql"].raw(query);
        }

        public void DeleteItemFromInventory(string token, string itemname)
        {
            if (!CheckItemIn(token, itemname)) { return; }
            string usernameQuery = $"SELECT username from player where token='{token}'";
            dynamic username = Exports["fivem-mysql"].raw(usernameQuery);
            string query = $"delete from inventory where username = '{username[0][0]}' and name = '{itemname}'";
            Exports["fivem-mysql"].raw(query);
        }
        
        public List<Dictionary<string, string>> GetItemsMetadata(string token, int quantity)
        {
            Dictionary<string, string> r = new Dictionary<string, string>();
            List<Dictionary<string, string>> listResult = new List<Dictionary<string, string>>();

            string query = $"select name, quantity, unit, image, descriptiontitle, description from inventory as pinv RIGHT join players as player on player.username = pinv.username where player.token ='{token}'";
            dynamic result = Exports["fivem-mysql"].raw(query);

            foreach (dynamic line in result)
            {
                r.Add("name", line[0]);
                r.Add("quantity", line[1]);
                r.Add("unit", line[2]);
                r.Add("image", line[3]);
                r.Add("descriptiontitle", line[4]);
                r.Add("description", line[5]);
                listResult.Add(r);
                r.Clear();
            }

            return listResult;
        }


    }
}