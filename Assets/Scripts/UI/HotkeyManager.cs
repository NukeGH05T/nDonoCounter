using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotkeyManager : MonoBehaviour
{
    public KeyCode toggleUIKey = KeyCode.H;
    public UISwitcher uISwitcher;
    void Update()
    {
        if (Input.GetKeyDown(toggleUIKey))
        {
            uISwitcher.toggleUIVisibiltiy();
        }
    }
}
