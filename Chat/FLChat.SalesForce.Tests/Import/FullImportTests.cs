using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.SalesForce.Import.Tests
{
    [TestClass]
    public class FullImportTests
    {
        [TestMethod]
        public void Create() {
            FullImport import = new FullImport(null);
        }
    }
}
