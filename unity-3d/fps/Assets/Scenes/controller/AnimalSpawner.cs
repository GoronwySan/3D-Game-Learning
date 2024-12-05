using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnimalSpawner : MonoBehaviour
{
    // 资源路径
    private string crowPath = "birds/dexsoft-birds/crow";
    private string duckFPath = "birds/dexsoft-birds/duck_f";
    private string duckMPath = "birds/dexsoft-birds/duck_m";

    // 每种动物需要生成的数量
    public int animalsPerType = 10;

    // 动物生成的位置范围
    public Vector3 spawnMin = new Vector3(-125, 0, -100);
    public Vector3 spawnMax = new Vector3(100, 0, 100);

    // 移动速度（根据动画类型可以调整）
    public float moveSpeed = 2f;

    // 存储所有的乌鸦
    private List<GameObject> allCrows = new List<GameObject>();

    // 游戏成功的提示
    public Text gameStatusText;

    // Start is called before the first frame update
    void Start()
    {
        // 使用 Resources.Load 加载动物 Prefabs
        GameObject crow = Resources.Load<GameObject>(crowPath);
        GameObject duck_f = Resources.Load<GameObject>(duckFPath);
        GameObject duck_m = Resources.Load<GameObject>(duckMPath);

        // 检查是否加载成功并打印调试信息
        if (crow == null)
        {
            Debug.LogWarning("Crow prefab not found at path: " + crowPath);
        }
        else
        {
            Debug.Log("Crow prefab loaded successfully.");
        }

        if (duck_f == null)
        {
            Debug.LogWarning("Female Duck prefab not found at path: " + duckFPath);
        }
        else
        {
            Debug.Log("Female Duck prefab loaded successfully.");
        }

        if (duck_m == null)
        {
            Debug.LogWarning("Male Duck prefab not found at path: " + duckMPath);
        }
        else
        {
            Debug.Log("Male Duck prefab loaded successfully.");
        }

        // 生成每种动物
        if (crow != null) SpawnAnimals(crow, "fly");
        if (duck_f != null) SpawnAnimals(duck_f, "walk");
        if (duck_m != null) SpawnAnimals(duck_m, "walk");
    }

    // 生成指定类型的动物
    void SpawnAnimals(GameObject animalPrefab, string animationName)
    {
        for (int i = 0; i < animalsPerType; i++)
        {
            // 随机生成位置
            Vector3 spawnPosition = new Vector3(
                Random.Range(spawnMin.x, spawnMax.x),
                animalPrefab.name.Contains("crow") ? Random.Range(10f, 20f) : 0f, // Crow 的 Y 坐标范围 (10, 20)
                Random.Range(spawnMin.z, spawnMax.z)
            );
            // 实例化动物
            GameObject animal = Instantiate(animalPrefab, spawnPosition, Quaternion.identity);

            // 设置标签为 'Enemy'
            animal.tag = "Enemy";

            // 添加物理属性并启用重力（默认情况）
            if (animalPrefab.name.Contains("crow"))
            {
                Rigidbody rb = animal.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = animal.AddComponent<Rigidbody>(); // 添加 Rigidbody 组件
                }
                rb.useGravity = false;  // 禁用重力，假设乌鸦不受重力影

                // 如果是乌鸦，添加到乌鸦列表中
                allCrows.Add(animal);
                Enemy enemy = animal.AddComponent<Crow>();  // 添加乌鸦行为
            }
            else
            {
                Enemy enemy = animal.AddComponent<Duck>();  // 添加鸭子行为
            }

            // 设置动物的动画等
            Animation animation = animal.GetComponent<Animation>();
            if (animation != null)
            {
                if (animation[animationName] != null)
                {
                    animation[animationName].wrapMode = WrapMode.Loop; // 设置动画循环播放
                    animation.Play(animationName); // 播放指定名称的动画
                }
                else
                {
                    Debug.LogWarning($"Animation clip '{animationName}' not found on {animalPrefab.name}. Please ensure an animation is attached.");
                }
            }
            else
            {
                Debug.LogWarning($"Animation component not found on {animalPrefab.name}. Animation may not play.");
            }

            // 给动物添加碰撞体
            Collider col = animal.GetComponent<Collider>();
            if (col == null)
            {
                col = animal.AddComponent<BoxCollider>(); // 添加 Collider 组件
            }
            BoxCollider boxCollider = col as BoxCollider;
            if (boxCollider != null)
            {
                boxCollider.size = new Vector3(3f, 3f, 3f);  // 设置大小
                boxCollider.center = new Vector3(0f, 1f, 0f);  // 设置中心点
            }
        }
    }

    // 当乌鸦被销毁时调用此方法
    public void OnCrowDestroyed(GameObject crow)
    {
        // 从列表中移除销毁的乌鸦
        if (allCrows.Contains(crow))
        {
            allCrows.Remove(crow);
        }

        // 如果所有的乌鸦都被销毁，显示游戏成功提示
        if (allCrows.Count == 0)
        {
            GameSuccess();
        }
    }

    // 游戏成功的处理方法
    void GameSuccess()
    {
        if (gameStatusText != null)
        {
            gameStatusText.text = "Game Success!";
            Cursor.lockState = CursorLockMode.None;  // 解锁鼠标
            Cursor.visible = true;  // 显示鼠标
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;  // 解锁鼠标
            Cursor.visible = true;  // 显示鼠标
            SceneManager.LoadScene("SampleScene");
            Debug.Log("Game Success!");
        }
    }
}
