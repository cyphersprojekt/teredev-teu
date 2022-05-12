using System;
using System.Text;
using System.Text.Encodings;
using System.Xml;
using System.Net;
using System.IO;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Pkcs;
using System.Web;
using System.Xml.Serialization;
using System.ComponentModel;


namespace LibreriaAfip
{
    public class Afip
    {

        // Afip has all the info (as always has) needed on the app and also instantiates needed services (Electronic Billing)
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


        //WSAA variables needed to send requests to WSN
        // Maybe this variables wont be here, need to test
        private UInt32 _globalUniqueID = 0;
        public string XML_LOGIN_REQUEST_TEMPLATE = "<loginTicketRequest><header><uniqueId></uniqueId><generationTime></generationTime><expirationTime></expirationTime></header><service></service></loginTicketRequest>";
        UInt32 uniqueId;
        DateTime generationTime;
        DateTime expirationTime;
        string sign;
        string token;


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
            this.electronicBilling = new ElectronicBilling(CUIT, PRODUCTION, SOAP_VERSION, WSAA_WSDL, WSAA_URL);
        }


        // Accesing WSAA and retrieving Token and Sign to access WSN. This will probably be refactored into multiple functions as it should stay static but is painful to read.
        public void LogIn(string service)
        {
            _globalUniqueID += 1;
            //nodes to be modified in template
            XmlNode xmlNodeUniqueId = default(XmlNode);
            XmlNode xmlNodeGenerationTime = default(XmlNode);
            XmlNode xmlNodeExpirationTime = default(XmlNode);
            XmlNode xmlNodeService = default(XmlNode);
            XmlDocument xmlLoginRequest = default(XmlDocument);

            //filling the nodes with info
            xmlLoginRequest.LoadXml(XML_LOGIN_REQUEST_TEMPLATE);
            xmlNodeUniqueId = xmlLoginRequest.SelectSingleNode("//uniqueId");
            xmlNodeGenerationTime = xmlLoginRequest.SelectSingleNode("//generationTime");
            xmlNodeExpirationTime = xmlLoginRequest.SelectSingleNode("//expirationTime");
            xmlNodeService = xmlLoginRequest.SelectSingleNode("//service");
            xmlNodeGenerationTime.InnerText = DateTime.Now.AddMinutes(-10).ToString("s");
            xmlNodeExpirationTime.InnerText = DateTime.Now.AddMinutes(+10).ToString("s");
            xmlNodeUniqueId.InnerText = Convert.ToString(_globalUniqueID);
            xmlNodeService.InnerText = service;                                                     /// TO DO, i have no clue the format this input should have!!!!

            // Signing our request with out crt and creating a base64 string out of it
            X509Certificate2 cert = new X509Certificate2();
            cert.Import(File.ReadAllBytes(CERT));

            Encoding encodedMsg = Encoding.UTF8;
            byte[] msgBytes = encodedMsg.GetBytes(xmlLoginRequest.OuterXml);

            ContentInfo infoContent = new ContentInfo(msgBytes);
            SignedCms signedCms = new SignedCms(infoContent);


            CmsSigner cmsSigner = new CmsSigner(cert);
            cmsSigner.IncludeOption = X509IncludeOption.EndCertOnly;

            signedCms.ComputeSignature(cmsSigner);

            byte[] encodedSignedCms = signedCms.Encode();

            string base64SignedCms = Convert.ToBase64String(encodedSignedCms);

            // Sending request to WSAA 



            string LoginResponse = "test";                                                                             // Missing actual web request client here. TO DO

            // Reading response from WSAA and filling Afip's security variables

            XmlDocument xmlLoginResponse = new XmlDocument();
            xmlLoginResponse.LoadXml(LoginResponse);

            uniqueId = UInt32.Parse(xmlLoginResponse.SelectSingleNode("//uniqueId").InnerText);
            generationTime = DateTime.Parse(xmlLoginResponse.SelectSingleNode("//generationTime").InnerText);
            expirationTime = DateTime.Parse(xmlLoginResponse.SelectSingleNode("//expirationTime").InnerText);
            sign = xmlLoginResponse.SelectSingleNode("//sign").InnerText;
            token = xmlLoginResponse.SelectSingleNode("//token").InnerText;

        }
        public class ElectronicBilling
        {
            public long CUIT;
            public bool PRODUCTION;
            public int SOAP_VERSION;
            public string WSAA_WSDL;
            public string WSAA_URL;

            public ElectronicBilling(long cuit, bool production, int soap_version, string wsaa_wsdl, string wsaa_ulr)
            {
                this.CUIT = cuit;
                this.PRODUCTION = production;
                this.SOAP_VERSION = soap_version;
                this.WSAA_URL = wsaa_ulr;
                this.WSAA_WSDL = wsaa_wsdl;
            }


            // General executor, takes xmls paths, inputs and type of operation to create appropiate requests
            public void ExecuteRequest(string operation, Object[] req)
            {
                string endpoint = "https://wswhomo.afip.gov.ar/wsfev1/service.asmx";
                XmlDocument soapEnvelopeXml = CreateSoapEnvolope(operation);
                HttpWebRequest webRequest = CreateWebRequest(endpoint, endpoint + "?op=" + operation);
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

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


            // Methods required to create a web request from xml
            private HttpWebRequest CreateWebRequest(string url, string operation)
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Headers.Add("SOAPAction", operation);
                webRequest.ContentType = "text/xml;charset=\"utf-8\"";
                webRequest.Accept = "text/xml";
                webRequest.Method = "POST";
                return webRequest;
            }

            private XmlDocument CreateSoapEnvolope(string operation)
            {
                XmlDocument soapEnvelopeDocument = new XmlDocument();
                string request = File.ReadAllText(@"\src\XMLs\" + operation + ".xml");
                soapEnvelopeDocument.LoadXml(request);
                return soapEnvelopeDocument;
            }

            private void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXML, HttpWebRequest webRequest)
            {
                using (Stream stream = webRequest.GetRequestStream())
                {
                    soapEnvelopeXML.Save(stream);
                }
            }



            // First method to test of the WS
            public void GetLastBill(int sales_point, int type)
            {
                Object[] salesPoint = new Object[2] { "ptoVenta", sales_point };
                Object[] billType = new Object[2] { "CbteTipo", type };
                Object[] req = new Object[2] { salesPoint, billType };


                this.ExecuteRequest("FECompUltimoAutorizado", req);
            }


        }
    }
}
