using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CavrnusDemo
{
    public class Car : MonoBehaviour
    {
        [FormerlySerializedAs("Wheels")] public List<GameObject> wheels;

        public void SetWheelsVisibility(bool vis)
        {
            Debug.Log($"Setting wheels vis to {vis}");
            wheels?.ForEach(go => go.gameObject.SetActive(vis));
        }
    }
}