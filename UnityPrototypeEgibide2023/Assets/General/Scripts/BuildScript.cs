using System;
using UnityEditor;
using UnityEngine;

public class BuildScript
{
    [MenuItem("Build/Build With Current Settings")]
    public static void BuildWithCurrentSettings()
    {
        // Get the array of EditorBuildSettingsScene objects
        EditorBuildSettingsScene[] editorBuildSettingsScenes = EditorBuildSettings.scenes;

        
        // Get the current date to create a unique folder for each build
        string date = DateTime.Now.ToString("yyyy-MM-dd");
        string buildPath = $"Builds/{date}/";
        
        // Transform the array into an array of string paths
        string[] scenePaths = new string[editorBuildSettingsScenes.Length];
        for (int i = 0; i < editorBuildSettingsScenes.Length; i++)
        {
            if (editorBuildSettingsScenes[i].enabled) 
            {
                scenePaths[i] = editorBuildSettingsScenes[i].path;
            }
        }

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scenePaths,
            locationPathName = buildPath + "YourGameName",
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        };

        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}