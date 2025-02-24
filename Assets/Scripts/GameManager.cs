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
    public TextMeshProUGUI statusText;  // 플레이어 상태 표시 UI

    private Player player;  // Player 스크립트 참조
    private Weapon weapon;  // ✅ Weapon 스크립트 참조

    void Start()
    {
        player = FindObjectOfType<Player>(); // 자동으로 Player 찾기
        weapon = FindObjectOfType<Weapon>(); // 자동으로 Weapon 찾기

        if (popupPanel != null)
            popupPanel.SetActive(false); // 처음엔 팝업 비활성화

        AssignButtonEvents(); // 버튼 이벤트 자동 설정

        currentTime = roundTime;
        UpdateStatusUI();  // ✅ 게임 시작할 때도 UI 표시!
        StartCoroutine(TimerCountdown());
    }

    void AssignButtonEvents()
    {
        if (optionButtons.Length == 4)
        {
            optionButtons[0].onClick.AddListener(() => OnOptionSelected(0)); // 공격력 증가
            optionButtons[1].onClick.AddListener(() => OnOptionSelected(1)); // 속도 증가
            optionButtons[2].onClick.AddListener(() => OnOptionSelected(2)); // 체력 증가
            optionButtons[3].onClick.AddListener(() => OnOptionSelected(3)); // 무기 개수 증가
        }
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
        if (player == null || weapon == null) return; // 예외 처리

        switch (optionIndex)
        {
            case 0:
                weapon.attackPower = weapon.attackPower + 5;
                Debug.Log($"공격력 증가! 현재 공격력: {weapon.attackPower}");  // ✅ 디버그 로그 추가
                break;
            case 1:
                player.speed += 2; 
                Debug.Log($"스피드 증가! 현재 스피드: {player.speed}");
                break;
            case 2:
                player.hp += 10; 
                Debug.Log($"체력 회복! 현재 체력: {player.hp}");
                break;
            /*case 3:
                player.AddWeapon(); 
                Debug.Log("무기 개수 증가!");
                break; */
        }

        UpdateStatusUI(); // 변경 즉시 UI 업데이트
        StartNextRound();
    }

    void StartNextRound()
    {
        Time.timeScale = 1f;  // 게임 다시 시작
        popupPanel.SetActive(false);  // 팝업창 숨김
        currentTime = roundTime;  // 타이머 리셋
        StartCoroutine(TimerCountdown());
    }

    void UpdateStatusUI()
    {
        if (statusText != null && player != null && weapon != null)
        {
            statusText.text = $"power: {weapon.attackPower}             speed : {player.speed}\n";
        }
    }
}
