using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    public GameObject[] objectsToKeepSafe;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        foreach (GameObject gameObject in objectsToKeepSafe)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
