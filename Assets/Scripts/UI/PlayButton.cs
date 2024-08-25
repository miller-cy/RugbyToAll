using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
   private Button button;
   public GameObject nameInputPanel;
   public Button confirmButton;
   public InputField inputField;

   private void Awake()
   {
    button = GetComponent <Button >();
    button.onClick .AddListener (StartGame);
    confirmButton .onClick.AddListener (ConfirmName);
   }
   private void StartGame()
   {
       //SceneManager . LoadScene ("Gameplay");
       if(PlayfabManager.instance.playerName == string.Empty)
       {
           nameInputPanel.SetActive(true);
          // confirmButton.onClick.AddListener(ConfirmName);
       }
       else
       {
           nameInputPanel.SetActive(false);
           TransitionManager.instance .Transition("Gameplay");
       }
   }
   private void ConfirmName()
   {
       PlayfabManager.instance.SubmitName ( inputField.text);
       nameInputPanel.SetActive(false);
    
   }  
   
}
