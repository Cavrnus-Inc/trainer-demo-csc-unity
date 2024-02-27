using System;
using CavrnusDemo;
using UnityEngine;
using UnityEngine.UI;

public class TrainingCompleteUI : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    
    private CarLiftProcedure proc;

    private Action onRestartPressed;
    
    public void Setup(CarLiftProcedure proc, Action onRestartPressed)
    {
        this.proc = proc;
        this.onRestartPressed = onRestartPressed;
        restartButton.onClick.AddListener(RestartButtonPressed);
    }

    private void RestartButtonPressed()
    {
        proc.StartProcedure();
        onRestartPressed?.Invoke();
    }
}
