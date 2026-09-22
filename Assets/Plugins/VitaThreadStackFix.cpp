// =============================================================================
// VitaThreadStackFix.cpp
// PS Vita (VitaSDK / Unity PSP2 IL2CPP) — C2-12828-1 Out of Memory fix.
//
// PROBLEM:
//   PS Vita user apps have ~280 MB usable RAM and small default thread
//   stacks. Every pthread_create() with the default stack (often 1-8 MB,
//   depending on the toolchain/libc) eats Vita memory and can push the game
//   over the limit -> C2-12828-1 (Out of Memory / stack overflow).
//
// FIX:
//   Create worker threads with an explicit 128 KB stack via pthread attributes.
//   128 KB is enough for small job workers (no big on-stack arrays, no deep
//   recursion) and saves megabytes per thread.
//
// USAGE:
//   Replace:
//       pthread_create(&thread, NULL, worker_func, arg);
//   With:
//       pthread_t thread;
//       VitaCreateThread(&thread, worker_func, arg);
//
//   Or keep your own call sites and copy the 6-line pattern below.
//
// NOTE: search of StandoffProj/Assets found no native pthread_create call
// sites in C# sources (Photon/Unity threads are managed by the engine).
// This file is the canonical native helper so any future .cpp/.suprx plugin
// code uses the safe pattern from the start.
// Build: compiled with psp2snc / arm-vita-eabi-g++ as part of the plugin.
// =============================================================================

#include <pthread.h>
#include <stddef.h>

// Vita-safe worker stack: 128 KB.
#define VITA_WORKER_STACK_SIZE (128 * 1024)

// Safe thread creation with capped stack.
// Returns 0 on success, pthread error code otherwise (same as pthread_create).
static int VitaCreateThread(pthread_t* outThread,
                            void* (*worker_func)(void*),
                            void* arg)
{
    if (outThread == NULL || worker_func == NULL)
    {
        return 22; // EINVAL
    }

    pthread_attr_t attr;
    pthread_attr_init(&attr);
    pthread_attr_setstacksize(&attr, VITA_WORKER_STACK_SIZE);

    int result = pthread_create(outThread, &attr, worker_func, arg);

    pthread_attr_destroy(&attr);
    return result;
}

// -----------------------------------------------------------------------------
// Example worker (delete or replace with real work):
// -----------------------------------------------------------------------------
// static void* VitaWorkerFunc(void* arg)
// {
//     (void)arg;
//     // ... do bounded work here, no large stack allocations ...
//     return NULL;
// }
//
// void VitaSpawnExampleWorker(pthread_t* outThread)
// {
//     pthread_attr_t attr;
//     pthread_attr_init(&attr);
//     pthread_attr_setstacksize(&attr, 128 * 1024);
//     pthread_create(outThread, &attr, VitaWorkerFunc, NULL);
//     pthread_attr_destroy(&attr);
// }
// -----------------------------------------------------------------------------

// ---------------------------------------------------------------------------
// IL2CPP NOTE (Unity 2017.4 PSP2):
// Unity job workers are tuned from managed/boot.config side instead:
//   job-worker-count=2
//   async-upload-buffer-size=16
// See ProjectSettings/boot.config and Assets/StreamingAssets/boot.config.
// ---------------------------------------------------------------------------
