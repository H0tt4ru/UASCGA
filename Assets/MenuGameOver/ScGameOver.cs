using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ScGameOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CursorManager.ShowCursor();
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

    public class CursorManager : MonoBehaviour
{
    public static void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

}
