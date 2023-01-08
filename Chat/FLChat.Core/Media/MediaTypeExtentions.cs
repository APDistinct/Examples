using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using FLChat.DAL;

namespace FLChat.Core.Media
{
    public static class MediaTypeExtentions
    {
        private class Val
        {
            public FileFormat Format;
            public MediaGroupKind Group;
            public Val(FileFormat format, MediaGroupKind group) {
                Format = format;
                Group = group;
            }
        }

        static byte[] signatureOffice = { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 };
        static byte[] signatureZip = { 0x50, 0x4B, 0x03, 0x04 };
        static byte[] signaturePdf = { 0x25, 0x50, 0x44, 0x46 };
        static byte[] signatureJpg = { 0xFF, 0xD8 };
        static byte[] signaturePng = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
        static byte[] signatureTiff = { 0x49, 0x49, 0x2A, 0x00 };
        static byte[] signatureBmp = { 0x42, 0x4D };
        static byte[] signatureGif = { 0x47, 0x49, 0x46, 0x38 };

        public static byte[] ToByteArray(this Image image, System.Drawing.Imaging.ImageFormat format)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(image, typeof(byte[]));

            //using (MemoryStream ms = new MemoryStream())
            //{
            //    image.Save(ms, format/*System.Drawing.Imaging.ImageFormat.Bmp*/);
            //    return ms.ToArray();
            //}
        }

        private static Dictionary<string, Val> dicFileFormat = new Dictionary<string, Val>
        {
            { "application/msword",  new Val( FileFormat.Office, MediaGroupKind.Document ) },
            { "application/vnd.ms-powerpoint", new Val( FileFormat.Office, MediaGroupKind.Document ) },
            { "application/vnd.visio", new Val( FileFormat.Office, MediaGroupKind.Document ) },
            { "application/vnd.ms-excel", new Val( FileFormat.Office, MediaGroupKind.Document ) },
            { "application/vnd.openxmlformats-officedocument.wordprocessingml.document", new Val( FileFormat.Zip, MediaGroupKind.Document ) },
            { "application/vnd.openxmlformats-officedocument.wordprocessingml.template", new Val( FileFormat.Zip, MediaGroupKind.Document ) },
            { "application/vnd.ms-word.document.macroEnabled.12", new Val( FileFormat.Zip, MediaGroupKind.Document ) },
            { "application/vnd.ms-word.template.macroEnabled.12", new Val( FileFormat.Zip, MediaGroupKind.Document ) },
            { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", new Val( FileFormat.Zip, MediaGroupKind.Document ) },
            { "application/vnd.openxmlformats-officedocument.spreadsheetml.template", new Val( FileFormat.Zip, MediaGroupKind.Document ) },
            { "application/vnd.ms-excel.sheet.macroEnabled.12", new Val( FileFormat.Zip, MediaGroupKind.Document ) },
            { "application/vnd.ms-excel.template.macroEnabled.12", new Val( FileFormat.Zip, MediaGroupKind.Document ) },
            { "application/vnd.ms-excel.addin.macroEnabled.12", new Val( FileFormat.Zip, MediaGroupKind.Document ) },
            { "application/vnd.ms-excel.sheet.binary.macroEnabled.12", new Val( FileFormat.Zip, MediaGroupKind.Document ) },
            { "application/vnd.openxmlformats-officedocument.presentationml.presentation", new Val( FileFormat.Zip, MediaGroupKind.Document ) },
            { "application/vnd.openxmlformats-officedocument.presentationml.template", new Val( FileFormat.Zip, MediaGroupKind.Document ) },
            { "application/vnd.openxmlformats-officedocument.presentationml.slideshow", new Val( FileFormat.Zip, MediaGroupKind.Document ) },
            { "application/vnd.ms-powerpoint.addin.macroEnabled.12", new Val( FileFormat.Zip, MediaGroupKind.Document ) },
            { "application/vnd.ms-powerpoint.presentation.macroEnabled.12", new Val( FileFormat.Zip, MediaGroupKind.Document ) },
            { "application/vnd.ms-powerpoint.template.macroEnabled.12", new Val( FileFormat.Zip, MediaGroupKind.Document ) },
            { "application/vnd.ms-powerpoint.slideshow.macroEnabled.12", new Val( FileFormat.Zip, MediaGroupKind.Document ) },
            { "application/pdf", new Val( FileFormat.Pdf, MediaGroupKind.Document ) },
            { "image/bmp", new Val( FileFormat.Bmp, MediaGroupKind.Image ) },
            { "image/png", new Val( FileFormat.Png, MediaGroupKind.Image ) },
            { "image/jpeg", new Val( FileFormat.Jpg, MediaGroupKind.Image ) },
            { "image/gif", new Val( FileFormat.Gif, MediaGroupKind.Image ) },
            { "image/tiff", new Val( FileFormat.Tiff, MediaGroupKind.Image ) },

        };

        public static bool? IsCorrectType(this byte[] bytes, string type) {
            bool? ret = null;
            try {
                ret = (dicFileFormat[type].Format == bytes.GetFormat());

            } catch {
                ret = null;
            }
            return ret;
        }

        public static MediaGroupKind? GetFileMediaGroup(this byte[] bytes, string type) {
            MediaGroupKind? ret = null;
            try {
                ret = dicFileFormat[type].Group;
            } catch {
                ret = null;
            }
            return ret;
        }

        public static string GetImageContentType(this byte[] imageData) {
            if (imageData.GetFormat() == FileFormat.Bmp) return "image/bmp";
            if (imageData.GetFormat() == FileFormat.Png) return "image/png";
            if (imageData.GetFormat() == FileFormat.Jpg) return "image/jpeg";
            return null;
        }

        public static string GetFileImageType(this byte[] imageData) {
            if (imageData.GetFormat() == FileFormat.Bmp) return "image/bmp";
            if (imageData.GetFormat() == FileFormat.Png) return "image/png";
            if (imageData.GetFormat() == FileFormat.Jpg) return "image/jpeg";
            if (imageData.GetFormat() == FileFormat.Gif) return "image/gif";
            if (imageData.GetFormat() == FileFormat.Tiff) return "image/tiff";
            return null;
        }

        public static string GetFileMediaType(this byte[] imageData) {
            if (imageData.GetFormat() == FileFormat.Bmp) return "image/bmp";
            if (imageData.GetFormat() == FileFormat.Png) return "image/png";
            if (imageData.GetFormat() == FileFormat.Jpg) return "image/jpeg";
            if (imageData.GetFormat() == FileFormat.Gif) return "image/gif";
            if (imageData.GetFormat() == FileFormat.Tiff) return "image/tiff";
            if (imageData.GetFormat() == FileFormat.Office) return "office";
            if (imageData.GetFormat() == FileFormat.Zip) return "office";
            if (imageData.GetFormat() == FileFormat.Pdf) return "office";
            return null;
        }

        public static FileFormat GetFormat(this byte[] file) {
            if (signatureOffice.SequenceEqual(file.Take(signatureOffice.Length))) return FileFormat.Office;
            if (signatureZip.SequenceEqual(file.Take(signatureZip.Length))) return FileFormat.Zip;
            if (signaturePdf.SequenceEqual(file.Take(signaturePdf.Length))) return FileFormat.Pdf;
            if (signatureJpg.SequenceEqual(file.Take(signatureJpg.Length))) return FileFormat.Jpg;
            if (signaturePng.SequenceEqual(file.Take(signaturePng.Length))) return FileFormat.Png;
            if (signatureTiff.SequenceEqual(file.Take(signatureTiff.Length))) return FileFormat.Tiff;
            if (signatureBmp.SequenceEqual(file.Take(signatureBmp.Length))) return FileFormat.Bmp;
            if (signatureGif.SequenceEqual(file.Take(signatureGif.Length))) return FileFormat.Gif;
            return FileFormat.Unknown;
        }

        public static bool GetImageSize(this byte[] file, out int? width, out int? height) {
            bool ret = true;
            width = null;
            height = null;
            try {
                MemoryStream ms = new MemoryStream(file);
                Image img = Image.FromStream(ms);
                //Bitmap img = new Bitmap(open.FileName);

                height = img.Height;
                width = img.Width;
            } catch {
                ret = false;
            }
            return ret;
        }

        public static byte[] GetRectangle(this byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                Image img = Image.FromStream(ms);
                Bitmap source = new Bitmap(img);
                var t = data.GetFileMediaType();

                //System.Drawing.Imaging.ImageFormat f = System.Drawing.Imaging.ImageFormat.Bmp;
                //f = GetFormat(source);
                //bool eq = f.Guid.Equals( System.Drawing.Imaging.ImageFormat.Bmp);
                //f = (System.Drawing.Imaging.ImageFormat)img.RawFormat;
                //eq = f == System.Drawing.Imaging.ImageFormat.Bmp;
                Bitmap CroppedImage = source.Clone(DoGetRectangle(source), source.PixelFormat);
                data = CroppedImage.ToByteArray(GetFormat(source));
                return data;
            }

        }

