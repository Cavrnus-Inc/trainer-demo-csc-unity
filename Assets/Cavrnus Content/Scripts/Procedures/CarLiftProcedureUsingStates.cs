using System.Collections.Generic;
using CavrnusSdk.API;
using UnityEngine;

namespace CavrnusDemo
{
    public class CarLiftProcedureUsingStates : MonoBehaviour
    {
        [Header("Procedure Steps")]
        public List<string> Steps;
        
        [Header("UI")]
        [SerializeField] private ProcedureBoard boardPrefab;
        [SerializeField] private Transform boardSpawnLocation;
        
        [Space]
        [SerializeField] private TrainingCompleteUI trainingCompleteUI;
        
        [Header("Managers")]
        [SerializeField] private Car car;
        [SerializeField] private CarLift carLift;
        [SerializeField] private TireInstallationManager tireInstallation;
        [SerializeField] private LerpPositionManager lerpPositionManager;
        
        [Header("Buttons and Levers")]
        [SerializeField] private Cavrnus2WayLever liftCarLever;
        [SerializeField] private CavrnusButton moveCarButton;
        [SerializeField] private CavrnusButton changeWheelsButton;

        private ProcedureBoard spawnedProcedureBoard;

        public CavrnusSpaceConnection SpaceConn;
        public string propertyName => "CurrentStep";
        public string containerName => "CarLiftProcedure";
        
        private void Start()
        {
            // Everything should be disabled until space join
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(csc => {
                SpaceConn = csc;
            });
        }
    }
}