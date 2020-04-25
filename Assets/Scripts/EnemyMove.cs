using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private float speed = 2.0F;
    [SerializeField] private Transform pos1, pos2;

    private float timer = 0;
    private bool timerReached = false;
    private Animator animation;
    private Vector3 dir;
    private SpriteRenderer sprite;

    void Start()
    {
        transform.position = pos1.position;
        sprite = GetComponent<SpriteRenderer>();
        animation = GetComponent<Animator>();
        animation.SetInteger("AnimState", 0);
    }

    void Update()
    {
        if (transform.position.x == pos1.position.x)
        {
            waiting(pos2);
        }
        else if (transform.position.x == pos2.position.x)
        {
            waiting(pos1);
        }
        if (timerReached) transform.position = Vector3.MoveTowards(transform.position, dir, speed * Time.deltaTime);
    }

    private void waiting(Transform pos)
    {
        if (timerReached)
        {
            animation.SetInteger("AnimState", 0);
            timerReached = false;
        }
        timer += Time.deltaTime;
        if (timer > 5)
        {
            timer = 0.0F;
            animation.SetInteger("AnimState", 2);
            dir = pos.position;
            sprite.flipX = !sprite.flipX;
            timerReached = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(pos1.position, pos2.position);
    }
}
