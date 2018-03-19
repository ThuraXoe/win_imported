using IWshRuntimeLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace Win2._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //CheckStartUpIn2();

            //string path = "C:\\Evantek\\Move\\Run.bat";
            //Process process = new Process()
            //{
            //    StartInfo = new ProcessStartInfo(path)
            //    {
            //        WorkingDirectory = Path.GetDirectoryName(path)
            //    }
            //};
            //process.Start();
            //process.WaitForExit();

            string displaycontroller_path = "C:\\Evantek\\DBSController\\Evantek_Display.exe";
            if (System.IO.File.Exists(displaycontroller_path))
            {
                Process displaycontroller_process = new Process()
                {
                    StartInfo = new ProcessStartInfo(displaycontroller_path)
                    {
                        WorkingDirectory = Path.GetDirectoryName(displaycontroller_path)
                    }
                };
                displaycontroller_process.Start();
                displaycontroller_process.WaitForExit();
            }
        }

        private void CheckStartUpIn2()
        {
            string allusuerstr = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Microsoft\Windows\Start Menu\Programs\Startup";
            string userstr = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            MessageBox.Show(allusuerstr);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //RemoveFolder(@"C:\Move");

            //CheckShorutcut();

            Process[] processes_move = Process.GetProcessesByName("AutoIt3");
            foreach (Process process in processes_move)
            {
                process.Kill();
                process.WaitForExit();
            }
        }
        private void RemoveFolder(string path)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo mainfolder = new DirectoryInfo(path);
                DirectoryInfo[] subfolder = mainfolder.GetDirectories();

                foreach (var folder in subfolder)
                {
                    this.RemoveFolder(folder.FullName);
                }

                FileInfo[] files = mainfolder.GetFiles();
                foreach (var file in files)
                {
                    file.Delete();
                }
                Directory.Delete(path);
            }
        }
        private void CheckShorutcut()
        {
            string alluserstartup = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Microsoft\Windows\Start Menu\Programs\Startup";
            string userstartup = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            List<string> lststrup = new List<string>() { alluserstartup, userstartup };
            List<string> lstshortcut = new List<string>() {
                "Run.lnk", "Evantek_Display.lnk","Evantek_Display.exe - Shortcut.lnk","Evantek_Display - Shortcut.lnk","Evantek_Display.exe","Run.bat - Shortcut.lnk","Run - Shortcut.lnk","Run.bat","Delay_Launcher.exe - Shortcut.lnk","Delay_Launcher - Shortcut.lnk","Delay_Launcher.exe"
            };

            int count = 0;
            foreach (var str in lststrup)
            {
                foreach (var shortcut in lstshortcut)
                {
                    string shorcut_path = Path.Combine(str, shortcut);
                    if (System.IO.File.Exists(shorcut_path))
                    {
                        count = count + 1;
                    }
                    //System.IO.File.Delete(str + "\\" + shortcut);
                }
            }

            MessageBox.Show(count.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string shortcut_file_path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Microsoft\Windows\Start Menu\Programs\Startup" + "\\test.lnk";
            string soruce_file_path = @"C:\sfp\a.txt";
            WshShellClass WshShell = new WshShellClass();
            IWshShortcut shortcut = (IWshShortcut)WshShell.CreateShortcut(shortcut_file_path);
            shortcut.TargetPath = soruce_file_path;
            shortcut.WorkingDirectory = @"C:\sfp\";
            //shortcut.Description = "EQMS Display Controller";
            //shortcut.IconLocation = @"icon path";
            shortcut.Save();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(IsUserAdministrator().ToString());
            ClearRegisteryUninstallHistory();
            string UninstallCommandString = "/x {0} /qn";

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            process.StartInfo = startInfo;

            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;

            startInfo.FileName = "msiexec.exe";
            startInfo.Arguments = string.Format(UninstallCommandString, "{B4034ED9-0C93-4BAB-9F92-521163F064AD}");

            process.Start();



            //TerminateAllProcess();
            //ClearRegisteryUninstallHistory();
        }
        private void ClearRegisteryUninstallHistory()
        {
            const string Win32Loc = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            const string Win64Loc = @"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

            RegistryKey rk32 = Registry.LocalMachine.OpenSubKey(Win32Loc, true);
            RegistryKey rk64 = Registry.LocalMachine.OpenSubKey(Win64Loc, true);

            string DisplayNameValue = "EQMS";

            if (rk64 != null)
            {
                using (RegistryKey rk = rk64)
                {
                    foreach (string skName in rk.GetSubKeyNames())
                    {
                        using (RegistryKey sk = rk.OpenSubKey(skName, false))
                        {
                            if (sk.GetValue("DisplayName") != null && Convert.ToString(sk.GetValue("DisplayName")).StartsWith(DisplayNameValue))
                            {
                                MessageBox.Show("productcode " + skName);
                                //rk.DeleteSubKey(skName);
                                break;
                            }
                        }
                    }
                }
            }

            if (rk32 != null)
            {
                using (RegistryKey rk = rk32)
                {
                    foreach (string skName in rk.GetSubKeyNames())
                    {
                        using (RegistryKey sk = rk.OpenSubKey(skName, false))
                        {
                            if (sk.GetValue("DisplayName") != null && Convert.ToString(sk.GetValue("DisplayName")).StartsWith(DisplayNameValue))
                            {
                                //rk.DeleteSubKey(skName);
                                MessageBox.Show("productcode " + skName);
                                break;
                            }
                        }
                    }
                }
            }
            MessageBox.Show("Done Registery Clean");
        }

        private void TerminateAllProcess()
        {
            //terminate controller process
            Process[] processes_controller = Process.GetProcessesByName("Evantek_Display");
            foreach (Process process in processes_controller)
            {
                process.Kill();
                process.WaitForExit();
            }

            //terminate mouse move process
            Process[] processes_move = Process.GetProcessesByName("AutoIt3");
            foreach (Process process in processes_move)
            {
                process.Kill();
                process.WaitForExit();
            }

            //terminate delaylauncher process
            Process[] processes_delay = Process.GetProcessesByName("Delay_Launcher");
            foreach (Process process in processes_delay)
            {
                process.Kill();
                process.WaitForExit();
            }
            MessageBox.Show("Done Terminate");
        }

        public bool IsUserAdministrator()
        {
            bool isAdmin;
            try
            {
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException ex)
            {
                isAdmin = false;
            }
            catch (Exception ex)
            {
                isAdmin = false;
            }
            return isAdmin;
        }

    }
}
