using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject HelpText;
    public GameObject TouchUI;

    private void Start()
    {
#if UNITY_STANDALONE
        HelpText.SetActive(true);
        TouchUI.SetActive(false);
#endif
#if UNITY_ANDROID
        HelpText.SetActive(false);
        TouchUI.SetActive(true);
#endif
    }
}
