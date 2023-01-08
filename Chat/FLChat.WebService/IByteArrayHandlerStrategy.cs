using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FLChat.DAL;
using FLChat.DAL.Model;
using System.Net;

namespace FLChat.WebService
{
    /// <summary>
    /// Handle http request as byte array
    /// </summary>
    public interface IByteArrayHandlerStrategy
    {
        /// <summary>
        /// Process Http request
        /// </summary>
        /// <param name="entities">Database entities</param>
        /// <param name="currUserInfo">Current user information</param>
        /// <param name="requestData">Request data as byte array</param>
        /// <param name="requestContentType">Request Content-Type header</param>
        /// <param name="responseData">Response data as byte array, null if have not data</param>
        /// <param name="responseContentType">Responce Content-Type header</param>
        /// <param name="fileName">Name of file, may be null</param>
        /// <returns>HttpStatus code</returns>
        int ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo,  
            NameValueCollection parameters,
            byte[] requestData, string requestContentType,
            out byte []responseData, out string responseContentType, out string fileName);

        bool IsReusable { get; }
    }

    /// <summary>
    /// Adapt IByteArrayHandlerStrategy to CheckAuthHttpHandler.IHandlerStrategy
    /// </summary>
    public class ByteArrayHandlerStrategyAdapter : CheckAuthHttpHandler.IHandlerStrategy
    {
        public const string KeyName = "Key";

        private readonly IByteArrayHandlerStrategy _handler;
        private readonly int _keyIndex;

        public ByteArrayHandlerStrategyAdapter(IByteArrayHandlerStrategy handler) {
            _handler = handler;
            _keyIndex = 0;
        }

        public ByteArrayHandlerStrategyAdapter(IByteArrayHandlerStrategy handler, int keyIndex) {
            _handler = handler;
            _keyIndex = keyIndex;
        }

        public bool IsReusable => _handler.IsReusable;

        public void ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, HttpContext context) {
            //extract data
            byte []requestData = null;
            if (context.Request.HttpMethod != "GET" && context.Request.HttpMethod != "DELETE") {
                if (context.Request.ContentLength == 0)
                    requestData = new byte[0];
                else
                    requestData = context.Request.BinaryRead(context.Request.ContentLength);
            }

            string requestContentType = context.Request.ContentType;

            //create parameters
            NameValueCollection parameters = null;
            if (_keyIndex != 0) {
                parameters = new NameValueCollection(1);
                string key = context.Request.Url.Segments[context.Request.Url.Segments.Length - _keyIndex];
                if (key[key.Length - 1] == '/')
                    key = key.Substring(0, key.Length - 1);
                parameters.Add(KeyName, key);                
            }

            //perform
            byte[] responseData;
            string responseContentType;
            context.Response.StatusCode = _handler.ProcessRequest(entities, currUserInfo, 
                parameters,
                requestData, requestContentType, 
                out responseData, out responseContentType, out string fileName);

            //write data
            if (responseContentType != null)
                context.Response.ContentType = responseContentType;
            if (fileName != null) {
                //string fn = string.Join("", fileName.ToCharArray()
                //    .Select(c => Char.IsLetterOrDigit(c) ? new string(c, 1) :  "%" + ((int)c).ToString("x2"))
                //    .ToArray());
                context.Response.AddHeader("Content-Disposition", $"attachment; filename=\"{fileName}\"");
            }
            if (responseData != null)
                context.Response.BinaryWrite(responseData);
        }
    }

    public static class IByteArrayHandlerStrategyExtentions
    {
        public static CheckAuthHttpHandler.IHandlerStrategy Adapt(this IByteArrayHandlerStrategy handler){
            return new ByteArrayHandlerStrategyAdapter(handler);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler">Handler strategy</param>
        /// <param name="keyIndex">Key index from end in Uri</param>
        /// <returns></returns>
        public static CheckAuthHttpHandler.IHandlerStrategy Adapt(this IByteArrayHandlerStrategy handler, int keyIndex) {
            return new ByteArrayHandlerStrategyAdapter(handler, keyIndex);
        }

        public static string GetKey(this IByteArrayHandlerStrategy strategy, NameValueCollection collection) {
            return collection[ByteArrayHandlerStrategyAdapter.KeyName];
        }
    }
}
