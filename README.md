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

Note : Allow to use the command **'loudness \[value\]'** or **'l \[value\]'** to query or set a custom **target integrated loudness**; Allow to use the command **'peak \[value\]'** or **'p \[value\]'** to query or set a custom **maximum true peak**.

## Workflow

1. Measure the original integrated loudness of the input audio
2. Calculate the gain value based on the target integrated loudness and the original integrated loudness
3. Apply gain to the original audio according to the calculated gain value
4. Apply a limiter to the output audio according to the maximum true peak
5. Measure the integrated loudness of the output audio
6. If the integrated loudness of the output audio is within the error range (± 0.5 LU), save the audio as a file, otherwise adjust the gain value and return to **step 3**

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

注：允许使用命令 **'loudness \[值\]'** 或 **'l \[值\]'** 查询或设置自定义**目标综合响度**；允许使用命令 **'peak \[值\]'** 或 **'p \[值\]'** 查询或设置自定义**最大真实峰值**。

## 工作流程

1. 测量输入音频的原始综合响度
2. 根据目标综合响度和原始综合响度计算增益值
3. 根据计算出的增益值将增益应用于原始音频
4. 根据最大真实峰值对输出音频应用限制器
5. 测量输出音频的综合响度
6. 如果输出音频的综合响度在误差范围内（±0.5 LU），则将音频另存为文件，否则调整增益值并返回**步骤3**

---
