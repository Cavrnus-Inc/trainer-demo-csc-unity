using CavrnusSdk.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CavrnusDemo
{
    public class ProcedureBoardEntry : MonoBehaviour
    {
        [SerializeField] private Image colorBg;

        [SerializeField] private Image icon;
        [SerializeField] private Color defaultBgColor = Color.white;
        [SerializeField] private Color defaultIconColor = Color.white;
        [SerializeField] private Color completedBgColor = Color.green;
        [SerializeField] private Color activeBgColor = Color.blue;
        
        [SerializeField] private TextMeshProUGUI number;
        [SerializeField] private TextMeshProUGUI description;
        
        private int stepId;
        public void Setup(CavrnusSpaceConnection conn, string containerName, string propertyName, int id, string title)
        {
            stepId = id;
            number.text = (id + 1).ToString();
            description.text = title;

            conn.BindFloatPropertyValue(containerName, propertyName, OnTaskComplete);
        }

        private void OnTaskComplete(float incomingTask)
        {
            var incomingTaskInt = (int) incomingTask;
            
            if (stepId == incomingTaskInt) // Current id is marked active
                MarkActive();
            else if (stepId < incomingTaskInt) // Mark all prev as complete
                SetCompleteStatus(true);
            else
                SetCompleteStatus(false); // Mark as default
        }

        private void MarkActive()
        {
            icon.color = defaultIconColor;
            colorBg.color = activeBgColor;
        }
        
        private void SetCompleteStatus(bool isComplete)
        {
            if (isComplete) {
                icon.color = completedBgColor;
                colorBg.color = completedBgColor;
            }
            else {
                icon.color = defaultIconColor;
                colorBg.color = defaultBgColor;
            }
        }
    }
}