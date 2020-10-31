//example_dll.h
#ifndef EXAMPLE_DLL_H
#define EXAMPLE_DLL_H



int __declspec(dllexport) GetIsVideoReadyToShow();
void* __declspec(dllexport) show(int windows);

char* __declspec(dllexport) StartFFPlay(int argc,  char **argv, int imgCount, int** imgPartRect);

void __declspec(dllexport) KillVideo();

#endif  // EXAMPLE_DLL_H