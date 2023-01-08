using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Core.Algorithms.Tests
{
    [TestClass]
    public class BaseLinkRouterTests
    {
        private const string strBase = "Base";
        private const string strI1 = "Invite";
        private const string strI2 = "Common";

        public abstract class Base
        {
            protected string ScenarioName = strBase;
            protected /*static*/ /*readonly*/ string ScenarioN = strBase;
            protected int scNum = 0;
            public string ScName => ScenarioName;
            public int ScNu => scNum;
            public int Result;
            public int RetResult => Result;

            protected /*readonly*/ int num0;

            public Base()
            {
                ScenarioN = strBase;
                num0 = 0;
            }

            public string StartScenario(/*string s = ScenarioName, int sc = scNum*/)
            {
                //return s + "  " + sc.ToString();
                return ScenarioName + "  " + scNum.ToString();
            }

            public string StartScenarioN(/*string s = ScenarioName, int sc = scNum*/)
            {
                //return s + "  " + sc.ToString();
                return ScenarioN + "  " + num0.ToString();
            }

        }

        public class I1 : Base
        {
            protected new string ScenarioName = strI1;
            protected new int scNum = 1;
            //public new static string ScName => ScenarioName;
            //public new static int ScNu => scNum;

            public I1 () : base()
            {
                ScenarioN = strI1;
                num0 = 1;
            }
        }

        public class I2 : Base
        {
            protected new string ScenarioName = strI2;
            protected new int scNum = 2;
            //public new static string ScName => ScenarioName;
            //public new static int ScNu => scNum;

            public I2() 
            {
                ScenarioN = strI2;
                num0 = 2;
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            I1 i1 = new I1();
            I2 i2 = new I2();
            //Assert.AreEqual(I1.ScName, strI1);
            //Assert.AreEqual(I2.ScName, strI2);
            //Assert.AreEqual(I1.ScNu, 1);
            //Assert.AreEqual(I2.ScNu, 2);
            //i1.Result = 3 + I1.ScNu;
            //i2.Result = 4 + I2.ScNu;
            //Assert.AreEqual(i1.RetResult, 4);
            //Assert.AreEqual(i2.RetResult, 6);
            var s1 = i1.StartScenario();
            var s2 = i2.StartScenario();
            s1 = i1.StartScenarioN();
            s2 = i2.StartScenarioN();
            Assert.IsTrue(s1.Contains(strI1));
            Assert.IsTrue(s2.Contains(strI2));
        }
    }
}
