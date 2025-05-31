using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Assassinated : MonoBehaviour
{
    public float assassinationRange = 2f;
    public LayerMask enemyLayerMask;
    private float lastAssassinationTime = -Mathf.Infinity;
    float backDistance = 1f;  // 적 뒤로 1 유닛
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastAssassinationTime >= 3f) // 3초 쿨타임 체크
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                EnemyAssassination target = FindAssassinationTarget();
                if (target != null)
                {
                    
                    Assassinated(target);
                    lastAssassinationTime = Time.time;
                }
            }
        }
        else
        {
            // 쿨타임 중이라면 무시하거나 효과음/피드백 줄 수도 있음
        }
    }
    
    EnemyAssassination FindAssassinationTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, assassinationRange, enemyLayerMask);
        List<EnemyAssassination> candidates = new List<EnemyAssassination>();

        foreach (var hit in hits)
        {
            var enemy = hit.GetComponent<EnemyAssassination>();
            if (enemy != null && enemy.CanBeAssassinated(transform))
            {
                candidates.Add(enemy);
            }
        }

        if (candidates.Count == 0)
            return null;

        // 거리순 정렬
        candidates.Sort((a, b) => Vector2.Distance(transform.position, a.transform.position)
            .CompareTo(Vector2.Distance(transform.position, b.transform.position)));

        return candidates[0];
    }


    private void Assassinated(EnemyAssassination target)
    {
        Vector3 backPosition = target.transform.position - (-target.transform.up) * backDistance; 
        transform.position = backPosition;
        float currentZ = target.transform.eulerAngles.z;
        float newZ = currentZ + 180f;
        transform.rotation = Quaternion.Euler(0f, 0f, newZ);
        target.OnAssassinate();
        
    }
}
