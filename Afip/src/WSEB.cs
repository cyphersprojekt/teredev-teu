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
        //TODO:
        //  - Create method/fabric for node creating, specially for CreateVoucher (nested repeteaded names), test method
        //  - Exception handling
        //  - Logging
        //  - Talk about input format 
        //  - Delete local dependencies, create proper constructors for proper library use by app

        string mainUrl = "https://wswhomo.afip.gov.ar/wsfev1/service.asmx";
        
        public void executeOperation(Dictionary<string, string> input, string operation)
        {
            //Input data format example

            // XML request creation and editing
            XmlDocument xmlRequest = new XmlDocument();
            xmlRequest.LoadXml(File.ReadAllText(@"E:\Source\teredev-teu\Afip\src\XMLs\" + operation + ".xml"));

            // XML namespace creation as "ar" prefix is present in template, maybe to be made a function later on
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlRequest.NameTable);
            namespaceManager.AddNamespace("ar", "http://ar.gov.afip.dif.FEV1/");

            // Selecting nodes and creating a list          ->                  ->                ->               ->                ->          ->   //TO DO: Create a method/fabric for the nodes
            XmlNode xmlNodeToken = xmlRequest.SelectSingleNode("//ar:Token", namespaceManager);
            XmlNode xmlNodeSign = xmlRequest.SelectSingleNode("//ar:Sign", namespaceManager);
            XmlNode xmlNodeCuit = xmlRequest.SelectSingleNode("//ar:Cuit", namespaceManager);
            XmlNode xmlNodeMonId = xmlRequest.SelectSingleNode("//ar:MonId", namespaceManager);
            XmlNode xmlNodeCbteTipo = xmlRequest.SelectSingleNode("//ar:CbteTipo", namespaceManager);
            XmlNode xmlNodeCbteNro = xmlRequest.SelectSingleNode("//ar:CbteNro", namespaceManager);
            XmlNode xmlNodePtoVta = xmlRequest.SelectSingleNode("//ar:PtoVta", namespaceManager);


            List<XmlNode> nodeList = new List<XmlNode> {xmlNodeToken, xmlNodeSign, xmlNodeCuit, xmlNodeMonId, xmlNodeCbteTipo, xmlNodeCbteNro, xmlNodePtoVta };
            
            // Editing nodes if they exist
            foreach(XmlNode node in nodeList)
            {
                if(node != null)
                {
                    node.InnerText = input[node.LocalName];
                }
            }

            // XML -> usable data to create web request
            string data = xmlRequest.OuterXml;
            HttpWebRequest webRequest = CreateWebRequest(mainUrl);
            WriteIntoHttpRequest(webRequest, data);

            // SEND BABY SEND
            HttpWebResponse httpResponse = (HttpWebResponse)webRequest.GetResponse();

            // Read response and save to desktop
            XmlDocument xmlResponse = ReadFromHttpResponse(httpResponse);
            xmlResponse.Save(@"C:\Users\54112\Desktop\" + operation + "Response.xml");
        }




        // Old methods, replaced by general one //

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

        //// FEParamGetPtosVenta
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

        //// FEParamGetTiposMonedas
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
            xmlResponse.Save(@"C:\Users\54112\Desktop\FEParamGetTiposMonedasResponse.xml");
        }

        ////FEParamGetCotizacion
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
            //xmlNodeToken.InnerText = token;
            //xmlNodeSign.InnerText = sign;
            //xmlNodeCuit.InnerText = cuit.ToString();
            //xmlNodeMonId.InnerText = "DOL";


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


        // Auxiliary methods

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
