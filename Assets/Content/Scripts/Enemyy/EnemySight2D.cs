using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class EnemySight2D : MonoBehaviour
{
    [Header("시야 설정")]
    public int rayCount = 50;                // 광선 수 (조밀도)
    public LayerMask obstacleMask;           // 장애물 레이어

    private EnemyAI enemyAI;
    
    private Mesh mesh;
    
    void Awake()
    {
        enemyAI = GetComponent<EnemyAI>();
    }
    
    void OnDisable()
    {
        if (mesh != null)
        {
            mesh.Clear();
        }
    }
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void LateUpdate()
    {
        
        DrawFieldOfView();
    }

    void DrawFieldOfView()
    {
        float viewDistance = enemyAI.viewRadius;
        float fovAngle = enemyAI.viewAngle;

        Vector3 origin = Vector3.zero;

        // transform.up 방향 각도를 구함 (2D에서 z축 회전 각)
        float forwardAngle = transform.eulerAngles.z-90f;

        // 부채꼴 시작 각도 = forwardAngle - (fovAngle / 2)
        float angle = forwardAngle - fovAngle / 2f;

        float angleIncrease = fovAngle / rayCount;

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        vertices.Add(origin); // 중심 꼭짓점

        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 dir = DirFromAngle(angle);  // 로컬 기준 방향
            Vector3 end;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewDistance, obstacleMask);
            if (hit.collider != null)
            {
                // hit.point는 월드 좌표 → 로컬 좌표로 변환
                end = transform.InverseTransformPoint(hit.point);
            }
            else
            {
                // 최대 거리 위치도 월드 좌표로 계산 후 로컬 좌표 변환
                Vector3 worldEndPoint = transform.position + dir * viewDistance;
                end = transform.InverseTransformPoint(worldEndPoint);
            }

            vertices.Add(end);

            if (i > 0)
            {
                triangles.Add(0);
                triangles.Add(i + 1);
                triangles.Add(i);
            }

            angle += angleIncrease;
        }


        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }


    // 주어진 각도로부터 단위 방향 벡터 반환
    Vector3 DirFromAngle(float angleDegrees)
    {
        float rad = angleDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad));
    }
}