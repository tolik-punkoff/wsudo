using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace wsudo
{
    public static class FindApp
    {
        private static string[] FindDirs = null;
        private static string[] Extensions = new string[] {"com","exe","bat","cmd"};
        
        private static string[] GetPathDirs()
        {
            string path_var=Environment.GetEnvironmentVariable("PATH");
            string[] dirs = path_var.Split(';');

            return dirs;
        }

        private static void GetFindDirs()
        {
            //получаем текущий каталог
            string curdir = Directory.GetCurrentDirectory();
            //получаем каталоги из PATH
            string[] path_dirs = GetPathDirs();

            List <string> tmpdirs = new List<string>();
            tmpdirs.Add(curdir);

            foreach (string dir in path_dirs)
            {
                if (dir.Trim() != string.Empty)
                {
                    tmpdirs.Add(dir.Trim());
                }
            }

            FindDirs = tmpdirs.ToArray();
        }

        private static bool IsExecutable(FileInfo fi)
        {
            //имя кончается на . - точно не исполняемый
            if (fi.Name.EndsWith(".")) return false;
            
            foreach (string ext in Extensions)
            {
                if (fi.Extension == "." + ext) return true;
            }
            return false;
        }

        private static string FindNoExt(string path, string FileName)
        {
            //имя кончается на . не ищем ничего
            if (FileName.EndsWith(".")) return null;
            string find = path;
            if (!find.EndsWith("\\")) find = find + "\\";
            find = find + FileName;

            foreach (string ext in Extensions)
            {
                string f = find + "." + ext;
                if (File.Exists(f)) return f;
            }
            return null;
        }

        public static string Find(string Command)
        {
            GetFindDirs();

            FileInfo fi = new FileInfo(Command);
            //если в команде есть \ - указан конкретный каталог
            if (Command.Contains("\\"))
            {
                if (fi.Extension == string.Empty) //расширение не указано
                {
                    return FindNoExt(fi.DirectoryName, fi.Name);
                }
                else
                {
                    if (IsExecutable(fi)) //это исполняемый файл
                    {
                        return Command;
                    }
                    //иначе это непонятно что и выполнять не надо
                }
            }
            else //надо поискать в текущем каталоге и в PATH-каталогах
            {
                string find = null;
                foreach (string dir in FindDirs)
                {
                    if (fi.Extension == string.Empty) //расширение не указано
                    {
                        find = FindNoExt(dir, fi.Name);
                        if (find != null) return find;
                    }
                    else //расширение указано
                    {
                        //вообще не исполняемый файл
                        if (!IsExecutable(fi)) return null;

                        //ищем в указанном каталоге
                        find = dir;
                        if (!find.EndsWith("\\")) find = find + "\\";
                        find = find + fi.Name;
                        if (File.Exists(find)) return find;
                        else find = null;
                    }
                }
            }

            return null;
        }
    }
}
