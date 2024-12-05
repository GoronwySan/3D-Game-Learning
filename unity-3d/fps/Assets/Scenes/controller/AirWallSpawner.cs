using UnityEngine;

public class AirWallSpawner : MonoBehaviour
{
    public Vector3 areaSize = new Vector3(100, 100, 100);  // 场地的尺寸
    public float wallThickness = 1f;  // 墙体的厚度

    void Start()
    {
        // 生成空气墙围绕整个场地
        CreateAirWalls(areaSize, wallThickness);
    }

    void CreateAirWalls(Vector3 areaSize, float wallThickness)
    {
        // 计算每一面墙的尺寸
        Vector3 wallSize = new Vector3(areaSize.x, areaSize.y, wallThickness);

        // 创建四面墙
        CreateWall(new Vector3(0, 0, 100), wallSize);  // 后墙
        CreateWall(new Vector3(0, 0, -100), wallSize); // 前墙
        CreateWall(new Vector3(100, 0, 0), new Vector3(wallThickness, areaSize.y, areaSize.z));  // 右墙
        CreateWall(new Vector3(-125, 0, 0), new Vector3(wallThickness, areaSize.y, areaSize.z)); // 左墙
    }

    void CreateWall(Vector3 position, Vector3 size)
    {
        // 创建一个新的物体作为墙体
        GameObject wall = new GameObject("AirWall");

        // 设置物体的位置
        wall.transform.position = position;

        // 添加碰撞体
        BoxCollider collider = wall.AddComponent<BoxCollider>();
        collider.size = size;

        // 设置碰撞体为物理墙（默认就是物理墙，无需设置为 Trigger）
        collider.isTrigger = false;

        // 为墙体添加刚体组件，让它具有物理属性
        Rigidbody rb = wall.AddComponent<Rigidbody>();
        rb.isKinematic = true; // 如果你不希望墙体移动，设置为 Kinematic
    }
}
