using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace InventoryList.Models
{
    public class Column
    {
        private string _name;
        private int _id;
        public Column(string name, int id = 0)
        {
            _name = name;
            _id = id;
        }
        public override bool Equals(System.Object otherColumn)
        {
            if (!(otherColumn is Column))
            {
                return false;
            }
            else
            {
                Column newColumn = (Column) otherColumn;
                return this.GetId().Equals(newColumn.GetId());
            }
        }
        public override int GetHashCode()
        {
            return this.GetId().GetHashCode();
        }
        public string GetName()
        {
            return _name;
        }
        public int GetId()
        {
            return _id;
        }
        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO columns (name) VALUES (@name);";

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._name;
            cmd.Parameters.Add(name);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

        }
        public static List<Column> GetAll()
        {
            List<Column> allColumns = new List<Column> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM columns;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
              int ColumnId = rdr.GetInt32(0);
              string ColumnName = rdr.GetString(1);
              Column newColumn = new Column(ColumnName, ColumnId);
              allColumns.Add(newColumn);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allColumns;
        }
        public static Column Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM columns WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int ColumnId = 0;
            string ColumnName = "";

            while(rdr.Read())
            {
              ColumnId = rdr.GetInt32(0);
              ColumnName = rdr.GetString(1);
            }
            Column newColumn = new Column(ColumnName, ColumnId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newColumn;
        }
        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM columns;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Item> GetItems()
        {
            List<Item> allColumnItems = new List<Item> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM items WHERE column_id = @column_id;";

            MySqlParameter columnId = new MySqlParameter();
            columnId.ParameterName = "@column_id";
            columnId.Value = this._id;
            cmd.Parameters.Add(columnId);


            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
              int itemId = rdr.GetInt32(0);
              string itemNameString = rdr.GetString(1);
              int itemColumnId = rdr.GetInt32(2);
              Item newItem = new Item(itemNameString, itemColumnId, itemId);
              allColumnItems.Add(newItem);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allColumnItems;
        }
    }
}
