using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.Rendering;

namespace DELTation.UnityPix.Editor
{
    [Overlay(typeof(SceneView), "PIX Tools")]
    [Icon(IconPath)]
    internal sealed class UnityPixToolbar : ToolbarOverlay, ITransientOverlay
    {
        private const string IconPath = "Packages/com.deltation.unity-pix/Assets/pix_icon.png";

        public UnityPixToolbar() : base(GetToolbarElementIds().ToArray()) { }

        public bool visible => CaptureUtility.IsSupported();

        private static IEnumerable<string> GetToolbarElementIds()
        {
            if (CaptureUtility.IsSupported())
            {
                yield return CaptureButton.ID;
            }
        }

        [EditorToolbarElement(ID, typeof(SceneView))]
        private class CaptureButton : EditorToolbarButton
        {
            public const string ID = nameof(UnityPixToolbar) + "/" + nameof(CaptureButton);

            public CaptureButton()
            {
                text = string.Empty;
                tooltip = "Make a PIX capture";
                icon = AssetDatabase.LoadAssetAtPath<Texture2D>(IconPath);
                clicked += OnClick;
            }

            private static void OnClick()
            {
                if (PIX.IsAttached())
                {
                    if (SceneView.lastActiveSceneView is { } sceneView)
                    {
                        CaptureUtility.MakeAndOpen(new SceneViewCaptureTarget(sceneView));
                    }
                    else
                    {
                        Debug.LogError("Scene View not found.");
                    }
                }
                else
                {
                    Debug.LogError("PIX is not attached.");
                }
            }
        }
    }
}