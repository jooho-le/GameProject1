using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float roundTime = 10f;  // 라운드 타이머 (초 단위)
    private float currentTime;
    public TextMeshProUGUI timerText;  // 타이머 UI
    public GameObject popupPanel;  // 4개의 팝업이 들어있는 Panel
    public Button[] optionButtons;  // 선택 가능한 4개의 버튼

    public Player player;  // Player 스크립트 참조
    public Weapon weapon;  // ✅ Weapon 스크립트 참조

    void Start()
    {
        currentTime = roundTime;
        popupPanel.SetActive(false); // 처음엔 비활성화
        StartCoroutine(TimerCountdown());
    }

    IEnumerator TimerCountdown()
    {
        while (currentTime > 0)
        {
            timerText.text = "Time: " + currentTime.ToString("F1");
            yield return new WaitForSeconds(1f);
            currentTime--;
        }

        GamePauseAndShowOptions();
    }

    void GamePauseAndShowOptions()
    {
        Time.timeScale = 0f;  // 게임 일시정지
        popupPanel.SetActive(true);  // 팝업창 표시
    }

    public void OnOptionSelected(int optionIndex)
    {
        // 선택한 옵션 적용
        switch (optionIndex)
        {
            case 0: weapon.attackPower += 5; break;  // ✅ 무기 공격력 증가
            case 1: player.speed += 2; break;       // 속도 증가
            case 2: player.hp += 10; break;         // 체력 증가
            case 3: Debug.Log("특별한 효과!"); break; // 기타 효과
        }

        StartNextRound();
    }

    void StartNextRound()
    {
        Time.timeScale = 1f;  // 게임 다시 시작
        popupPanel.SetActive(false);  // 팝업창 숨김
        currentTime = roundTime;  // 타이머 리셋
        StartCoroutine(TimerCountdown());
    }
}
