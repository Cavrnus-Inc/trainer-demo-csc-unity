using System;
using System.Collections.Generic;
using CavrnusSdk.API;
using UnityEngine;

namespace CavrnusDemo
{
    public class CarLiftProcedure : MonoBehaviour
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
            trainingCompleteUI.Setup(this, OnRestartButtonPressed);
            
            // Everything should be disabled until space join
            DeactivateAllItems();
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(csc => {
                SpaceConn = csc;
                SetupUI();
                SetupSteps();
            });
        }
        
        private void OnRestartButtonPressed()
        {
            trainingCompleteUI.gameObject.SetActive(false);
        }

        private void DeactivateAllItems()
        {
            car.SetWheelsVisibility(true);
            liftCarLever.SetActiveState(false);
            moveCarButton.SetActiveState(false);
            changeWheelsButton.SetActiveState(false);
            trainingCompleteUI.gameObject.SetActive(false);
        }
        
        public void StartProcedure()
        {
            DeactivateAllItems();
            ResetItemTransforms();
            UnregisterEverything();
            
            SpaceConn.PostFloatPropertyUpdate(containerName, propertyName, 0);
        }
        
        private void ResetItemTransforms()
        {
            carLift.ResetTransform();
            lerpPositionManager.ResetToStart();
        }

        private void SetupUI()
        {
            spawnedProcedureBoard = Instantiate(boardPrefab, boardSpawnLocation);
            spawnedProcedureBoard.Setup(this);
        }
                
        private void PostNextStepToActivate(int stepCompleted)
        {
            UnregisterEverything();
            SpaceConn.PostFloatPropertyUpdate(containerName, propertyName, stepCompleted);
        }

        private void SetupSteps()
        {
            // Default to first step. Also creates new container with def value if doesn't exist
            SpaceConn.DefineFloatPropertyDefaultValue(containerName, propertyName, 0); 
            SpaceConn.BindFloatPropertyValue(containerName, propertyName, currentStep => {
                switch (currentStep) {
                    case 0:
                        MoveCarToLiftStep();
                        break;
                    case 1:
                        LiftCarStep();
                        break;
                    case 2:
                        RemoveTiresStep();
                        break;
                    case 3:
                        InstallTiresStep();
                        break;
                    case 4:
                        LowerCarStep();
                        break;
                    case 5:
                        MoveCarFinalStep();
                        break;
                    case 6:
                        ShowRestartProcedureUI();
                        break;
                    default: 
                        throw new Exception("Invalid step number received!");
                }
            });
        }
        
        #region Steps

        private void StepEntry()
        {
            UnregisterEverything();
            DeactivateAllItems();
        }

        #region Step 0 -- Move Car to Lift

        private void MoveCarToLiftStep()
        {
            Debug.Log($"Starting {nameof(MoveCarToLiftStep)}");

            StepEntry();
            
            moveCarButton.SetActiveState(true);
            
            moveCarButton.OnInteract += MoveCarToLiftButtonPress;
            lerpPositionManager.OnWayPointReached.AddListener(LiftPositionReached);
        }
        
        private void MoveCarToLiftButtonPress()
        {
            lerpPositionManager.TargetPosition(LerpPositionManager.PositionEnum.AutoLift);
        }

        private void LiftPositionReached()
        {
            PostNextStepToActivate(1);
        }

        #endregion

        #region Step 1 -- Lift car up

        private void LiftCarStep()
        {
            Debug.Log("Starting Step One...");
            
            StepEntry();
            
            carLift.enabled = true;
            liftCarLever.SetActiveState(true);
            liftCarLever.ForceRotationTo(Cavrnus2WayLever.LeverStateEnum.Deactivated);

            liftCarLever.OnInteract += LeverPulledToRaiseLift;
            carLift.OnTopReached.AddListener(AutoLiftTopReached);
        }
        
        private void LeverPulledToRaiseLift()
        {
            carLift.Raise();
            Debug.Log("Lever Activated");  
        }
        
        private void AutoLiftTopReached()
        {
            PostNextStepToActivate(2);
        }

        #endregion

        #region Step 2 -- Uninstall Tires

        private void RemoveTiresStep()
        {
            Debug.Log("Starting Step Two...");

            StepEntry();
            carLift.enabled = false;
            changeWheelsButton.SetActiveState(true);
            liftCarLever.ForceRotationTo(Cavrnus2WayLever.LeverStateEnum.Activated);
            changeWheelsButton.OnInteract += WheelsButtonRemovePressed;
        }

        private void WheelsButtonRemovePressed()
        {
            PostNextStepToActivate(3);
        }

        #endregion
        
        #region Step 3 -- Install Tires

        private void InstallTiresStep()
        {
            Debug.Log("Starting Step Three...");
            StepEntry();
            car.SetWheelsVisibility(false);
            changeWheelsButton.SetActiveState(true);
            liftCarLever.ForceRotationTo(Cavrnus2WayLever.LeverStateEnum.Activated);
            changeWheelsButton.OnInteract += WheelButtonPressedInstalled;
        }

        private void WheelButtonPressedInstalled()
        {
            PostNextStepToActivate(4);
        }

        private void LeverLowerLift()
        {
            carLift.Lower();
        }

        #endregion
        
        #region Step 4 -- Lower Car

        private void LowerCarStep()
        {
            car.SetWheelsVisibility(true);
            Debug.Log("Starting Step Four...");
            
            StepEntry();
            
            liftCarLever.SetActiveState(true);
            carLift.ForceState(CarLift.PositionEnum.Top);

            liftCarLever.ForceRotationTo(Cavrnus2WayLever.LeverStateEnum.Activated);
            liftCarLever.OnInteract += LeverPulledToLowerLift;
            carLift.OnBottomReached.AddListener(OnLiftBottomReached);
        }

        private void LeverPulledToLowerLift()
        {
            carLift.Lower();
        }

        private void OnLiftBottomReached()
        {
            PostNextStepToActivate(5);
        }
               
        #endregion
        
        #region Procedure Complete

        private void MoveCarFinalStep()
        {
            Debug.Log("Starting final step...");
            StepEntry();

            moveCarButton.SetActiveState(true);
            moveCarButton.OnInteract += FinalButtonPressToFinish;
            lerpPositionManager.OnWayPointReached.AddListener(FinalPositionReached);
        }

        private void FinalPositionReached()
        {
            PostNextStepToActivate(6);
        }

        private void FinalButtonPressToFinish()
        {
            lerpPositionManager.TargetPosition(LerpPositionManager.PositionEnum.End);
        }
        
        #endregion

        #region Restart UI Step
        
        private void ShowRestartProcedureUI()
        {
            StepEntry();
            trainingCompleteUI.gameObject.SetActive(true);
        }

        #endregion

        private void UnregisterEverything()
        {
            moveCarButton.OnInteract -= FinalButtonPressToFinish;
            lerpPositionManager.OnWayPointReached.RemoveListener(FinalPositionReached);
            
            liftCarLever.OnInteract -= LeverLowerLift;
            carLift.OnBottomReached.RemoveListener(OnLiftBottomReached);
            
            liftCarLever.OnInteract -= LeverPulledToRaiseLift;
            carLift.OnTopReached.RemoveListener(AutoLiftTopReached);
            
            moveCarButton.OnInteract -= MoveCarToLiftButtonPress;
            lerpPositionManager.OnWayPointReached.RemoveListener(LiftPositionReached);
            changeWheelsButton.OnInteract -= WheelsButtonRemovePressed;
            changeWheelsButton.OnInteract -= WheelButtonPressedInstalled;
        }
        
        #endregion
        
        #region Editor Stuff

        public void NextStep()
        {
            if (SpaceConn == null) return;

            var currentStep = SpaceConn.GetFloatPropertyValue(containerName, propertyName);

            if (currentStep + 1 < Steps.Count) {
                SpaceConn.PostFloatPropertyUpdate(containerName, propertyName, currentStep + 1);
            }
        }

        public void PreviousStep()
        {
            if (SpaceConn == null) return;

            var currentStep = SpaceConn.GetFloatPropertyValue(containerName, propertyName);

            if (currentStep - 1 >= 0) {
                SpaceConn.PostFloatPropertyUpdate(containerName, propertyName, currentStep - 1);
            }
        }
        
        #endregion
    }
}