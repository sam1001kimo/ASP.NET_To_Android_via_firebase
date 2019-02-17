using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Text;
using System.Configuration;
using System.IO;

namespace FCMnotificationTest.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        [HttpGet]
        [Route("sendmessage")]
        public IHttpActionResult SendMessage() {
            var data = new
            {
                to = "device token",
                notification = new {
                    body = "bbbbbb",
                    title = "pqiweofp",
 
                },

                data = new
                {
                    message = "XXXX",
                    name = "CCCC",
                    userId = "1",
                    status = true
                }
            };
            SendNotification(data);
            return Ok();
        }

        public void SendNotification(object data)
        {
            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(data);
            Byte[] byteArray = Encoding.UTF8.GetBytes(json);
            SendNotification(byteArray);
        }

        public void SendNotification(Byte[] byteArray) {
            try
            {
                string server_api_key = ConfigurationManager.AppSettings["SERVER_API_KEY"];
                string sender_id = ConfigurationManager.AppSettings["SENDER_ID"];
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                tRequest.Headers.Add($"Authorization:key={server_api_key}");
                tRequest.Headers.Add($"Sender:id={sender_id}");

                tRequest.ContentLength = byteArray.Length;
                Stream dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse tresponse = tRequest.GetResponse();
                dataStream = tresponse.GetResponseStream();
                StreamReader tReader = new StreamReader(dataStream);

                string sResponseFromServer = tReader.ReadToEnd();

                tReader.Close();
                dataStream.Close();
                tresponse.Close();
            }
            catch (Exception) {
            }


        }
    }
}
