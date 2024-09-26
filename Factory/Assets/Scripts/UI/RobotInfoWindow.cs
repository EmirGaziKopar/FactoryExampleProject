using TMPro;
using UnityEngine;

public class RobotInfoWindow : MonoBehaviour
{
   public RobotController RobotController { get; set; }
   public string RobotName { get; set; }
  
   [SerializeField] private TextMeshProUGUI _robotNameText;
   [SerializeField] private TextMeshProUGUI _robotStateText;
   [SerializeField] private TextMeshProUGUI _robotPositionText;

   private void Update()
   {
      _robotNameText.text = RobotName;
      _robotStateText.text = RobotController.CurrentState.ToString();
      _robotPositionText.text = RobotController.transform.position.ToString();
   }
}
