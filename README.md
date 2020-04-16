# R128Normalization

A auto normalization program followed EBU R128 standard

## Features

- Auto normalization for sound files
- Following EBU R128 standard and ITU BS.1770 standard (LUFS)
- Core algorithm is fully implemented in C#
- Using [FFmpeg](https://www.ffmpeg.org/) for decoding (allow many input formats) 

## Usage

Place the files like this:

- root
  - inputs
    - file1.mp3
    - file2.wav
    - ...
  - outputs
    - ...
  - ffmpeg.exe
  - R128Normalization.exe

And run
