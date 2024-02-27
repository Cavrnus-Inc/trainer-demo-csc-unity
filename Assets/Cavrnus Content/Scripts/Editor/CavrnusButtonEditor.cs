using UnityEditor;
using UnityEngine;

namespace CavrnusDemo
{
    [CustomEditor(typeof(CavrnusButton))]
    public class CavrnusButtonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var script = (CavrnusButton)target;

            if (GUILayout.Button("Interact"))
                script.Interact();
        }
    }
}