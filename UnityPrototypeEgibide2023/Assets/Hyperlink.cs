using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hyperlink : MonoBehaviour
{
    // Start is called before the first frame update
    public static void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
}
