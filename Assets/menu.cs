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

    public void StartButton(string SampleScene){
        SceneManager.LoadScene(SampleScene);
    }

//     private IEnumerator LoadSceneWithDelay(string SampleScene){
//     // Tambahkan efek transisi atau delay jika perlu
//     yield return new WaitForSeconds(1f); // Delay 1 detik
//     SceneManager.LoadScene(SampleScene);
// }

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
    public void sound_volume(float volume){
        PlayerPrefs.SetFloat("volume", volume);
    }
}
