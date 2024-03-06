using System;
using UnityEngine;

namespace CavrnusDemo
{
    public class Tire : MonoBehaviour, ICustomInteractable
    {
        public enum TireTypeEnum
        {
            FL = 0,
            FR = 1,
            BL = 2,
            BR = 3,
        }

        [SerializeField] private Outline outline;
        [SerializeField] private Collider col;
        
        public TireTypeEnum TireType;
        
        public Action<GameObject> OnInteract{ get; set; }

        public void SetActiveState(bool state)
        {
            outline.enabled = state;
            col.enabled = state;
        }

        public void SetVisibility(bool state)
        {
            gameObject.SetActive(state);
        }

        public void Interact()
        {
            OnInteract?.Invoke(gameObject);
        }
    }
}