using System;

namespace LibreriaAfip
{
    public class Afip
    {

        string WSAA_WSDL;
        string WSAA_URL;
        string CERT;
        string PRIVATEKEY;
        string PASSPHRASE;
        string RES_FOLDER;
        string TA_FOLDER;
        int SOAP_VERSION;
        long CUIT;
        bool PRODUCTION;
        bool EXCEPTIONS;
        ElectronicBilling electronicBilling = new ElectronicBilling();

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
        }

    }
    public class ElectronicBilling
    {
        public int executeRequest(string operation, Object[] req)
        {
            return 666;
        }
        public int getLastBill(int sales_point, int type)
        {
            Object[] salesPoint = new Object[2] { "ptoVenta", sales_point };
            Object[] billType = new Object[2] { "CbteTipo", type };
            Object[] req = new Object[2] { salesPoint, billType };


            return this.executeRequest("FECompUltimoAutorizado", req);
        }
        
    }


        
}
