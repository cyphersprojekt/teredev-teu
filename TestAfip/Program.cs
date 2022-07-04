using System;
using System.Collections.Generic;
using LibreriaAfip;
namespace TestAfip
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = new System.Diagnostics.Stopwatch();

            Dictionary<string, string> inputData = new Dictionary<string, string>();
            inputData.Add("Token", "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiIHN0YW5kYWxvbmU9InllcyI/Pgo8c3NvIHZlcnNpb249IjIuMCI+CiAgICA8aWQgc3JjPSJDTj13c2FhaG9tbywgTz1BRklQLCBDPUFSLCBTRVJJQUxOVU1CRVI9Q1VJVCAzMzY5MzQ1MDIzOSIgZHN0PSJDTj13c2ZlLCBPPUFGSVAsIEM9QVIiIHVuaXF1ZV9pZD0iMTA4MTY1NjM2OCIgZ2VuX3RpbWU9IjE2NTY4ODgxMjgiIGV4cF90aW1lPSIxNjU2OTMxMzg4Ii8+CiAgICA8b3BlcmF0aW9uIHR5cGU9ImxvZ2luIiB2YWx1ZT0iZ3JhbnRlZCI+CiAgICAgICAgPGxvZ2luIGVudGl0eT0iMzM2OTM0NTAyMzkiIHNlcnZpY2U9IndzZmUiIHVpZD0iU0VSSUFMTlVNQkVSPUNVSVQgMjA0MjMwMjExNTMsIENOPXRldSIgYXV0aG1ldGhvZD0iY21zIiByZWdtZXRob2Q9IjIyIj4KICAgICAgICAgICAgPHJlbGF0aW9ucz4KICAgICAgICAgICAgICAgIDxyZWxhdGlvbiBrZXk9IjIwNDIzMDIxMTUzIiByZWx0eXBlPSI0Ii8+CiAgICAgICAgICAgICAgICA8cmVsYXRpb24ga2V5PSIyMzQyNTc1ODIwOSIgcmVsdHlwZT0iNCIvPgogICAgICAgICAgICA8L3JlbGF0aW9ucz4KICAgICAgICA8L2xvZ2luPgogICAgPC9vcGVyYXRpb24+Cjwvc3NvPgo=");
            inputData.Add("Sign", "ccFlL+HoyZHI2+PIZJpzj0Egv8MMvFvajAbgqczYySgzdIy8BBOPAIUmQKw9i/VoCRc+HOXpaRjPHMR4+kNNfoK+gV15uCkMBvvr4hT1oPN2eEm2hyXVetXh5BpciO5Jcg2UVFacBeOShDrHBLZksJy620J++5lSDi8phJ45xd4=");
            inputData.Add("Cuit", "23425758209");
            inputData.Add("MonId", "PES");
            inputData.Add("CbteTipo", "6");
            inputData.Add("CbteNro", "0");
            inputData.Add("PtoVta", "3");
            inputData.Add("CantReg", "1");
            inputData.Add("Concepto", "1");
            inputData.Add("DocTipo", "80");
            inputData.Add("DocNro", "20423021153");
            inputData.Add("CbteDesde", "1");
            inputData.Add("CbteHasta", "1");
            inputData.Add("CbteFch", "20220703");
            inputData.Add("ImpTotal", "666");
            inputData.Add("ImpTotConc", "666");
            inputData.Add("ImpNeto", "0");
            inputData.Add("ImpOpEx", "0");
            inputData.Add("ImpTrib", "0");
            inputData.Add("ImpIVA", "0");
            inputData.Add("FchServDesde", "");
            inputData.Add("FchServHasta", "");
            inputData.Add("FchVtoPago", "");
            inputData.Add("MonCotiz", "1");




            //
            // FECompUltimoAutorizado - FECompConsultar - FEParamGetPtosVenta - FECAESolicitar
            //

            WSEB wseb = new WSEB();
            watch.Start();
            wseb.executeOperation(inputData, "FECAESolicitar");
            watch.Stop();
            Console.WriteLine($"request attempt took {watch.ElapsedMilliseconds} ms");


            //WSLI wsli = new WSLI() { CERT = "C:\\Users\\54112\\Desktop\\certs\\cert.pfx" };
            //watch.Start();
            //wsli.LogIn();
            //watch.Stop();
            //Console.WriteLine($"Log in attempt took {watch.ElapsedMilliseconds} ms");

            //WSEB wseb = new WSEB();
            //watch.Start();
            //wseb.GetCurrencyPrice();
            //watch.Stop();
            //Console.WriteLine($"request attempt took {watch.ElapsedMilliseconds} ms");
        }
    }
}
