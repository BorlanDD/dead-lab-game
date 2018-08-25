using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintUI : MonoBehaviour
{

    public GameObject hintInteractionUI;
    public Text taskHintUI;

    #region SINGLETON PATTERN
    private static HintUI userInterface;
    public static HintUI GetInstance()
    {
        return userInterface;
    }

    private void Awake()
    {
        userInterface = this;
    }

    #endregion

    public void InteractionHintUIState(bool state)
    {
        hintInteractionUI.SetActive(state);
    }

    public void ShowTaskHint(string description)
    {
        taskHintUI.text = description;
        taskHintUI.gameObject.SetActive(true);
    }

    public void HideTaskHint()
    {
        taskHintUI.gameObject.SetActive(false);
    }
}
