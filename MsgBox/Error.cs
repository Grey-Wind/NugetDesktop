using System;
using System.Windows;
using ColorfulConsole;

namespace MsgBox
{
    public class Error
    {
        readonly private static string e = "错误代码：";

        public static void Msg(string eCode) {
            MessageBox.Show(e + eCode, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void Print(string eCode)
        {
            Colorful.Print(e + eCode, ConsoleColor.Red);
        }
    }
}
