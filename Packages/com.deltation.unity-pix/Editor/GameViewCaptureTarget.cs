using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEditor;

namespace DELTation.UnityPix.Editor
{
    internal sealed class GameViewCaptureTarget : ICaptureTarget
    {
        private readonly EditorWindow _gameView;
        private bool _updated;

        public GameViewCaptureTarget([NotNull] EditorWindow gameView) => _gameView = gameView ?? throw new ArgumentNullException(nameof(gameView));

        public async Task RenderAsync()
        {
            _updated = false;
            _gameView.Show();
            _gameView.Focus();
            _gameView.Repaint();

            EditorApplication.delayCall += () => _updated = true;

            while (!_updated)
            {
                await Task.Yield();
            }

            await Task.Yield();
        }

        public int NumFrames => 1;
    }
}