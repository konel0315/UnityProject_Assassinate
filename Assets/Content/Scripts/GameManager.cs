using UnityEngine;
using TMPro;  // TMP 쓰려면 추가

public class GameManager : MonoBehaviour
{
    public EnemyManager enemyManager;
    public PlayerHealth playerHealth;
    public ScoreUIController scoreUIController;

    public TextMeshProUGUI clearTimeText;  // UI 텍스트 연결용

    public float clearTime; // 게임 클리어까지 걸린 시간 저장하는 변수

    private bool gameCleared = false;

    public AudioSource audioSource;
    
    private void Start()
    {
        enemyManager.OnAllEnemiesDefeated += HandleGameClear;
        clearTime = 0;
    }

    private void Update()
    {
        if (!gameCleared)
        {
            clearTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(clearTime / 60f);
            int seconds = Mathf.FloorToInt(clearTime % 60f);
            clearTimeText.text = $"Time: {minutes:00}:{seconds:00}";

        }
    }
    private void HandleGameClear()
    {
        
        if (gameCleared) return;  // 중복 방지
        
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        
        gameCleared = true;
        Time.timeScale = 0f;
        int remainingHearts = playerHealth.currentHealth();
        scoreUIController.ShowScore((int)clearTime, remainingHearts);
    }
}