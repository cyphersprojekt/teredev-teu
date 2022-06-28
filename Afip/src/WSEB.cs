using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicBilling;

namespace LibreriaAfip.src
{
    public class WSEB
    {
        public long CUIT;
        public string WSAA_URL;

        public void dummy()
        {
            

            
        }


        // General executor, takes xmls paths, inputs and type of operation to create appropiate requests
        //public void ExecuteRequest(string operation, Object[] req)
        //{
        //    string endpoint = "https://wswhomo.afip.gov.ar/wsfev1/service.asmx";
        //    XmlDocument soapEnvelopeXml = CreateSoapEnvolope(operation);
        //    HttpWebRequest webRequest = CreateWebRequest(endpoint, endpoint + "?op=" + operation);
        //    InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

        //    IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

        //    asyncResult.AsyncWaitHandle.WaitOne();

        //    string soapResult;

        //    using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
        //    {
        //        using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
        //        {
        //            soapResult = reader.ReadToEnd();
        //        }
        //        Console.WriteLine(soapResult);
        //    }
        //}


        //// Methods required to create a web request from xml
        //private HttpWebRequest CreateWebRequest(string url, string operation)
        //{
        //    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
        //    webRequest.Headers.Add("SOAPAction", operation);
        //    webRequest.ContentType = "text/xml;charset=\"utf-8\"";
        //    webRequest.Accept = "text/xml";
        //    webRequest.Method = "POST";
        //    return webRequest;
        //}

        //private XmlDocument CreateSoapEnvolope(string operation)
        //{
        //    XmlDocument soapEnvelopeDocument = new XmlDocument();
        //    string request = File.ReadAllText(@"\src\XMLs\" + operation + ".xml");
        //    soapEnvelopeDocument.LoadXml(request);
        //    return soapEnvelopeDocument;
        //}

        //private void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXML, HttpWebRequest webRequest)
        //{
        //    using (Stream stream = webRequest.GetRequestStream())
        //    {
        //        soapEnvelopeXML.Save(stream);
        //    }
        //}



        //// First method to test of the WS
        //public void GetLastBill(int sales_point, int type)
        //{
        //    Object[] salesPoint = new Object[2] { "ptoVenta", sales_point };
        //    Object[] billType = new Object[2] { "CbteTipo", type };
        //    Object[] req = new Object[2] { salesPoint, billType };


        //    this.ExecuteRequest("FECompUltimoAutorizado", req);
        //}


    }
}
