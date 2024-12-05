using UnityEngine;

public class TargetController : MonoBehaviour
{
    public GameObject targetPrefab;  // 目标预制体
    public GameObject a;  // 目标预制体

    public int targetCount = 10;     // 生成的目标数量
    public Vector3 startPosition = new Vector3(100f, 0f, -150f); // 目标起始位置
    public Vector3 rotation = new Vector3(90f, 0f, 0f);  // 目标旋转角度
    public Vector3 scale = new Vector3(50f, 1f, 50f);    // 目标缩放比例

    public Vector3 s = new Vector3(105f, 0f, -180f); // 目标起始位置
    public Vector3 r = new Vector3(-90f, 0f, 0f);  // 目标旋转角度
    public Vector3 sc = new Vector3(200f, 20f, 20f);    // 目标缩放比例

    public int totalTargetsDestroyed = 0;  // 已销毁的目标数量
    private GameObject[] allTargets;  // 所有目标

    void Start()
    {
        // 加载资源
        targetPrefab = Resources.Load<GameObject>("target/Military target");
        a = Resources.Load<GameObject>("target/Training table");

        if (targetPrefab == null)
        {
            Debug.LogError("目标预制体加载失败！确保路径正确并存在目标资源。");
            return;
        }

        // 生成目标
        allTargets = new GameObject[targetCount];
        for (int i = 0; i < targetCount; i++)
        {
            SpawnTarget(i);
        }
        for (int i = 0; i < 9; i++)
        {
            SpawnTable(i);
        }
    }

    void SpawnTarget(int index)
    {
        // 计算目标位置
        Vector3 spawnPosition = startPosition + new Vector3(index * 10f, 0f, 0f);  // 每个目标按 X 轴偏移

        // 实例化目标
        GameObject target = Instantiate(targetPrefab, spawnPosition, Quaternion.Euler(rotation));

        // 设置目标标签
        target.tag = "Target";

        // 设置目标缩放
        target.transform.localScale = scale;

        // 将目标添加到 allTargets 数组
        allTargets[index] = target;
    }

    void SpawnTable(int index)
    {
        // 计算目标位置
        Vector3 ss = s + new Vector3(index * 10f, 0f, 0f);  // 每个目标按 X 轴偏移

        // 实例化目标
        GameObject target = Instantiate(a, ss, Quaternion.Euler(r));

        // 设置目标缩放
        target.transform.localScale = sc;
    }

    public void OnTargetDestroyed()
    {
        // 增加销毁的目标数量
        totalTargetsDestroyed++;

        // 检查是否所有目标都已销毁
        if (totalTargetsDestroyed >= targetCount)
        {
            Debug.Log("所有目标已销毁！");
        }
    }

    // 销毁所有目标对象的公有函数
    public void DestroyAllTargets()
    {
        // 遍历所有目标并销毁
        for (int i = 0; i < allTargets.Length; i++)
        {
            if (allTargets[i] != null)
            {
                Destroy(allTargets[i]);
            }
        }

        // 清空 allTargets 数组
        allTargets = new GameObject[targetCount];
        Debug.Log("所有目标已销毁！");
    }

    // 外部脚本可以调用此方法来销毁 TargetController 脚本
    public void DestroyController()
    {
        Debug.Log("销毁 TargetController 脚本！");
        Destroy(this);  // 销毁 TargetController 脚本
    }
}

public class Destroyable : MonoBehaviour
{
    public delegate void DestroyedHandler();
    public event DestroyedHandler onDestroyed;

    // 销毁对象时调用
    void OnDestroy()
    {
        // 如果有注册的销毁事件，就触发它
        onDestroyed?.Invoke();
    }
}
