using UnityEngine;
using UnityEngine.SceneManagement;

public class Arrow : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // 获取碰撞物体的标签或类型
        string tag = collision.gameObject.tag;

        // 根据碰撞物体的标签或类型处理不同的反应
        switch (tag)
        {
            case "Enemy":
                // 如果碰到敌人，产生伤害或者其他反应
                HandleEnemyCollision(collision);
                break;
            case "Wood":
                // 如果碰到木材，可能会卡住箭或造成轻微反弹
                HandleWoodCollision(collision);
                break;
            case "Ground":
                // 如果碰到地面，箭可能会停下来
                HandleGroundCollision(collision);
                break;
            case "Target":
                // 如果碰到目标，执行相关逻辑
                HandleTargetCollision(collision);
                break;
            case "Training":
                // 如果碰到目标，执行相关逻辑
                HandleTrainingCollision(collision);
                break;
            case "quit":
                // 如果碰到目标，执行相关逻辑
                HandleQuitCollision(collision);
                break;
            case "switch":
                // 如果碰到目标，执行相关逻辑
                ImageMovement i;
                // 获取 TargetController 脚本引用
                i = FindObjectOfType<ImageMovement>();
                i.SwitchImages();
                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero; // 停止箭的运动
                }
                break;
            case "return":
                // 如果碰到目标，执行相关逻辑
                Cursor.lockState = CursorLockMode.None;  // 解锁鼠标
                Cursor.visible = true;  // 显示鼠标
                SceneManager.LoadScene("SampleScene");
                break;
            default:
                // 对其他类型的物体进行默认处理
                HandleDefaultCollision(collision);
                break;
        }
    }

    // 处理与敌人的碰撞
    private void HandleEnemyCollision(Collision collision)
    {
        ScoreManager.Instance.AddScore(10);  // 增加 10 分
                                             // 获取敌人对象（Animal）
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        // 如果敌人存在，调用 TakeDamage 方法
        if (enemy != null)
        {
            enemy.TakeDamage(100);  // 通知敌人受到伤害
        }

        Debug.Log("箭击中敌人！");
        // 销毁箭或做其他处理
        gameObject.SetActive(false);  // 停用箭
    }

    // 处理与木材的碰撞
    private void HandleWoodCollision(Collision collision)
    {
        Debug.Log("箭卡住了木材！");
        // 停止箭的运动，可能不再回池
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero; // 停止箭的运动
        }
    }

    // 处理与地面的碰撞
    private void HandleGroundCollision(Collision collision)
    {
        Debug.Log("箭落地！");
        // 停止物理模拟，使箭静止
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;  // 停止物理模拟
        }
    }

    // 处理与目标的碰撞
    private void HandleTargetCollision(Collision collision)
    {
        ScoreManager.Instance.AddScore(50);  // 增加 50 分
        Debug.Log("箭击中目标！");
        //Rigidbody rb = GetComponent<Rigidbody>();
        //if (rb != null)
        //{
        //    rb.isKinematic = true;  // 停止物理模拟
        //}
        // 你可以在这里做更多的处理，比如让目标消失或触发特殊效果
        //collision.gameObject.SetActive(false);  // 假设目标在被击中后消失
        //gameObject.SetActive(false);  // 停用箭
                                      // 在敌人死亡位置生成爆炸效果
        GameObject explosionEffect = Resources.Load<GameObject>("CFXR Prefabs/Explosions/CFXR4 Firework HDR Shoot Single (Random Color)");
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, collision.transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("未找到爆炸效果资源！");
        }
        // 销毁碰撞的目标对象
        Destroy(collision.gameObject);  // 销毁目标对象
        TargetController targetController;
        // 获取 TargetController 脚本引用
        targetController = FindObjectOfType<TargetController>();
        targetController.OnTargetDestroyed();
        // 在需要的条件下（比如所有目标都被销毁），销毁 TargetController 脚本
        if (targetController != null && targetController.totalTargetsDestroyed >= targetController.targetCount + 10)
        {
            targetController.DestroyController();  // 通过外部脚本销毁 TargetController 脚本
            Invoke(nameof(HandleQuitCollision), 10f);
            HandleQuitCollision(collision);
        }
    }

    private void HandleTrainingCollision(Collision collision)
    {

        GameObject a = GameObject.Find("GameObject");
        GameObject newTarget = Instantiate(a, new Vector3(0, 0, 0), Quaternion.identity);

            // 为新对象添加TargetController脚本
        newTarget.AddComponent<TargetController>();

        GameObject c = GameObject.Find("PlayerCapsule");
        CharacterController characterController;
        // 获取角色控制器组件
        characterController = c.GetComponent<CharacterController>();
        characterController.enabled = false; // 禁用控制器
        c.transform.position = new Vector3(150, 0, -185); // 直接设置位置
        characterController.enabled = true; // 启用控制器

    }

    private void HandleQuitCollision(Collision collision)
    {
        GameObject c = GameObject.Find("PlayerCapsule");
        CharacterController characterController;
        // 获取角色控制器组件
        characterController = c.GetComponent<CharacterController>();
        characterController.enabled = false; // 禁用控制器
        c.transform.position = new Vector3(0, 0, -10); // 直接设置位置
        characterController.enabled = true; // 启用控制器
        TargetController targetController;
        // 获取 TargetController 脚本引用
        targetController = FindObjectOfType<TargetController>();
        targetController.DestroyAllTargets();

    }

    // 默认碰撞处理
    private void HandleDefaultCollision(Collision collision)
    {
        Debug.Log("箭与" + collision.gameObject.name + "发生了碰撞");
        // 停止物理模拟，使箭静止
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;  // 停止物理模拟
        }
        // 可以选择销毁箭或做其他处理
        //gameObject.SetActive(false);  // 停用箭
    }
}
