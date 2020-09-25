//example_dll.h
#ifndef EXAMPLE_DLL_H
#define EXAMPLE_DLL_H



int __declspec(dllexport) GetIsVideoReadyToShow();
void* __declspec(dllexport) show();

char* __declspec(dllexport) StartFFPlay(int argc,  char **argv);

void __declspec(dllexport) KillVideo();

#endif  // EXAMPLE_DLL_H