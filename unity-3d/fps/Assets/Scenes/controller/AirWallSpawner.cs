using UnityEngine;

public class AirWallSpawner : MonoBehaviour
{
    public Vector3 areaSize = new Vector3(100, 100, 100);  // ���صĳߴ�
    public float wallThickness = 1f;  // ǽ��ĺ��

    void Start()
    {
        // ���ɿ���ǽΧ����������
        CreateAirWalls(areaSize, wallThickness);
    }

    void CreateAirWalls(Vector3 areaSize, float wallThickness)
    {
        // ����ÿһ��ǽ�ĳߴ�
        Vector3 wallSize = new Vector3(areaSize.x, areaSize.y, wallThickness);

        // ��������ǽ
        CreateWall(new Vector3(0, 0, 100), wallSize);  // ��ǽ
        CreateWall(new Vector3(0, 0, -100), wallSize); // ǰǽ
        CreateWall(new Vector3(100, 0, 0), new Vector3(wallThickness, areaSize.y, areaSize.z));  // ��ǽ
        CreateWall(new Vector3(-125, 0, 0), new Vector3(wallThickness, areaSize.y, areaSize.z)); // ��ǽ
    }

    void CreateWall(Vector3 position, Vector3 size)
    {
        // ����һ���µ�������Ϊǽ��
        GameObject wall = new GameObject("AirWall");

        // ���������λ��
        wall.transform.position = position;

        // �����ײ��
        BoxCollider collider = wall.AddComponent<BoxCollider>();
        collider.size = size;

        // ������ײ��Ϊ����ǽ��Ĭ�Ͼ�������ǽ����������Ϊ Trigger��
        collider.isTrigger = false;

        // Ϊǽ����Ӹ������������������������
        Rigidbody rb = wall.AddComponent<Rigidbody>();
        rb.isKinematic = true; // ����㲻ϣ��ǽ���ƶ�������Ϊ Kinematic
    }
}
