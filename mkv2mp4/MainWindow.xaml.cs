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


        //文件保存目录
        private string fileDirectory;

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
                        fileDirectory = fbd.SelectedPath;
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
                    fileDirectory = Path.GetDirectoryName(fileDialog.FileName);
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
            for(int i = 0; i<fileToConvert.Count; i++)
            {
                MkvConvert.TryGetMkvInfo(Path.Combine(fileDirectory, fileToConvert[i].FileName), out MkvInfo info);
                Debug.WriteLine(info.Title);
                Debug.WriteLine(info.Duration);
            }
        }
    }

    class MkvFile
    {
        public int Num { get; set; }
        public string FileName { get; set; }
    }
}
