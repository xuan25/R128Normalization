using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Wav
{
    public class WavWriter
    {
        public double[][] SampleData { get; set; }
        public int SampleRate { get; set; }
        public short AudioFormat { get; set; }
        public short BitsPerSample { get; set; }
        public SortedDictionary<string, string> Infos { get; set; }
        public Encoding StringEncoding { get; set; }

        /// <summary>
        /// The Constructor of WavWriter
        /// </summary>
        /// <param name="sampleData">Sample data which shoule be a double array[channel][sample] between -1.0 to 1.0</param>
        /// <param name="sampleRate">Sample rate of the data</param>
        /// <param name="audioFormat">The format code to encode. 0x0001 implies PCM, 0x0003 implies IEEE float</param>
        /// <param name="bitsPerSample">Number of bits per sample (aka. bit depth), should be one of 16, 24, 32 or 64</param>
        /// <param name="infoDict">The info of the wave file (See 2-14 at p23 in the "Multimedia Programming Interface and Data Specifications 1.0")</param>
        /// <param name="stringEncoding">Charset of the strings in the file</param>
        public WavWriter(double[][] sampleData, int sampleRate, short audioFormat, short bitsPerSample, IDictionary<string, string> infoDict, Encoding stringEncoding)
        {
            SampleData = sampleData;
            SampleRate = sampleRate;
            AudioFormat = audioFormat;
            BitsPerSample = bitsPerSample;
            if (infoDict != null)
            {
                Infos = new SortedDictionary<string, string>(infoDict);
            }

            StringEncoding = stringEncoding;

            switch (AudioFormat)
            {
                case 0x0001:
                    switch (BitsPerSample)
                    {
                        case 32:
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;
                case 0x0003:
                    switch (BitsPerSample)
                    {
                        case 32:
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Generate a stream of the wave file 
        /// </summary>
        /// <returns>The stream of the current wave file</returns>
        public Stream GetStream()
        {
            byte[] fmtBody;
            MemoryStream fmtBodyStream = new MemoryStream();
            using (BinaryWriter binaryWriter = new BinaryWriter(fmtBodyStream))
            {
                short audioFormat = AudioFormat;
                short numChannels = (short)SampleData.Length;
                int sampleRate = SampleRate;
                short bitsPerSample = BitsPerSample;
                int byteRate = sampleRate * numChannels * bitsPerSample / 8;
                short blockAlign = (short)(numChannels * bitsPerSample / 8);

                binaryWriter.Write(audioFormat);
                binaryWriter.Write(numChannels);
                binaryWriter.Write(sampleRate);
                binaryWriter.Write(byteRate);
                binaryWriter.Write(blockAlign);
                binaryWriter.Write(bitsPerSample);

                fmtBody = fmtBodyStream.ToArray();
            }

            byte[] infoListBody = null;
            if (Infos != null && Infos.Count > 0)
            {
                MemoryStream infoListBodyStream = new MemoryStream();
                using (BinaryWriter binaryWriter = new BinaryWriter(infoListBodyStream))
                {
                    binaryWriter.Write(Encoding.ASCII.GetBytes("INFO"));
                    foreach (string key in Infos.Keys)
                    {
                        string formatedKey = key;
                        int keyLength = formatedKey.Length;
                        if (keyLength > 4)
                        {
                            formatedKey = formatedKey.Substring(0, 4);
                        }
                        else if (keyLength < 4)
                        {
                            formatedKey = formatedKey.PadRight(4, '\0');
                        }

                        binaryWriter.Write(Encoding.ASCII.GetBytes(formatedKey));

                        string value = Infos[key] + '\0';
                        byte[] valueBytes = StringEncoding.GetBytes(value);
                        int valueLength = valueBytes.Length;
                        binaryWriter.Write(valueLength);

                        binaryWriter.Write(valueBytes);
                        if (valueLength % 2 != 0)
                        {
                            binaryWriter.Write((byte)0);
                        }
                    }

                    infoListBody = infoListBodyStream.ToArray();
                }
            }

            byte[] dataBody;
            MemoryStream dataBodyStream = new MemoryStream();
            using (BinaryWriter binaryWriter = new BinaryWriter(dataBodyStream))
            {
                switch (AudioFormat)
                {
                    case 0x0001:
                        switch (BitsPerSample)
                        {
                            case 16:
                                for (int pos = 0; pos < SampleData[0].Length; pos++)
                                {
                                    for (int chan = 0; chan < SampleData.Length; chan++)
                                    {
                                        short value = (short)Math.Round(SampleData[chan][pos] * short.MaxValue);
                                        binaryWriter.Write(value);
                                    }
                                }
                                break;
                            case 24:
                                for (int pos = 0; pos < SampleData[0].Length; pos++)
                                {
                                    for (int chan = 0; chan < SampleData.Length; chan++)
                                    {
                                        int value = (int)Math.Round(SampleData[chan][pos] * 8388607);
                                        byte[] buffer = BitConverter.GetBytes(value);
                                        binaryWriter.Write(buffer, 0, 3);
                                    }
                                }
                                break;
                            case 32:
                                for (int pos = 0; pos < SampleData[0].Length; pos++)
                                {
                                    for (int chan = 0; chan < SampleData.Length; chan++)
                                    {
                                        int value = (int)Math.Round(SampleData[chan][pos] * int.MaxValue);
                                        binaryWriter.Write(value);
                                    }
                                }
                                break;
                            case 64:
                                for (int pos = 0; pos < SampleData[0].Length; pos++)
                                {
                                    for (int chan = 0; chan < SampleData.Length; chan++)
                                    {
                                        long value = (long)Math.Round(SampleData[chan][pos] * long.MaxValue);
                                        binaryWriter.Write(value);
                                    }
                                }
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    case 0x0003:
                        switch (BitsPerSample)
                        {
                            case 32:
                                for (int pos = 0; pos < SampleData[0].Length; pos++)
                                {
                                    for (int chan = 0; chan < SampleData.Length; chan++)
                                    {
                                        float value = (float)SampleData[chan][pos];
                                        binaryWriter.Write(value);
                                    }
                                }
                                break;
                            case 64:
                                for (int pos = 0; pos < SampleData[0].Length; pos++)
                                {
                                    for (int chan = 0; chan < SampleData.Length; chan++)
                                    {
                                        double value = SampleData[chan][pos];
                                        binaryWriter.Write(value);
                                    }
                                }
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
                dataBody = dataBodyStream.ToArray();
            }

            byte[] riffBody;
            MemoryStream riffBodyStream = new MemoryStream();
            using (BinaryWriter binaryWriter = new BinaryWriter(riffBodyStream))
            {
                binaryWriter.Write(Encoding.ASCII.GetBytes("WAVE"));
                binaryWriter.Write(Encoding.ASCII.GetBytes("fmt "));
                binaryWriter.Write(fmtBody.Length);
                binaryWriter.Write(fmtBody);
                if (infoListBody != null)
                {
                    binaryWriter.Write(Encoding.ASCII.GetBytes("LIST"));
                    binaryWriter.Write(infoListBody.Length);
                    binaryWriter.Write(infoListBody);
                }
                binaryWriter.Write(Encoding.ASCII.GetBytes("data"));
                binaryWriter.Write(dataBody.Length);
                binaryWriter.Write(dataBody);

                riffBody = riffBodyStream.ToArray();
            }


            MemoryStream riffStream = new MemoryStream();
            using (BinaryWriter binaryWriter = new BinaryWriter(riffStream, Encoding.Default, true))
            {
                binaryWriter.Write(Encoding.ASCII.GetBytes("RIFF"));
                binaryWriter.Write(riffBody.Length);
                binaryWriter.Write(riffBody);
            }
            riffStream.Position = 0;
            return riffStream;
        }

        /// <summary>
        /// Save current wave to a file
        /// </summary>
        /// <param name="fileName">FileName to save</param>
        public void Save(string fileName)
        {
            using (Stream stream = GetStream())
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
                {
                    stream.CopyTo(fileStream);
                }
            }
        }
    }

}
