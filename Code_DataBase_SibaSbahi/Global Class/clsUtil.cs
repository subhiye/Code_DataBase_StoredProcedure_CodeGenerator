using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Code_DataBase_SibaSbahi
{
    public class clsUtil
    {
        public static void OpeningSqlApp()
        {
            // Get The Path to The Desktop 

            string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Combine The desktop Path Wit h The applcation's

            string AppPath = Path.Combine(DesktopPath, @"C:\Program Files (x86)\Microsoft SQL Server Management Studio 20\Common7\IDE\Ssms.exe");
            Process.Start(AppPath);
        }

        public static void OpeningVisualStudio()
        {
            // Get The Path to The Desktop 

            string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Combine The desktop Path Wit h The applcation's

            string AppPath = Path.Combine(DesktopPath, @"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe");
            Process.Start(AppPath);
        }

        public static void ColorText(string Text, Color color, RichTextBox rtxbox)
        {
            int Index = rtxbox.Text.IndexOf(Text);
            if (Index != -1)
            {
                int Length = Text.Length;
                rtxbox.Select(Index, Length);
                rtxbox.SelectionColor = color;
            }
        }

        public static string GetFilePath()
        {
            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string reposPath = Path.Combine(userFolder, "source", "repos");
            return reposPath;
        }
    }
}
