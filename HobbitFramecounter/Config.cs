using System;

namespace HobbitFramecounter
{
    class Config
    {
        public static string baseDir = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
        public static string convertedDir = baseDir + "\\Converted Videos";
        public static string ffmpegDir = baseDir + "\\FFmpeg\\bin\\ffmpeg.exe";
    }
}
