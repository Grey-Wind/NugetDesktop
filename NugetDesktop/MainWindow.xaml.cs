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
            ChangeLanguage();
        }

        private void LanguageBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeLanguage(); // 使用当前的 MainWindow 实例来初始化语言设置
        }

        private void ChangeLanguage()
        {
            var selectedIndex = LanguageBox.SelectedIndex;
            
            // 简体中文
            if (selectedIndex == 0)
            {
                ReadJsonFile("SimplifiedChinese");
            }
            // 繁体中文
            else if (selectedIndex == 1)
            {
                ReadJsonFile("TraditionalChinese");
            }
            // 英文
            else if (selectedIndex == 2)
            {
                ReadJsonFile("English");
            }
            // 报错
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

                JObject MwObject = (JObject)jsonObject["Mw"];

                if (MwObject != null)
                {
                    foreach (var property in MwObject.Properties())
                    {
                        string propertyName = property.Name;
                        string propertyValue = property.Value.ToString();

                        if (propertyName == "Title")
                        {
                            Mw.Title = propertyValue;
                        }
                    }
                }
            }
            else
            {
                Error.Msg("002-nf");
            }
        }
    }
}
