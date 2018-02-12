using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Net;
using System.IO;

namespace UPF
{
    /// <summary>
    /// Contains a set of helper methods for interacting with the http protocol.
    /// </summary>
    public static class NetHelper
    {
        #region GetText Method

        public static string GetText(string uri, string method = "GET")
        {
            string result = null;

            try
            {
                //create a HttpWebRequest with the uri, method and content type specified as parameters.
                HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
                request.Method = method;

                //get the response (actual execution is made here)
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream responseStream = response.GetResponseStream();
                using (StreamReader responseStreamReader = new StreamReader(responseStream))
                {
                    result = responseStreamReader.ReadToEnd();
                    response.Close();
                    responseStreamReader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to complete request.", ex);
            }

            return result;
        }

        #endregion

        #region GetXml Method

        public static XElement GetXml(string uri, string method = "GET")
        {
            string text = GetText(uri, method);
            XElement xml = null;
            try
            {
                xml = XElement.Parse(text);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to parse XML from URI '" + uri + "'.", ex);
            }
            return xml;
        }

        #endregion

        #region XmlWebRequest Method

        /// <summary>
        /// Makes a request to a uri with the intent of sending XML. Designed 
        /// specifically for REST scenarios, but would work with others as well. 
        /// </summary>
        /// <param name="uri">The <see cref="String"/> containing the uri to connect to.</param>
        /// <param name="xml">The <see cref="XElement"/> containing the XML to send.</param>
        /// <param name="method">The <see cref="String"/> containing the method to use. Default is POST.</param>
        /// <param name="contentType">The <see cref="String"/> containing the content type. Default is text/xml.</param>
        /// <returns></returns>
        public static XElement XmlWebRequest(string uri, XElement xml, string method = "POST", string contentType = "text/xml")
        {
            XElement result = null;
            string xmlData = XmlWebRequest(uri, xml.ToString(), method, contentType);
            if (!string.IsNullOrWhiteSpace(xmlData))
            {
                try
                {
                    result = XElement.Parse(xmlData, LoadOptions.PreserveWhitespace);
                }
                catch (Exception ex)
                {
                    throw new Exception("Unable to load XML into XElement.", ex);
                }
            }
            return result;
        }

        public static string XmlWebRequest(string uri, string xml, string method = "POST", string contentType = "text/xml")
        {
            string result = null;

            try
            {
                //create a HttpWebRequest with the uri, method and content type specified as parameters.
                HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
                request.Method = method;
                request.ContentType = contentType;

                //get a byte array of the XML data.
                byte[] buffer = Encoding.UTF8.GetBytes(xml);

                //set the content length to be posted.
                request.ContentLength = buffer.Length;

                //create a stream object to write the buffer byte array to.
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(buffer, 0, buffer.Length);

                //get the response (actual execution is made here)
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream responseStream = response.GetResponseStream();
                using (StreamReader responseStreamReader = new StreamReader(responseStream))
                {
                    result = responseStreamReader.ReadToEnd();
                    response.Close();
                    responseStreamReader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to complete request.", ex);
            }

            return result;
        }

        #endregion

    }
}
