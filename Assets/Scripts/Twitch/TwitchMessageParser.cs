using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchMessageParser : MonoBehaviour
{
    public static int ParseBitsFromIRCMessage(string ircMessage)
    {
        // Split the IRC message by semicolons
        string[] beforePrvmsg = ircMessage.Split("PRIVMSG");
        string[] messageParts = beforePrvmsg[0].Split(';');

        // Initialize bits variable
        int bits = 0;

        // Iterate through each part of the message
        foreach (string part in messageParts)
        {
            print(part);
            // Split each part by '=' to get the key-value pair
            string[] keyValue = part.Split('=');
            if (keyValue.Length == 2)
            {
                string key = keyValue[0];
                string value = keyValue[1];
                print(key + " ||| " + value);
                // Check if the key is 'bits'
                if (key == "bits")
                {
                    // Parse the value of 'bits' as an integer
                    if (!int.TryParse(value, out bits))
                    {
                        // Handle the case where 'bits' value is not an integer
                        Debug.LogError("Error: 'bits' value is not an integer");
                        bits = 0;
                    }
                    break;
                }
            }
        }

        return bits;
    }
    
    /*
     * -1 - No Valid Subscription Found
     * 0 - Prime (1 atm)
     * 1 - Tier 1
     * 2 - Tier 2
     * 3 - Tier 3
    */
    public static int ParseSubscriptionTier(string message)
    {
        // Chat Sub verification using "PRIVMSG"
        string[] parts = message.Split("PRIVMSG");
        
        // Scanning "msg-param-sub-plan" for "Prime", "1000", "2000" and "3000"
        if (parts.Length > 0)
        {
            string[] keyValuePairs = parts[0].Split(';');
            foreach (string pair in keyValuePairs)
            {
                string[] keyValue = pair.Split('=');
                if (keyValue.Length == 2 && keyValue[0].Trim() == "msg-param-sub-plan")
                {
                    string value = keyValue[1].Trim();
                    switch (value)
                    {
                        case "Prime":
                            return 1;
                        case "1000":
                            return 1;
                        case "2000":
                            return 2;
                        case "3000":
                            return 3;
                        default:
                            return -1; // Unknown tier
                    }
                }
            }
        }
        
        return -1; // Subscription tier not found
    }
}
