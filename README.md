# DLLFFPLAY  

Use to use FFPLAY as dll.  
You can show the same view in different windows and resize this windows (see WPF example)  

Step to start:  
1-download the project https://github.com/m-ab-s/media-autobuild_suite and build the ffmpeg with your settings (here I used LGPL x86 64 bits)  
2-Add the following files example_dll.c and example_dll.h in the folder "media\build\fftools"  
3-Add and erase the MakeFile (rename as makefile by removing "FfmpegGit") in the folder media\build\ffmpeg-git\fftools  
4-execute mingw32 or mingw64 (depending of your configuration) in the folder media\msys64:  
cd build  
make  

You can generate a video stream with obs in avi , h246 in localhost to make some test.  


