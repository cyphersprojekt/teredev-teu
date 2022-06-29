using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.IO;
using System.Xml;

namespace LibreriaAfip
{
    public class WSEB
    {
        string mainUrl = "https://wswhomo.afip.gov.ar/wsfev1/service.asmx";
        // Deleted all fields, add as needed


        public void dummy() // Dummy method to test soap client against afip wsfev1
        {
            XmlDocument xmlRequest = new XmlDocument();
            xmlRequest.LoadXml(File.ReadAllText(@"E:\Source\teredev-teu\Afip\src\XMLs\dummy.xml"));
            string data = xmlRequest.OuterXml;

            HttpWebRequest webRequest = CreateWebRequest(mainUrl);
            WriteIntoHttpRequest(webRequest, data);

            HttpWebResponse httpResponse = (HttpWebResponse)webRequest.GetResponse();

            XmlDocument xmlResponse = ReadFromHttpResponse(httpResponse);
            xmlResponse.Save(@"C:\Users\54112\Desktop\dummy.xml");
        }

        public void GetPointOfSale()
        {
            // Hardcoding for tests oops
            string sign = "FMpkECNLJb7BzNoo1++D2AWkokEr6FCM8/IHWu5Gldt3QXzJbww2QUK2s1xgjC8PGBW++/zkkQffX081vvOnghXj6U7Z3Ci13MYIlyL7oVGe9+M1xYIHSGXFKyyBqXSmbJZuVa8V1zkR03nA8B1p577EXFpNq209RMQEAsgFtTE=";
            string token = "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiIHN0YW5kYWxvbmU9InllcyI/Pgo8c3NvIHZlcnNpb249IjIuMCI+CiAgICA8aWQgc3JjPSJDTj13c2FhaG9tbywgTz1BRklQLCBDPUFSLCBTRVJJQUxOVU1CRVI9Q1VJVCAzMzY5MzQ1MDIzOSIgZHN0PSJDTj13c2ZlLCBPPUFGSVAsIEM9QVIiIHVuaXF1ZV9pZD0iNDIwNTYyMjE3OSIgZ2VuX3RpbWU9IjE2NTY1MTU4OTkiIGV4cF90aW1lPSIxNjU2NTU5MTU5Ii8+CiAgICA8b3BlcmF0aW9uIHR5cGU9ImxvZ2luIiB2YWx1ZT0iZ3JhbnRlZCI+CiAgICAgICAgPGxvZ2luIGVudGl0eT0iMzM2OTM0NTAyMzkiIHNlcnZpY2U9IndzZmUiIHVpZD0iU0VSSUFMTlVNQkVSPUNVSVQgMjA0MjMwMjExNTMsIENOPXRvZG9lbnVub3Rlc3RpbmciIGF1dGhtZXRob2Q9ImNtcyIgcmVnbWV0aG9kPSIyMiI+CiAgICAgICAgICAgIDxyZWxhdGlvbnM+CiAgICAgICAgICAgICAgICA8cmVsYXRpb24ga2V5PSIyMDQyMzAyMTE1MyIgcmVsdHlwZT0iNCIvPgogICAgICAgICAgICA8L3JlbGF0aW9ucz4KICAgICAgICA8L2xvZ2luPgogICAgPC9vcGVyYXRpb24+Cjwvc3NvPgo=";
            long cuit = 12345678;
            //

            // XML request creation and editing
            XmlDocument xmlRequest = new XmlDocument();
            xmlRequest.LoadXml(File.ReadAllText(@"E:\Source\teredev-teu\Afip\src\XMLs\FEParamGetPtosVenta.xml"));


            //// Not currently working, although it works perfectly fine on the sister library on login xml, curious.
            //XmlNode xmlNodeToken = xmlRequest.SelectSingleNode("ar:Token");
            //XmlNode xmlNodeSign = xmlRequest.SelectSingleNode("ar:Sign");
            //XmlNode xmlNodeCuit = xmlRequest.SelectSingleNode("ar:Cuit");

            //xmlNodeToken.InnerText = token;
            //xmlNodeSign.InnerText = sign;
            //xmlNodeCuit.InnerText = cuit.ToString();


            // XML -> usable data to create web request
            string data = xmlRequest.OuterXml;
            HttpWebRequest webRequest = CreateWebRequest(mainUrl);
            WriteIntoHttpRequest(webRequest, data);

            // SEND BABY SEND
            HttpWebResponse httpResponse = (HttpWebResponse)webRequest.GetResponse();

            // Read response and save to desktop
            XmlDocument xmlResponse = ReadFromHttpResponse(httpResponse);
            xmlResponse.Save(@"C:\Users\54112\Desktop\GetPtoSaleResponse.xml");
        }

        static private HttpWebRequest CreateWebRequest(string url)
        {
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "text/xml;charset=utf-8";
            return httpRequest;
        }
        static private void WriteIntoHttpRequest(HttpWebRequest request, string data)
        {
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(data);
            }
        }
        static private XmlDocument ReadFromHttpResponse(HttpWebResponse response)
        {
            XmlDocument xmlOutput = new XmlDocument();
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            {
                string result = streamReader.ReadToEnd();
                xmlOutput.LoadXml(result);
            }
            return xmlOutput;
        }

    }
}
