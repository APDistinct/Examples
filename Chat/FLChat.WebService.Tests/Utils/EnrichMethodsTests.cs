using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Specialized;

namespace FLChat.WebService.Utils.Tests
{
    [TestClass]
    public class EnrichMethodsTests
    {
        private class TestClass
        {
            public string Key { get; set; }
            public string Field { get; set; }
            public string Field2 { get; set; }
        }

        [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        private class TestClassJson
        {
            [JsonProperty(PropertyName = "key_field")]
            public string Key { get; set; }
            [JsonProperty(PropertyName = "first_field")]
            public string Field { get; set; }
            public string Field2 { get; set; }
        }

        [TestMethod]
        public void EnrichPropery_ExistsObject() {
            IEnrichMethod<TestClass> method = new EnrichProperty<TestClass>("Key");
            TestClass c = new TestClass() { Field = "111" };
            method.Enrich(ref c, "value");

            Assert.AreEqual("value", c.Key);
            Assert.AreEqual("111", c.Field);
        }

        [TestMethod]
        public void EnrichPropery_NullObject() {
            IEnrichMethod<TestClass> method = new EnrichProperty<TestClass>("Key");
            TestClass c = null;
            method.Enrich(ref c, "value");

            Assert.IsNotNull(c);
            Assert.AreEqual("value", c.Key);
            Assert.IsNull(c.Field);
        }

        [TestMethod]
        public void EnrichProperty_Entich() {
            IEnrichMethod<TestClass> method = new EnrichProperty<TestClass>("Key");
            TestClass c = null;

            method.Enrich(ref c, "Field", "f1");
            method.Enrich(ref c, "Field2", "f2");
            Assert.IsNotNull(c);
            Assert.AreEqual("f1", c.Field);
            Assert.AreEqual("f2", c.Field2);
        }

        [TestMethod]
        public void EnrichProperty_Entich_Json() {
            IEnrichMethod<TestClassJson> method = new EnrichProperty<TestClassJson>("key_field");
            TestClassJson c = null;

            method.Enrich(ref c, "key value");
            method.Enrich(ref c, "first_field", "f1");
            method.Enrich(ref c, "field2", "f2");
            Assert.IsNotNull(c);
            Assert.AreEqual("key value", c.Key);
            Assert.AreEqual("f1", c.Field);
            Assert.AreEqual("f2", c.Field2);
        }

        [TestMethod]
        public void EnrichProperty_DefConstr() {
            IEnrichMethod<TestClass> method = new EnrichProperty<TestClass>();
            TestClass c = null;

            Assert.ThrowsException<NotSupportedException>(() => method.Enrich(ref c, "val"));
            method.Enrich(ref c, "Field", "1");
            Assert.AreEqual("1", c.Field);
        }

        [TestMethod]
        public void EnrichString_Test() {
            IEnrichMethod<string> method = new EnrichString();
            string input = null;
            method.Enrich(ref input, "value");
            Assert.AreEqual("value", input);
            Assert.ThrowsException<NotSupportedException>(() => method.Enrich(ref input, "f1", "v1"));
        }

        [TestMethod]
        public void EnrichJObject_ExistsObject() {
            IEnrichMethod<JObject> method = new EnrichJObject("Key");
            JObject obj = new JObject();
            obj["Field"] = "111";
            method.Enrich(ref obj, "value");

            Assert.AreEqual("value", obj["Key"]);
            Assert.AreEqual("111", obj["Field"]);
        }

        [TestMethod]
        public void EnrichJObject_NullObject() {
            IEnrichMethod<JObject> method = new EnrichJObject("Key");
            JObject obj = null;
            method.Enrich(ref obj, "value");

            Assert.IsNotNull(obj);
            Assert.AreEqual("value", obj["Key"]);
        }

        [TestMethod]
        public void EnrichJObject_Enrich() {
            IEnrichMethod<JObject> method = new EnrichJObject("Key");
            JObject obj = null;

            method.Enrich(ref obj, "f1", "v1");
            method.Enrich(ref obj, "f2", "v2");
            Assert.IsNotNull(obj);
            Assert.AreEqual("v1", (string)obj["f1"]);
            Assert.AreEqual("v2", (string)obj["f2"]);
        }

        [TestMethod]
        public void EnrichJObject_DefConstr() {
            IEnrichMethod<JObject> method = new EnrichJObject();
            JObject c = null;

            Assert.ThrowsException<NotSupportedException>(() => method.Enrich(ref c, "val"));
            method.Enrich(ref c, "Field", "1");
            Assert.AreEqual("1", (string)c["Field"]);
        }


        [TestMethod]
        public void EnrichMethodExtentions_Enrich() {
            NameValueCollection nvc = new NameValueCollection() {
                { "Field", "1" },
                { "Field2", "2" },
            };
            TestClass c = null;
            IEnrichMethod<TestClass> method = new EnrichProperty<TestClass>("Key");
            method.Enrich(ref c, nvc);
            Assert.IsNotNull(c);
            Assert.AreEqual("1", c.Field);
            Assert.AreEqual("2", c.Field2);
        }
    }
}
