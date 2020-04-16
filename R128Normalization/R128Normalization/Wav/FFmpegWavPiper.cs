using System;
using System.Diagnostics;
using System.IO;

/*
 *    This file is a part of the Wav encoder/decoder utils.
 *    Copyright (C) 2020  Xuan525
 *
 *    This program is free software: you can redistribute it and/or modify
 *    it under the terms of the GNU General Public License as published by
 *    the Free Software Foundation, either version 3 of the License, or
 *    (at your option) any later version.
 *
 *    This program is distributed in the hope that it will be useful,
 *    but WITHOUT ANY WARRANTY; without even the implied warranty of
 *    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *    GNU General Public License for more details.
 *
 *    You should have received a copy of the GNU General Public License
 *    along with this program.  If not, see <https://www.gnu.org/licenses/>.
 *    
 *    Email : shanboxuan@me.com
 *    Github : https://github.com/xuan525
*/

namespace Wav
{
    public class FFmpegWavPiper
    {
        public class FFmepgNotFoundException : Exception { }
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

            try
            {
                process.Start();
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                if(ex.NativeErrorCode == 2)
                {
                    throw new FFmepgNotFoundException();
                }
                else
                {
                    throw;
                }
            }

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
