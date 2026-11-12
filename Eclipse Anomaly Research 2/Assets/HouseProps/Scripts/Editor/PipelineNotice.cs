using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

namespace FS_HousePropsVol2
{
    [InitializeOnLoad]
    public class PipelineNotice : AssetPostprocessor
    {
        private static readonly string SHOWN_KEY = "FinwardStudios_PipelineNoticeShown_" + Application.dataPath.GetHashCode();

        static PipelineNotice()
        {
            // Delay call to ensure Unity Editor graphics settings are fully loaded
            EditorApplication.delayCall += CheckRenderPipeline;
        }

        // OnPostprocessAllAssets reliably triggers right after importing a package in Unity 6
        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            CheckRenderPipeline();
        }

        private static void CheckRenderPipeline()
        {
            // If it has ALREADY popped up once in this project, never pop up automatically again
            if (EditorPrefs.GetBool(SHOWN_KEY, false))
                return;

            // In Unity 6, GraphicsSettings.currentRenderPipeline directly returns 
            // the active RenderPipelineAsset across Quality Settings and Default Settings.
            RenderPipelineAsset currentPipeline = GraphicsSettings.currentRenderPipeline;

            // Fallback for asset checks prior to initial frame render
            if (currentPipeline == null)
            {
                currentPipeline = QualitySettings.renderPipeline != null
                    ? QualitySettings.renderPipeline
                    : GraphicsSettings.defaultRenderPipeline;
            }

            // Built-in Render Pipeline Detection (null pipeline asset)
            if (currentPipeline == null)
            {
                EditorPrefs.SetBool(SHOWN_KEY, true);

                ShowDialog(
                    "Built-in Render Pipeline detected.\n\nImport Built-in compatibility package found in /Built-In folder."
                );
                return;
            }

            string pipelineTypeName = currentPipeline.GetType().Name;
            string pipelineFullTypeName = currentPipeline.GetType().FullName ?? string.Empty;
            string pipelineAssetName = currentPipeline.name;

            // URP Detection (If active, set shown flag and exit)
            if (pipelineTypeName.Contains("Universal", System.StringComparison.OrdinalIgnoreCase) ||
                pipelineFullTypeName.Contains("Universal", System.StringComparison.OrdinalIgnoreCase))
            {
                EditorPrefs.SetBool(SHOWN_KEY, true);
                return;
            }

            // HDRP Detection (Checks type short name, full namespace type name, and asset object name)
            bool isHDRP = pipelineTypeName.Contains("HighDefinition", System.StringComparison.OrdinalIgnoreCase) ||
                         pipelineTypeName.Contains("HDRP", System.StringComparison.OrdinalIgnoreCase) ||
                         pipelineTypeName.Contains("High Fidelity", System.StringComparison.OrdinalIgnoreCase) ||
                         pipelineFullTypeName.Contains("HighDefinition", System.StringComparison.OrdinalIgnoreCase) ||
                         pipelineFullTypeName.Contains("HDRP", System.StringComparison.OrdinalIgnoreCase) ||
                         pipelineAssetName.Contains("HDRP", System.StringComparison.OrdinalIgnoreCase) ||
                         pipelineAssetName.Contains("HighDefinition", System.StringComparison.OrdinalIgnoreCase);

            if (isHDRP)
            {
                EditorPrefs.SetBool(SHOWN_KEY, true);

                ShowDialog(
                    "High Definition Render Pipeline detected.\n\nImport High Definition compatibility package found in /HDRP folder."
                );
            }
        }

        private static void ShowDialog(string message)
        {
            EditorUtility.DisplayDialog(
                "Finward Studios - Render Pipeline Notice",
                message,
                "OK"
            );
        }

        // Menu item to re-trigger the check manually under Finward Studios
        /*
        [MenuItem("FinwardStudios/Check Pipeline Notice")]
        public static void ManualCheck()
        {
          EditorPrefs.DeleteKey(SHOWN_KEY);
          CheckRenderPipeline();
        }
        */
}
}