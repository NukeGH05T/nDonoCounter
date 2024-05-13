using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Net.Sockets;
using System.IO;
using TMPro;


public class TwitchConnect : MonoBehaviour
{
    public TMP_InputField channel;
    public string tokenUser;
    
    public UnityEvent<string, string> OnChatMessage;
    public UnityEvent<int> OnTwitchConnected;
    public UnityEvent<int> OnTwitchDisconnected;
    public UnityEvent<int> OnTwitchBitsReceived;
    public UnityEvent<int> OnTwitchSubReceived;
    TcpClient Twitch;
    StreamReader Reader;
    StreamWriter Writer;
    
    const string URL = "irc.chat.twitch.tv";
    const int PORT = 6667;

    private string User = "Chattle";
    private string Oauth;// = "oauth:4t9lljtze4cwrsdwqarxro4g8oy6l0"; //twitchapps.com/tmi
    private string Channel;

    private float pingCounter = 0;

    public void ConnectToTwitch()
    {
        Twitch = new TcpClient(URL, PORT);
        Reader = new StreamReader(Twitch.GetStream());
        Writer = new StreamWriter(Twitch.GetStream());
        
        Writer.WriteLine("PASS " + Oauth);
        Writer.WriteLine("NICK " + User.ToLower());
        Writer.WriteLine("JOIN #" + Channel.ToLower());
        Writer.WriteLine("CAP REQ :twitch.tv/commands twitch.tv/tags");
        //Writer.WriteLine("PRIVMSG #" + Channel + " :Connected to chat. Token granted by @" + tokenUser);
        Writer.Flush();
        OnTwitchConnected?.Invoke(1); //Sending int just to trigger it, 1 doesn't mean anything
    }

    public void SendIRC()
    {
        SendIRCMessage("Message sent via IRC");
    }
    
    public void SendIRC(TMP_InputField input)
    {
        SendIRCMessage(input.text);
        input.text = "";
    }

    public void AttemptTwitchConnection(string oauth)
    {
        Oauth = "oauth:" + oauth;
        Channel = channel.text;

        if (User != null && Oauth != null && Channel != null)
        {
            ConnectToTwitch();
        }
        else
        {
            print("Please make sure to submit all the required info!");
        }
    }

    private void Update()
    {
        if (Twitch == null)
        {
            return;
        }
        
        pingCounter += Time.deltaTime;
        if (pingCounter > 60)
        {
            Writer.WriteLine("PING " + URL);
            Writer.Flush();
            pingCounter = 0;
        }
        if (!Twitch.Connected)
        {
            OnTwitchDisconnected?.Invoke(0);
            print("Reconnecting...");
            ConnectToTwitch();
        }

        if (Twitch.Available > 0)
        {
            string message = Reader.ReadLine();

            if (message.Contains("PRIVMSG"))
            {
                // :nukegh05t!nukegh05t@nukegh05t.tmi.twitch.tv PRIVMSG #nukegh05t :Heya
                int splitPoint = message.IndexOf("!");
                string chatter = message.Substring(1, splitPoint - 1);

                splitPoint = message.IndexOf(":", 1);
                string content = message.Substring(splitPoint + 1);
                OnChatMessage?.Invoke(chatter, content);

                //Dono Handling
                //Bits
                int bits = TwitchMessageParser.ParseBitsFromIRCMessage(message);
                if (bits != 0)
                {
                    OnTwitchBitsReceived?.Invoke(bits);
                }
                
                //Subs
                int subTier = TwitchMessageParser.ParseSubscriptionTier(message);
                if (subTier >= 0)
                {
                    OnTwitchSubReceived?.Invoke(subTier);
                }
                //Dollars
                
                print(message);
            }
            else
            {
                print(message);
            }
        }
    }
    
    public void SendIRCMessage(string message)
    {
        try
        {
            Writer.WriteLine("PRIVMSG #" + Channel + " :" + message);
            Writer.Flush();
            print("Sent IRC Message");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to send message: " + e.Message);
        }
    }
}
