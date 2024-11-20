using UnityEditor;
using UnityEngine;

public class AddMeshColliders : EditorWindow
{
    [MenuItem("Tools/Add Mesh Colliders to Objects with Mesh Renderer")]
    public static void AddMeshCollidersToAll()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        int colliderCount = 0;

        foreach (GameObject obj in allObjects)
        {
            MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
            MeshCollider meshCollider = obj.GetComponent<MeshCollider>();

            if (meshRenderer != null && meshCollider == null)
            {
                obj.AddComponent<MeshCollider>();
                colliderCount++;
            }
        }

        Debug.Log($"Added MeshColliders to {colliderCount} objects.");
    }
}
