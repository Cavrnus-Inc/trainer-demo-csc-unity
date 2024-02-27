using UnityEditor;
using UnityEngine;

namespace CavrnusDemo
{
    [CustomEditor(typeof(CarLiftProcedure))]
    public class CarLiftProcedureEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var script = (CarLiftProcedure) target;
            
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Next")) {
                script.NextStep();
            }

            if (GUILayout.Button("Previous")) {
                script.PreviousStep();
            }

            if (GUILayout.Button("Reset")) {
                script.StartProcedure();
            }
            
            GUILayout.EndHorizontal();
        }
    }
}