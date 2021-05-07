using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace HandWriting_output
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length==0)
            {
                Application.Run(new Form1());
            }
            else if(args.Length==1)
            {
                if(args[0]=="/?" || args[0]=="/h")
                {
                    help();
                }
                else
                {
                    help();
                }
                
            }
            else if(args.Length==2)
            {
                if (args[0] == "/s")
                {
                    Application.Run(new Form1(args[1],false));
                }
                else if(args[0]=="/f")
                {
                    if(System.IO.File.Exists(args[1]))
                    {
                        Application.Run(new Form1(args[1], true));
                    }
                    else
                    {
                        Console.Write("文件不存在。\r\n");
                    }
                }
                else
                {
                    help();
                }
            }
            else
            {
                help();
            }
            
        }

        static void help()
        {
            Process.Start("cmd.exe", "/c echo 用法: " + System.Diagnostics.Process.GetCurrentProcess().ProcessName + " [/s \"字符串\"] [/f \"文件名\"] [/? | /h]&echo.&echo     没有参数   运行程序。&echo     /?	显示帮助。&echo     /h	显示帮助。&echo     /s \"字符串\"  转换指定的字符串为手写体。&echo     /f \"文件名\"  转换这个文件的内容为手写体。必须是文本文件。&pause");
        }
    }
}
