using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterMechanics : MonoBehaviour
{
    [SerializeField] private bool isHoming;
    [SerializeField] private Vector2 dir;

    public GameObject bullet;
    public Transform bulletPos;

    private float cooldown;

    private float timePassed;
    private Collider2D col;

    void Awake()
    {
        timePassed = 0f;
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        cooldown = PlayerMovement.Instance.bulletRate;
        if(PlayerMovement.Instance.timerActive)
        {
            timePassed += Time.deltaTime;
        }
        if(timePassed >= cooldown && cooldown>0.1f)
        {
            timePassed = 0;
            Shoot();
        }

        if(isHoming)
        {
            Vector3 direction = GameObject.FindGameObjectWithTag("Player").transform.position - this.transform.position;
            float rotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotation + 30);
        }
    }

    void Shoot()
    {
        GameObject bul = Instantiate(bullet, bulletPos.position, Quaternion.identity);
        bul.gameObject.GetComponent<BulletBehaviour>().ignore = col;
        bul.gameObject.GetComponent<BulletBehaviour>().isHoming = isHoming;
        bul.gameObject.GetComponent<BulletBehaviour>().dir = dir;
    }
}
