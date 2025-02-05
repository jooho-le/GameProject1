using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // TextMeshPro 네임스페이스

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health = 100f;  // 체력 변수 (기본값: 100)
    private Rigidbody2D target;  // Player의 Rigidbody2D를 타겟으로 설정
    private bool isLived = true;

    private Rigidbody2D rigid;
    private SpriteRenderer spriter;

    // Damage text 관련 변수
    private TextMeshPro damageText; // 자식 오브젝트에 추가할 TextMeshPro 컴포넌트

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();

        // Player를 찾아서 target에 할당
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            target = player.GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.LogError("Player object not found! Ensure your player has the 'Player' tag.");
        }

        // Enemy의 자식 오브젝트를 생성하여 DamageText를 위한 TextMeshPro 컴포넌트를 추가
        GameObject textObj = new GameObject("DamageText");
        textObj.transform.SetParent(transform); 
        // 적의 위쪽에 배치 (localPosition으로 상대적 위치 설정)
        textObj.transform.localPosition = new Vector3(0, 1.5f, 0);  

        // TextMeshPro 컴포넌트 추가
        damageText = textObj.AddComponent<TextMeshPro>();
        damageText.fontSize = 3;           // 글씨 크기 (필요에 따라 조정)
        damageText.color = Color.red;      // 글씨 색상
        damageText.alignment = TextAlignmentOptions.Center;
        damageText.text = "";              // 초기 텍스트는 비워둠
        damageText.enabled = false;        // 처음에는 텍스트를 숨김
    }

    void FixedUpdate() 
    {
        if(!isLived) // isLived가 false이면 실행하지 않음
            return; 

        // 타겟(플레이어)와의 방향 벡터 계산
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; 
        rigid.MovePosition(rigid.position + nextVec); // 현재위치 + 다음위치

        // 주의: rigid.velocity를 매 프레임 0으로 만드는 경우 밀리는 효과 등이 없어질 수 있으므로 필요에 따라 제거
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate() 
    {
        if(!isLived) 
            return; 

        // 플레이어 위치에 따라 스프라이트 방향을 반전시킴
        spriter.flipX = target.position.x < rigid.position.x;
    }

    // 체력 감소 함수
    public void TakeDamage(float damage)
    {
        if (!isLived) return;

        health -= damage;
        DisplayDamageText(damage);

        if (health <= 0)
        {
            Die();
        }
    }

    // 적이 죽을 때 처리
    private void Die()
    {
        isLived = false;
        Destroy(gameObject);
    }

    // 데미지 텍스트를 표시하는 함수
    private void DisplayDamageText(float damage)
    {
        damageText.text = damage.ToString();
        damageText.enabled = true;
        // 코루틴을 통해 일정 시간 후 텍스트 숨김
        StartCoroutine(HideDamageText());
    }

    private IEnumerator HideDamageText()
    {
        yield return new WaitForSeconds(1f);  // 1초 후
        damageText.enabled = false;
    }
}
