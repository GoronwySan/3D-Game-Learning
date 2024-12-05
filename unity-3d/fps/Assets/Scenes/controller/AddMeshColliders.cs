using UnityEngine;
using UnityEngine.AI;

public class AddMeshColliders : MonoBehaviour
{
    void Start()
    {
        // ��������������
        foreach (Transform child in transform)
        {
            // ����������Ƿ��� MeshRenderer ����������ж��Ƿ���Ҫ��� MeshCollider �� NavMeshObstacle��
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                // ���û�� Collider �������� MeshCollider
                Collider existingCollider = child.GetComponent<Collider>();
                if (existingCollider == null)
                {
                    MeshCollider meshCollider = child.gameObject.AddComponent<MeshCollider>();
                    meshCollider.convex = true; // ���� MeshCollider Ϊ͹��
                    meshCollider.isTrigger = true; // ʹ��ײ���Ϊ������
                }

                // ��� NavMeshObstacle ���
                NavMeshObstacle navMeshObstacle = child.gameObject.GetComponent<NavMeshObstacle>();
                if (navMeshObstacle == null)
                {
                    navMeshObstacle = child.gameObject.AddComponent<NavMeshObstacle>();
                    navMeshObstacle.carving = true; // ���ö�̬ carving ����
                    navMeshObstacle.carveOnlyStationary = false; // ����̬�ƶ����ϰ���Ӱ�쵼������
                }
            }
        }
    }
}
