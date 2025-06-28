using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace DELTation.UnityPix.Editor
{
    internal static class GameViewCaptureTool
    {
        private const string CaptureMenuItemPath = "Tools/Unity PIX/Capture Game View";

        [MenuItem(CaptureMenuItemPath)]
        public static void Capture()
        {
            if (TryGetGameViewWindow(out EditorWindow gameView))
            {
                CaptureUtility.MakeAndOpen(new GameViewCaptureTarget(gameView));
            }
            else
            {
                Debug.LogError("Game View window not found.");
            }
        }

        private static bool TryGetGameViewWindow(out EditorWindow gameView)
        {
            var gameViewType = Type.GetType("UnityEditor.GameView, UnityEditor");
            Assert.IsNotNull(gameViewType);
            gameView = EditorWindow.GetWindow(gameViewType);
            return gameView != null;
        }

        [MenuItem(CaptureMenuItemPath, true)]
        private static bool CaptureValidate() => CaptureUtility.IsSupported() && TryGetGameViewWindow(out EditorWindow _);
    }
}