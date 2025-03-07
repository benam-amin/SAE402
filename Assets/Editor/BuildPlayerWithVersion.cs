using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;
using System.IO;
using System.Text.RegularExpressions;

// https://stackoverflow.com/questions/75630802/create-new-folder-for-the-project-to-get-build-into-throught-script-c-sharp
public class BuildPlayerWithVersion : MonoBehaviour
{
    [MenuItem("**Debug**/Create build")]
    public static void CreateBuild()
    {
        BuildPlayerOptions buildPlayerOptions = BuildPlayerWindow.DefaultBuildMethods.GetBuildPlayerOptions(new BuildPlayerOptions());
        string version = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

        string productNameWithExt = Regex.Match(buildPlayerOptions.locationPathName, @"" + Application.productName + ".([A-z])\\w+").Groups[0].ToString();
        string path = buildPlayerOptions.locationPathName.Replace(productNameWithExt, "");

        string finalPath = Path.Join(path, version);
        Directory.CreateDirectory(finalPath);

        buildPlayerOptions.locationPathName = $"{finalPath}/{productNameWithExt}";

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Application.OpenURL($"file://{finalPath}");
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }
}