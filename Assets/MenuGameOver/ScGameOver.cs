using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ScGameOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainMenu(string SampleScene){
        SceneManager.LoadScene(SampleScene);
    }
    public void StartButton(string MainMenu){
        SceneManager.LoadScene(MainMenu);
    }
}
