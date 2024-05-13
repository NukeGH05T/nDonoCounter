using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TwitchChatUpdater : MonoBehaviour
{
    public TMP_Text chatWindow;
    
    private void Start()
    {
        chatWindow.color = Color.green;
    }
    
    public void updateChat(string chatter, string msg)
    {
        chatWindow.SetText(chatWindow.GetParsedText() + " ||| " + chatter + ": " + msg);
    }
}
