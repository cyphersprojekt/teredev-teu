using System;
using LibreriaAfip;
namespace TestAfip
{
    class Program
    {
        static void Main(string[] args)
        {
            WSLI wsli = new WSLI() { CERT = "C:\\Users\\54112\\Desktop\\proyectoAfip\\cert.pfx"};
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            wsli.LogIn();
            watch.Stop();
            Console.WriteLine($"Log in attempt took {watch.ElapsedMilliseconds} ms");



        }
    }
}
