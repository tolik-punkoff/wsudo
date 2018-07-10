using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Diagnostics;

namespace wsudo
{
    public static class RunApp
    {
        public static string ErrorMessage = "";
        
        public static bool IsAdmin()
        {
            WindowsPrincipal pricipal = new WindowsPrincipal(
                WindowsIdentity.GetCurrent());

            return pricipal.IsInRole(
                WindowsBuiltInRole.Administrator);
        }

        public static bool Excecute(string path, string parameters)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = path;
            psi.Arguments = parameters;            
            bool admin = IsAdmin();

            if (!admin)
            {
                psi.Verb = "runas";
            }

            try
            {
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                if (admin) //админские права уже были, что-то испортилось
                {
                    ErrorMessage = ex.Message;
                    return false;
                }
               //иначе может быть просто нажали отмену в UAC
            }
            return true;
        }
    }
}
