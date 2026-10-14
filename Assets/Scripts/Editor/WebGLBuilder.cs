using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace SanAndreasUnity.Editor
{
    public static class WebGLBuilder
    {
        // CI entrypoint: -executeMethod SanAndreasUnity.Editor.WebGLBuilder.BuildForCI
        public static void BuildForCI()
        {
            string buildPath = Environment.GetEnvironmentVariable("SAU_WEBGL_BUILD_PATH");
            if (string.IsNullOrWhiteSpace(buildPath))
                buildPath = Path.Combine(Application.dataPath, "../build/WebGL");

            Debug.Log($"[WebGLBuilder] Building WebGL to: {buildPath}");
            Directory.CreateDirectory(buildPath);

            var options = new BuildPlayerOptions
            {
                scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes),
                locationPathName = buildPath,
                target = BuildTarget.WebGL,
                options = BuildOptions.None,
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                throw new Exception($"WebGL build failed with result: {report.summary.result}");
            }

            Debug.Log($"[WebGLBuilder] Success. Size: {report.summary.totalSize} bytes");
        }

        [MenuItem(EditorCore.MenuName + "/Build WebGL")]
        public static void BuildFromMenu()
        {
            BuildForCI();
        }
    }
}
