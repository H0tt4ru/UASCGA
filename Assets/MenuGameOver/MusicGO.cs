using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicGO : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // if (GameObject.Find("one") != null)
        // {
        //     Destroy(GameObject.Find("one"));
        // }
        if(GameObject.Find("one on")==null){
        GetComponent<AudioSource>().Play();
        gameObject.name = "one";
        PlayerPrefs.SetFloat("volume", 1f);
        }

        if(GameObject.Find("two on")==null){
        GetComponent<AudioSource>().Play();
        gameObject.name = "two";
        PlayerPrefs.SetFloat("volume", 0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume");
    }
}