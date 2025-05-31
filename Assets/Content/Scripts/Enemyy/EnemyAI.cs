using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyAI : MonoBehaviour
{
    
    public float viewRadius = 3f;
    public float viewAngle = 60f;
    
    public float chaseDelay = 3f;    // 추격 유지 시간 (초)
    
    private float chaseTimer = 0f;
    private float saveSpeed = 0f;
    
    public LayerMask obstacleMask;    
    public Transform playerTransform;
    public List<Transform> patrolPoints;
    
    private EnemyMover mover;
    private EnemyState currentState = EnemyState.Patrol;
    private EnemyAttack attack;

    public GameObject alertUI;
    private float alertTimer = 0f;

    
    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<EnemyMover>();
        attack = GetComponent<EnemyAttack>();
        if (patrolPoints != null && patrolPoints.Count > 0)
        {
            mover.SetPatrolPoints(patrolPoints);
        }
    }

    void Update()
    {
        bool canSeePlayer = IsPlayerVisible(playerTransform);
        if(saveSpeed == 0) {saveSpeed = mover.defaultMoveSpeed;}
        if (canSeePlayer)
        {
            alertUI.SetActive(true);
            alertTimer = 1f; // 1초 유지
            currentState = EnemyState.Attack;
            mover.SetTarget(playerTransform.position);
            chaseTimer = chaseDelay;  // 시야 확보 시 추격 타이머 초기화
            if (attack != null)
            {
                attack.SetPlayerVisible(true);
                mover.defaultMoveSpeed = 0;
            }
        }
        else
        {
            mover.defaultMoveSpeed = saveSpeed;
            if (chaseTimer > 0)
            {
                // 아직 추격 유지 시간 남음
                chaseTimer -= Time.deltaTime;
                currentState = EnemyState.Chase;
                mover.SetTarget(playerTransform.position); // 마지막 플레이어 위치 추적
                
                if(attack != null)
                    attack.SetPlayerVisible(false);
            }
            else
            {
                // 추격 포기, 패트롤로 복귀
                currentState = EnemyState.Patrol;
                mover.ClearTarget();
                
                if(attack != null)
                    attack.SetPlayerVisible(false);
            }
            if (alertTimer > 0)
            {
                alertTimer -= Time.deltaTime;
                if (alertTimer <= 0)
                {
                    alertUI.SetActive(false); // 1초 후 꺼짐
                }
            }
        }

        mover.SetState(currentState);
    }

    public bool IsPlayerVisible(Transform player)
    {
        Vector2 toPlayer = (player.position - transform.position).normalized;
        Vector2 forward = transform.rotation * -Vector2.up;

        float angle = Vector2.Angle(forward, toPlayer);
        if (angle > viewAngle / 2f) return false;

        float distance = Vector2.Distance(player.position, transform.position);
        if (distance > viewRadius) return false;

        // 플레이어까지 장애물이 있는지 Raycast 검사
        RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer, distance, obstacleMask);
        if (hit.collider != null)
        {
            // 만약 히트된 콜라이더가 플레이어가 아니면 시야 차단됨
            if (hit.collider.transform != player)
                return false;
        }

        return true;
    }

}