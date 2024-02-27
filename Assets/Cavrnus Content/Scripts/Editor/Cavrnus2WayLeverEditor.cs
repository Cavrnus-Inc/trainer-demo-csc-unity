using UnityEditor;
using UnityEngine;

namespace CavrnusDemo
{
    [CustomEditor(typeof(Cavrnus2WayLever))]
    public class Cavrnus2WayLeverEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var script = (Cavrnus2WayLever)target;

            if (GUILayout.Button("Interact"))
            {
                script.Interact();
            }
        }
    }
}