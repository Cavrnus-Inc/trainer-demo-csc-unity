using System;
using System.Collections.Generic;
using System.Linq;
using CavrnusSdk.API;
using UnityEngine;

namespace CavrnusDemo
{
    public class TireInstallationManager : MonoBehaviour    
    {
        [SerializeField] private Car car;
        [SerializeField] private List<Tire> tiresToInstall;
        public CavrnusSpaceConnection SpaceConn{ get; set; }

        private const string ContainerName = "TireInstallation";

        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(csc => {
                SpaceConn = csc;
            });
        }

        private Action onComplete;
        private Tire.TireTypeEnum[] tires;
        private List<IDisposable> disp = new List<IDisposable>();
        public void SetInstallationStatus(Action onComplete, params Tire.TireTypeEnum [] tires)
        {
            this.onComplete = onComplete;
            this.tires = tires;
            
            foreach (var tireType in tires)
            {
                var tireToInstall = tiresToInstall.FirstOrDefault(tire => tire.TireType == tireType);
                if (tireToInstall != null)
                {
                    if (IsTireInstalled(tireType))
                    {
                        car.SetTireVisibility(tireType, true);
                        SetTireVisibility(tireType, false);
                    }
                    else
                    {
                        car.SetTireVisibility(tireType, false);
                        SetTireVisibility(tireType, true);
                        SetTireInteractable(tireType, true);
                        
                        tireToInstall.OnInteract += OnTireInstalled;
                    }
                }
                else
                {
                    Debug.Log($"Tire type: {tireType} not found in list!");
                }
            }
        }

        private void OnTireInstalled(GameObject go)
        {
            var tire = go.GetComponent<Tire>().TireType;
            
            Debug.Log("Install Tire and Post");
            SpaceConn.PostBoolPropertyUpdate(ContainerName,tire.ToString(),true);

            disp.Add(SpaceConn.BindBoolPropertyValue(ContainerName, tire.ToString(), OnTireInstallPostComplete));

            SetTireVisibility(tire, false);
            SetTireInteractable(tire, false);
            car.SetTireVisibility(tire, true);
        }

        private void OnTireInstallPostComplete(bool obj)
        {
            if (AllTiresInstalled())
                onComplete?.Invoke();
        }

        public void SetTireVisibility(Tire.TireTypeEnum tire, bool vis)
        {
            foreach (var carTire in tiresToInstall) {
                if (carTire.TireType == tire)
                    carTire.SetVisibility(vis);
            }
        }
        
        public void SetTireInteractable(Tire.TireTypeEnum tire, bool state)
        {
            foreach (var carTire in tiresToInstall) {
                if (carTire.TireType == tire)
                    carTire.SetActiveState(state);
            }
        }

        public void ResetTires()
        {
            Debug.Log("ResetTires");

            foreach (var tire in tiresToInstall)
                SpaceConn.PostBoolPropertyUpdate(ContainerName,tire.TireType.ToString(),false);
            
            car.SetAllTiresVisibility(true);

            foreach (var d in disp)
                d.Dispose();
        }

        private bool AllTiresInstalled()
        {
            var val = tires.All(tire => {
                var v = SpaceConn.GetBoolPropertyValue(ContainerName, tire.ToString());
                Debug.Log($"{tire} is installed: {v}");

                return v;
            });

            return val;
        }

        private bool IsTireInstalled(Tire.TireTypeEnum tire)
        {
            return SpaceConn.GetBoolPropertyValue(ContainerName, tire.ToString());
        }
    }
}