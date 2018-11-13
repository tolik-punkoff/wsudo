using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Diagnostics;
using System.ComponentModel;

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
            catch (Win32Exception wex)
            {
                if (wex.NativeErrorCode == 1223) //нажали "Отмену" в окне UAC
                {
                    return true;
                }
                else //какой-то другой Win32 Error
                {
                    ErrorMessage = wex.NativeErrorCode.ToString() + " " + wex.Message;
                    return false;
                }
            }
            catch (Exception ex) //какой-то другой Exception
            {
                ErrorMessage = ex.Message;
                return false;
            }
            return true;
        }
    }
}
