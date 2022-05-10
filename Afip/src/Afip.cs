using System;
using System.Xml;
using System.Net;
using System.IO;

namespace LibreriaAfip
{
    public class Afip
    {

        public string WSAA_WSDL;
        public string WSAA_URL;
        public string CERT;
        public string PRIVATEKEY;
        public string PASSPHRASE;
        public string RES_FOLDER;
        public string TA_FOLDER;
        public int SOAP_VERSION;
        public long CUIT;
        public bool PRODUCTION;
        public bool EXCEPTIONS;
        public ElectronicBilling electronicBilling;

        public Afip(string WSAA_WSDL, string WSAA_URL, string CERT, string PRIVATEKEY, string PASSPHRASE, string RES_FOLDER, string TA_FOLDER, long CUIT, bool PRODUCTION, bool EXCEPTIONS, int SOAP_VERSION)
        {
            this.WSAA_WSDL = WSAA_WSDL;
            this.WSAA_URL = WSAA_URL;
            this.CERT = CERT;
            this.PRIVATEKEY = PRIVATEKEY;
            this.SOAP_VERSION = SOAP_VERSION;
            this.PASSPHRASE = PASSPHRASE;
            this.RES_FOLDER = RES_FOLDER;
            this.TA_FOLDER = TA_FOLDER;
            this.CUIT = CUIT;
            this.PRODUCTION = PRODUCTION;
            this.EXCEPTIONS = EXCEPTIONS;
            this.electronicBilling = new ElectronicBilling(this);
        }

    }
    public class ElectronicBilling
    {
        public Afip afip;
        public ElectronicBilling(Afip afip)
        {
            this.afip = afip;
        }

        public void executeRequest(string operation, Object[] req)
        {
            string endpoint = "https://wswhomo.afip.gov.ar/wsfev1/service.asmx";
            XmlDocument soapEnvelopeXml = createSoapEnvolope(operation);
            HttpWebRequest webRequest = createWebRequest(endpoint, endpoint + "?op=" + operation);
            insertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            asyncResult.AsyncWaitHandle.WaitOne();

            string soapResult;

            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = reader.ReadToEnd();
                }
                Console.WriteLine(soapResult);
            }               
        }

        private HttpWebRequest createWebRequest(string url, string operation)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", operation);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private XmlDocument createSoapEnvolope(string operation)
        {
            XmlDocument soapEnvelopeDocument = new XmlDocument();
            string request = File.ReadAllText(@"\src\XMLs\" + operation + ".xml");
            soapEnvelopeDocument.LoadXml(request);
            return soapEnvelopeDocument;
        }

        private void insertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXML, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXML.Save(stream);
            }
        }

        public void getLastBill(int sales_point, int type)
        {
            Object[] salesPoint = new Object[2] { "ptoVenta", sales_point };
            Object[] billType = new Object[2] { "CbteTipo", type };
            Object[] req = new Object[2] { salesPoint, billType };


            this.executeRequest("FECompUltimoAutorizado", req);
        }
        
        
    }


        
}
