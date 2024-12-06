using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menu : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject InfoPanel;
    // Start is called before the first frame update
    void Start()
    {
        MenuPanel.SetActive(true);
        InfoPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton(string TestScene){
        SceneManager.LoadScene(TestScene);
    }

    public void InfoButton(){
        MenuPanel.SetActive(false);
        InfoPanel.SetActive(true);
    }

    public void BackButton(){
        MenuPanel.SetActive(true);
        InfoPanel.SetActive(false);
    }

    public void KeluarButton(){
        Application.Quit();
        Debug.Log("Keluar");
    }
}
