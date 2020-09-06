using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using ListBox = System.Windows.Controls.ListBox;
using System.Threading;

namespace mkv2mp4
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private ObservableCollection<MkvFile> fileToConvert = new ObservableCollection<MkvFile>();

        private MkvInfo videoInfo;

        //文件保存目录
        private string fileDirectory;

        public MainWindow()
        {
            InitializeComponent();
            fileToConvert.CollectionChanged += FileToConvert_CollectionChanged;
        }

        private void FileToConvert_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            fileCountTextBlock.Text = fileToConvert.Count.ToString();
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
            
            for (int i = 0; i<fileToConvert.Count; i++)
            {
                string fullPathName = Path.Combine(fileDirectory, fileToConvert[i].FileName);
                int result = MkvConvert.TryGetMkvInfo(fullPathName, out MkvInfo info);
                if (result == 0)
                {
                    videoInfo = videoInfo ?? info;
                    int res = MkvConvert.ConvertMkv2Mp4(fullPathName, videoInfo);
                    Debug.WriteLine("convert result: " + res);
                }
            }
        }

        private void FileList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ListView listView = e.OriginalSource as System.Windows.Controls.ListView;

            int index = listView.SelectedIndex;
            if (index != -1)
            {
                MkvConvert.TryGetMkvInfo(Path.Combine(fileDirectory, fileToConvert[index].FileName), out videoInfo);

                foreach(TrackInfo track in videoInfo.Tracks)
                {
                    track.PropertyChanged += Track_PropertyChanged;
                }
                trackInfoView.ItemsSource = videoInfo.Tracks;
            }
        }

        private void Track_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            trackInfoView.ItemsSource = videoInfo.Tracks;
        }
    }

    public class MkvFile
    {
        public int Num { get; set; }
        public string FileName { get; set; }
    }
}
