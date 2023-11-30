using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    [SerializeField] private int lvlcount;
    public GameObject buttons;
    [SerializeField] private TMP_Text deathCount;
    [SerializeField] private TMP_Text thanks;

    void Awake()
    {
        int unlocked = PlayerPrefs.GetInt("Levels", 1);
            for (int i = 1; i <= 10; i++)
            {
            if (i < unlocked)
            {
                buttons.transform.GetChild(i - 1).gameObject.GetComponent<Image>().color = Color.green;
                if (PlayerPrefs.GetInt("Coin" + (i.ToString()), 0) == 1)
                {
                    buttons.transform.GetChild(i - 1).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    buttons.transform.GetChild(i - 1).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                }
            }
            if(i==unlocked)
            {
                buttons.transform.GetChild(i - 1).gameObject.GetComponent<Image>().color = Color.white;
            }
            if(i>unlocked)
            {
                buttons.transform.GetChild(i - 1).gameObject.GetComponent<Image>().color = Color.grey;
            }
            }
        deathCount.text = ("TOTAL DEATHS: " + (PlayerPrefs.GetInt("Deaths", 0)).ToString());
        if(unlocked > 10)
        {
            thanks.text = "Thanks for beating my game <3";
        }
        else
        {
            thanks.text = " ";
        }
    }

    public void LoadLevel(int x)
    {
        if (PlayerPrefs.GetInt("Levels", 1) >= x)
        {
            SceneManager.LoadScene(x+1);
        }
    }
}
