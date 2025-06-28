using System.Runtime.InteropServices;

namespace DELTation.UnityPix.Editor
{
    internal static class UnityPixBindings
    {
        private const string DLLName =
#if (PLATFORM_IOS || PLATFORM_TVOS || PLATFORM_BRATWURST || PLATFORM_SWITCH) && !UNITY_EDITOR
            "__Internal";
#else
            "UnityPix";
#endif
        [DllImport(DLLName)]
        public static extern uint IsPixLoaded();

        [DllImport(DLLName)]
        public static extern uint PixGpuCaptureNextFrames([MarshalAs(UnmanagedType.LPWStr)] string filename, uint numFrames);

        [DllImport(DLLName)]
        public static extern void OpenPixCapture([MarshalAs(UnmanagedType.LPWStr)] string filename);
    }
}