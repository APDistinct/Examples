using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.DAL.Import.Excel.Tests
{
    [TestClass]
    public class ExcelUserEnumerableTests
    {
        [TestMethod]
        public void ExcelUserEnumerable_Test() {
            ExcelUserEnumerable excel = new ExcelUserEnumerable(".\\Res\\ColumnIndexes.xlsx", new ColumnIndexes.FixedColumnIndexes());
            CollectionAssert.AreEqual(new int[] { 2726553, 700971525 }, excel.Select(u => u.FLUserNumber).ToArray());
        }
    }
}
