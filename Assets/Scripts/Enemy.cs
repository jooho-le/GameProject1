using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // TextMeshPro 네임스페이스

public class Enemy : MonoBehaviour
{
    public float speed;
    [SerializeField]
    public float health = 100f;      // 체력 변수 (기본값: 100)
    [SerializeField]
    public float enemyAttackPower = 10f;   // enemy의 공격력 변수

    private Rigidbody2D target;      // Player의 Rigidbody2D를 타겟으로 설정
    private bool isLived = true;

    private Rigidbody2D rigid;
    private SpriteRenderer spriter;

    // DamageText와 AttackPowerText를 위한 변수
    private TextMeshPro damageText;       
    private TextMeshPro attackPowerText;  

    // 💰 코인 관련 변수 추가
    public GameObject coinPrefab; // 인스펙터에서 코인 프리팹을 할당하세요.

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

        // DamageText 자식 오브젝트 생성 (데미지 표시용)
        GameObject dmgTextObj = new GameObject("DamageText");
        dmgTextObj.transform.SetParent(transform);
        dmgTextObj.transform.localPosition = new Vector3(0, 1.5f, 0);  // enemy 위쪽에 배치
        damageText = dmgTextObj.AddComponent<TextMeshPro>();
        damageText.fontSize = 3;            
        damageText.color = Color.red;       
        damageText.alignment = TextAlignmentOptions.Center;
        damageText.text = "";               
        damageText.enabled = false;         

        // AttackPowerText 자식 오브젝트 생성 (공격력 표시용)
        GameObject apTextObj = new GameObject("AttackPowerText");
        apTextObj.transform.SetParent(transform);
        apTextObj.transform.localPosition = new Vector3(0, 2.5f, 0);
        attackPowerText = apTextObj.AddComponent<TextMeshPro>();
        attackPowerText.fontSize = 3;         
        attackPowerText.color = Color.yellow; 
        attackPowerText.alignment = TextAlignmentOptions.Center;
        attackPowerText.text = "AP: " + enemyAttackPower.ToString();
        attackPowerText.enabled = true;
    }

    void FixedUpdate() 
    {
        if (!isLived) return; 

        // 타겟(플레이어)와의 방향 벡터 계산 및 이동 처리
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; 
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero; 
    }

    void LateUpdate() 
    {
        if (!isLived) return; 
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

        // 💰 코인 생성 기능 추가
        if (coinPrefab != null)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("coinPrefab이 설정되지 않았습니다! 인스펙터에서 할당하세요.");
        }

        Destroy(gameObject);
    }

    // 데미지 텍스트를 표시하는 함수
    private void DisplayDamageText(float damage)
    {
        damageText.text = damage.ToString();
        damageText.enabled = true;
        StartCoroutine(HideDamageText());
    }

    // 일정 시간 후 데미지 텍스트 숨김 처리
    private IEnumerator HideDamageText()
    {
        yield return new WaitForSeconds(1f);
        damageText.enabled = false;
    }
}
