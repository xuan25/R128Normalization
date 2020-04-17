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
            Console.WriteLine("Type a path to start the process.");
            Console.WriteLine("Type 'loudness [value]' or 'l [value]' to show or set the target integrated loudness (LUFS).");
            Console.WriteLine("Type 'peak [value]' or 'p [value]' to show or set the maximum true peak (dB).");
            Console.WriteLine("Type 'exit' or 'quit' to exit.");
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
                    string[] command = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (command[0] == "show")
                    {
                        if(command.Length == 1)
                        {
                            Console.WriteLine("R128Normalization  Copyright (C) 2020  Xuan525");
                            Console.WriteLine("This program is under GPLv3 license;");
                            Console.WriteLine("This program comes with ABSOLUTELY NO WARRANTY; for details type `show w'.");
                            Console.WriteLine("This is free software, and you are welcome to redistribute it");
                            Console.WriteLine("under certain conditions; type `show c' for details.");
                        }
                        else if(command[1] == "w")
                        {
                            Console.WriteLine("This program is distributed in the hope that it will be useful,");
                            Console.WriteLine("but WITHOUT ANY WARRANTY; without even the implied warranty of");
                            Console.WriteLine("MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the");
                            Console.WriteLine("GNU General Public License for more details.");
                        }
                        else if(command[1] == "c")
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
                    }
                    else if(command[0] == "loudness" || command[0] == "l")
                    {
                        if (command.Length == 1)
                        {
                            Console.WriteLine($"The target integrated loudness currently is {Normalization.TargetIntegratedLufs} LUFS");
                        }
                        else
                        {
                            if(double.TryParse(command[1], out double value))
                            {
                                Normalization.TargetIntegratedLufs = value;
                                Console.WriteLine($"The target integrated loudness currently is {Normalization.TargetIntegratedLufs} LUFS");
                            }
                            else
                            {
                                Console.WriteLine("Unknow command");
                            }
                        }
                    }
                    else if (command[0] == "peak" || command[0] == "p")
                    {
                        if (command.Length == 1)
                        {
                            Console.WriteLine($"The maximum true peak currently is {Normalization.TargetMaximumTruePeak} LUFS");
                        }
                        else
                        {
                            if (double.TryParse(command[1], out double value))
                            {
                                Normalization.TargetMaximumTruePeak = value;
                                Console.WriteLine($"The maximum true peak currently is {Normalization.TargetMaximumTruePeak} LUFS");
                            }
                            else
                            {
                                Console.WriteLine("Unknow command");
                            }
                        }
                    }
                    else if (command[0] == "exit" || command[0] == "quit")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Unknow command");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
