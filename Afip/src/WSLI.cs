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
using Serilog;

namespace LibreriaAfip
{
    public class WSLI
    {
        public string CERT;
        public CertHelper certHelper = new CertHelper();

        private UInt32 _globalUniqueID = 0;
        UInt32 uniqueId;
        DateTime generationTime;
        DateTime expirationTime;
        string sign;
        string token;

        // Accesing WSAA and retrieving Token and Sign to access WSN.
        public void LogIn()
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

            xmlNodeUniqueId.InnerText = Convert.ToString(_globalUniqueID);
            xmlNodeGenerationTime.InnerText = DateTime.Now.AddMinutes(-10).ToString("s");
            xmlNodeExpirationTime.InnerText = DateTime.Now.AddMinutes(+10).ToString("s");
            xmlNodeService.InnerText = "wsfe";

            // Saving request for debugging
            xmlLoginRequest.Save(@"C:\Users\54112\Desktop\request.xml");

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

            if(sign != null && token != null)
            {
                Log.Information("Correctly signed in into WSAA at {GenerationTime}, until {ExpirationTime}.", generationTime, expirationTime);
            }
        
        }

        public class CertHelper {
            public X509Certificate2 GetCert(string filePath)
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException();
                }
                X509Certificate2 cert = new X509Certificate2();
                try
                {
                    cert = new X509Certificate2(File.ReadAllBytes(filePath));                    
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine("Certificate file for selected operation was not found", ex.Message);

                }
                catch (IOException ex)
                {
                    Console.WriteLine("Certificate file could not be accessed", ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error while trying to create cert", ex.Message);
                }
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
    }
}
