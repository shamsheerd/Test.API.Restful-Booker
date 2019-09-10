using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using log4net;

namespace Test.API.Framework
{
    public class RestClient
    {
        #region Members

        private readonly string baseAddress;
        private readonly string acceptType;
        private readonly string contentType;
        private readonly string userName;
        private readonly string password;

        #endregion

        #region Constructor

        ILog log = LogManager.GetLogger(typeof(RestClient));

        public RestClient()
        {
            baseAddress = ConfigurationManager.AppSettings["BaseAddress"];
            acceptType = ConfigurationManager.AppSettings["AcceptType"];
            contentType = ConfigurationManager.AppSettings["ContentType"];
            userName = ConfigurationManager.AppSettings["UserName"];
            password = ConfigurationManager.AppSettings["Password"];
        }

        #endregion
        
        #region Methods

        /// <summary>
        /// Invoke GET
        /// </summary>
        /// <typeparam name="T">Required Data binded to class.</typeparam>
        /// <param name="path">Query params.</param>
        /// <returns>Data binded to class.</returns>
        public T Get<T>(string path) where T : class
        {
            log.Info(string.Format("Invoking GET call with path '{0}'", path));
                
            return DoRestCall<T, T>(Verb.GET, path);
        }

        /// <summary>
        /// Post the content.
        /// </summary>
        /// <typeparam name="T">Content type binded to class.</typeparam>
        /// <param name="path">Query.</param>
        /// <param name="content">Content.</param>
        /// <returns>Response binded to class object.</returns>
        public TResult Post<T, TResult>(string path, T content) where T : class
             where TResult : class
        {
            log.Info(string.Format("Invoking POST call with path '{0}'", path));
            return DoRestCall<T, TResult>(Verb.POST, path, content);
        }

        /// <summary>
        /// Delete the content.
        /// </summary>
        /// <param name="path">Query</param>
        public void Delete(string path)
        {
            log.Info(string.Format("Invoking DELETE call with path '{0}'", path));
            DoRestCall<object, object>(Verb.DELETE, path, null, false);
        }

        #region Helpers

        /// <summary>
        /// Invoke Rest call.
        /// </summary>
        /// <typeparam name="T">Content binded to class.</typeparam>
        /// <typeparam name="TResult">Response binded to class.</typeparam>
        /// <param name="verb">Rest action.</param>
        /// <param name="path">Query.</param>
        /// <param name="content">Content.</param>
        /// <param name="result">if true, return response.</param>
        /// <returns></returns>
        private TResult DoRestCall<T, TResult>(Verb verb, string path, T content = null, bool result = true)
           where TResult : class
           where T : class
        {
            var request = CreateRequest(path, verb);

            if (content != null)
            {
                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    var payload = JsonConvert.SerializeObject(content);
                    log.Info("Request payload: " + payload);
                    writer.Write(payload);
                }
            }

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (result)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        var responsePayload = reader.ReadToEnd();
                        log.Info("Response payload: " + responsePayload);

                        return JsonConvert.DeserializeObject<TResult>(responsePayload);
                    }
                }
            }

            return default(TResult);
        }

        /// <summary>
        /// Create Request.
        /// </summary>
        /// <param name="path">Query.</param>
        /// <param name="verb">Rest action.</param>
        /// <returns>Request.</returns>
        private HttpWebRequest CreateRequest(string path, Verb verb = Verb.GET)
        {
            var request = WebRequest.Create(new Uri(baseAddress + path)) as HttpWebRequest;
            request.Method = verb.GetStringValue();
            request.Proxy = WebRequest.GetSystemWebProxy();

            request.Accept = acceptType;
            request.ContentType = contentType;

            if (verb == Verb.DELETE)
            {
                request.Headers.Add("Authorization", GenerateBasicAuthorization());

            }

            return request;
        }

        /// <summary>
        /// Generate Auth token.
        /// </summary>
        /// <param name="creds">Credentials.</param>
        /// <returns>Auth token.</returns>
        private string GenerateBasicAuthorization()
        {
            string sAuth = userName + ":" + password;
            byte[] binaryData = new Byte[sAuth.Length];
            binaryData = Encoding.UTF8.GetBytes(sAuth);
            return "Basic " + Convert.ToBase64String(binaryData);
        }

        #endregion

        #endregion

    }
}
