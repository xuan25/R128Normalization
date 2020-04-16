using R128;
using System;
using System.IO;

namespace LufsNormalization
{
    internal class Program
    {
        private static void Main(string[] args)
        {
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
                R128Normalization.Normalize(input, output);

                Console.WriteLine();
                //break;
            }
            Console.WriteLine("Finished");
        }
    }
}
