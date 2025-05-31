using UnityEngine;

public class FullscreenToggle : MonoBehaviour
{
    // 창 모드일 때 사용할 해상도
    public int windowWidth = 1280;
    public int windowHeight = 720;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Screen.fullScreen)
            {
                // 전체화면 → 창모드로 전환
                Screen.SetResolution(windowWidth, windowHeight, false);
            }
            else
            {
                // 창모드 → 전체화면으로 전환 (현재 디스플레이 해상도 사용)
                Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true);
            }
        }
    }
}