using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Random = System.Random;
using TextAsset = UnityEngine.TextCore.Text.TextAsset;

public class UISwitcher : MonoBehaviour
{
    public TMP_Text donothonTimer;
    
    public GameObject[] uIElementsToHide;
    public GameObject[] uIElementsToShow;

    public GameObject uIHider;

    public TMP_FontAsset[] fonts;
    public TMP_InputField colorInput;

    public void showConnectedUIElements()
    {
        foreach (GameObject gameObject in uIElementsToHide)
        {
            gameObject.SetActive(false);
        }
        
        foreach (GameObject gameObject in uIElementsToShow)
        {
            gameObject.SetActive(true);
        }
    }

    public void toggleUIVisibiltiy()
    {
        uIHider.SetActive(!uIHider.activeSelf);
    }

    public void changeFont(int fontNo)
    {
        switch (fontNo)
        {
            case 1:
                donothonTimer.font = fonts[0];
                break;
            case 2:
                donothonTimer.font = fonts[1];
                break;
            case 3:
                donothonTimer.font = fonts[2];
                break;
        }
    }

    public void changeColor()
    {
        Color color;
        if (ColorUtility.TryParseHtmlString("#"+colorInput.text, out color))
        {
            donothonTimer.color = color;
        }
    }

    public void generateRandomColor()
    {
        colorInput.text = String.Format("{0:X6}", new Random().Next(0x1000000));
    }
}
