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
using System.Xml.Linq;
using Login;

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
        public CertHelper certHelper;


        //WSAA variables needed to send requests to WSN
        // Maybe this variables wont be here, need to test
        private UInt32 _globalUniqueID = 0;
        UInt32 uniqueId;
        DateTime generationTime;
        DateTime expirationTime;
        string sign;
        string token;


        //public Afip(string WSAA_WSDL, string WSAA_URL, string CERT, string PRIVATEKEY, string PASSPHRASE, string RES_FOLDER, string TA_FOLDER, long CUIT, bool PRODUCTION, bool EXCEPTIONS, int SOAP_VERSION)
        //{
        //    this.WSAA_WSDL = WSAA_WSDL;
        //    this.WSAA_URL = WSAA_URL;
        //    this.CERT = CERT;
        //    this.PRIVATEKEY = PRIVATEKEY;
        //    this.SOAP_VERSION = SOAP_VERSION;
        //    this.PASSPHRASE = PASSPHRASE;
        //    this.RES_FOLDER = RES_FOLDER;
        //    this.TA_FOLDER = TA_FOLDER;
        //    this.CUIT = CUIT;
        //    this.PRODUCTION = PRODUCTION;
        //    this.EXCEPTIONS = EXCEPTIONS;
        //    this.electronicBilling = new ElectronicBilling(CUIT, PRODUCTION, SOAP_VERSION, WSAA_WSDL, WSAA_URL);
        //}


        // Accesing WSAA and retrieving Token and Sign to access WSN. This will probably be refactored into multiple functions as it should stay static but is painful to read.
        public void LogIn() //service would be "wsfe" here
        {
            string xmlLoginTemplate = "<loginTicketRequest><header><uniqueId></uniqueId><generationTime></generationTime><expirationTime></expirationTime></header><service></service></loginTicketRequest>";
            _globalUniqueID += 1;
            
            //nodes to be modified in template
            XmlNode xmlNodeUniqueId = default(XmlNode);
            XmlNode xmlNodeGenerationTime = default(XmlNode);
            XmlNode xmlNodeExpirationTime = default(XmlNode);
            XmlNode xmlNodeService = default(XmlNode);
            XmlDocument xmlLoginRequest = new XmlDocument();

            //filling the nodes with info
            xmlLoginRequest.LoadXml(xmlLoginTemplate);
            xmlNodeUniqueId = xmlLoginRequest.SelectSingleNode("//uniqueId");
            xmlNodeGenerationTime = xmlLoginRequest.SelectSingleNode("//generationTime");
            xmlNodeExpirationTime = xmlLoginRequest.SelectSingleNode("//expirationTime");
            xmlNodeService = xmlLoginRequest.SelectSingleNode("//service");
            xmlNodeGenerationTime.InnerText = DateTime.Now.AddMinutes(-10).ToString("s");
            xmlNodeExpirationTime.InnerText = DateTime.Now.AddMinutes(+10).ToString("s");
            xmlNodeUniqueId.InnerText = Convert.ToString(_globalUniqueID);
            xmlNodeService.InnerText = "wsfe";

            // Signing our request with out crt and creating a base64 string out of it
            
            // Creating cert needed for signing
            X509Certificate2 cert = certHelper.GetCert(CERT);


            // Converting final xml into bytes
            Encoding encodedMsg = Encoding.UTF8;
            byte[] msgBytes = encodedMsg.GetBytes(xmlLoginRequest.OuterXml);

            // Signing bytes with cert and creating a CMS
            byte[] encodedSignedMsg = certHelper.SignMsg(cert, msgBytes);


            // Converting CMS into Base64String
            string base64SignedMsg = Convert.ToBase64String(encodedSignedMsg);

            // Creating SOAP client and login request
            LoginCMSClient client = new LoginCMSClient();
            loginCmsRequestBody body = new loginCmsRequestBody(base64SignedMsg);
            loginCmsRequest request = new loginCmsRequest(body);

            // Sending request
            loginCmsResponse response = client.loginCms(request);        
            
            

            // Reading response from WSAA and storing it in machine
            XmlDocument xmlLoginResponse = new XmlDocument();
            xmlLoginResponse.LoadXml(response.Body.loginCmsReturn);
            xmlLoginResponse.Save(@"C:\Users\54112\Desktop\response.xml");

            // Extracting info from respone
            uniqueId = UInt32.Parse(xmlLoginResponse.SelectSingleNode("//uniqueId").InnerText);
            generationTime = DateTime.Parse(xmlLoginResponse.SelectSingleNode("//generationTime").InnerText);
            expirationTime = DateTime.Parse(xmlLoginResponse.SelectSingleNode("//expirationTime").InnerText);
            sign = xmlLoginResponse.SelectSingleNode("//sign").InnerText;
            token = xmlLoginResponse.SelectSingleNode("//token").InnerText;
        }

        public class CertHelper {
            public X509Certificate2 GetCert(string filePath)
            {
                X509Certificate2 cert =  new X509Certificate2(File.ReadAllBytes(filePath));
                return cert;
            }

            public byte[] SignMsg(X509Certificate2 cert, byte[] msg)
            {
                //Converting from bytes to CMS
                ContentInfo infoContent = new ContentInfo(msg);
                SignedCms signedCms = new SignedCms(infoContent);

                // Creating a signer with cert
                CmsSigner cmsSigner = new CmsSigner(cert);
                cmsSigner.IncludeOption = X509IncludeOption.EndCertOnly;

                //Signing
                signedCms.ComputeSignature(cmsSigner);

                // Converting to bytes again
                byte[] encodedSignedCms = signedCms.Encode();
                return encodedSignedCms;
            }
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
