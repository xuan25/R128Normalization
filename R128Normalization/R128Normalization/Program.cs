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
            Console.WriteLine(@"R128Normalization  Copyright (C) 2020  Xuan525");
            Console.WriteLine(@"This program is under GPLv3 license;");
            Console.WriteLine(@"This program comes with ABSOLUTELY NO WARRANTY;");
            Console.WriteLine(@"This is free software, and you are welcome to redistribute it");
            Console.WriteLine(@"under certain conditions;");

            Directory.CreateDirectory("inputs");
            Directory.CreateDirectory("outputs");

            DirectoryInfo inputsDirectory = new DirectoryInfo("inputs");
            FileInfo[] inputs = inputsDirectory.GetFiles();
            for (int i = 0; i < inputs.Length; i++)
            {
                Console.WriteLine("{0}/{1}", i + 1, inputs.Length);
                string input = inputs[i].FullName;
                string output = Path.Combine(new DirectoryInfo("outputs").FullName, string.Format("{0}.wav", Path.GetFileNameWithoutExtension(input)));

                // Process
                Normalization.Normalize(input, output);

                Console.WriteLine();
                //break;
            }
            Console.WriteLine("Finished");
        }
    }
}
