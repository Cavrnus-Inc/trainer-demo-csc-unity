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

            if (procedure.ProcedureInfo.Steps.Count > 0) {
                for (var index = 0; index < carProcedure.ProcedureInfo.Steps.Count; index++) {
                    var go = Instantiate(entryPrefab, entryContainer);
                    go.Setup(procedure.SpaceConn, procedure.ProcedureInfo.ProcedureId, procedure.ProcedureInfo.StepsId, index, carProcedure.ProcedureInfo.Steps[index].Title);
                }
            }
            else
                Debug.Log("Currently there are no steps in the procedure!!!");
            
            resetButton.onClick.AddListener(OnResetClick);
        }

        private void OnResetClick() => carProcedure.StartProcedure();
    }
}