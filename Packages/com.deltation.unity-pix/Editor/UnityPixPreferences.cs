using System.Collections.Generic;
using UnityEditor;

namespace DELTation.UnityPix.Editor
{
    internal static class UnityPixPreferences
    {
        private const string CaptureFolderPathKey = nameof(UnityPixPreferences) + "_" + nameof(CaptureFolderName);

        public static string CaptureFolderName
        {
            get => EditorPrefs.GetString(CaptureFolderPathKey, "../UnityPixCaptures");

            private set => EditorPrefs.SetString(CaptureFolderPathKey, value);
        }

        [SettingsProvider]
        public static SettingsProvider CreatePreferencesProvider()
        {
            var provider = new SettingsProvider("Preferences/Unity PIX", SettingsScope.User)
            {
                label = "Unity PIX",

                guiHandler = _ =>
                {
                    EditorGUI.BeginChangeCheck();
                    string captureFolderPath = EditorGUILayout.TextField("Capture Folder (Relative)", CaptureFolderName);
                    if (EditorGUI.EndChangeCheck())
                    {
                        CaptureFolderName = captureFolderPath;
                    }
                },

                keywords = new HashSet<string>(new[] { "PIX", "UnityPIX", "Capture" }),
            };

            return provider;
        }
    }
}