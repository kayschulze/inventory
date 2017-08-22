using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using InventoryList.Models;

namespace InventoryList.Tests
{
  [TestClass]
  public class ColumnTests : IDisposable
  {
        public ColumnTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=3306;database=inventory_test;";
        }



    [TestMethod]
    public void GetAll_CategoriesEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = Column.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_ReturnsTrueForSameName_Column()
    {
      //Arrange, Act
      Column firstColumn = new Column("Household chores");
      Column secondColumn = new Column("Household chores");

      //Assert
      Assert.AreEqual(firstColumn, secondColumn);
    }

    [TestMethod]
    public void Save_SavesColumnToDatabase_ColumnList()
    {
      //Arrange
      Column testColumn = new Column("Household chores");
      testColumn.Save();

      //Act
      List<Column> result = Column.GetAll();
      List<Column> testList = new List<Column>{testColumn};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

  [TestMethod]
    public void Save_DatabaseAssignsIdToColumn_Id()
    {
      //Arrange
      Column testColumn = new Column("Household chores");
      testColumn.Save();

      //Act
      Column savedColumn = Column.GetAll()[0];

      int result = savedColumn.GetId();
      int testId = testColumn.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Find_FindsColumnInDatabase_Column()
    {
      //Arrange
      Column testColumn = new Column("Household chores");
      testColumn.Save();

      //Act
      Column foundColumn = Column.Find(testColumn.GetId());

      //Assert
      Assert.AreEqual(testColumn, foundColumn);
    }

    [TestMethod]
    public void GetItems_RetrievesAllTasksWithColumn_ItemList()
    {
      Column testColumn = new Column("Household chores");
      testColumn.Save();

      Item firstItem = new Item("Mow the lawn", testColumn.GetId());
      firstItem.Save();
      Item secondItem = new Item("Do the dishes", testColumn.GetId());
      secondItem.Save();

      List<Item> testItemList = new List<Item> {firstItem, secondItem};
      List<Item> resultItemList = testColumn.GetItems();

      CollectionAssert.AreEqual(testItemList, resultItemList);
    }

    public void Dispose()
    {
      Item.DeleteAll();
      Column.DeleteAll();
    }
  }
}