        private static System.Drawing.Rectangle DoGetRectangle(Bitmap source)
        {
            int min = Math.Min(source.Width, source.Height);
            int max = Math.Max(source.Width, source.Height);
            int h = (max - min) / 2;
            Rectangle rectangle;
            if(source.Width > source.Height)
            {
                rectangle = new Rectangle(h, 0, source.Height, source.Height);
            }
            else
            {
                rectangle = new Rectangle(0, h, source.Width, source.Width);
            }
            return rectangle;
        }

        private static System.Drawing.Imaging.ImageFormat GetFormat(Image image)
        {
            System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Png;
            Guid guid = image.RawFormat.Guid;
            if (guid == System.Drawing.Imaging.ImageFormat.Bmp.Guid)
                format = System.Drawing.Imaging.ImageFormat.Bmp;
            if (guid == System.Drawing.Imaging.ImageFormat.Gif.Guid)
                format = System.Drawing.Imaging.ImageFormat.Gif;
            if (guid == System.Drawing.Imaging.ImageFormat.Jpeg.Guid)
                format = System.Drawing.Imaging.ImageFormat.Jpeg;
            if (guid == System.Drawing.Imaging.ImageFormat.Png.Guid)
                format = System.Drawing.Imaging.ImageFormat.Png;
            if (guid == System.Drawing.Imaging.ImageFormat.Tiff.Guid)
                format = System.Drawing.Imaging.ImageFormat.Tiff;
            return format;
        }

    }

    public enum FileFormat
    {
        Unknown,
        Office,
        Zip,
        Pdf,
        Jpg,
        Png,
        Tiff,
        Bmp,
        Gif
    }
}
