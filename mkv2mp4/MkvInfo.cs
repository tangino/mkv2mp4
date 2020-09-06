using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mkv2mp4
{
    class MkvInfo
    {
        public string Title { get; set; }

        public string Duration { get; set; }

        public ObservableCollection<TrackInfo> Tracks { get; set; } = new ObservableCollection<TrackInfo>();

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("Title: {0}, Duration: {1}\n", Title, Duration);
            foreach(TrackInfo track in Tracks)
            {
                stringBuilder.AppendLine(track.ToString());
            }
            return stringBuilder.ToString();
        }
    }
}
