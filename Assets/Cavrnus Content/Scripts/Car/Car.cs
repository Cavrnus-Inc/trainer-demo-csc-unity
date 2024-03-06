using System.Collections.Generic;
using UnityEngine;

namespace CavrnusDemo
{
    public class Car : MonoBehaviour
    {
        public List<Tire> TiresOnCar;

        public void SetAllTiresVisibility(bool vis)
        {
            Debug.Log($"Setting wheels vis to {vis}");
            TiresOnCar?.ForEach(go => go.gameObject.SetActive(vis));
        }

        public void SetTireVisibility(Tire.TireTypeEnum tire, bool vis)
        {
            foreach (var carTire in TiresOnCar) {
                if (carTire.TireType == tire) {
                    carTire.SetVisibility(vis);
                }
            }
        }
    }
}