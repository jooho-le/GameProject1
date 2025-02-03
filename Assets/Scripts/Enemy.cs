using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public Rigidbody2D target;
    bool isLived = true;

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate() 
    {
        if(!isLived) // islived가 아니면 나가라(return)
            return; 

        Vector2 dirVec = target.position - rigid.position; // 타겟위치 - 나의 위치
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; 
        rigid.MovePosition(rigid.position + nextVec); // 현재위치 + 다음위치 
        rigid.velocity = Vector2.zero; // 물리충돌이 위치이동에 영향x
    }

    void LateUpdate() 
    {
        if(!isLived) // islived가 아니면 나가라(return)
            return; 

        spriter.flipX = target.position.x < rigid.position.x;
    }
}
