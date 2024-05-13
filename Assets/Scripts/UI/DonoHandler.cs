using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class DonoHandler : MonoBehaviour
{
    public TimerControl timerControl;

    [Header("Bits")]
    public TMP_InputField secondPerHBits;
    
    [Header("Subs")]
    public TMP_InputField secondPerSub;
    public TMP_InputField secondPerSub2;
    public TMP_InputField secondPerSub3;
    
    [Header("Dollars - N/A")]
    public TMP_InputField secondPerDollar;
    
    // Per 100 Bits | 100 bits = 90 secs
    public void AddTimeForBits(int bits)
    {
        //TODO: Add an input field to let users set the time/bit ratio
        float timeToAdd = (bits/100) * float.Parse(secondPerHBits.text);
        timerControl.AddTimeFromString("0h0m"  + timeToAdd + "s");
    }
    
    // Per 1 Sub | 1 sub = 450 secs
    public void AddTimeForSubs(int _subTier)
    {
        //TODO: Add an input field to let users set the time/bit ratio
        float timeToAdd = 0;
        switch (_subTier)
        {
            case 1:
                timeToAdd = float.Parse(secondPerSub.text);
                break;
            case 2:
                timeToAdd = float.Parse(secondPerSub2.text);
                break;
            case 3:
                timeToAdd = float.Parse(secondPerSub3.text);
                break;
        }
        timerControl.AddTimeFromString("0h0m"  + timeToAdd + "s");
    }
    
    // Per 1 USD | 1 USD = 90 secs
    public void AddTimeForDollars(int _dollars)
    {
        //TODO: Add an input field to let users set the time/bit ratio
        float timeToAdd = _dollars * float.Parse(secondPerDollar.text);
        timerControl.AddTimeFromString("0h0m"  + timeToAdd + "s");
    }

    public void EnableInput()
    {
        secondPerHBits.interactable = true;
        secondPerSub.interactable = true;
        secondPerSub2.interactable = true;
        secondPerSub3.interactable = true;
        secondPerDollar.interactable = true;
    }
    
    public void DisableInput()
    {
        secondPerHBits.interactable = false;
        secondPerSub.interactable = false;
        secondPerSub2.interactable = false;
        secondPerSub3.interactable = false;
        secondPerDollar.interactable = false;
    }
}
