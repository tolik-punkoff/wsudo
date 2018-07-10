using System;
using System.Collections.Generic;
using System.Text;

namespace wsudo
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                PrintHelp();
                return;
            }
            if ((args[0] == "/?"))
            {
                PrintHelp();
                return;
            }

            string app = args[0];
            string parameters = "";

            if (args.Length > 1) //собираем параметры
            {
                for (int i = 1; i < args.Length; i++)
                {
                    parameters = parameters + args[i] + " ";
                }
            }

            string FindResult = FindApp.Find(app);
            if (FindResult == null)
            {
                Console.WriteLine(app + " not found or not executable file.");
            }
            else
            {
                if (!RunApp.Excecute(app, parameters))
                {
                    Console.WriteLine("ERROR: " + RunApp.ErrorMessage);
                }
            }                                    
        }

        static void PrintHelp()
        {
            Console.WriteLine("NedoSudo (L) Werwolf, 2018");
            Console.WriteLine("Internal program for K.B.S.");
            Console.WriteLine("Use: ");
            Console.WriteLine("wsudo <program> [parameters]");
        }
    }
}
