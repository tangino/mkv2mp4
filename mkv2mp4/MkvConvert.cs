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

        /// <summary>
        /// 尝试获取MKV视频文件的信息，如果参数文件不是MKV文件，返回值是一个非0的值
        /// </summary>
        /// <param name="fileName">待转换的MKV视频文件</param>
        /// <param name="info">获取到的MKV视频文件信息</param>
        /// <returns></returns>
        public static int TryGetMkvInfo(string fileName, out MkvInfo info)
        {
            info = new MkvInfo();
            ParseMkvInfo infoParser = new ParseMkvInfo();
            var mkvInfoProcess = new Process
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
                mkvInfoProcess.Start();

                infoParser.Parse(mkvInfoProcess.StandardOutput, out info);

                mkvInfoProcess.WaitForExit(5000);
                if (mkvInfoProcess.HasExited)
                {
                    return mkvInfoProcess.ExitCode;
                }

                mkvInfoProcess.Kill();
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
