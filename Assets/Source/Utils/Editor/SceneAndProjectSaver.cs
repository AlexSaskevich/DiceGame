#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Source.Utils.Editor
{
    public class SceneAndProjectSaver : MonoBehaviour
    {
        [MenuItem("File/Save Scene And Project %#&s")]
        private static void SaveSceneAndProject()
        {
            EditorApplication.ExecuteMenuItem("File/Save");
            EditorApplication.ExecuteMenuItem("File/Save Project");
            Debug.Log("Saved scene and project");
        }
    }
}
#endif