using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEditor;

namespace DELTation.UnityPix.Editor
{
    internal sealed class SceneViewCaptureTarget : ICaptureTarget
    {
        private readonly SceneView _sceneView;

        public SceneViewCaptureTarget([NotNull] SceneView sceneView) => _sceneView = sceneView ?? throw new ArgumentNullException(nameof(sceneView));

        public async Task RenderAsync()
        {
            _sceneView.Show();
            _sceneView.Focus();
            _sceneView.Repaint();
            _sceneView.camera.Render();
            await Task.Yield();
        }

        public int NumFrames =>

            // First present is typically a simple blit, we should wait for the next one to finish.
            2;
    }
}