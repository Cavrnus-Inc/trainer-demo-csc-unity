using System;
using UnityEngine;

namespace CavrnusDemo
{
    public interface ICustomInteractable
    {
        Action<GameObject> OnInteract { get; set; }

        void SetActiveState(bool state);
        void SetVisibility(bool state);
        void Interact();
    }
}