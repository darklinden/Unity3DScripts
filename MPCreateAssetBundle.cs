using System;

using System.IO;

using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

using Object=UnityEngine.Object;

public class MPCreateAssetBundle : MonoBehaviour {
    
    private static String[] mpAssetDir = new String[]{"iphone", "ipad", "macosx", "pc"};// , "universal"}; <-- not needed, use the /Resources/ folder.
    
    [MenuItem("Custom/Monkey Prism/Create All Assetbundles #&i")]
    
    public static void Execute() {
        
        Debug.Log("Creating Assetbundles");
        
        bool blnFound = false;
        
        String currentDir = null;
        
        foreach (Object o in Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets)) {
            
            blnFound = true;
            
            if (o.name.Contains("@")) continue; //animations!
            
            String assetPath = AssetDatabase.GetAssetPath(o);
            
            if (!File.Exists(assetPath)) {
                
                currentDir = assetPath;
                
                continue; //Files only.
                
            }
            
            //Only check those directories that we have specified in the mpAssetDir
            
            Debug.Log(assetPath);
            
            String currentBuildType = null;
            
            foreach (String s in mpAssetDir) {
                
                if (assetPath.Contains("/"+s+"/")) {
                    
                    currentBuildType = s;
                    
                    break;
                    
                }
                
            }
            
            if (currentBuildType == null) continue; //if the directory is not found to be one from the mpAssetDir bail out.
            
            string assetBundleName = null, genericName = null;
            
            List<Object> toinclude = new List<Object>();
            
            //Generate pre-fabs for everything in the finished pre-fab directory.
            
            if (o.GetType() == typeof(GameObject)) {
                
                Debug.Log("GameObject " + currentDir);
                
                
                
                String d = CharacterRoot((GameObject)o);
                
                d += "materials/";
                
                Debug.Log(d);
                
                List<Material> materials = CollectAll<Material>(d);
                
                Debug.Log("materials count=" + materials.Count);
                
                genericName = o.name.ToLower();
                
                assetBundleName = currentBuildType + "-prefab-" + genericName;
                
                //Package up the prefabs in the iPhone directory.
                
                toinclude.Add(o);
                
                //Do we need to add in a material?  I think so.
                
                foreach (Material m in materials) {
                    
                    Debug.Log("Material Name=" + m.name);
                    
                    if (m.name.Contains(genericName)) {
                        
                        toinclude.Add(m);
                        
                        Debug.Log("Added a new material!");
                        
                    }
                    
                } //end foreach
                
            }
            
            if (assetBundleName == null) continue;
            
            // Create a directory to store the generated assetbundles.
            
            if (!Directory.Exists(AssetbundlePath))
                
                Directory.CreateDirectory(AssetbundlePath);
            
            // Delete existing assetbundles for current object
            
            string[] existingAssetbundles = Directory.GetFiles(AssetbundlePath);
            
            foreach (string bundle in existingAssetbundles) {
                
                if (bundle.EndsWith(".assetbundle") && bundle.Contains("/assetbundles/" + assetBundleName))
                    
                    File.Delete(bundle);
                
            }
            
            //Directories expected.
            
            Debug.Log("currentBuildType = " + currentBuildType);
            
            
            
            //path = AssetbundlePath + bundleName + ".assetbundle";
            
            if (toinclude.Count > 0) {
                
                String path = AssetbundlePath + assetBundleName + ".assetbundle";
                
                Debug.Log(path);
                
                if (currentBuildType.Equals(mpAssetDir[0]) || currentBuildType.Equals(mpAssetDir[1])) //iPhone & iPad
                    
                    BuildPipeline.BuildAssetBundle(null, toinclude.ToArray(), path, BuildAssetBundleOptions.CollectDependencies, BuildTarget.iPhone);
                
                else //TODO: might need to condition further and might want to use an enum with the conditional.
                    
                    BuildPipeline.BuildAssetBundle(null, toinclude.ToArray(), path, BuildAssetBundleOptions.CollectDependencies);
                
            }
            
        } //end foreach
        
        if (!blnFound) {
            
            Debug.Log("no objects were found for building assets with.");
            
        }
        
    }
    
    public static string AssetbundlePath
    
    {
        
        get { return "assetbundles" + Path.DirectorySeparatorChar; }
        
    }
    
    // This method loads all files at a certain path and
    
    // returns a list of specific assets.
    
    public static List<T> CollectAll<T>(string path) where T : Object
    
    {
        
        List<T> l = new List<T>();
        
        string[] files = Directory.GetFiles(path);
        
        foreach (string file in files)
        
        {
            
            if (file.Contains(".meta")) continue;
            
            T asset = (T) AssetDatabase.LoadAssetAtPath(file, typeof(T));
            
            if (asset == null) throw new Exception("Asset is not " + typeof(T) + ": " + file);
            
            l.Add(asset);
            
        }
        
        return l;
        
    }
    
    // Returns the path to the directory that holds the specified FBX.
    
    static string CharacterRoot(GameObject character)
    
    {
        
        string root = AssetDatabase.GetAssetPath(character);
        
        return root.Substring(0, root.LastIndexOf('/') + 1);
        
    }
    
}