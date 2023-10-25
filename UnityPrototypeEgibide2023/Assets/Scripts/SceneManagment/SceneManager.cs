using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{


    public void CambiarEscena(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }

    public void Salir()
    {
        Application.Quit();
    }
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
