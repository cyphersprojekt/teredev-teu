using System;
using System.Collections.Generic;
using LibreriaAfip;
using Serilog;
using Serilog.Templates;
namespace TestAfip
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithProperty("Application", "TEU")
                .WriteTo.Console( new ExpressionTemplate (
                    "{ {@t, @mt, @l, @x, ..@p} }\n"))
                .CreateLogger();

            var watch = new System.Diagnostics.Stopwatch();

            Data data = new Data();


            data.Token = "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiIHN0YW5kYWxvbmU9InllcyI/Pgo8c3NvIHZlcnNpb249IjIuMCI+CiAgICA8aWQgc3JjPSJDTj13c2FhaG9tbywgTz1BRklQLCBDPUFSLCBTRVJJQUxOVU1CRVI9Q1VJVCAzMzY5MzQ1MDIzOSIgZHN0PSJDTj13c2ZlLCBPPUFGSVAsIEM9QVIiIHVuaXF1ZV9pZD0iMzc3NDExMzE3MyIgZ2VuX3RpbWU9IjE2NTcwMzQxNjIiIGV4cF90aW1lPSIxNjU3MDc3NDIyIi8+CiAgICA8b3BlcmF0aW9uIHR5cGU9ImxvZ2luIiB2YWx1ZT0iZ3JhbnRlZCI+CiAgICAgICAgPGxvZ2luIGVudGl0eT0iMzM2OTM0NTAyMzkiIHNlcnZpY2U9IndzZmUiIHVpZD0iU0VSSUFMTlVNQkVSPUNVSVQgMjA0MjMwMjExNTMsIENOPXRldSIgYXV0aG1ldGhvZD0iY21zIiByZWdtZXRob2Q9IjIyIj4KICAgICAgICAgICAgPHJlbGF0aW9ucz4KICAgICAgICAgICAgICAgIDxyZWxhdGlvbiBrZXk9IjIwNDIzMDIxMTUzIiByZWx0eXBlPSI0Ii8+CiAgICAgICAgICAgICAgICA8cmVsYXRpb24ga2V5PSIyMzQyNTc1ODIwOSIgcmVsdHlwZT0iNCIvPgogICAgICAgICAgICA8L3JlbGF0aW9ucz4KICAgICAgICA8L2xvZ2luPgogICAgPC9vcGVyYXRpb24+Cjwvc3NvPgo=";
            data.Sign = "HlpNtnL+dCbsAWzP/qVW2ksHw1vtZwfYLWRmMzZSOL9E2IjnGMwsx70v/weAYBZ0UIB7mok6oPbl6XTozmofGb1HgxlVCP3w5PQfcR729dffp8zpB7GiIsquyVsR2CVxCC4Tv6vhHjRJ/rkc4EQQylNHFr0SE3pO3F8Uibl7FmU=";
            data.Cuit = "23425758209";
            data.MonId = "PES";
            data.CbteTipo = "6";
            data.CbteNro = "1";
            data.PtoVta = "3";
            data.CantReg =  "1";
            data.Concepto =  "1";
            data.DocTipo =  "80";
            data.DocNro = "20423021153";
            data.CbteDesde = "1";
            data.CbteHasta =  "1";
            data.CbteFch = "20220703";
            data.ImpTotal = "666";
            data.ImpTotConc = "666";
            data.ImpNeto =  "0";
            data.ImpOpEx = "0";
            data.ImpTrib = "0";
            data.ImpIva = "0";
            data.FchServDesde = "";
            data.FchServHasta = "";
            data.FchVtoPago = "";
            data.MonCotiz = "1";




            //
            // FECompUltimoAutorizado - FECompConsultar - FEParamGetPtosVenta - FECAESolicitar
            //

            WSEB wseb = new WSEB();
            watch.Start();
            wseb.executeOperation(data, "FECompConsultar");
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

            Console.ReadLine();
        }
    }
}
