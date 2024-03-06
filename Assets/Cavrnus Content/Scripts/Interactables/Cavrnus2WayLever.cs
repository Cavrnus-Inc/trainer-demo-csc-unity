using System;
using System.Collections;
using UnityEngine;

namespace CavrnusDemo
{
    public class Cavrnus2WayLever : MonoBehaviour, ICustomInteractable
    {
        public enum LeverStateEnum
        {
            Activated,
            Deactivated
        }
        
        public Action<GameObject> OnInteract { get; set; }

        [SerializeField] private float rotActive;
        [SerializeField] private float rotDeactive;
        [SerializeField] private Transform target;
        
        [SerializeField] private Collider col;
        [SerializeField] private Outline outline;

        private LeverStateEnum NextState { get; set; }
        
        public void SetActiveState(bool state)
        {
            col.enabled = state;
            outline.enabled = state;
        }
        
        public void SetVisibility(bool state)
        {
            gameObject.SetActive(state);
        }

        public void ForceRotationTo(LeverStateEnum lse)
        {
            var curRot = target.transform.localEulerAngles;
            if (lse == LeverStateEnum.Activated)
            {
                target.transform.localEulerAngles = new Vector3(rotActive, curRot.y, curRot.z);
                NextState = LeverStateEnum.Deactivated;
            }
            else
            {
                target.transform.localEulerAngles = new Vector3(rotDeactive, curRot.y, curRot.z);
                NextState = LeverStateEnum.Activated;
            }
        }

        public void Interact()
        {
            SetActiveState(false);
            StartCoroutine(ActivateRoutine());
            
            OnInteract?.Invoke(gameObject);
        }

        private IEnumerator ActivateRoutine()
        {
            var timeElapsed = 0f;

            var startRotation = target.localRotation;
            var dir = NextState == LeverStateEnum.Activated ? rotActive : rotDeactive;
            var newRot = new Vector3(dir, startRotation.y, startRotation.z);
            var targetRotation = Quaternion.Euler(newRot);

            while (timeElapsed < 1.5f)
            {
                var t = timeElapsed / 1.5f;
                target.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);
                timeElapsed += Time.deltaTime;
                
                yield return null;
            }

            target.localRotation = targetRotation;
        }
    }
}