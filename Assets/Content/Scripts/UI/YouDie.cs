using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class YouDieUI : MonoBehaviour
{
    [SerializeField] private GameObject youDiePanel;

    public AudioSource audioSource;
    
    
// 소리 재생 중이면 멈추기
       
    public void ShowYouDieUI()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        Time.timeScale = 0f; // 게임 정지
        // 예시 (LeanTween 사용 시)
        youDiePanel.transform.localScale = Vector3.zero;
        youDiePanel.SetActive(true);
        LeanTween.scale(youDiePanel, Vector3.one * 1.2f, 0.2f)
            .setEaseOutBack()
            .setIgnoreTimeScale(true)
            .setOnComplete(() => {
                LeanTween.scale(youDiePanel, Vector3.one, 0.1f)
                    .setIgnoreTimeScale(true);
                
                LeanTween.scale(youDiePanel, Vector3.one * 1.1f, 1f)
                    .setEaseInOutSine()
                    .setLoopPingPong()
                    .setIgnoreTimeScale(true);

            });


    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        // 현재 씬 다시 로드
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        
    }
}