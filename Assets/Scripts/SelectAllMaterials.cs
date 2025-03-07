using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SelectAllMaterials : EditorWindow
{
    [MenuItem("Tools/Select All Materials in Scene")]
    static void SelectMaterials()
    {
        // Get all renderers in the scene
        Renderer[] renderers = FindObjectsOfType<Renderer>();
        HashSet<Material> uniqueMaterials = new HashSet<Material>();

        // Collect all unique materials
        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.sharedMaterials)
            {
                if (mat != null)
                    uniqueMaterials.Add(mat);
            }
        }

        // Convert to an array of UnityEngine.Object
        Object[] materialsArray = new Object[uniqueMaterials.Count];
        int index = 0;
        foreach (Material mat in uniqueMaterials)
        {
            materialsArray[index] = mat;
            index++;
        }

        // Select in Project window
        Selection.objects = materialsArray;

        Debug.Log($"Selected {materialsArray.Length} materials in the Project window.");
    }
}
