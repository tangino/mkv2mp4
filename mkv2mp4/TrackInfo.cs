using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;

namespace mkv2mp4
{
    public class TrackInfo : INotifyPropertyChanged
    {
        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (value != isEnabled)
                {
                    isEnabled = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string TrackNum { get; set; }

        public string Resolution { get; set; }

        public string TrackType { get; set; }

        public string CodecID { get; set; }

        public string TrackLanguage { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("TrackNum: " + TrackNum);
            stringBuilder.AppendLine("Resolution: " + Resolution);
            stringBuilder.AppendLine("TrackType: " + TrackType);
            stringBuilder.AppendLine("CodeID: " + CodecID);
            stringBuilder.AppendLine("TrackLanguage: " + TrackLanguage);
            return stringBuilder.ToString();
        }
    }
}