using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public GameObject rayEffectPrefab; // 射线特效预制体
    public GameObject cam;
    private Camera mainCamera;
    public bool start = false;
    public void Init()
    {
        start = true;
        // 在此处添加延迟初始化逻辑（如需要）
        Debug.Log("PickupObject initialized");
    }

    void Start()
    {
        // 初始化摄像机引用
        mainCamera = cam != null ? cam.GetComponent<Camera>() : Camera.main;
        rayEffectPrefab = Resources.Load<GameObject>("RayEffect Variant");

    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            HandleObjectPickup();
        }
    }

    //private void ShowRayEffect(Vector3 start, Vector3 end)
    //{
    //    if (rayEffectPrefab == null)
    //    {
    //        Debug.LogError("Ray effect prefab is missing!");
    //        return;
    //    }

    //    // 实例化射线特效对象
    //    GameObject rayEffect = Instantiate(rayEffectPrefab);

    //    // 配置 LineRenderer
    //    LineRenderer lineRenderer = rayEffect.GetComponent<LineRenderer>();
    //    if (lineRenderer != null)
    //    {
    //        lineRenderer.SetPosition(0, start); // 起点
    //        lineRenderer.SetPosition(1, end);   // 终点
    //        lineRenderer.startWidth = 0.1f;     // 起点宽度
    //        lineRenderer.endWidth = 0.1f;       // 终点宽度
    //        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
    //        lineRenderer.material.color = Color.red; // 设置射线颜色为红色
    //    }
    //    else
    //    {
    //        Debug.LogError("LineRenderer component is missing on the ray effect prefab!");
    //    }

    //    // 自动销毁特效
    //    Destroy(rayEffect, 3.0f); // 3秒后销毁特效
    //}

    private void HandleObjectPickup()
    {
        if (start)
        {
            if (mainCamera == null)
            {
                Debug.LogWarning("Camera reference is missing.");
                return;
            }

            if (SSDirector.getInstance() == null)
            {
                Debug.LogWarning("SSDirector instance is null.");
                return;
            }

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // 检查游戏是否结束
                if (SSDirector.getInstance().gameEnd) return;
                // 显示射线动画效果
                //ShowRayEffect(ray.origin, hit.point);
                // 如果命中的对象是地面，直接返回
                if (hit.transform.gameObject.CompareTag("Finish"))
                {
                    Debug.Log("Hit the ground. Returning.");
                    return;
                }


                // 如果命中的对象没有父对象，处理对象本身
                if (hit.transform.parent == null)
                {
                    Debug.Log("Hit object has no parent, processing it directly.");
                    ProcessHitObject(hit.transform.gameObject);
                    return;
                }

                // 如果父对象存在，处理父对象
                if (hit.transform.parent.gameObject != null)
                {
                    Debug.Log("Processing parent object.");
                    ProcessHitObject(hit.transform.gameObject);
                }
                else
                {
                    Debug.Log("Hit object is null or does not have a valid parent. Returning.");
                }
            }
            else
            {
                Debug.Log("No object was hit by the ray.");
            }
        }
    }



    private void ProcessHitObject(GameObject hitObject)
    {
        SSDirector ssdDirector = SSDirector.getInstance();

        // 计算分数，基于与参考位置的距离
        Vector3 referencePoint = new Vector3(0, 1, 0);  // 可设为变量以方便配置
        int credit = (int)Vector3.Distance(hitObject.transform.position, referencePoint);
        //ssdDirector.AddCreditCount(credit);
        ScoreManager.Instance.AddScore(10); // 增加得分
        //ssdDirector.AddCreditCount(10);

        // 获取对象的索引并调用下落动作
        int objectIndex = ssdDirector.firstcontroller.getGameObjectIndex(hitObject.transform.position);
        if (objectIndex != -1)
        {
            ssdDirector.ccam.UFODrop(objectIndex);
        }
        else
        {
            Debug.LogWarning("Object index not found.");
        }
    }
}
