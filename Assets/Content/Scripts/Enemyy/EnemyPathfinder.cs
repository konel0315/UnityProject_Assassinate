using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyPathfinder : MonoBehaviour
{
    
    public Tilemap floorTilemap;
    
    
    
    HashSet<Vector2Int> walkablePositions = new HashSet<Vector2Int>();
    void Start()
    {
        InitializeWalkablePositions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    void InitializeWalkablePositions()
    {
        walkablePositions.Clear();
        int width = 20;
        int height = 30;

        for (int x = -width; x <= width; x++)
        {
            for (int y = -height; y <= height; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                if (floorTilemap.HasTile(cellPos))
                {
                    walkablePositions.Add(new Vector2Int(x, y));
                }
            }
        }
    }
    
    public List<Vector2Int> FindPath(Vector2Int startPos, Vector2Int goalPos)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        queue.Enqueue(startPos);
        cameFrom[startPos] = startPos;

        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(0, 1),   // 위
            new Vector2Int(0, -1),  // 아래
            new Vector2Int(1, 0),   // 오른쪽
            new Vector2Int(-1, 0),  // 왼쪽
        };

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            if (current == goalPos)
            {
                // 도착했으면 경로 생성
                return ReconstructPath(cameFrom, startPos, goalPos);
            }

            foreach (Vector2Int dir in directions)
            {
                Vector2Int neighbor = current + dir;

                // 이동 가능하고, 아직 방문 안 했으면
                if (walkablePositions.Contains(neighbor) && !cameFrom.ContainsKey(neighbor))
                {
                    queue.Enqueue(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }

        // 경로 못 찾으면 null 반환
        return null;
    }

    // 3. 경로 되짚기 함수
    List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int start, Vector2Int goal)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int current = goal;

        while (current != start)
        {
            path.Add(current);
            current = cameFrom[current];
        }
        path.Add(start);
        path.Reverse();
        return path;
    }
    
}
