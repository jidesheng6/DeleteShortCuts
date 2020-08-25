using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace DeleteShorts
{
    class RkeyMain
    {

        private void GetPathName(out string SystemDir, out string UProfile)
        {
            string SystemRootDir, UserProfileDir;
            SystemRootDir = Environment.GetEnvironmentVariable("systemroot");
            UserProfileDir = Environment.GetEnvironmentVariable("userprofile");
            SystemDir = SystemRootDir;
            UProfile = UserProfileDir;
        }
        public void RegIstryMethod()
        {
            string Shorts_Path = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Shell Icons";
            RegistryKey Rkey0 = Registry.LocalMachine.OpenSubKey(Shorts_Path,true);
            if (Rkey0 == null)
            {
                MessageBox.Show("系统检测到您的计算机注册表内没有Shell Icons项,将为您的计算机添加注册表项目，若发生杀毒软件提示，属于正常现象，请悉知。","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                try
                {
                    RegistryKey Rkey1 = Registry.LocalMachine.CreateSubKey(Shorts_Path, true);
                    if (Rkey1 != null)
                    {
                        string SystmDir, Udir;
                        GetPathName(out SystmDir, out Udir);
                        string Short_Value = $@"{SystmDir}\system32\imageres.dll,197";
                        Rkey1.SetValue("29", Short_Value, RegistryValueKind.String);
                        SetFileAttr();
                        RestartExplorer();

                    }
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("请使用管理员身份运行","发生权限读取错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    Application.Exit();
                }
                
            }
            else
            {
                if (Rkey0.GetValueNames().Contains("29") == true)
                {
                    MessageBox.Show("系统检测到您的计算机中已经被设置了去除快捷方式箭头，请不要再试", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Application.Exit();
                    
                }
                else
                {
                    string SystmDir, Udir;
                    GetPathName(out SystmDir, out Udir);
                    string Short_Value = $@"{SystmDir}\system32\imageres.dll,197";
                    Rkey0.SetValue("29", Short_Value,RegistryValueKind.String);
                    MessageBox.Show("系统检测到您的设置不正确，已经将您的设置重新引导","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    SetFileAttr();
                    RestartExplorer();
                   
                }
                
            }
            
        }
        private void SetFileAttr()
        {

            string SystmDir, Udir;
            GetPathName(out SystmDir, out Udir);
            string IconCacheDir = $@"{Udir}\AppData\Local\iconcache.db";
            bool FileIsExit = File.Exists(IconCacheDir);
            if (FileIsExit == true)
            {
                File.SetAttributes(IconCacheDir,FileAttributes.Normal);
                File.Delete(IconCacheDir);
            }
            else
            {
                
                
            }
        }
        private void RestartExplorer()
        {
            string ProcessName = "explorer";
            foreach (Process Pname in Process.GetProcessesByName(ProcessName))
            {
                if (Pname == null)
                {
                    MessageBox.Show("您的桌面进程已结束，系统将启动您的桌面进程", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Process.Start("explorer.exe");
                }
                else
                {
                    Pname.Kill();
                    Application.Exit();
                }
            }
        }
    }
}
