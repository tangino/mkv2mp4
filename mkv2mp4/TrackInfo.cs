using System.Text;

namespace mkv2mp4
{
    public class TrackInfo
    {
        public string TrackNum { get; set; }

        public string Resolution { get; set; }

        public string TrackType { get; set; }

        public string CodeID { get; set; }

        public string TrackLanguage { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("TrackNum: " + TrackNum);
            stringBuilder.AppendLine("Resolution: " + Resolution);
            stringBuilder.AppendLine("TrackType: " + TrackType);
            stringBuilder.AppendLine("CodeID: " + CodeID);
            stringBuilder.AppendLine("TrackLanguage: " + TrackLanguage);
            return stringBuilder.ToString();
        }
    }
}