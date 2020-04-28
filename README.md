# R128Normalization

***中文说明请往下看***

A audio auto normalization tool which followed EBU R128 standard. 

[Download](https://github.com/xuan525/R128Normalization/releases)

## Features

- Auto normalization for audio files
- Following EBU R128 standard and ITU BS.1770 standard (LUFS)
- Core algorithm is fully implemented in C#
- Using [FFmpeg](https://www.ffmpeg.org/) for decoding files which are not wav format
- Support batch processing
- Allow custom standard

## Usage

1. Place the [ffmpeg.exe](https://ffmpeg.zeranoe.com/builds/win64/static/ffmpeg-4.2.2-win64-static.zip) with the main program in the same directory or install the FFmpeg in the environment.
2. Run the main program.
3. Input or drag the file or the directory into the interactive window and press Enter. 
4. Wait for processing.
5. The output will be in the same parent directory with a "_norm" postfix in the filename or directory name.

## Commands
- Type     **'loudness [value]'** or  **'l [value]'** for query or set **Target integrated loudness (LUFS)**.
- Type         **'peak [value]'** or  **'p [value]'** for query or set **Maximum true peak (dB)**.
- Type       **'attack [value]'** or  **'a [value]'** for query or set **Attack duration for Limiter (s)**.
- Type      **'release [value]'** or  **'r [value]'** for query or set **Release duration for Limiter (s)**.
- Type  **'attackCurve [value]'** or **'ac [value]'** for query or set **Attack curve tension for Limiter (1.0 - 8.0)**.
- Type **'releaseCurve [value]'** or **'rc [value]'** for query or set **Release curve tension for Limiter (1.0 - 8.0)**.
- Type **'check'** or **'c'** to check the current parameters.
- Type **'exit'** or **'quit'** to exit.

## Workflow

1. Measure the original integrated loudness of the input audio
2. Calculate the gain value based on the target integrated loudness and the original integrated loudness
3. Apply gain to the original audio according to the calculated gain value
4. Apply a limiter to the output audio according to the maximum true peak
5. Measure the integrated loudness of the output audio
6. If the integrated loudness of the output audio is within the error range (± 0.5 LU), save the audio as a file, otherwise adjust the gain value and return to **step 3**

## Tech 3341, Table 1 - Minimum-requirements test signals

| Test case | Test signal | Expected response and accepted tolerances | Result |
| ---- | ---- | ---- | ---- |
| 1 | Stereo sine wave, 1000 Hz, −23.0 dBFS (per-channel peak level);<br>signal applied in phase to both channels simultaneous; 20 s duration | M, S, I = −23.0 ±0.1 LUFS<br>M, S, I = 0.0 ±0.1 LU | Passed |
| 2 | As #1 at −33.0 dBFS | M, S, I = −33.0 ±0.1 LUFS<br>M, S, I = −10.0 ±0.1 LU | Passed |
| 3 | 3 tones similar to #1 but with the following durations and levels:<br>10 s at −36.0 dBFS; 60 s at −23.0 dBFS; 10 s at −36.0 dBFS | I = −23.0 ±0.1 LUFS<br>I = 0.0 ±0.1 LU | Passed |
| 4 | 5 tones similar to #1 but with the following durations and levels:<br>10 s at −72.0 dBFS; 10 s at −36.0 dBFS; 60 s at −23.0 dBFS; 10 s at<br>−36.0 dBFS; 10 s at −72.0 dBFS | I = −23.0 ±0.1 LUFS<br>I = 0.0 ±0.1 LU | Passed |
| 5 | 3 tones similar to #1 but with the following durations and levels:<br>20 s at −26.0 dBFS; 20.1 s at −20.0 dBFS; 20 s at −26.0 dBFS | I = −23.0 ±0.1 LUFS<br>I = 0.0 ±0.1 LU | Passed |
| 6 | 5.0 channel sine wave, 1000 Hz, 20 s duration, with per-channel peak<br>levels as follows:<br>&nbsp;−28.0 dBFS in L and R<br>&nbsp;−24.0 dBFS in C<br>&nbsp;−30.0 dBFS in Ls and Rs | I = −23.0 ±0.1 LUFS<br>I = 0.0 ±0.1 LU | Passed |
| 7 | Authentic programme 1, stereo, narrow loudness range (NLR) <br>programme segment; similar in genre to a commercial/promo | I = −23.0 ±0.1 LUFS<br>I = 0.0 ±0.1 LU | Passed |
| 8 | Authentic programme 2, stereo, wide loudness range (WLR) <br>programme segment; similar in genre to a movie/drama | I = −23.0 ±0.1 LUFS<br>I = 0.0 ±0.1 LU | Test audio not found |
| 9 | 2 tones similar to #1 but with the following durations and levels:<br>(1.34 s at −20.0 dBFS; 1.66 s at −30.0 dBFS) repeated 5 times | S = −23.0 ±0.1 LUFS, <br>constant after 3 s | Passed |
| 10 | <u>For file-based meters</u>. 20 segments with tones similar to #1 but with<br>the following durations and levels:<br>(i * 0.15 s of silence; 3 s at −23.0 dBFS; 1 s of silence)<br>for i = 0, 1, 2, …, 19 | Max S = −23.0 ±0.1 LUFS, for <br>each segment | Falied |
| 11 | <u>For ‘live’ meters</u>. 20 tones similar to #1 but with the following<br>durations and levels:<br>(i * 0.15 s of silence; 3 s at −38.0+i dBFS; 3 – i * 0.15 s of silence)<br>for i = 0, 1, 2, …, 19 | Max S = −38.0, −37.0, −36.0, <br>…, −19.0 ±0.1 LUFS, <br>successive values | Falied |
| 12 | 2 tones similar to #1 but with the following durations and levels:<br>(0.18 s at −20.0 dBFS; 0.22 s at −30.0 dBFS) repeated 25 times | M = −23.0 ±0.1 LUFS, <br>constant after 1 s | Passed |
| 13 | <u>For file-based meters</u>. 20 segments with tones similar to #1 but with<br>the following durations and levels:<br>(i * 20 ms of silence; 400 ms at −23.0 dBFS; 1 s of silence)<br>for i = 0, 1, 2, …, 19 | Max M = −23.0 ±0.1 LUFS, for<br>each segment | Failed |
| 14 | <u>For ‘live’ meters</u>. 20 tones similar to #1 but with the following<br>durations and levels:<br>(i * 20 ms of silence; 400 ms at −38.0+i dBFS; 400 – i * 20 ms of<br>silence) for i = 0, 1, 2, …, 19 | Max M = −38.0, −37.0, −36.0, <br>…, −19.0 ±0.1 LUFS, <br>successive values | Failed |
| 15 | Stereo sine wave with frequency fs/4 Hz, amplitude 0.50 FFS<sup>2</sup>, phase<br>0.0 degrees. The frequency fs/4 denotes 12 kHz for a sample-rate of<br>48 kHz, etc. The duration of the synthesized tone does not matter,<br>but the tone should be tapered with a 10 ms fade-in and fade–out. | Max. true-peak level<br>= −6.0 +0.2/−0.4 dBTP | Passed |
| 16 | Stereo sine wave with frequency fs/4 Hz, amplitude 0.50 FFS, phase<br>45.0 degrees | Max. true-peak level = −6.0<br>+0.2/−0.4 dBTP | Passed |
| 17 | Stereo sine wave with frequency fs/6 Hz, amplitude 0.50 FFS, phase<br>60.0 degrees | Max. true-peak level = −6.0<br>+0.2/−0.4 dBTP | Passed |
| 18 | Stereo sine wave with frequency fs/8 Hz, amplitude 0.50 FFS, phase<br>67.5 degrees | Max. true-peak level = −6.0<br>+0.2/−0.4 dBTP | Passed |
| 19 | Stereo sine wave with frequency fs/4 Hz, amplitude 1.41 FFS, phase<br>45.0 degrees | Max. true-peak level = +3.0<br>+0.2/−0.4 dBTP | Passed |
| 20 | Stereo sine wave with frequency fs/6 Hz, amplitude 0.50 FFS,<br>containing a single period of a sine wave with frequency fs/4,<br>amplitude 1.00; the signal being continuous in phase at both sides of<br>the single period.<br>The signal is synthesized at 4*fs (e.g. 192 kHz), and then lowpass<br>(anti-aliasing) filtered and downsampled to fs with a 0 samples<br>offset. The total duration of the synthesized tone does not matter,<br>but the tone should be tapered with a short fade-in and fade–out. | Max. true-peak level = 0.0<br>+0.2/−0.4 dBTP | Passed |
| 21 | As #20, but downsampled with a 1 samples offset (at the 4*fs rate). | Max. true-peak level = 0.0<br>+0.2/−0.4 dBTP | Passed |
| 22 | As #20, but downsampled with a 2 samples offset (at the 4*fs rate). | Max. true-peak level = 0.0<br>+0.2/−0.4 dBTP | Passed |
| 23 | As #20, but downsampled with a 3 samples offset (at the 4*fs rate). | Max. true-peak level = 0.0<br>+0.2/−0.4 dBTP | Passed |

## References

1. ITU-R BS.1770-4, Algorithms to measure audio programme loudness and true-peak audio level (https://www.itu.int/rec/R-REC-BS.1770-4-201510-I)
2. EBU R 128, Loudness normalisation and permitted maximum level of audio signals (https://tech.ebu.ch/publications/e128)
3. EBU TECH 3341, Loudness Metering: ‘EBU Mode’ metering to supplement loudness normalisation in accordance with EBU R 128 (https://tech.ebu.ch/publications/tech3341)
4. EBU Tech 3342, Loudness Range: A measure to supplement loudness normalisation in accordance with EBU R 128 (https://tech.ebu.ch/publications/tech3342)
5. EBU Tech 3343, Guidelines for Production of Programmes in accordance with EBU R 128 (https://tech.ebu.ch/publications/tech3343)
6. EBU Tech 3344, Guidelines for Distribution and Reproduction of Programmes in accordance with EBU R 128 (https://tech.ebu.ch/publications/tech3344)
7. Minimum-requirements test signals for ‘EBU Mode’ loudness meters (https://tech.ebu.ch/publications/ebu_loudness_test_set)

---

# R128标准化

***For english description, please scroll up***

遵循EBU R128标准的音频自动标准化工具。

[下载](https://github.com/xuan525/R128Normalization/releases)

## 特性

- 自动标准化音频文件
- 遵循EBU R128标准和ITU BS.1770标准（LUFS）
- 核心算法在C＃中完全实现
- 使用[FFmpeg](https://www.ffmpeg.org/)解码非wav格式的文件
- 支持批量处理
- 允许自定义标准

## 用法

1. 将[ffmpeg.exe](https://ffmpeg.zeranoe.com/builds/win64/static/ffmpeg-4.2.2-win64-static.zip)与主程序放在同一目录中，或在环境中安装FFmpeg。
2. 运行主程序。
3. 将文件或目录输入或拖动到交互式窗口中，然后按Enter。
4. 等待处理。
5. 输出将在相同的父目录中，文件名或目录名称中带有“ _norm”后缀。

## 命令
- 输入     **'loudness [值]'** 或  **'l [值]'** 来查询或设置 **目标综合响度 (LUFS)**。
- 输入         **'peak [值]'** 或  **'p [值]'** 来查询或设置 **最大真实峰值 (dB)**。
- 输入       **'attack [值]'** 或  **'a [值]'** 来查询或设置 **限制器的攻击持续时间 (s)**。
- 输入      **'release [值]'** 或  **'r [值]'** 来查询或设置 **限制器的释放持续时间 (s)**。
- 输入  **'attackCurve [值]'** 或 **'ac [值]'** 来查询或设置 **限制器的攻击曲线张力 (1.0 - 8.0)**。
- 输入 **'releaseCurve [值]'** 或 **'rc [值]'** 来查询或设置 **限制器的释放曲线张力 (1.0 - 8.0)**。
- 输入 **'check'** 或 **'c'** 来检查当前参数。
- 输入 **'exit'** 或 **'quit'** 来退出。

## 工作流程

1. 测量输入音频的原始综合响度
2. 根据目标综合响度和原始综合响度计算增益值
3. 根据计算出的增益值将增益应用于原始音频
4. 根据最大真实峰值对输出音频应用限制器
5. 测量输出音频的综合响度
6. 如果输出音频的综合响度在误差范围内（±0.5 LU），则将音频另存为文件，否则调整增益值并返回**步骤3**

---
