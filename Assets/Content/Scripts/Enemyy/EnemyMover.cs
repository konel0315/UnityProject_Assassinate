using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{

    private EnemyPathfinder pathfinder; // 에디터에서 연결
    private List<Vector2Int> patrolGridPoints = new List<Vector2Int>();
    private Vector3? chaseTarget = null;

    private EnemyState currentState = EnemyState.Patrol;

    private List<Vector2Int> path;
    private int pathIndex = 0;
    private int patrolIndex = 0;

    public float defaultMoveSpeed = 2f;
    public float findMoveSpeed = 1f;

    private void Awake()
    {
        pathfinder = GetComponent<EnemyPathfinder>();
    }

    void Start()
    {
        InvokeRepeating(nameof(UpdatePath), 0f, 1f);
    }

    public void SetPatrolPoints(List<Transform> patrolPoints)
    {
        patrolGridPoints.Clear();
        foreach (var point in patrolPoints)
        {
            patrolGridPoints.Add(WorldToGrid(point.position));
        }
    }

    public void SetTarget(Vector3 targetPos)
    {
        chaseTarget = targetPos;
    }

    public void ClearTarget()
    {
        chaseTarget = null;
    }

    public void SetState(EnemyState state)
    {
        if (state == EnemyState.Patrol)
        {
            findMoveSpeed = 1;
        }
        else if (state == EnemyState.Chase)
        {
            findMoveSpeed = 1f;
        }
        currentState = state;
    }

    void UpdatePath()
    {
        if (pathfinder == null) return;

        Vector2Int start = WorldToGrid(transform.position);
        Vector2Int goal;

        if ((currentState == EnemyState.Chase && chaseTarget.HasValue)||(currentState == EnemyState.Attack && chaseTarget.HasValue))
        {
            goal = WorldToGrid(chaseTarget.Value);
        }
        else
        {
            if (patrolGridPoints.Count == 0) return;
            goal = patrolGridPoints[patrolIndex];
        }
        path = pathfinder.FindPath(start, goal);
        pathIndex = 0;
    }

    void Update()
    {
        if (path == null || pathIndex >= path.Count) return;

        Vector3 targetWorldPos = GridToWorld(path[pathIndex]);
        transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, defaultMoveSpeed * findMoveSpeed*Time.deltaTime);
        
        Vector3 direction = (targetWorldPos - transform.position);
        if (direction.magnitude >= 0.15f)
        {
            direction.Normalize();
            Rotation(direction);
        }
        
        if (Vector3.Distance(transform.position, targetWorldPos) < 0.05f)
        {
            pathIndex++;

            if (currentState == EnemyState.Patrol && pathIndex >= path.Count)
            {
                patrolIndex = (patrolIndex + 1) % patrolGridPoints.Count;
                UpdatePath();
            }
        }
    }

    void Rotation(Vector3 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // x축 이동이 더 크면 좌우 판단
            if (direction.x > 0)
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);  // 오른쪽
            else
                transform.rotation = Quaternion.Euler(0f, 0f, -90f); // 왼쪽
        }
        else
        {
            // y축 이동이 더 크면 상하 판단
            if (direction.y > 0)
                transform.rotation = Quaternion.Euler(0f, 0f, 180f);  // 위
            else
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);  // 아래
        }
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        Vector3Int cell = pathfinder.floorTilemap.WorldToCell(worldPos);
        return new Vector2Int(cell.x, cell.y);
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return pathfinder.floorTilemap.GetCellCenterWorld(new Vector3Int(gridPos.x, gridPos.y, 0));
    }
}
