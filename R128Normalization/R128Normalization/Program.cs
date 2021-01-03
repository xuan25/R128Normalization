using R128;
using System;
using System.IO;

/*
 *    This file is a part of the R128Normalization utils demo
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

namespace R128Normalization
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("R128Normalization  Copyright (C) 2020  Xuan525");
            Console.WriteLine("This program is under GPLv3 license;");
            Console.WriteLine("This program comes with ABSOLUTELY NO WARRANTY; for details type `show w'.");
            Console.WriteLine("This is free software, and you are welcome to redistribute it");
            Console.WriteLine("under certain conditions; type `show c' for details.");
            Console.WriteLine();
            Console.WriteLine("Type a Path to start the process.");
            Console.WriteLine("Or you can use any of the commands below:");
            Console.WriteLine("    Type     'loudness [value]' or  'l [value]' for query or set Target integrated loudness (LUFS).");
            Console.WriteLine("    Type         'peak [value]' or  'p [value]' for query or set Maximum true peak (dB).");
            Console.WriteLine("    Type       'attack [value]' or  'a [value]' for query or set Attack duration for Limiter (s).");
            Console.WriteLine("    Type      'release [value]' or  'r [value]' for query or set Release duration for Limiter (s).");
            Console.WriteLine("    Type  'attackCurve [value]' or 'ac [value]' for query or set Attack curve tension for Limiter (1.0 - 8.0).");
            Console.WriteLine("    Type 'releaseCurve [value]' or 'rc [value]' for query or set Release curve tension for Limiter (1.0 - 8.0).");
            Console.WriteLine("    Type   'loopVerify [value]' or 'lv [value]' for query or set LUFS Loop Verification (true, false).");
            Console.WriteLine("    Type 'check' or 'c' to check the current parameters.");
            Console.WriteLine("    Type 'exit' or 'quit' to exit.");
            Console.WriteLine();

            while (true)
            {
                Console.Write("File/Directory/Command> ");
                string input = string.Empty;
                while (string.IsNullOrEmpty(input))
                {
                    input = Console.ReadLine().Trim().Trim('"');
                }
                
                if (Directory.Exists(input))
                {
                    Console.WriteLine();
                    DirectoryInfo directoryInfo = new DirectoryInfo(input);
                    string outputDirectory = Path.Combine(directoryInfo.Parent.FullName, $"{directoryInfo.Name}_norm");
                    Directory.CreateDirectory(outputDirectory);

                    FileInfo[] fileInfos = directoryInfo.GetFiles();
                    for (int i = 0; i < fileInfos.Length; i++)
                    {
                        Console.WriteLine("{0}/{1}", i + 1, fileInfos.Length);
                        FileInfo fileInfo = fileInfos[i];
                        Normalization.Normalize(fileInfo.FullName, Path.Combine(outputDirectory, $"{Path.GetFileNameWithoutExtension(fileInfo.Name)}.wav"));
                        Console.WriteLine();
                    }
                    Console.WriteLine("Finnished!");
                }
                else if (File.Exists(input))
                {
                    Console.WriteLine();
                    Normalization.Normalize(input, Path.Combine(Path.GetDirectoryName(input), $"{Path.GetFileNameWithoutExtension(input)}_norm.wav"));
                    Console.WriteLine();
                    Console.WriteLine("Finnished!");
                }
                else
                {
                    if (!ProcessCommand(input))
                        Console.WriteLine("Unknow command");
                }
                Console.WriteLine();
            }
        }

        private static bool ProcessCommand(string input)
        {
            string[] command = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            switch (command[0])
            {
                case "show":
                    if (command.Length == 1)
                    {
                        Console.WriteLine("R128Normalization  Copyright (C) 2020  Xuan525");
                        Console.WriteLine("This program is under GPLv3 license;");
                        Console.WriteLine("This program comes with ABSOLUTELY NO WARRANTY; for details type `show w'.");
                        Console.WriteLine("This is free software, and you are welcome to redistribute it");
                        Console.WriteLine("under certain conditions; type `show c' for details.");
                    }
                    else if (command[1] == "w")
                    {
                        Console.WriteLine("This program is distributed in the hope that it will be useful,");
                        Console.WriteLine("but WITHOUT ANY WARRANTY; without even the implied warranty of");
                        Console.WriteLine("MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the");
                        Console.WriteLine("GNU General Public License for more details.");
                    }
                    else if (command[1] == "c")
                    {
                        Console.WriteLine("This program is free software: you can redistribute it and/or modify");
                        Console.WriteLine("it under the terms of the GNU General Public License as published by");
                        Console.WriteLine("the Free Software Foundation, either version 3 of the License, or");
                        Console.WriteLine("(at your option) any later version.");
                    }
                    else
                    {
                        Console.WriteLine("Unknow command");
                    }
                    break;
                case "loudness":
                case "l":
                    if (command.Length > 1)
                        if (double.TryParse(command[1], out double value))
                            Normalization.TargetIntegratedLufs = value;
                        else
                            return false;
                    Console.WriteLine($"The current Target integrated loudness is {Normalization.TargetIntegratedLufs} LUFS");
                    break;
                case "peak":
                case "p":
                    if (command.Length > 1)
                        if (double.TryParse(command[1], out double value))
                            Normalization.MaximumTruePeak = value;
                        else
                            return false;
                    Console.WriteLine($"The current Maximum true peak is {Normalization.MaximumTruePeak} dBTP");
                    break;
                case "attack":
                case "a":
                    if (command.Length > 1)
                        if (double.TryParse(command[1], out double value))
                            Normalization.LimiterAttack = value;
                        else
                            return false;
                    Console.WriteLine($"The current Attack duration for Limiter is {Normalization.LimiterAttack} s");
                    break;
                case "release":
                case "r":
                    if (command.Length > 1)
                        if (double.TryParse(command[1], out double value))
                            Normalization.LimiterRelease = value;
                        else
                            return false;
                    Console.WriteLine($"The current Release duration for Limiter is {Normalization.LimiterRelease} s");
                    break;
                case "attackCurve":
                case "ac":
                    if (command.Length > 1)
                        if (double.TryParse(command[1], out double value))
                            Normalization.LimiterAttackCurve = value;
                        else
                            return false;
                    Console.WriteLine($"The current Attack curve tension for Limiter is {Normalization.LimiterAttackCurve}");
                    break;
                case "releaseCurve":
                case "rc":
                    if (command.Length > 1)
                        if (double.TryParse(command[1], out double value))
                            Normalization.LimiterReleaseCurve = value;
                        else
                            return false;
                    Console.WriteLine($"The current Release curve tension for Limiter is {Normalization.LimiterReleaseCurve}");
                    break;
                case "loopVerify":
                case "lv":
                    if (command.Length > 1)
                        if (bool.TryParse(command[1], out bool value))
                            Normalization.LoopVerify = value;
                        else
                            return false;
                    Console.WriteLine($"Loop verification: {Normalization.LimiterReleaseCurve}");
                    break;
                case "check":
                case "c":
                    Console.WriteLine($"The current Target integrated loudness is {Normalization.TargetIntegratedLufs} LUFS");
                    Console.WriteLine($"The current Maximum true peak is {Normalization.MaximumTruePeak} dBTP");
                    Console.WriteLine($"The current Attack duration for Limiter is {Normalization.LimiterAttack} s");
                    Console.WriteLine($"The current Release duration for Limiter is {Normalization.LimiterRelease} s");
                    Console.WriteLine($"The current Attack curve tension for Limiter is {Normalization.LimiterAttackCurve}");
                    Console.WriteLine($"The current Release curve tension for Limiter is {Normalization.LimiterReleaseCurve}");
                    Console.WriteLine($"Loop verification: {Normalization.LimiterReleaseCurve}");
                    break;
                case "exit":
                case "quit":
                    Environment.Exit(0);
                    break;
                default:
                    return false;
            }
            return true;
        }
    }
}
