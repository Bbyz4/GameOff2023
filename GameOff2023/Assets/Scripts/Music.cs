using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour
{
    public AudioSource bgmusic;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
        bgmusic = this.GetComponent<AudioSource>();

    }

    void Update()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                bgmusic.volume = 0.05f;
                break;
            case 1:
                bgmusic.volume = 0.05f;
                break;
            default:
                bgmusic.volume = 0.15f;
                break;
        }
    }
}
