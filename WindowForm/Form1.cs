using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ServiceState s = ServiceInstaller.GetServiceStatus("BCS");
            if (s.ToString() == "NotFound")
            {
                //register and start
            }
            else if (s.ToString() == "Running")
            {
                //stop 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Evantek_Display
            //AutoIt3
            Process[] processes = Process.GetProcessesByName("Delay_Launcher");
            foreach (Process process in processes)
            {
                process.Kill();
                process.WaitForExit();
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("Evantek_Display");
            foreach (Process process in processes)
            {
                process.Kill();
                process.WaitForExit();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process[] lstprocess = Process.GetProcesses();
            foreach (Process p in lstprocess)
            {
                label1.Text += p.ProcessName + "\r\n";
            }
            ReplaceController();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Process.Start("C:\\Custom\\WindowForm.exe");
        }
        private void DeleteFile(FileInfo[] Files)
        {
            foreach (FileInfo file in Files)
            {
                file.Delete();
            }
        }
        private void DeleteDirectory(DirectoryInfo[] Directories)
        {
            foreach (DirectoryInfo dir in Directories)
            {
                dir.Delete(true);
            }
        }
        private void CleanController()
        {
            string path = "C:/JInstaller/DBSContoller";
            if (Directory.Exists(path))
            {
                DirectoryInfo di = new DirectoryInfo(path);
                DeleteFile(di.GetFiles());
                DeleteDirectory(di.GetDirectories());
                //foreach (FileInfo file in di.GetFiles())
                //{
                //    file.Delete();
                //}

                //foreach (DirectoryInfo dir in di.GetDirectories())
                //{
                //    dir.Delete(true);
                //}
                Directory.Delete(path);
            }
        }
        private void ReplaceController()
        {
            string source = "C:/cd/DBSContoller";
            string destpath = "C:/dest/DBSContoller";
            string backupdirectory = @"C:\Backup";



            //if (Directory.Exists(destpath))
            //{
            //    DirectoryInfo di = new DirectoryInfo(destpath);
            //    foreach (FileInfo file in di.GetFiles())
            //    {
            //        file.Delete();
            //    }
            //    foreach (DirectoryInfo dir in di.GetDirectories())
            //    {
            //        dir.Delete(true);
            //    }
            //    Directory.Delete(destpath);
            //}

            if (Directory.Exists(destpath))
            {

                //Directory.Move(destpath, backupdirectory + DateTime.Now.ToString("_MMMdd_yyyy_HHmmss"));
                //Directory.Move(source, destpath);

                foreach (var file in Directory.GetFiles(source))
                {
                    File.Copy(file, Path.Combine(destpath, Path.GetFileName(file)), false);
                }


            }
        }

        public void Test(dynamic obj)
        {
            string str = "";
            Form1 frm = new Form1();
            this.Test(str);
            this.Test(frm);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //RemoveFolder("C:/Evantek");
            RemoveSubDirectory("C:/Evantek",false);
            //CopyFolder(@"C:\WorkSheet\InstallerSheet\NeedFilesandFolderInInstaller\Collection DBS Controller\LocalDBSController\19.10.2017", @"C:\Evantek");
        }
        private void RemoveFolder(string path)
        {
            DirectoryInfo mainfolder = new DirectoryInfo(path);
            DirectoryInfo[] subfolder = mainfolder.GetDirectories().ToArray();

            foreach (var folder in subfolder)
            {
                this.RemoveFolder(folder.FullName);
            }

            FileInfo[] files = mainfolder.GetFiles().ToArray();
            foreach (var file in files)
            {
                file.Delete();
            }
            Directory.Delete(path);

        }
        private void RemoveSubDirectory(string path,bool issub)
        {
            DirectoryInfo mainfolder = new DirectoryInfo(path);
            DirectoryInfo[] subfolder = mainfolder.GetDirectories().ToArray();

            foreach (var folder in subfolder)
            {
                this.RemoveSubDirectory(folder.FullName,true);
            }

            if (issub)
            {
                FileInfo[] files = mainfolder.GetFiles().ToArray();
                foreach (var file in files)
                {
                    file.Delete();
                }
                Directory.Delete(path);
            }
        }
        private void CopyFolder2(DirectoryInfo source, DirectoryInfo target, bool isRoot)
       {
            foreach (DirectoryInfo dir in source.GetDirectories())
                CopyFolder2(dir, target.CreateSubdirectory(dir.Name), false);
            if (!isRoot)
            {
                foreach (FileInfo file in source.GetFiles())
                    file.CopyTo(Path.Combine(target.FullName, file.Name), true);
            }
        }
        private void CopyFolder(string SourcePath, string DestinationPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);


        }

        private void button7_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(this.GetUninstallString("EQMS Display Controller - 1.0.32"));
            //regtest2();
            regRefracted();
        }
        public void regtest2()
        {
            const string Win32Loc = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
                                    
            const string Win64Loc = @"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

            List<Application> apps = new List<Application>();
            //string location = bool64BitOs ? Win64Loc : Win32Loc;
            string location = Win64Loc;

            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(location,false))
            {
                foreach (string skName in rk.GetSubKeyNames())
                {
                    
                    using (RegistryKey sk = rk.OpenSubKey(skName,false))
                    {
                        //if (sk.GetValue("DisplayName") != null && Convert.ToString(sk.GetValue("DisplayName"))== "EQMS Display Controller - 1.0.32")
                        if (sk.GetValue("DisplayName") != null && Convert.ToString(sk.GetValue("DisplayName")).Contains("EQMS Display Controller -") )
                        {
                            
                            string s = sk.GetValue("DisplayName").ToString();
                            //rk.DeleteValue(skName);
                            break;

                        }
                        //apps.Add(new Application { DisplayName = sk.GetValue("DisplayName") == null ? "" : (string)sk.GetValue("DisplayName"), AppKey = skName });
                    }
                }
            }

        }
        private void regRefracted()
        {
            const string Win32Loc = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            const string Win64Loc = @"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

            RegistryKey rk32 = Registry.LocalMachine.OpenSubKey(Win32Loc,true);
            RegistryKey rk64 = Registry.LocalMachine.OpenSubKey(Win64Loc,true);

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

                                string s = sk.GetValue("DisplayName").ToString();
                                rk.DeleteSubKey(skName);
                                //rk.DeleteValue(skName);
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
                                string s = sk.GetValue("DisplayName").ToString();
                                rk.DeleteSubKey(skName);
                                //rk.DeleteValue(skName);
                                break;
                            }
                        }
                    }
                }
            }

        }
        public void deleteregistersetting()
        {
            //try
            //{
            //    RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            //    if (rkApp.GetValue("EQMS Display Controller - 1.0.32") != null)
            //    {
            //        string s = "ok";
            //        //rkApp.DeleteValue("Steve");
            //        //rkApp.DeleteSubKey("Steve", false);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    string msg = ex.Message.ToString();
            //}
        }
        private string GetUninstallString(string msiName)
        {
            //Utility.WriteLog("Entered GetUninstallString(msiName) - Parameters: msiName = " + msiName);
            string uninstallString = string.Empty;
            try
            {
                //string path = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Installer\\UserData\\S-1-5-18\\Products";
                string path = "SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{3484D541-E9A8-4455-A1C3-93686961D347}";
                RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(path);
                //RegistryKey key = Registry.LocalMachine.OpenSubKey(path);

                

                foreach (string tempKeyName in key.GetSubKeyNames())
                {
                    RegistryKey tempKey = key.OpenSubKey(tempKeyName + "\\InstallProperties");
                    if (tempKey != null)
                    {
                        //string s= Convert.ToString(tempKey.GetValue("EQMS Display Controller - 1.0.32"));
                        if (string.Equals(Convert.ToString(tempKey.GetValue("EQMS Display Controller - 1.0.32")), msiName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            uninstallString = Convert.ToString(tempKey.GetValue("UninstallString"));
                            uninstallString = uninstallString.Replace("/I", "/X");
                            uninstallString = uninstallString.Replace("MsiExec.exe", "").Trim();
                            uninstallString += " /quiet /qn";
                            break;
                        }
                    }
                }

                return uninstallString;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DirectoryInfo source = new DirectoryInfo(@"C:\TestDest");
            DirectoryInfo target = new DirectoryInfo(@"C:\Evantek");
            CopyFolder2(source,target,true);
        }
    }
}
