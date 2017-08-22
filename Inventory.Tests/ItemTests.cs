using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using InventoryList.Models;

namespace InventoryList.Tests
{

    [TestClass]
    public class ItemTests : IDisposable
    {
        public ItemTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=3306;database=inventory_test;";
        }
        public void Dispose()
        {
          Item.DeleteAll();
          Column.DeleteAll();
        }

        [TestMethod]
        public void Equals_OverrideTrueForSameDescription_Item()
        {
          //Arrange, Act
          Item firstItem = new Item("Mow the lawn", 1);
          Item secondItem = new Item("Mow the lawn", 1);

          //Assert
          Assert.AreEqual(firstItem, secondItem);
        }

        [TestMethod]
        public void Save_SavesItemToDatabase_ItemList()
        {
          //Arrange
          Item testItem = new Item("Mow the lawn", 1);
          testItem.Save();

          //Act
          List<Item> result = Item.GetAll();
          List<Item> testList = new List<Item>{testItem};

          //Assert
          CollectionAssert.AreEqual(testList, result);
        }
       [TestMethod]
        public void Save_DatabaseAssignsIdToObject_Id()
        {
          //Arrange
          Item testItem = new Item("Mow the lawn", 1);
          testItem.Save();

          //Act
          Item savedItem = Item.GetAll()[0];

          int result = savedItem.GetId();
          int testId = testItem.GetId();

          //Assert
          Assert.AreEqual(testId, result);
        }

        [TestMethod]
        public void Find_FindsItemInDatabase_Item()
        {
          //Arrange
          Item testItem = new Item("Mow the lawn", 1);
          testItem.Save();

          //Act
          Item foundItem = Item.Find(testItem.GetId());

          //Assert
          Assert.AreEqual(testItem, foundItem);
        }
    }
}
