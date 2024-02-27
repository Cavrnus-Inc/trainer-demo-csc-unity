using UnityEngine;
using UnityEngine.UI;

namespace CavrnusDemo
{
    public class ProcedureBoard : MonoBehaviour
    {
        [SerializeField] private ProcedureBoardEntry entryPrefab;
        [SerializeField] private Transform entryContainer;

        [SerializeField] private Button resetButton;

        private CarLiftProcedure carProcedure;
        
        public void Setup(CarLiftProcedure procedure)
        {
            carProcedure = procedure;

            if (carProcedure.Steps.Count > 0) {
                for (var index = 0; index < carProcedure.Steps.Count; index++) {
                    var go = Instantiate(entryPrefab, entryContainer);
                    go.Setup(procedure.SpaceConn, procedure.containerName, procedure.propertyName, index, carProcedure.Steps[index]);
                }
            }
            else
                Debug.Log("Currently there are no steps in the procedure!!!");
            
            resetButton.onClick.AddListener(OnResetClick);
        }

        private void OnResetClick() => carProcedure.StartProcedure();
    }
}