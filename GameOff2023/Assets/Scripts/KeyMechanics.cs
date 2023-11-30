using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMechanics : MonoBehaviour
{
    [SerializeField] private GameObject door;

    void Awake()
    {
        this.gameObject.SetActive(true);
        door.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag=="Player")
        {
            this.gameObject.SetActive(false);
            door.SetActive(false);
        }
    }
}
