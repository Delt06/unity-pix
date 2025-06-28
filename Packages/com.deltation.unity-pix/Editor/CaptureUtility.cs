using System;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;

namespace DELTation.UnityPix.Editor
{
    internal static class CaptureUtility
    {
        private const string CaptureFileExtension = ".wpix";
        private const string CaptureDateFormat = "yyyy-MM-dd--HH-mm-ss";

        private static string _pendingCaptureFilePath;
        private static GraphicsFence _pendingCaptureFence;

        public static bool IsSupported() => SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D12 && UnityPixBindings.IsPixLoaded() != 0;

        public static async void MakeAndOpen([NotNull] ICaptureTarget captureTarget)
        {
            try
            {
                if (captureTarget == null)
                {
                    throw new ArgumentNullException(nameof(captureTarget));
                }

                if (SystemInfo.graphicsDeviceType != GraphicsDeviceType.Direct3D12)
                {
                    Debug.LogError("Unity PIX: PIX Integration is only supported on Direct3D12.");
                    return;
                }

                if (UnityPixBindings.IsPixLoaded() == 0)
                {
                    Debug.LogError(
                        "Unity PIX: PIX is not attached. Either launch the project through PIX or add -unity-pix-capture to the command line argument list."
                    );
                    return;
                }

                if (_pendingCaptureFilePath != null)
                {
                    Debug.LogError("Unity PIX: a capture is in progress.");
                    return;
                }

                string directoryName = Path.Combine(Application.dataPath, UnityPixPreferences.CaptureFolderName);
                Directory.CreateDirectory(directoryName!);

                EnsureCaptureFilesAreIgnoredInGit(directoryName);

                _pendingCaptureFilePath = Path.Combine(directoryName, "capture_" + DateTime.Now.ToString(CaptureDateFormat) + CaptureFileExtension);

                Debug.Log("Unity PIX: starting a PIX capture...");

                uint result = UnityPixBindings.PixGpuCaptureNextFrames(_pendingCaptureFilePath, (uint) captureTarget.NumFrames);
                Assert.IsTrue(result == 0, "BeginPixCapture failed.");

                await captureTarget.RenderAsync();

                _pendingCaptureFence = Graphics.CreateGraphicsFence(GraphicsFenceType.AsyncQueueSynchronisation, SynchronisationStageFlags.AllGPUOperations);

                EditorApplication.update += WaitForRenderFinish;
            }
            catch (Exception e)
            {
                Debug.LogError("Unity PIX: an exception occured when making a PIX capture:");
                Debug.LogException(e);
            }
        }

        private static void EnsureCaptureFilesAreIgnoredInGit(string directoryName)
        {
            string gitIgnorePath = Path.Combine(directoryName, ".gitignore");
            if (!File.Exists(gitIgnorePath))
            {
                File.WriteAllText(gitIgnorePath, "*" + CaptureFileExtension);
            }
        }

        private static async void WaitForRenderFinish()
        {
            try
            {
                EditorApplication.update -= WaitForRenderFinish;

                while (!_pendingCaptureFence.passed)
                { 
                    await Task.Yield();
                }

                _pendingCaptureFence = default;

                if (_pendingCaptureFilePath == null)
                {
                    return;
                }

                try
                {
                    Debug.Log("Unity PIX: successfully took a PIX capture: " + Path.GetFullPath(_pendingCaptureFilePath));

                    await Task.Yield();

                    UnityPixBindings.OpenPixCapture(_pendingCaptureFilePath);
                }
                finally
                {
                    _pendingCaptureFilePath = null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Unity PIX: an exception occurred during {nameof(WaitForRenderFinish)}:");
                Debug.LogException(e);
            }
        }
    }
}