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
            inputData.Add("Token", "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiIHN0YW5kYWxvbmU9InllcyI/Pgo8c3NvIHZlcnNpb249IjIuMCI+CiAgICA8aWQgc3JjPSJDTj13c2FhaG9tbywgTz1BRklQLCBDPUFSLCBTRVJJQUxOVU1CRVI9Q1VJVCAzMzY5MzQ1MDIzOSIgZHN0PSJDTj13c2ZlLCBPPUFGSVAsIEM9QVIiIHVuaXF1ZV9pZD0iMjgyMjg4NDIwNSIgZ2VuX3RpbWU9IjE2NTY2MjI4NDUiIGV4cF90aW1lPSIxNjU2NjY2MTA1Ii8+CiAgICA8b3BlcmF0aW9uIHR5cGU9ImxvZ2luIiB2YWx1ZT0iZ3JhbnRlZCI+CiAgICAgICAgPGxvZ2luIGVudGl0eT0iMzM2OTM0NTAyMzkiIHNlcnZpY2U9IndzZmUiIHVpZD0iU0VSSUFMTlVNQkVSPUNVSVQgMjA0MjMwMjExNTMsIENOPXRvZG9lbnVub3Rlc3RpbmciIGF1dGhtZXRob2Q9ImNtcyIgcmVnbWV0aG9kPSIyMiI+CiAgICAgICAgICAgIDxyZWxhdGlvbnM+CiAgICAgICAgICAgICAgICA8cmVsYXRpb24ga2V5PSIyMDQyMzAyMTE1MyIgcmVsdHlwZT0iNCIvPgogICAgICAgICAgICA8L3JlbGF0aW9ucz4KICAgICAgICA8L2xvZ2luPgogICAgPC9vcGVyYXRpb24+Cjwvc3NvPgo=");
            inputData.Add("Sign", "BySHekBqjl1un0BVsufqbPP3QMthAirJXw4R0j+DDU4mOPDi+Jabrf9KaSZkHzWvI7S17I5ou1OCSncWANn8oXKjrl5vVxl52RBYSAvkXuyjRgBTcE4xO8WkJd5PiP+P7H0j99MseiMVYP3nQZb1vL55aQT0VpM8X1mbCRzbrHQ=");
            inputData.Add("Cuit", "");
            inputData.Add("MonId", "");
            inputData.Add("CbteTipo", "");
            inputData.Add("CbteNro", "");
            inputData.Add("PtoVta", "");

            WSEB wseb = new WSEB();
            watch.Start();
            wseb.executeOperation(inputData, "FEParamGetTiposTributos");
            watch.Stop();
            Console.WriteLine($"request attempt took {watch.ElapsedMilliseconds} ms");


            //WSLI wsli = new WSLI() { CERT = "C:\\Users\\54112\\Desktop\\proyectoAfip\\cert.pfx" };
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
