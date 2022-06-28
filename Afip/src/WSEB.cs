using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicBilling;
using System.Web;
using System.Net;
using System.IO;
using System.Xml;

namespace LibreriaAfip
{
    public class WSEB
    {
        // Deleted all fields, add as needed
        

        public void dummy() // Dummy method to test soap client against afip wsfev1
        {
            var url = "https://wswhomo.afip.gov.ar/wsfev1/service.asmx";
            XmlDocument response = new XmlDocument();

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "text/xml;charset=utf-8";

            XmlDocument template = new XmlDocument();
            template.LoadXml(File.ReadAllText(@"E:\Source\teredev-teu\Afip\src\XMLs\dummy.xml"));

            string data = template.OuterXml;

            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                response.LoadXml(result);
            }
            response.Save(@"C:\Users\54112\Desktop\dummyResponse.xml");           


        }

    }
}
