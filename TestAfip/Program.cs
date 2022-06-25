using System;
using LibreriaAfip;
namespace TestAfip
{
    class Program
    {
        static void Main(string[] args)
        {
            Afip afip = new Afip() { CERT = "C:\\Users\\54112\\Desktop\\proyectoAfip\\cert.pfx", PRIVATEKEY = "C:\\Users\\Maxi\\Desktop\\key", CUIT = 20423021153, EXCEPTIONS = false, PRODUCTION = false, SOAP_VERSION = 0, WSAA_URL = "https://wsaahomo.afip.gov.ar/ws/services/LoginCms?WSDL", WSAA_WSDL = "", PASSPHRASE = "", electronicBilling = null, RES_FOLDER = "", TA_FOLDER = "" };

            afip.LogIn();




        }
    }
}
