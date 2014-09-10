using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Net;

namespace pjsipTest {


    class JSONHelper {

        private const string basePath = "http://dragon-p2p.herokuapp.com/";
        private const string listPath = basePath + "sdp_lists.json";
        private const string registerPath = basePath + "sdp_lists/sdp_register";

        public static T Deserialize<T>(string data) where T : class, new() {
            T result = null;

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(data))) {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

                result = (SDPArray)serializer.ReadObject(stream) as T;
            }
            return result;
        }

        public static void GetList(EventHandler handler) {
            HttpWebRequest request = WebRequest.Create(listPath) as HttpWebRequest;

            request.BeginGetResponse(new AsyncCallback(result => {
                HttpWebRequest r = result.AsyncState as HttpWebRequest;
                HttpWebResponse response = r.EndGetResponse(result) as HttpWebResponse;

                using (Stream stream = response.GetResponseStream()) {
                    using (StreamReader streamRead = new StreamReader(stream)) {
                        string responseString = streamRead.ReadToEnd();

                        SDPArray sdpArray = JSONHelper.Deserialize<SDPArray>(responseString);
                        handler(sdpArray, null);
                    }
                }
            }), request);
        }

        public static void Register(string name, string data) {
            HttpWebRequest request = WebRequest.Create(registerPath) as HttpWebRequest;

            request.Method = "POST";
            request.ContentType = "application/json";

            nameTemp = name;
            dataTemp = data;
            
            string jsonStr = "{\"user\":\"" + nameTemp + "\",\"sdp\":\"" + dataTemp + "\"}";
            byte[] strByte = Encoding.UTF8.GetBytes(jsonStr);
            request.ContentLength = strByte.Length;

            request.BeginGetRequestStream(new AsyncCallback(GetRequestCB), request);
        }

        private static string nameTemp;
        private static string dataTemp;

        private static void GetRequestCB(IAsyncResult result) {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;

            // End the operation
            Stream postStream = request.EndGetRequestStream(result);
            string jsonStr = "{\"user\":\"" + nameTemp + "\",\"sdp\":\"" + dataTemp + "\"}";
            byte[] strByte = Encoding.UTF8.GetBytes(jsonStr);

            // Write to the request stream.
            postStream.Write(strByte, 0, strByte.Length);
            postStream.Close();

            // Start the asynchronous operation to get the response
            request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);
        
        }

        private static void GetResponseCallback(IAsyncResult asynchronousResult) {
            try {
                HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

                // End the operation
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                string responseString = streamRead.ReadToEnd();
                Console.WriteLine(responseString);
                // Close the stream object
                streamResponse.Close();
                streamRead.Close();

                // Release the HttpWebResponse
                response.Close();
            }
            catch (Exception) { }
            
        }
    }

    [DataContract]
    class SDP {

        [DataMember(Name="id")]
        public string ID { get; set; }

        [DataMember(Name="user")]
        public string User { get; set; }

        [DataMember(Name="sdp")]
        public string Data { get; set; }
    }

    [CollectionDataContract]
    class SDPArray : List<SDP> {
    }
}
