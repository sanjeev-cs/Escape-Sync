using UnityEngine; // Namespace provides core unity funtionalities like GameObjects, Transform etc.
using UnityEditor; // Namespace provides tools for creating cutom editor 
using System.IO; // Namespace provides basic C# classes and functionalities like firl I\O, exceptions, and more. 

// Static class can only have static methods
public static class MySetup
{
    // Adds a custome menu item in the unity editor: "My Setup > Create Folders" and the below method "CreateMyFolders()" automatically assined to this menu item.
    [MenuItem("My Setup/Create Folders")]
    public static void CreateMyFolders()
    {
        Folders.CreateFolders("_Project", "Animation", "Art", "Audio", "Fonts", "Materials", "Prefabs", "ScriptableObjects", "Scripts", "Settings");
        // Refresh the Asset Database to make the new folders visible in the Unity Editor.
        AssetDatabase.Refresh();
    }
    static class Folders
    {
        /* 
            "root" : the root directory where all folders will be created
            "params" : allows to pass multiple folder names as arguments.
            "Application.datapath" : returns the absolute path of the assets folder
            "Path.Combine" : combines muliple string    
            "var" : auto assigns the varible type
        */
        public static void CreateFolders(string root, params string[] folders)
        {
            // Combine game assets folder path with root path(here we will name it "_Project" and assigns to the full path
            var fullPath = Path.Combine(Application.dataPath, root);
            foreach (string folder in folders)
            {
                // Combine the full path(root folder) with the current folder name and creates the complete path for the current folder
                var folderPath = Path.Combine(fullPath, folder);
                // Check if the folder is already there
                if (!Directory.Exists(folderPath))
                {
                    // If the folder is not there then creates it
                    Directory.CreateDirectory(folderPath);
                }
            }
        }
    }
}
