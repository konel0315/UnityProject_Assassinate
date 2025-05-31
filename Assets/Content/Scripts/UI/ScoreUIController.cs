using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUIController : MonoBehaviour
{
    
    public GameObject scorePanel;
    public TextMeshProUGUI clearTimeText;
    public TextMeshProUGUI heartCountText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI clearTimeScoreText;
    public TextMeshProUGUI heartScoreText;
    public Button RetryButton;
    public AudioSource scoreSound;

    public void ShowScore(int clearTime, int heartCount)
    {
        scorePanel.SetActive(true);
        int clearTimeScore = Mathf.Max(0, 1800 - clearTime * 10);
        int heartScore = heartCount * 500;
        int finalScore = clearTimeScore + heartScore;

        scoreSound.Play();  // 점수 애니메이션 시작할 때 한 번만 재생

        clearTimeText.gameObject.SetActive(true);
        LeanTween.value(0, clearTime, 1f).setIgnoreTimeScale(true).setOnUpdate((float val) =>
        {
            clearTimeText.text = $"Clear Time: {Mathf.FloorToInt(val)}s";
        }).setOnComplete(() =>
        {
            clearTimeScoreText.gameObject.SetActive(true);
            LeanTween.value(0, clearTimeScore, 1f).setIgnoreTimeScale(true).setOnUpdate((float val) =>
            {
                clearTimeScoreText.text = $"Time Score: {Mathf.FloorToInt(val)}";
            }).setOnComplete(() =>
            {
                heartCountText.gameObject.SetActive(true);
                LeanTween.value(0, heartCount, 1f).setIgnoreTimeScale(true).setOnUpdate((float val) =>
                {
                    heartCountText.text = $"Save Heart: {Mathf.FloorToInt(val)}";
                }).setOnComplete(() =>
                {
                    heartScoreText.gameObject.SetActive(true);
                    LeanTween.value(0, heartScore, 1f).setIgnoreTimeScale(true).setOnUpdate((float val) =>
                    {
                        heartScoreText.text = $"Heart Score: {Mathf.FloorToInt(val)}";
                    }).setOnComplete(() =>
                    {
                        finalScoreText.gameObject.SetActive(true);
                        LeanTween.value(0, finalScore, 1f).setIgnoreTimeScale(true).setOnUpdate((float val) =>
                        {
                            finalScoreText.text = $"Score: {Mathf.FloorToInt(val)}";
                        }).setOnComplete(() =>
                        {
                            scoreSound.Stop();  // 모든 점수 애니 완료 후에 소리 멈춤
                        });
                        RetryButton.gameObject.SetActive(true);
                    });
                });
            });
        });
    }
    public void Retry()
    {
        Time.timeScale = 1f;
        // 현재 씬 다시 로드
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        
    }
}