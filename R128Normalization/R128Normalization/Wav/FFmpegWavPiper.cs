using System;
using System.Diagnostics;
using System.IO;

namespace Wav
{
    public class FFmpegWavPiper
    {
        public static Stream GetF64WavStream(string fileName, int sampleRate, DataReceivedEventHandler logReceivedHandler)
        {
            Process process = CreateWorkingProcess("ffmpeg.exe", string.Format("-i \"{0}\" -ar {1} -acodec pcm_f64le -f wav -", fileName, sampleRate), null, logReceivedHandler, null);
            return process.StandardOutput.BaseStream;
        }

        public static Stream GetF64WavStream(string fileName, DataReceivedEventHandler logReceivedHandler)
        {
            Process process = CreateWorkingProcess("ffmpeg.exe", string.Format("-i \"{0}\" -acodec pcm_f64le -f wav -", fileName), null, logReceivedHandler, null);
            return process.StandardOutput.BaseStream;
        }

        public static Stream GetS64WavStream(string fileName, int sampleRate, DataReceivedEventHandler logReceivedHandler)
        {
            Process process = CreateWorkingProcess("ffmpeg.exe", string.Format("-i \"{0}\" -ar {1} -acodec pcm_s64le -f wav -", fileName, sampleRate), null, logReceivedHandler, null);
            return process.StandardOutput.BaseStream;
        }

        public static Stream GetS64WavStream(string fileName, DataReceivedEventHandler logReceivedHandler)
        {
            Process process = CreateWorkingProcess("ffmpeg.exe", string.Format("-i \"{0}\" -acodec pcm_s64le -f wav -", fileName), null, logReceivedHandler, null);
            return process.StandardOutput.BaseStream;
        }

        public static Stream GetF32WavStream(string fileName, int sampleRate, DataReceivedEventHandler logReceivedHandler)
        {
            Process process = CreateWorkingProcess("ffmpeg.exe", string.Format("-i \"{0}\" -ar {1} -acodec pcm_f32le -f wav -", fileName, sampleRate), null, logReceivedHandler, null);
            return process.StandardOutput.BaseStream;
        }

        public static Stream GetF32WavStream(string fileName, DataReceivedEventHandler logReceivedHandler)
        {
            Process process = CreateWorkingProcess("ffmpeg.exe", string.Format("-i \"{0}\" -acodec pcm_f32le -f wav -", fileName), null, logReceivedHandler, null);
            return process.StandardOutput.BaseStream;
        }

        public static Stream GetS32WavStream(string fileName, int sampleRate, DataReceivedEventHandler logReceivedHandler)
        {
            Process process = CreateWorkingProcess("ffmpeg.exe", string.Format("-i \"{0}\" -ar {1} -acodec pcm_s32le -f wav -", fileName, sampleRate), null, logReceivedHandler, null);
            return process.StandardOutput.BaseStream;
        }

        public static Stream GetS32WavStream(string fileName, DataReceivedEventHandler logReceivedHandler)
        {
            Process process = CreateWorkingProcess("ffmpeg.exe", string.Format("-i \"{0}\" -acodec pcm_s32le -f wav -", fileName), null, logReceivedHandler, null);
            return process.StandardOutput.BaseStream;
        }

        public static Process CreateWorkingProcess(string fileName, string arguments, DataReceivedEventHandler outputDataReceivedHandler, DataReceivedEventHandler errorDataReceivedHandler, EventHandler exitedHandler)
        {
            Process process = new Process();
            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            if (outputDataReceivedHandler != null)
            {
                process.OutputDataReceived += outputDataReceivedHandler;
            }

            if (errorDataReceivedHandler != null)
            {
                process.ErrorDataReceived += errorDataReceivedHandler;
            }

            if (exitedHandler != null)
            {
                process.EnableRaisingEvents = true;
                process.Exited += exitedHandler;
            }

            process.Start();

            if (outputDataReceivedHandler != null)
            {
                process.BeginOutputReadLine();
            }

            if (errorDataReceivedHandler != null)
            {
                process.BeginErrorReadLine();
            }

            return process;
        }

    }

}
