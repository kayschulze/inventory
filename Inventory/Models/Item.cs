using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace InventoryList.Models
{
    public class Item
    {
        private string _itemname;
        private int _id;
        private int _columnId;

        public Item(string itemname, int columnId, int id = 0)
        {
            _itemname = itemname;
            _columnId = columnId;
            _id = id;
        }

        public override bool Equals(System.Object otherItem)
        {
          if (!(otherItem is Item))
          {
            return false;
          }
          else
          {
             Item newItem = (Item) otherItem;
             bool idEquality = this.GetId() == newItem.GetId();
             bool itemNameEquality = this.GetItemName() == newItem.GetItemName();
             bool columnEquality = this.GetColumnId() == newItem.GetColumnId();
             return (idEquality && itemNameEquality && columnEquality);
           }
        }
        public override int GetHashCode()
        {
             return this.GetItemName().GetHashCode();
        }

        public string GetItemName()
        {
            return _itemname;
        }
        public int GetId()
        {
            return _id;
        }
        public int GetColumnId()
        {
            return _columnId;
        }
        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO items (itemname, column_id) VALUES (@itemname, @column_id);";

            MySqlParameter itemname = new MySqlParameter();
            itemname.ParameterName = "@itemname";
            itemname.Value = this._itemname;
            cmd.Parameters.Add(itemname);

            MySqlParameter columnId = new MySqlParameter();
            columnId.ParameterName = "@column_id";
            columnId.Value = this._columnId;
            cmd.Parameters.Add(columnId);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Item> GetAll()
        {
            List<Item> allItems = new List<Item> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM items;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
              int itemId = rdr.GetInt32(0);
              string itemNameString = rdr.GetString(1);
              int itemColumnId = rdr.GetInt32(2);
              Item newItem = new Item(itemNameString, itemColumnId, itemId);
              allItems.Add(newItem);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allItems;
        }
        public static Item Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM items WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int itemId = 0;
            string itemNameString = "";
            int itemColumnId = 0;

            while(rdr.Read())
            {
              itemId = rdr.GetInt32(0);
              itemNameString = rdr.GetString(1);
              itemColumnId = rdr.GetInt32(2);
            }

            Item newItem = new Item(itemNameString, itemColumnId, itemId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newItem;
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM items;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
    }
}
