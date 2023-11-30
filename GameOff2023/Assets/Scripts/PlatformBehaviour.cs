using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
    private PlatformEffector2D eff;
    private BoxCollider2D box;
    [SerializeField] private LayerMask playerLayer;

    void Awake()
    {
        eff = GetComponent<PlatformEffector2D>();
        box = GetComponent<BoxCollider2D>();
    }


    void Update()
    {
        if(Input.GetKey(KeyCode.S) && IsOnPlatform())
        {
            StartCoroutine(JumpThrough());
        }
    }

    private bool IsOnPlatform()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(box.bounds.center, box.bounds.size, 0, Vector2.up, 0.1f, playerLayer);
        return raycastHit.collider != null;
    }

    private IEnumerator JumpThrough()
    {
        eff.rotationalOffset = 180;
        yield return new WaitForSeconds(0.1f);
        eff.rotationalOffset = 0;
    }
}
