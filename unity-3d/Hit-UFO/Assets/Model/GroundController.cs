using UnityEngine;

public class GroundController : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject ground;

    void Start()
    {
        // �Զ������������
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("δ�ҵ������������ȷ�������д���һ������ 'MainCamera' ��ǩ���������");
            return;
        }

        // ����һ���µĵ��� GameObject
        ground = new GameObject("Ground");

        // ���һ��ƽ�� Mesh ����Ϊ������ʾ
        MeshFilter meshFilter = ground.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = ground.AddComponent<MeshRenderer>();

        // Ϊ�������ָ��һ��������ƽ�� Mesh
        meshFilter.mesh = CreatePlaneMesh();

        // ���õ������ı�ǩΪ "Finish"
        ground.tag = "Finish";

        // ���һ����ײ�壨���� BoxCollider�����Ա�������������ײ
        BoxCollider collider = ground.AddComponent<BoxCollider>();
        collider.size = new Vector3(20, 1, 20); // ���ú��ʵĴ�С

        // ���ز�Ӧ�õ������
        Texture2D groundTexture = Resources.Load<Texture2D>("Textures/GroundTexture");
        if (groundTexture != null)
        {
            Material groundMaterial = new Material(Shader.Find("Standard"));
            groundMaterial.mainTexture = groundTexture;
            meshRenderer.material = groundMaterial;
        }
        else
        {
            Debug.LogError("GroundTexture not found in Resources/Textures folder.");
        }

        // ���õ���ĳ�ʼλ��
        ground.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y - 5, mainCamera.transform.position.z);
        ground.transform.localScale = new Vector3(20, 1, 20); // ��������Ĵ�С
    }

    void Update()
    {
        // ���µ����ˮƽλ�ã�ʹ����������
        if (mainCamera != null)
        {
            Vector3 newPos = mainCamera.transform.position;
            newPos.y = ground.transform.position.y; // ���ֵ���� y ���겻��
            ground.transform.position = newPos;
        }
    }

    // ����һ���򵥵�ƽ�� Mesh
    Mesh CreatePlaneMesh()
    {
        Mesh planeMesh = new Mesh();

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(-5, 0, -5); // ���½�
        vertices[1] = new Vector3(5, 0, -5);  // ���½�
        vertices[2] = new Vector3(-5, 0, 5);  // ���Ͻ�
        vertices[3] = new Vector3(5, 0, 5);   // ���Ͻ�

        int[] triangles = new int[6];
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;
        triangles[3] = 1;
        triangles[4] = 2;
        triangles[5] = 3;

        planeMesh.vertices = vertices;
        planeMesh.triangles = triangles;
        planeMesh.RecalculateNormals();

        return planeMesh;
    }
}
