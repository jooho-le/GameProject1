using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;  // 🔴 UI 패널 사용을 위해 추가
using TMPro;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    [SerializeField] public float speed;
    [SerializeField] public float hp = 100f;

    private bool isGameOver = false;
    private int coinCount = 0; // 💰 코인 개수

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    private TextMeshPro hpText;
    private TextMeshPro damageTakenText;

    public TextMeshProUGUI coinText; // 🎯 Canvas UI 연결
    private Image damageOverlay;     // 🔴 붉은 화면 효과용 UI 패널
    private float overlayDuration = 0.2f; // 0.5초 지속

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();

        // 🔴 UI 패널 찾기
        GameObject overlayObj = GameObject.Find("DamageOverlay");
        if (overlayObj != null)
        {
            damageOverlay = overlayObj.GetComponent<Image>();
            damageOverlay.color = new Color(1, 0, 0, 0); // 시작할 때 투명
        }
        else
        {
            Debug.LogError("DamageOverlay UI 패널을 찾을 수 없습니다!");
        }

        // HP 텍스트 생성
        GameObject hpTextObj = new GameObject("HPText");
        hpTextObj.transform.SetParent(transform);
        hpTextObj.transform.localPosition = new Vector3(0, 0.5f, 0);
        hpText = hpTextObj.AddComponent<TextMeshPro>();
        hpText.fontSize = 3;
        hpText.color = Color.green;
        hpText.alignment = TextAlignmentOptions.Center;
        hpText.text = "HP: " + hp.ToString();

        // 데미지 텍스트 생성
        GameObject dmgTextObj = new GameObject("DamageTakenText");
        dmgTextObj.transform.SetParent(transform);
        dmgTextObj.transform.localPosition = new Vector3(0, 0.8f, 0);
        damageTakenText = dmgTextObj.AddComponent<TextMeshPro>();
        damageTakenText.fontSize = 3;
        damageTakenText.color = Color.red;
        damageTakenText.alignment = TextAlignmentOptions.Center;
        damageTakenText.text = "";
        damageTakenText.enabled = false;
    }

    void Start()
    {
        UpdateCoinUI(); // 🎯 초기 UI 업데이트
    }

    void FixedUpdate()
    {
        if (!isGameOver)
        {
            Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
        }
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    void LateUpdate()
    {
        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x > 0;
        }
        hpText.text = "HP: " + hp.ToString();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                float damage = enemy.enemyAttackPower;
                TakeDamage(damage);
                DisplayDamageTakenText(damage);
            }
        }
    }

    // 💰 코인과 충돌하면 호출되는 함수
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            AddCoin(); // ✅ 코인 획득
            Destroy(collision.gameObject); // 코인 제거
        }
    }

    // ✅ 코인을 증가시키고 UI를 업데이트하는 함수
    public void AddCoin()
    {
        coinCount++;
        UpdateCoinUI();
        Debug.Log("Coin Collected! Total: " + coinCount);
    }

    // ✅ 코인 UI를 업데이트하는 함수
    private void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = " : " + coinCount; // 🎯 UI에 코인 개수 표시
        }
    }

    void TakeDamage(float damage)
    {
        hp -= damage;
        Debug.Log("Player HP: " + hp);
        if (hp <= 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(ShowDamageEffect()); // 🔴 피격 효과 실행
        }
    }

    void DisplayDamageTakenText(float damage)
    {
        damageTakenText.text = "-" + damage.ToString();
        damageTakenText.enabled = true;
        StartCoroutine(HideDamageTakenText());
    }

    IEnumerator HideDamageTakenText()
    {
        yield return new WaitForSeconds(1f);
        damageTakenText.enabled = false;
    }

    IEnumerator ShowDamageEffect()
    {
        if (damageOverlay != null)
        {
            damageOverlay.color = new Color(1, 0, 0, 0.2f); // 🔴 반투명 빨간색
            yield return new WaitForSeconds(overlayDuration);
            damageOverlay.color = new Color(1, 0, 0, 0); // ⬜ 다시 투명하게
        }
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("GAME OVER");
    }

    void OnGUI()
    {
        if (isGameOver)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 50;
            style.normal.textColor = Color.red;
            style.alignment = TextAnchor.MiddleCenter;  // 🎯 텍스트 중앙 정렬

            float width = 200;
            float height = 50;
            float x = (Screen.width - width) / 2;
            float y = (Screen.height - height) / 2;

            GUI.Label(new Rect(x, y, width, height), "GAME OVER", style);
        }
    }

}
