using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Rigidbody2D body;
    private float force;
    private Collider2D collid;
    public Collider2D ignore;
    public bool isHoming;
    public Vector2 dir;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 direction;
        if (isHoming)
        {
            direction = player.transform.position - transform.position;
        }
        else
        {
            direction = dir;
        }
        force = PlayerMovement.Instance.bulletForce;
        body.velocity = new Vector2(direction.x, direction.y).normalized * force;

        float rotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation + 90);

        collid = GetComponent <Collider2D>();
        Physics2D.IgnoreCollision(collid, ignore);

        GameObject[] h = GameObject.FindGameObjectsWithTag("Platform");
        GameObject[] g = GameObject.FindGameObjectsWithTag("Bullet");

        foreach (GameObject plat in h)
        {
            Physics2D.IgnoreCollision(plat.gameObject.GetComponent<Collider2D>(), collid);
        }
        foreach (GameObject bull in g)
        {
            Physics2D.IgnoreCollision(bull.gameObject.GetComponent<Collider2D>(), collid);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag != "Shooter" && col.gameObject.tag != "Bullet")
        {
            Destroy(gameObject);
        }
    }
}
