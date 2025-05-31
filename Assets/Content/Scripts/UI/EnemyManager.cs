using System;
using UnityEngine; // UI 사용시
using TMPro;
using Unity.VisualScripting;

public class EnemyManager : MonoBehaviour
{
    public int totalEnemies = 9;  // 현재 맵에 존재하는 적 수
    public int originEnemies = 9; // 적 수 표시할 UI 텍스트
    public TMP_Text enemyCountText;
    public Action OnAllEnemiesDefeated;
    
    void Start()
    {
        UpdateEnemyCount();
    }
    
    public void RemoveEnemy()
    {
        totalEnemies--;
        if (totalEnemies <= 0)
        {
            totalEnemies = 0;
            OnAllEnemiesDefeated?.Invoke();
        }
        UpdateEnemyCount();
    }

    void UpdateEnemyCount()
    {
        if(enemyCountText != null)
            enemyCountText.text = $"{totalEnemies} / {originEnemies}";
    }
}