using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.IO;
using System.Xml;
using Serilog;
using Serilog.Templates;

namespace LibreriaAfip
{
    public class WSEB
    {
        //TODO:
        //  - Logging

        string mainUrl = "https://wswhomo.afip.gov.ar/wsfev1/service.asmx";
        
        public void executeOperation(Data input, string operation)
        {
            string templatePaths = @"E:\Source\teredev-teu\Afip\src\XMLs\" + operation + ".xml";

            // XML request creation
            XmlDocument xmlRequest = LoadTemplate(templatePaths);            

            // Adding request data to template
            xmlRequest = EditRequest(xmlRequest, input);

            // XML -> usable data to create web request
            xmlRequest.Save(@"C:\Users\54112\Desktop\" + operation + "Request.xml");
            string data = xmlRequest.OuterXml;
            HttpWebRequest webRequest = CreateWebRequest(mainUrl);
            WriteIntoHttpRequest(webRequest, data);

            // SEND BABY SEND
            HttpWebResponse httpResponse = (HttpWebResponse)webRequest.GetResponse();

            // Read response and save to desktop
            XmlDocument xmlResponse = ReadFromHttpResponse(httpResponse);
            xmlResponse.Save(@"C:\Users\54112\Desktop\" + operation + "Response.xml");
        }


        // Auxiliary methods
        static private XmlDocument EditRequest(XmlDocument request, Data data)
        {
            if (data == null)
            {
                throw new ArgumentNullException();
            }

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(request.NameTable);
            namespaceManager.AddNamespace("ar", "http://ar.gov.afip.dif.FEV1/");

            XmlNode xmlNodeToken = request.SelectSingleNode("//ar:Token", namespaceManager);
            XmlNode xmlNodeSign = request.SelectSingleNode("//ar:Sign", namespaceManager);
            XmlNode xmlNodeCuit = request.SelectSingleNode("//ar:Cuit", namespaceManager);
            XmlNode xmlNodeMonId = request.SelectSingleNode("//ar:MonId", namespaceManager);
            XmlNode xmlNodeCbteTipo = request.SelectSingleNode("//ar:CbteTipo", namespaceManager);
            XmlNode xmlNodeCbteNro = request.SelectSingleNode("//ar:CbteNro", namespaceManager);
            XmlNode xmlNodePtoVta = request.SelectSingleNode("//ar:PtoVta", namespaceManager);
            XmlNode xmlNodeCantReg = request.SelectSingleNode("//ar:CantReg", namespaceManager);
            XmlNode xmlNodeConcepto = request.SelectSingleNode("//ar:Concepto", namespaceManager);
            XmlNode xmlNodeDocTipo = request.SelectSingleNode("//ar:DocTipo", namespaceManager);
            XmlNode xmlNodeDocNro = request.SelectSingleNode("//ar:DocNro", namespaceManager);
            XmlNode xmlNodeCbteDesde = request.SelectSingleNode("//ar:CbteDesde", namespaceManager);
            XmlNode xmlNodeCbteHasta = request.SelectSingleNode("//ar:CbteHasta", namespaceManager);
            XmlNode xmlNodeCbteFch = request.SelectSingleNode("//ar:CbteFch", namespaceManager);
            XmlNode xmlNodeImpTotal = request.SelectSingleNode("//ar:ImpTotal", namespaceManager);
            XmlNode xmlNodeImpTotConc = request.SelectSingleNode("//ar:ImpTotConc", namespaceManager);
            XmlNode xmlNodeImpNeto = request.SelectSingleNode("//ar:ImpNeto", namespaceManager);
            XmlNode xmlNodeImpOpEx = request.SelectSingleNode("//ar:ImpOpEx", namespaceManager);
            XmlNode xmlNodeImpTrib = request.SelectSingleNode("//ar:ImpTrib", namespaceManager);
            XmlNode xmlNodeImpIVA = request.SelectSingleNode("//ar:ImpIVA", namespaceManager);
            XmlNode xmlNodeFchServDesde = request.SelectSingleNode("//ar:FchServDesde", namespaceManager);
            XmlNode xmlNodeFchServHasta = request.SelectSingleNode("//ar:FchServHasta", namespaceManager);
            XmlNode xmlNodeFchVtoPago = request.SelectSingleNode("//ar:FchVtoPago", namespaceManager);
            XmlNode xmlNodeMonCotiz = request.SelectSingleNode("//ar:MonCotiz", namespaceManager);

            List<XmlNode> nodeList = new List<XmlNode> {xmlNodeToken, xmlNodeSign, xmlNodeCuit, xmlNodeMonId, xmlNodeCbteTipo, xmlNodeCbteNro, xmlNodePtoVta,
            xmlNodeCantReg, xmlNodeConcepto, xmlNodeDocTipo, xmlNodeDocNro, xmlNodeCbteDesde, xmlNodeCbteHasta, xmlNodeCbteFch, xmlNodeImpTotal, xmlNodeImpTotConc, xmlNodeImpNeto, xmlNodeImpOpEx,
            xmlNodeImpTrib, xmlNodeImpIVA, xmlNodeFchServDesde, xmlNodeFchServHasta, xmlNodeFchVtoPago, xmlNodeMonCotiz };

            try
            {
                // Editing nodes if they exist
                foreach (XmlNode node in nodeList)
                {
                    if (node != null)
                    {
                        // Getting the property from data equal to a node name is done here by reflection: I have no clue how good/bad this is or what kind of exceptions could this cause
                        var value = data.GetType().GetProperty(node.LocalName).GetValue(data, null);
                        node.InnerText = value.ToString();                        
                    }
                }                
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("Data object was null", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while trying to edit XML template", ex.Message);
            }
            return request;

        }

        static private XmlDocument LoadTemplate(string path)
        {
            XmlDocument template = new XmlDocument();

            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            try
            {
                template.LoadXml(File.ReadAllText(path));
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Template file for selected operation was not found", ex.Message);

            }
            catch (IOException ex)
            {
                Console.WriteLine("Template file could not be accessed", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while trying to read template XMLs", ex.Message);
            }
            
            return template;
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
            try
            {
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    if (data == null)
                    {
                        throw new ArgumentNullException();
                    }
                    streamWriter.Write(data);
                }
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("An argument was invalid, most probably, the data object being loaded into the web request was null or empty", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while trying to write into web request", ex.Message);
            }

        }
        static private XmlDocument ReadFromHttpResponse(HttpWebResponse response)
        {
            if (response == null)
            {
                throw new ArgumentNullException();
            }

            XmlDocument xmlOutput = new XmlDocument();

            try
            {
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    xmlOutput.LoadXml(result);
                }
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("Respose was empty", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while trying to read response", ex.Message);
            }

            return xmlOutput;
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
    }
}
