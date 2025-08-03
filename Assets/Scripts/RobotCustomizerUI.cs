using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RobotCustomizerUI : MonoBehaviour
{
   public RobotCustomizer customizer;

       [Header("Cabeza")]
       public Button leftHeadButton;
       public Button rightHeadButton;
       public TextMeshProUGUI headNameText;

       [Header("Tronco")]
       public Button leftTrunkButton;
       public Button rightTrunkButton;
       public TextMeshProUGUI trunkNameText;

       [Header("Brazos")]
       public Button leftArmsButton;
       public Button rightArmsButton;
       public TextMeshProUGUI armsNameText;

       [Header("Piernas")]
       public Button leftLegsButton;
       public Button rightLegsButton;
       public TextMeshProUGUI legsNameText;

       [Header("Llave")]
       public Button leftKeyButton;
       public Button rightKeyButton;
       public TextMeshProUGUI keyNameText;

       void Start()
       {
           leftHeadButton.onClick.AddListener(() => customizer.PreviousHead());
           rightHeadButton.onClick.AddListener(() => customizer.NextHead());

           leftTrunkButton.onClick.AddListener(() => customizer.PreviousTrunk());
           rightTrunkButton.onClick.AddListener(() => customizer.NextTrunk());

           leftArmsButton.onClick.AddListener(() => customizer.PreviousArms());
           rightArmsButton.onClick.AddListener(() => customizer.NextArms());

           leftLegsButton.onClick.AddListener(() => customizer.PreviousLegs());
           rightLegsButton.onClick.AddListener(() => customizer.NextLegs());

           leftKeyButton.onClick.AddListener(() => customizer.PreviousKey());
           rightKeyButton.onClick.AddListener(() => customizer.NextKey());
       }

        public void BackMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

       void Update()
       {
           headNameText.text = customizer.GetCurrentPartName(RobotCustomizer.PartType.Head);
           trunkNameText.text = customizer.GetCurrentPartName(RobotCustomizer.PartType.Trunk);
           armsNameText.text = customizer.GetCurrentPartName(RobotCustomizer.PartType.Arms);
           legsNameText.text = customizer.GetCurrentPartName(RobotCustomizer.PartType.Legs);
           keyNameText.text = customizer.GetCurrentPartName(RobotCustomizer.PartType.Key);
       }
}
