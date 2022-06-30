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
        string sign = "FsO6TOQ2lRCq50BUwaxt/M4V/u2GDv1ej8eEpqAKZDWSBA7c/jqrEQetG1NjlxizmPwaHbEfcg+orWx5elMTBm/PxydPoXe98uhosk9yt+keaQBXCQA15ILXBlwMD/X4FHQZubxAc8yQx6Ja2XY9RoP4YhNPbLOc0LuzH6XTflg=";
        string token = "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiIHN0YW5kYWxvbmU9InllcyI/Pgo8c3NvIHZlcnNpb249IjIuMCI+CiAgICA8aWQgc3JjPSJDTj13c2FhaG9tbywgTz1BRklQLCBDPUFSLCBTRVJJQUxOVU1CRVI9Q1VJVCAzMzY5MzQ1MDIzOSIgZHN0PSJDTj13c2ZlLCBPPUFGSVAsIEM9QVIiIHVuaXF1ZV9pZD0iMTg3ODY1NTgyNSIgZ2VuX3RpbWU9IjE2NTY1NTIwMzMiIGV4cF90aW1lPSIxNjU2NTk1MjkzIi8+CiAgICA8b3BlcmF0aW9uIHR5cGU9ImxvZ2luIiB2YWx1ZT0iZ3JhbnRlZCI+CiAgICAgICAgPGxvZ2luIGVudGl0eT0iMzM2OTM0NTAyMzkiIHNlcnZpY2U9IndzZmUiIHVpZD0iU0VSSUFMTlVNQkVSPUNVSVQgMjA0MjMwMjExNTMsIENOPXRvZG9lbnVub3Rlc3RpbmciIGF1dGhtZXRob2Q9ImNtcyIgcmVnbWV0aG9kPSIyMiI+CiAgICAgICAgICAgIDxyZWxhdGlvbnM+CiAgICAgICAgICAgICAgICA8cmVsYXRpb24ga2V5PSIyMDQyMzAyMTE1MyIgcmVsdHlwZT0iNCIvPgogICAgICAgICAgICA8L3JlbGF0aW9ucz4KICAgICAgICA8L2xvZ2luPgogICAgPC9vcGVyYXRpb24+Cjwvc3NvPgo=";
        long cuit = 1;


        // FEDummy
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

        // FEParamGetPtosVenta
        public void GetPointOfSale()
        {
            // XML request creation and editing
            XmlDocument xmlRequest = new XmlDocument();
            xmlRequest.LoadXml(File.ReadAllText(@"E:\Source\teredev-teu\Afip\src\XMLs\FEParamGetPtosVenta.xml"));

            // XML namespace creation as "ar" prefix is present in template, maybe to be made a function later on
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlRequest.NameTable);
            namespaceManager.AddNamespace("ar", "http://ar.gov.afip.dif.FEV1/");

            // Selecting nodes
            XmlNode xmlNodeToken = xmlRequest.SelectSingleNode("//ar:Token", namespaceManager);
            XmlNode xmlNodeSign = xmlRequest.SelectSingleNode("//ar:Sign", namespaceManager);
            XmlNode xmlNodeCuit = xmlRequest.SelectSingleNode("//ar:Cuit", namespaceManager);

            // Editing nodes
            xmlNodeToken.InnerText = token;
            xmlNodeSign.InnerText = sign;
            xmlNodeCuit.InnerText = cuit.ToString();


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

        // FEParamGetTiposMonedas
        public void GetCurrencyIds()
        {
            // XML request creation and editing
            XmlDocument xmlRequest = new XmlDocument();
            xmlRequest.LoadXml(File.ReadAllText(@"E:\Source\teredev-teu\Afip\src\XMLs\FEParamGetTiposMonedas.xml"));

            // XML namespace creation as "ar" prefix is present in template, maybe to be made a function later on
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlRequest.NameTable);
            namespaceManager.AddNamespace("ar", "http://ar.gov.afip.dif.FEV1/");

            // Selecting nodes
            XmlNode xmlNodeToken = xmlRequest.SelectSingleNode("//ar:Token", namespaceManager);
            XmlNode xmlNodeSign = xmlRequest.SelectSingleNode("//ar:Sign", namespaceManager);
            XmlNode xmlNodeCuit = xmlRequest.SelectSingleNode("//ar:Cuit", namespaceManager);

            // Editing nodes
            xmlNodeToken.InnerText = token;
            xmlNodeSign.InnerText = sign;
            xmlNodeCuit.InnerText = cuit.ToString();


            // XML -> usable data to create web request
            string data = xmlRequest.OuterXml;
            HttpWebRequest webRequest = CreateWebRequest(mainUrl);
            WriteIntoHttpRequest(webRequest, data);

            // SEND BABY SEND
            HttpWebResponse httpResponse = (HttpWebResponse)webRequest.GetResponse();

            // Read response and save to desktop
            XmlDocument xmlResponse = ReadFromHttpResponse(httpResponse);
            xmlResponse.Save(@"C:\Users\54112\Desktop\FEParamGetTiposMonedasResponse.xml");
        }

        //FEParamGetCotizacion
        public void GetCurrencyPrice()
        {
            // XML request creation and editing
            XmlDocument xmlRequest = new XmlDocument();
            xmlRequest.LoadXml(File.ReadAllText(@"E:\Source\teredev-teu\Afip\src\XMLs\FEParamGetCotizacion.xml"));

            // XML namespace creation as "ar" prefix is present in template, maybe to be made a function later on
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlRequest.NameTable);
            namespaceManager.AddNamespace("ar", "http://ar.gov.afip.dif.FEV1/");

            // Selecting nodes
            XmlNode xmlNodeToken = xmlRequest.SelectSingleNode("//ar:Token", namespaceManager);
            XmlNode xmlNodeSign = xmlRequest.SelectSingleNode("//ar:Sign", namespaceManager);
            XmlNode xmlNodeCuit = xmlRequest.SelectSingleNode("//ar:Cuit", namespaceManager);
            XmlNode xmlNodeMonId = xmlRequest.SelectSingleNode("//ar:MonId", namespaceManager);

            // Editing nodes
            xmlNodeToken.InnerText = token;
            xmlNodeSign.InnerText = sign;
            xmlNodeCuit.InnerText = cuit.ToString();
            xmlNodeMonId.InnerText = "DOL";


            // XML -> usable data to create web request
            string data = xmlRequest.OuterXml;
            HttpWebRequest webRequest = CreateWebRequest(mainUrl);
            WriteIntoHttpRequest(webRequest, data);

            // SEND BABY SEND
            HttpWebResponse httpResponse = (HttpWebResponse)webRequest.GetResponse();

            // Read response and save to desktop
            XmlDocument xmlResponse = ReadFromHttpResponse(httpResponse);
            xmlResponse.Save(@"C:\Users\54112\Desktop\GetCurrencyPriceResponse.xml");
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
