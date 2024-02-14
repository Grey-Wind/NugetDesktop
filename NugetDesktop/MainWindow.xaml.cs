using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using MsgBox;
using Newtonsoft.Json.Linq;

namespace NugetDesktop
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LanguageInitialize();
        }

        private void LanguageInitialize()
        {
            var selectedIndex = LanguageBox.SelectedIndex;

            if (selectedIndex == 0)
            {
                ReadJsonFile("Chinese");
            }
            else if (selectedIndex == 1)
            {
                // 执行与索引为1对应的代码
                Console.WriteLine("选中了第二个选项");
            }
            else if (selectedIndex == 2)
            {
                // 执行与索引为2对应的代码
                Console.WriteLine("选中了第三个选项");
            }
            // 默认情况下，可以执行与其他索引对应的代码
            else
            {
                Error.Msg("错误代码：001-nf|en");
            }
        }

        // 读取JSON文件内容
        private void ReadJsonFile(string fileName)
        {
            string jsonFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "language");
            string jsonFilePath = Path.Combine(jsonFolderPath, fileName + ".json");

            if (File.Exists(jsonFilePath))
            {
                string jsonContent = File.ReadAllText(jsonFilePath);
                JObject jsonObject = JObject.Parse(jsonContent);

                JObject LabelObject = (JObject)jsonObject["Label"];

                if (LabelObject != null)
                {
                    foreach (var property in LabelObject.Properties())
                    {
                        string propertyName = property.Name;
                        string propertyValue = property.Value.ToString();

                        if (FindName(propertyName) is Label label)
                        {
                            label.Content = propertyValue;
                        }
                    }
                }
            }
            else
            {
                Error.Msg("002-nf");
            }
        }

        private void LanguageBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LanguageInitialize();
        }
    }
}
