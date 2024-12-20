using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.Find("menuMusic on")==null){
        GetComponent<AudioSource>().Play();
        gameObject.name = "menuMusic";
        PlayerPrefs.SetFloat("volume", 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume");
    }
}
