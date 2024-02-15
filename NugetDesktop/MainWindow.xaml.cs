using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MsgBox;
using Newtonsoft.Json.Linq;
using ShellLibrary;

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
            ChangeLanguage();
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

                JObject BtnObject = (JObject)jsonObject["Button"];

                if (BtnObject != null)
                {
                    foreach (var property in BtnObject.Properties())
                    {
                        string propertyName = property.Name;
                        string propertyValue = property.Value.ToString();

                        if (FindName(propertyName) is Button button)
                        {
                            button.Content = propertyValue;
                        }
                    }
                }
            }
            else
            {
                Error.Msg("002-nf");
            }
        }

        private async void Push(object sender, RoutedEventArgs e)
        {
            string empty = " ";
            string FolderPath = FolderPathTBox.Text;
            string Api = ApiKeyTBox.Text;
            string FileName = FileNameTBox.Text;
            string SourceLink = "https://api.nuget.org/v3/index.json";
            string FilePath = FolderPath + "/" + FileName;

            // 判断输入参数是否有空值
            if (string.IsNullOrEmpty(FolderPath) || string.IsNullOrEmpty(Api) || string.IsNullOrEmpty(FileName))
            {
                // 有空值，进行错误处理
                Error.Msg("003-argu/e\n输入参数不能为空");
            }
            else if (!System.IO.File.Exists(FilePath))
            {
                // 测试准备发布的文件是否有基础问题
                Error.Msg("003-fnd\n准备发布的文件不存在！");
            }
            else if (!FileName.EndsWith(".nupkg", StringComparison.OrdinalIgnoreCase))
            {
                // 测试准备发布的文件是否为 .nupkg 文件
                Error.Msg("003-few\n准备发布的文件后缀名错误，即不为 .nupkg");
            }
            else
            {
                string setKey = "cd /d" + empty + "nuget" + empty + "&&" + empty + "nuget.exe setApiKey" + empty + Api;

                string push = "cd /d" + empty + FolderPath + empty + "&&" + empty + "dotnet" + empty + "push" + FileName + "-ApiKey" + Api + "-Source" + empty + SourceLink;

                await Task.Run(() =>
                {
                    Shell.RunCommand(setKey);
                    Shell.RunCommand(push, "1", "true", true, false, false);
                });
            }
        }
    }
}
