using System;

namespace CavrnusDemo
{
    public interface ICustomInteractable
    {
        Action OnInteract { get; set; }

        void SetActiveState(bool state);
        void Interact();
    }
}