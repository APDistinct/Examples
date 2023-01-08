using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.Utils.Tests
{
    [TestClass]
    public class TypeExtentionsTests
    {
        private class TestType
        {
            [JsonProperty(PropertyName = "user_id")]
            public Guid Id = Guid.Empty;
            [JsonProperty(PropertyName = "full_name")]
            public string FullName { get; set; }
            //[JsonProperty(PropertyName = "phone")]
            public string Phone = "";
            [JsonProperty(Required = Required.AllowNull)]
            public string Field1 { get; set; }
        }

        //[TestMethod]
        //public void UserProfileInfoTest()
        //{
        //    string[] vsIn = { "FullName", "Phone", "Email" };
        //    string[] vsOut = { "full_name", "phone", "email" };            
        //    var dic = typeof(UserProfileInfo).GetJsonPropertyName(vsIn);
        //    for (int i = 0; i < vsIn.Length; ++i)
        //    {
        //        Assert.AreEqual(dic[vsOut[i]], vsIn[i]);
        //    }
        //}

        [TestMethod]
        public void CommonTypeTestManyNames()
        {
            string[] vsIn = { "FullName", "Phone", "Email" };
            string[] vsOut = { "full_name", "Phone"};            
            var dic = typeof(TestType).GetJsonPropertyName(vsIn);
            Assert.AreEqual(dic.Count, 2);
            for (int i = 0; i < vsOut.Length; ++i)
            {
                Assert.AreEqual(dic[vsOut[i]], vsIn[i]);
            }
        }

        [TestMethod]
        public void CommonTypeTestOneName()
        {            
            string strF = "Field1";            
            var strOut = typeof(TestType).GetJsonPropertyName(strF);
            Assert.AreEqual(strOut, strF);            
        }


        [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        private class TestType_snake
        {
            public int FieldOne => 1;

            [JsonProperty(PropertyName = "second")]
            public int FieldTwo => 2;

            public int FieldThree = 3;

            [JsonProperty(PropertyName = "fourth")]
            public int FieldFour = 3;
        }

        [JsonObject(Description = "descr")]
        private class TestType_def
        {
            public int FieldOne => 1;

            [JsonProperty(PropertyName = "second")]
            public int FieldTwo => 2;

            public int FieldThree = 3;

            [JsonProperty(PropertyName = "fourth")]
            public int FieldFour = 3;
        }

        [TestMethod]
        public void TypeExtentions_JsonPropertyName_Snake() {
            Type t = typeof(TestType_snake);
            string json = JsonConvert.SerializeObject(new TestType_snake());
            JObject jo = JObject.Parse(json);

            Assert.AreEqual("field_one", t.GetJsonPropertyName(nameof(TestType_snake.FieldOne)));
            Assert.AreEqual("second", t.GetJsonPropertyName(nameof(TestType_snake.FieldTwo)));
            Assert.AreEqual("field_three", t.GetJsonPropertyName(nameof(TestType_snake.FieldThree)));
            Assert.AreEqual("fourth", t.GetJsonPropertyName(nameof(TestType_snake.FieldFour)));
            Assert.IsTrue(jo.ContainsKey(t.GetJsonPropertyName(nameof(TestType_snake.FieldOne))));
            Assert.IsTrue(jo.ContainsKey(t.GetJsonPropertyName(nameof(TestType_snake.FieldTwo))));
            Assert.IsTrue(jo.ContainsKey(t.GetJsonPropertyName(nameof(TestType_snake.FieldThree))));
            Assert.IsTrue(jo.ContainsKey(t.GetJsonPropertyName(nameof(TestType_snake.FieldFour))));

            t = typeof(TestType_def);
            json = JsonConvert.SerializeObject(new TestType_def());
            jo = JObject.Parse(json);
            Assert.AreEqual("FieldOne", t.GetJsonPropertyName(nameof(TestType_def.FieldOne)));
            Assert.AreEqual("second", t.GetJsonPropertyName(nameof(TestType_def.FieldTwo)));
            Assert.AreEqual("FieldThree", t.GetJsonPropertyName(nameof(TestType_def.FieldThree)));
            Assert.AreEqual("fourth", t.GetJsonPropertyName(nameof(TestType_def.FieldFour)));
            Assert.IsTrue(jo.ContainsKey(t.GetJsonPropertyName(nameof(TestType_def.FieldOne))));
            Assert.IsTrue(jo.ContainsKey(t.GetJsonPropertyName(nameof(TestType_def.FieldTwo))));
            Assert.IsTrue(jo.ContainsKey(t.GetJsonPropertyName(nameof(TestType_def.FieldThree))));
            Assert.IsTrue(jo.ContainsKey(t.GetJsonPropertyName(nameof(TestType_def.FieldFour))));
        }

        private class TestTypeIgnore
        {
            [JsonIgnore]
            public string Field { get; set; }
        }

        [TestMethod]
        public void TypeExtentions_JsonPropertyName_JsonIgnore() {
            Assert.IsNull(typeof(TestTypeIgnore).GetJsonPropertyName(nameof(TestTypeIgnore.Field)));
        }

        [TestMethod]
        public void TypeExtentions_GetJsonPropertiesName() {
            string[] json = typeof(TestType_snake).GetJsonPropertiesName().Select(p => p.Item2).ToArray();
            CollectionAssert.AreEquivalent(
                new string[] { "field_one", "second", "field_three", "fourth" },
                json);
        }
    }

}
