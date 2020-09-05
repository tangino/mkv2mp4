using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace mkv2mp4
{
    class MkvConvert
    {
        private static readonly string EXTRACT_APP_NAME = "mkvextract.exe";
        private static readonly string INFO_APP_NAME = "mkvinfo.exe";
        private static readonly string COMBINE_APP_NAME = "mp4box.exe";
        private static readonly int ERR_EXIT_CODE = 0x10086;

        public static int ParseMkvInfo(string fileName, out List<string> info)
        {
            info = new List<string>();
            var mkvInfo = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = INFO_APP_NAME,
                    Arguments = fileName,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            try
            {
                mkvInfo.Start();
                while (!mkvInfo.StandardOutput.EndOfStream)
                {
                    string line = mkvInfo.StandardOutput.ReadLine();
                    info.Add(line);
                }

                mkvInfo.WaitForExit(5000);
                if (mkvInfo.HasExited)
                {
                    return mkvInfo.ExitCode;
                }

                mkvInfo.Kill();
                return ERR_EXIT_CODE;
            }
            catch (Exception e)
            {
                Debug.WriteLine("error occured in ParseMkvInfo method: {0} ", e.Message);
                return ERR_EXIT_CODE;
            }
        }

        public static void ConvertMkv2Mp4(string fileName)
        {
            ExtractMkv(fileName);
            RePackageMp4(fileName);
        }

        private static void ExtractMkv(string fileName)
        {

        }

        private static void RePackageMp4(string fileName)
        {

        }
    }
}
