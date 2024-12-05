using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowController : MonoBehaviour
{
    private AudioSource audioSource;  // 音频源组件
    private AudioClip arrowSound;  // 用于存储箭的音效
    public GameObject player;  // 玩家角色引用，确保在 Inspector 中赋值

    private Animator crossbowAnimator;
    private bool isFiring = false;
    private Camera mainCamera;

    // 用于存放箭的对象池
    public GameObject arrowPrefab;
    private Queue<GameObject> arrowPool = new Queue<GameObject>();
    private List<GameObject> activeArrows = new List<GameObject>(); // 正在使用的箭
    public int poolSize = 15;
    public float arrowSpeed = 500f;
    public float recycleTime = 10f;

    public float customGravity = -9f;  // 你可以调整这个值来改变箭的重力

    private Dictionary<GameObject, int> shootingPositions = new Dictionary<GameObject, int>(); // 记录每个ShootingPosition的射击次数

    void Start()
    {
        // 确保玩家对象在场景中
        if (player == null)
        {
            player = GameObject.Find("PlayerCapsule");  // 假设玩家的 GameObject 名为 PlayerCapsule
        }

        // 获取 AudioSource 组件
        audioSource = GetComponent<AudioSource>();

        // 如果没有 AudioSource 组件，则添加一个
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        crossbowAnimator = GetComponent<Animator>();
        mainCamera = Camera.main;

        // 加载箭的Prefab
        if (arrowPrefab == null)
        {
            arrowPrefab = Resources.Load<GameObject>("crossbow/Arrow");
        }


        // 初始化箭对象池
        for (int i = 0; i < poolSize; i++)
        {
            GameObject arrow = Instantiate(arrowPrefab);
            arrow.SetActive(false); // 初始时所有箭都不显示
            arrowPool.Enqueue(arrow);
            Arrow arr = arrow.AddComponent<Arrow>();
        }

        // 从 Resources 文件夹加载箭的音效
        LoadArrowSound();

        // 初始化每个标记为ShootingPosition的物体的射击次数
        GameObject[] shootingPositionsArray = GameObject.FindGameObjectsWithTag("ShootingPosition");
        foreach (GameObject position in shootingPositionsArray)
        {
            shootingPositions[position] = 0;  // 每个位置初始射击次数为0
        }
    }

    // 加载箭的音效
    private void LoadArrowSound()
    {
        // 加载存储在 Resources 文件夹中的音效
        arrowSound = Resources.Load<AudioClip>("7924");

        // 检查音效是否加载成功
        if (arrowSound == null)
        {
            Debug.LogError("Arrow sound not found in Resources/Sounds/ArrowSound.");
        }
        else
        {
            Debug.Log("Arrow sound loaded successfully.");
        }
    }    

    void Update()
    {
        // Crossbow跟随main camera的旋转
        Vector3 targetDirection = mainCamera.transform.forward;
        transform.rotation = Quaternion.LookRotation(targetDirection);

        // 鼠标左键触发 Fire 动作
        if (Input.GetMouseButtonDown(0)) // 0表示鼠标左键
        {
            // 先检查是否站在标记为ShootingPosition的物体上
            if (IsPlayerAtShootingPosition())
            {
                // 切换 firing 状态，交替触发 Fire 动画
                isFiring = !isFiring;  // 切换状态

                // 设置 Fire 参数，根据 isFiring 状态来触发动画
                crossbowAnimator.SetBool("Fire", isFiring);
                if (!isFiring) FireArrow();
            }
            else
            {
                Debug.Log("You must stand on a ShootingPosition to shoot.");
            }
        }

    }

    // 判断玩家是否站在标记为ShootingPosition的物体上
    // 判断玩家是否站在标记为ShootingPosition的物体上
    private bool IsPlayerAtShootingPosition()
    {
        foreach (var entry in shootingPositions)
        {
            GameObject shootingPosition = entry.Key;

            // 获取玩家的位置
            Vector3 playerPosition = player.transform.position;

            // 检查玩家是否在 ShootingPosition 的碰撞体范围内，且射击次数未超过 10 次
            if (shootingPosition.GetComponent<Collider>().bounds.Contains(playerPosition) && shootingPositions[shootingPosition] < 100)
            {
                return true;
            }
        }
        return false;
    }


    // 获取是否能射箭的公有函数
    public bool CanShootArrow()
    {
        foreach (var entry in shootingPositions)
        {
            GameObject shootingPosition = entry.Key;

            // 获取玩家的位置
            Vector3 playerPosition = player.transform.position;

            // 检查玩家是否在 ShootingPosition 的碰撞体范围内，且射击次数未超过 10 次
            if (shootingPosition.GetComponent<Collider>().bounds.Contains(playerPosition) && shootingPositions[shootingPosition] < 100)
            {
                Debug.Log($"Player can shoot from {shootingPosition.name}. Remaining shots: {10 - shootingPositions[shootingPosition]}");
                return true;
            }
        }

        Debug.Log("Player cannot shoot: Not in any valid ShootingPosition.");
        return false;
    }

    // 获取剩余箭的数量
    public int GetRemainingArrows()
    {
        foreach (var entry in shootingPositions)
        {
            GameObject shootingPosition = entry.Key;

            // 获取玩家的位置
            Vector3 playerPosition = player.transform.position;

            // 检查玩家是否在 ShootingPosition 的碰撞体范围内
            if (shootingPosition.GetComponent<Collider>().bounds.Contains(playerPosition))
            {
                int remainingArrows = 10 - shootingPositions[shootingPosition];
                Debug.Log($"Remaining arrows at {shootingPosition.name}: {remainingArrows}");
                return remainingArrows;
            }
        }

        Debug.Log("No valid ShootingPosition: Returning 0 arrows.");
        return 0;  // 如果不满足条件，则返回 0
    }



    // 发射箭
    private void FireArrow()
    {
        GameObject arrow;

        if (arrowPool.Count > 0)
        {
            // 从池中获取一个箭
            arrow = arrowPool.Dequeue();
        }
        else
        {
            // 如果没有可用的箭，回收最早的活动箭
            arrow = activeArrows[0];
            activeArrows.RemoveAt(0); // 从活动箭列表移除
            arrow.SetActive(false);  // 禁用箭
        }

        // 激活箭并设置位置和旋转
        arrow.SetActive(true);
        arrow.transform.position = transform.position;
        arrow.transform.rotation = transform.rotation;

        // 添加到活动箭列表
        activeArrows.Add(arrow);

        // 获取箭的刚体组件并施加力
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 确保刚体不是 Kinematic
            rb.isKinematic = false;
            rb.mass = 0.1f;  // 将质量设置为较小值，减轻重力的影响
            rb.drag = 0.05f;  // 降低空气阻力
            rb.angularDrag = 0.05f;  // 降低角度阻力
            rb.velocity = Vector3.zero; // 重置速度
            rb.useGravity = false;
            rb.AddForce(Vector3.up * customGravity, ForceMode.Acceleration);
            rb.AddForce(transform.forward * arrowSpeed, ForceMode.VelocityChange);
        }

        // 播放箭发射的音效
        if (arrowSound != null)
        {
            audioSource.PlayOneShot(arrowSound);  // 播放一次音效
        }

        // 更新当前射击位置的射击次数
        foreach (var entry in shootingPositions)
        {
            GameObject shootingPosition = entry.Key;
            Vector3 playerPosition = player.transform.position;
            // 检查玩家是否在当前射击位置
            if (shootingPosition.GetComponent<Collider>().bounds.Contains(playerPosition))
            {
                shootingPositions[shootingPosition]++;  // 更新射击次数
                //Debug.Log($"Updated shooting position {shootingPosition.name} shot count: {shootingPositions[shootingPosition]}");
                break;  // 只更新一个位置
            }
        }

        // 启动回收箭的协程
        StartCoroutine(RecycleArrow(arrow));
    }

    // 回收箭，10秒后
    private IEnumerator RecycleArrow(GameObject arrow)
    {
        // 等待10秒后回收箭
        yield return new WaitForSeconds(recycleTime);

        // 停止箭的运动并隐藏
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        if (rb != null && rb.isKinematic == false)
        {
            rb.velocity = Vector3.zero; // 停止刚体的移动
        }

        activeArrows.Remove(arrow); // 从活动箭列表移除
        arrow.SetActive(false); // 隐藏箭
        arrowPool.Enqueue(arrow); // 将箭返回到对象池
    }
}
