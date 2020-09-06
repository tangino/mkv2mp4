using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mkv2mp4
{
    class ParseMkvInfo
    {
        public void Parse(StreamReader reader, out MkvInfo mkvInfo)
        {
            mkvInfo = new MkvInfo();

            string line;

            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                string[] lines = line.Trim(new char[] { ' ', '|', '+' }).Split(new char[] { ':' });

                if (lines[0].StartsWith("Title"))
                {
                    mkvInfo.Title = lines[1].Trim();
                }
                else if (lines[0].StartsWith("Duration"))
                {
                    string temp = string.Format("{0}:{1}:{2}", lines[1], lines[2], lines[3]);
                    mkvInfo.Duration = temp.Split(new char[] { '.' })[0].Trim();
                }
                ParseTrackInfo(lines, mkvInfo);
            }
        }

        private void ParseTrackInfo(string[] lines, MkvInfo mkvInfo)
        {
            int index = mkvInfo.Tracks.Count - 1;

            //parse track num
            if (lines[0].StartsWith("Track number"))
            {
                TrackInfo trackInfo = new TrackInfo
                {
                    IsEnabled = true,
                };
                string tempNum = (lines[1].Split(new char[] { '(' })[0].Trim());
                trackInfo.TrackNum = (int.Parse(tempNum) - 1).ToString();
                mkvInfo.Tracks.Add(trackInfo);
            }

            //parse track type
            if (lines[0].StartsWith("Track type"))
            {
                switch (lines[1].Trim())
                {
                    case "video":
                        mkvInfo.Tracks[index].TrackType = "video";
                        break;

                    case "audio":
                        mkvInfo.Tracks[index].TrackType = "audio";
                        break;

                    case "subtitles":
                        mkvInfo.Tracks[index].TrackType = "subtitles";
                        break;
                }
            }

            //parse resolution
            if (lines[0].StartsWith("Pixel width"))
            {
                mkvInfo.Tracks[index].Resolution = lines[1].Trim();
            }
            else if (lines[0].StartsWith("Pixel height"))
            {
                mkvInfo.Tracks[index].Resolution = mkvInfo.Tracks[index].Resolution + "x" + lines[1].Trim();
            }

            //parse codec id
            if (lines[0].StartsWith("Codec ID"))
            {
                mkvInfo.Tracks[index].CodecID = lines[1];
            }

            //parse language
            if (lines[0].StartsWith("Language"))
            {
                mkvInfo.Tracks[index].TrackLanguage = lines[1];
            }
        }
    }
}
