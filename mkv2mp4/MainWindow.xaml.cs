using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using System.Collections.ObjectModel;

namespace mkv2mp4
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private ObservableCollection<MkvFile> fileToConvert = new ObservableCollection<MkvFile>();

        private string filePath;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BatchMode_Checked(object sender, RoutedEventArgs e)
        {
            if (batchMode.IsChecked.Value)
                addButton.Content = "添加目录";
            else
                addButton.Content = "添加文件";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string[] tempName = { };
            if (batchMode.IsChecked.Value)
            {
                //添加目录
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        filePath = fbd.SelectedPath;
                        tempName = Directory.GetFiles(fbd.SelectedPath);  
                    }
                }
            }
            else
            {
                //添加文件
                OpenFileDialog fileDialog = new OpenFileDialog();
                if (fileDialog.ShowDialog() == true)
                {
                    filePath = Path.GetDirectoryName(fileDialog.FileName);
                    tempName = new string[1];
                    tempName[0] = fileDialog.FileName;  //fileDialog.FileName包含目录部分
                }
            }
            ExtractFileName(tempName);
            fileList.ItemsSource = fileToConvert;
        }

        private void ExtractFileName(string[] tempName)
        {
            fileToConvert.Clear();
            
            if (tempName.Length != 0)
            {
                for(int i = 0;i <tempName.Length; i++)
                {
                    MkvFile file = new MkvFile
                    {
                        Num = i + 1,
                        FileName = Path.GetFileName(tempName[i])
                    };
                    fileToConvert.Add(file);
                }
            }
        }

        private void ExecuteTask_Click(object sender, RoutedEventArgs e)
        {
            List<string> info;
            foreach(MkvFile file in fileToConvert)
            {
                string fullPath = Path.Combine(filePath, file.FileName);
                if (MkvConvert.ParseMkvInfo(fullPath, out info) != 0) //not a mkv file 
                {

                }
                else
                {

                }
            }
        }
    }

    class MkvFile
    {
        public int Num { get; set; }
        public string FileName { get; set; }
    }
}
