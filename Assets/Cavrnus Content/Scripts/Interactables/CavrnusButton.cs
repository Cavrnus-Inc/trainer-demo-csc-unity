using System;
using UnityEngine;

namespace CavrnusDemo {
    public class CavrnusButton : MonoBehaviour, ICustomInteractable
    {
        [SerializeField] private Collider col;
        
        [SerializeField] private Animator animator;
        [SerializeField] private Outline outline;
        
        private static readonly int Press = Animator.StringToHash("Press");

        public Action<GameObject> OnInteract { get; set; }
        public void SetActiveState(bool state)
        {
            enabled = state;
            outline.enabled = state;
            col.enabled = state;
        }
        
        public void SetVisibility(bool state)
        {
            gameObject.SetActive(state);
        }

        public void Interact()
        {
            SetActiveState(false);
            animator.SetTrigger(Press);
            OnInteract?.Invoke(gameObject);
        }
    }
}