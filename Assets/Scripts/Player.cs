using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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

    // ✅ UI에 배치할 코인 개수 텍스트
    public TextMeshProUGUI coinText; // 🎯 Canvas에 있는 UI 연결

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();

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
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50), "GAME OVER", style);
        }
    }
}
