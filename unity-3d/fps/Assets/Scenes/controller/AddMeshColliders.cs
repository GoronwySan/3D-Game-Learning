using UnityEngine;
using UnityEngine.AI;

public class AddMeshColliders : MonoBehaviour
{
    void Start()
    {
        // 遍历所有子物体
        foreach (Transform child in transform)
        {
            // 检查子物体是否有 MeshRenderer 组件（用于判断是否需要添加 MeshCollider 和 NavMeshObstacle）
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                // 如果没有 Collider 组件，添加 MeshCollider
                Collider existingCollider = child.GetComponent<Collider>();
                if (existingCollider == null)
                {
                    MeshCollider meshCollider = child.gameObject.AddComponent<MeshCollider>();
                    meshCollider.convex = true; // 设置 MeshCollider 为凸形
                    meshCollider.isTrigger = true; // 使碰撞体成为触发器
                }

                // 添加 NavMeshObstacle 组件
                NavMeshObstacle navMeshObstacle = child.gameObject.GetComponent<NavMeshObstacle>();
                if (navMeshObstacle == null)
                {
                    navMeshObstacle = child.gameObject.AddComponent<NavMeshObstacle>();
                    navMeshObstacle.carving = true; // 启用动态 carving 功能
                    navMeshObstacle.carveOnlyStationary = false; // 允许动态移动的障碍物影响导航网格
                }
            }
        }
    }
}
