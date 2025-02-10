using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public float roundTime; // 라운드 시간
    private float timer;          // 남은 시간
    private bool isGameActive = true;

    public TextMeshProUGUI timerText;  // 🕒 UI 타이머 표시
    public GameObject shopPopup;       // 🏪 상점 UI

    void Start()
    {
        timer = roundTime;
        shopPopup.SetActive(false);  // 처음에는 상점 숨기기
        StartCoroutine(StartRound());
    }

    void Update()
    {
        if (isGameActive)
        {
            timer -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Ceil(timer).ToString(); // 정수로 표시

            if (timer <= 0)
            {
                EndRound();
            }
        }
    }

    IEnumerator StartRound()
    {
        isGameActive = true;
        timer = roundTime;
        yield return null;
    }

    void EndRound()
    {
        isGameActive = false;
        shopPopup.SetActive(true);  // 🏪 상점 팝업 활성화
    }

    // ✅ 상점에서 아이템 선택 후 호출될 함수
    public void OnShopSelection()
    {
        shopPopup.SetActive(false); // 상점 닫기
        StartCoroutine(StartRound()); // 다음 라운드 시작
    }
}
