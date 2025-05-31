using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAssassination : MonoBehaviour , IAssassinatable
{
    
    
    private EnemyAI enemyAI;      // 미리 연결
    private EnemyMover enemyMover;
    private EnemySight2D enemySight;
    private EnemyManager enemyManager;
    
    public float assassinationRange = 2f;
    public AudioClip assassinateSound;
    private Transform playerTransform;

    
    [SerializeField] private GameObject[] bloodEffects;
    [SerializeField] private GameObject assassinationUI;
    private AudioSource audioSource;

    private void Awake()
    {
        enemyAI = GetComponent<EnemyAI>();
        playerTransform = enemyAI.playerTransform;

        enemyManager = FindObjectOfType<EnemyManager>();
        enemyMover = GetComponent<EnemyMover>();
        enemySight = GetComponent<EnemySight2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    // Update is called once per frame
    void Update()
    {
        bool canBeAssassinated = CanBeAssassinated(playerTransform);
        assassinationUI.SetActive(canBeAssassinated);
    }
    Vector2 RotateVector(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
    }

    private void SpawnBloodEffect()
    {
        if (bloodEffects == null || bloodEffects.Length == 0)
            return;

        // 랜덤 인덱스 선택
        int index = Random.Range(0, bloodEffects.Length);

        // 선택된 이펙트 활성화 및 위치 맞춤
        GameObject effect = bloodEffects[index];
        effect.SetActive(true);
    }
    
    public bool CanBeAssassinated(Transform player)
    {
        float viewradius = enemyAI.viewRadius;
        float viewAngle = enemyAI.viewAngle;
        Vector2 toPlayer = (player.position - transform.position).normalized;
        Vector2 forward = transform.rotation * -Vector2.up;
        
        float angle = Vector2.Angle(forward , toPlayer);

        bool isOutOfview = angle > viewAngle / 2f;
        float distance = Vector2.Distance(player.position, transform.position);
        bool isInRange = distance <= assassinationRange;
        
        return isOutOfview && isInRange;
    }

    public void OnAssassinate()
    {   
        assassinationUI.SetActive(false);
        enemyAI.alertUI.SetActive(false);
        
        // AI 즉시 멈춤
        
        
        this.enabled = false;
        enemyAI.enabled = false;
        enemyMover.enabled = false;
        enemySight.enabled = false;
        
        
        // 코루틴 시작 - 화면 멈춤, 효과, 딜레이, 삭제 순서 처리
        StartCoroutine(AssassinateSequence());
    }

    private IEnumerator AssassinateSequence()
    {
        // 게임 일시정지
        Time.timeScale = 0f;
    
        // timeScale 영향을 받지 않는 딜레이 (0.3초)
        yield return new WaitForSecondsRealtime(0.3f);
        audioSource.PlayOneShot(assassinateSound);
        // 카메라 흔들기 시작
        StartCoroutine(ShakeCamera(0.2f, 0.1f));

        // 피 효과 생성
        SpawnBloodEffect();
        
        //적 수 -1
        enemyManager.RemoveEnemy();
        
        // 게임 재개
        Time.timeScale = 1f;

        // 3초 대기 (게임 진행 중)
        yield return new WaitForSeconds(3f);

        // 적 오브젝트 삭제
        Destroy(gameObject);
    }

    private IEnumerator ShakeCamera(float duration, float magnitude)
    {
        Vector3 originalPos = Camera.main.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.localPosition = originalPos + new Vector3(x, y, 0);

            // timeScale 영향 없이 시간 측정
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        // 흔들기 종료 후 원위치 복귀
        Camera.main.transform.localPosition = originalPos;
    }



}



/*void OnDrawGizmosSelected()
    {
        Vector2 pos = transform.position;
        Vector2 forward = transform.rotation * -Vector2.up;

        Gizmos.color = Color.yellow;

        // 왼쪽 시야선
        Vector2 leftRay = RotateVector(forward, -viewAngle / 2f) * viewRadius;
        Gizmos.DrawLine(pos, pos + leftRay);

        // 오른쪽 시야선
        Vector2 rightRay = RotateVector(forward, viewAngle / 2f) * viewRadius;
        Gizmos.DrawLine(pos, pos + rightRay);
    }*/

// 각도(degrees) 만큼 벡터를 회전시키는 함수
