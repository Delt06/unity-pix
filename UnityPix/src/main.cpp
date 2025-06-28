#include <optional>
#include <iostream>

#include <d3d12.h>
#include <d3dx12.h>
#include <dxgi.h>
#include <winapifamily.h>
#include <windows.h>

#include <pix3.h>
#include <shlobj.h>

#include <Unity/IUnityGraphics.h>
#include <Unity/IUnityGraphicsD3D12.h>
#include <Unity/IUnityLog.h>

namespace
{
    IUnityInterfaces* s_UnityInterfaces = nullptr;
    IUnityGraphics*   s_Graphics = nullptr;
    IUnityLog*        s_Log = nullptr;

    bool ShouldLoadWinPixDLL(const int argc, const LPWSTR* argv)
    {
        for (int i = 0; i < argc; i++)
        {
            if (lstrcmpW(argv[i], L"-unity-pix-capture") == 0)
            {
                return true;
            }
        }

        return false;
    }

    std::optional<std::wstring> GetLatestWinPixGpuCapturerPath()
    {
        LPWSTR programFilesPath = nullptr;
        SHGetKnownFolderPath(FOLDERID_ProgramFiles, KF_FLAG_DEFAULT, nullptr, &programFilesPath);

        std::wstring pixSearchPath = programFilesPath + std::wstring(L"\\Microsoft PIX\\*");

        WIN32_FIND_DATAW findData;
        bool             foundPixInstallation = false;
        wchar_t          newestVersionFound[MAX_PATH];

        HANDLE hFind = FindFirstFileW(pixSearchPath.c_str(), &findData);
        if (hFind != INVALID_HANDLE_VALUE)
        {
            do
            {
                if (((findData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) ==
                     FILE_ATTRIBUTE_DIRECTORY) &&
                    (findData.cFileName[0] != '.'))
                {
                    if (!foundPixInstallation ||
                        wcscmp(newestVersionFound, findData.cFileName) <= 0)
                    {
                        foundPixInstallation = true;
                        StringCchCopyW(newestVersionFound, _countof(newestVersionFound),
                                       findData.cFileName);
                    }
                }
            } while (FindNextFileW(hFind, &findData) != 0);
        }

        FindClose(hFind);

        if (!foundPixInstallation)
        {
            return std::nullopt;
        }

        wchar_t output[MAX_PATH];
        StringCchCopyW(output, pixSearchPath.length(), pixSearchPath.data());
        StringCchCatW(output, MAX_PATH, &newestVersionFound[0]);
        StringCchCatW(output, MAX_PATH, L"\\WinPixGpuCapturer.dll");

        return &output[0];
    }
} // namespace

extern "C" uint32_t UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API IsPixLoaded()
{
    return GetModuleHandleW(L"WinPixGpuCapturer.dll") != nullptr;
}

extern "C" uint32_t UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API
PixGpuCaptureNextFrames(IN PCWSTR fileName, IN UINT32 numFrames)
{
    return PIXGpuCaptureNextFrames(fileName, numFrames);
}

extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API OpenPixCapture(IN LPWSTR filePath)
{
    PIXOpenCaptureInUI(filePath);
}

static void UNITY_INTERFACE_API OnGraphicsDeviceEvent([[maybe_unused]] UnityGfxDeviceEventType eventType)
{
}

static void UNITY_INTERFACE_API OnRenderEvent(int eventID) {}

extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API
UnityPluginLoad(IUnityInterfaces* unityInterfaces)
{
    s_UnityInterfaces = unityInterfaces;
    if (s_UnityInterfaces == nullptr)
    {
        std::cerr << "Unity PIX: the pointer to IUnityInterfaces is nullptr" << std::endl;
        return;
    }

    s_Log = s_UnityInterfaces->Get<IUnityLog>();
    s_Graphics = s_UnityInterfaces->Get<IUnityGraphics>();

    const auto   commandLine = GetCommandLineW();
    int          argc = 0;
    LPWSTR*      argv = CommandLineToArgvW(commandLine, &argc);

    if (ShouldLoadWinPixDLL(argc, argv))
    {
        // Check to see if a copy of WinPixGpuCapturer.dll has already been injected into the
        // application. This may happen if the application is launched through the PIX UI.
        if (!IsPixLoaded())
        {
            if (const auto pathResult = GetLatestWinPixGpuCapturerPath();
                pathResult.has_value())
            {
                LoadLibraryW(pathResult.value().c_str());
                UNITY_LOG(s_Log, "Unity PIX: loaded WinPixGpuCapturer.dll.");

                // Hide the overlay
                PIXSetHUDOptions(PIX_HUD_SHOW_ON_NO_WINDOWS);
            }
            else
            {
                UNITY_LOG_ERROR(s_Log, "Unity PIX: failed to find PIX installation.");
            }
        }
        else
        {
            UNITY_LOG(s_Log, "Unity PIX: WinPixGpuCapturer.dll is already loaded.");
        }
    }

    LocalFree(argv);

    // Run OnGraphicsDeviceEvent(initialize) manually on plugin load
    OnGraphicsDeviceEvent(kUnityGfxDeviceEventInitialize);
}

extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API UnityPluginUnload()
{
    s_Graphics->UnregisterDeviceEventCallback(OnGraphicsDeviceEvent);
    s_Graphics = nullptr;
    s_Log = nullptr;
}
