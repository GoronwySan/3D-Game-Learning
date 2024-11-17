using UnityEngine;

public class GroundController : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject ground;

    void Start()
    {
        // 自动查找主摄像机
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("未找到主摄像机，请确保场景中存在一个带有 'MainCamera' 标签的摄像机。");
            return;
        }

        // 创建一个新的地面 GameObject
        ground = new GameObject("Ground");

        // 添加一个平面 Mesh 来作为地面显示
        MeshFilter meshFilter = ground.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = ground.AddComponent<MeshRenderer>();

        // 为地面对象指定一个基本的平面 Mesh
        meshFilter.mesh = CreatePlaneMesh();

        // 设置地面对象的标签为 "Finish"
        ground.tag = "Finish";

        // 添加一个碰撞体（例如 BoxCollider），以便与其他物体碰撞
        BoxCollider collider = ground.AddComponent<BoxCollider>();
        collider.size = new Vector3(20, 1, 20); // 设置合适的大小

        // 加载并应用地面材质
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

        // 设置地面的初始位置
        ground.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y - 5, mainCamera.transform.position.z);
        ground.transform.localScale = new Vector3(20, 1, 20); // 调整地面的大小
    }

    void Update()
    {
        // 更新地面的水平位置，使其跟随摄像机
        if (mainCamera != null)
        {
            Vector3 newPos = mainCamera.transform.position;
            newPos.y = ground.transform.position.y; // 保持地面的 y 坐标不变
            ground.transform.position = newPos;
        }
    }

    // 创建一个简单的平面 Mesh
    Mesh CreatePlaneMesh()
    {
        Mesh planeMesh = new Mesh();

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(-5, 0, -5); // 左下角
        vertices[1] = new Vector3(5, 0, -5);  // 右下角
        vertices[2] = new Vector3(-5, 0, 5);  // 左上角
        vertices[3] = new Vector3(5, 0, 5);   // 右上角

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
