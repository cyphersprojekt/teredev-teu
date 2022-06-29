using System;
using LibreriaAfip;
namespace TestAfip
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = new System.Diagnostics.Stopwatch();

            //WSLI wsli = new WSLI() { CERT = "C:\\Users\\54112\\Desktop\\proyectoAfip\\cert.pfx" }; 
            //watch.Start();
            //wsli.LogIn();
            //watch.Stop();
            //Console.WriteLine($"Log in attempt took {watch.ElapsedMilliseconds} ms");

            WSEB wseb = new WSEB();
            watch.Start();
            wseb.GetPointOfSale();
            watch.Stop();
            Console.WriteLine($"dummy request attempt took {watch.ElapsedMilliseconds} ms");
        }
    }
}
