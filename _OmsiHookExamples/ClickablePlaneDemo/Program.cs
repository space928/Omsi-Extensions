using System;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Threading;
using OmsiHook;

namespace ClickablePlaneDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("#=#=#=#=#=# OmsiExtensions Testing #=#=#=#=#=#");

            OmsiHook.OmsiHook omsi = new();
            omsi.AttachToOMSI().Wait();
            Console.Clear();
        }
    }
}
