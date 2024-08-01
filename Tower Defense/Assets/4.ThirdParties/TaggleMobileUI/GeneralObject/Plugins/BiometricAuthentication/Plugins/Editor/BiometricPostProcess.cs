#if UNITY_IOS
	using System.IO;
	using UnityEngine;
	using UnityEditor;
	using UnityEditor.Callbacks;
	using UnityEditor.iOS.Xcode;

public class BiometricPostProcess
{

	[PostProcessBuild(100)]
	public static void OnPostProcessBuild(BuildTarget buildTarget, string buildPath)
	{
		if (buildTarget == BuildTarget.iOS)
		{
			// Get plist
			string plistPath = buildPath + "/Info.plist";
			PlistDocument plist = new PlistDocument();
			plist.ReadFromString(File.ReadAllText(plistPath));
       
			// Get root
			PlistElementDict rootDict = plist.root;
       
			// Change value of CFBundleVersion in Xcode plist
			var buildKey = "NSFaceIDUsageDescription";
			rootDict.SetString(buildKey,"YES");
       
			// Write to file
			File.WriteAllText(plistPath, plist.WriteToString());
			
		}
	}
	
	private static void CopyRaw(string pathToBuiltProject)
	{
		string inDir = Path.Combine("Plugins/iOS/BiometricAuthentication/", "Raw");
		if (Directory.Exists(inDir))
		{
			string outDir = Path.Combine(pathToBuiltProject, "Data/Raw");
			if (!Directory.Exists(outDir))
			{
				Directory.CreateDirectory(outDir);
			}

			FileInfo[] fileInfos = new DirectoryInfo(inDir).GetFiles();
			if (fileInfos != null)
			{
				foreach (FileInfo fileInfo in fileInfos)
				{
					if (!fileInfo.Name.EndsWith(".meta"))
					{
						File.Copy(fileInfo.FullName, Path.Combine(outDir, fileInfo.Name), true);
					}
				}
			}
		}
	}
}

#endif