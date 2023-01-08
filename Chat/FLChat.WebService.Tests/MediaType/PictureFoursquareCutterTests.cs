using System;
using System.Drawing;
using System.Drawing.Imaging;
using FLChat.Core.Media;
using FLChat.WebService.MediaType;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.WebService.MediaType.Tests
{
    [TestClass]
    public class PictureFoursquareCutterTests
    {
        private PictureFoursquareCutter cutter = new PictureFoursquareCutter();

        [TestMethod]
        public void PictureCutterTestWidth()
        {
            var image = new Bitmap(13, 11);
            PictureCutterBase(image);
        }

        [TestMethod]
        public void PictureCutterTestHeight()
        {
            var image = new Bitmap(12, 21);
            PictureCutterBase(image);
        }

        private void PictureCutterBase(Bitmap image)
        {
            byte[] arr = image.ToByteArray(ImageFormat.Bmp);
            byte[] darr = new byte[arr.Length];
            arr.CopyTo(darr, 0);
            cutter.Cut(ref arr);
            var pri = arr.GetImageSize(out int? width, out int? height);
            Assert.IsTrue(pri);
            Assert.AreEqual(width, height);
            //var t1 = arr.GetImageContentType();
            //var t2 = darr.GetImageContentType();
            //Assert.AreEqual(t1, t2);
        }

        [TestMethod]
        public void PictureCutterTestFiles()
        {
            string[] fileNames = { "Chat.bmp", "Chat.jpg", "Chat.png" };
            foreach (var fileName in fileNames)
            {
                var image = new Bitmap(".\\Image\\" + fileName);
                PictureCutterBase(image);                
            }
        }        

        //[TestMethod]
        //public void GetImageSizeTest_NO()
        //{
        //    string fileName = "ChatNotImage.bmp";
        //    var image = new Bitmap(".\\Image\\" + fileName);

            
        //    Assert.IsFalse(ret);
        //    Assert.IsNull(width);
        //    Assert.IsNull(height);

        //}
    }
}
