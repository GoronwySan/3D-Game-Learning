using UnityEngine;

public class HealingItem : MonoBehaviour
{
    // 碰撞检测
    // void OnCollisionEnter(Collision collision)
    // {
    //     // 检查是否碰到标记为 "Player" 的对象
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         // 获取Player脚本并调用HealHealth函数
    //         Player playerScript = collision.gameObject.GetComponent<Player>();
    //         if (playerScript != null)
    //         {
    //             playerScript.HealHealth(10);
    //         }
    //     }
    // }

    // 如果使用触发器的话，可以使用 OnTriggerEnter
    void OnTriggerEnter(Collider other)
    {
        // 输出调试信息，检测是否与触发器接触
        Debug.Log($"HealingItem OnTriggerEnter: 检测到与对象 {other.gameObject.name} 的碰撞。");

        // 检查是否碰到标记为 "Player" 的对象
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("HealingItem: 碰到Player对象，开始恢复生命值。");

            // 获取Player脚本并调用HealHealth函数
            Player playerScript = other.gameObject.GetComponent<Player>();
            if (playerScript != null)
            {
                Debug.Log("HealingItem: 调用Player脚本的HealHealth函数。");
                playerScript.HealHealth(10); // 假设恢复 10 点生命值
            }
            else
            {
                Debug.LogWarning("HealingItem: 找不到Player脚本！");
            }

            // 加载并显示爆炸效果
            GameObject explosionEffect = Resources.Load<GameObject>("CFXR Prefabs/Magic Misc/CFXR3 Magic Aura A (Runic)");
            if (explosionEffect != null)
            {
                Debug.Log("HealingItem: 实例化爆炸效果。");
                GameObject effectInstance = Instantiate(explosionEffect, transform.position, Quaternion.identity);
                // 延时3秒后销毁爆炸效果
                Destroy(effectInstance, 3f);
            }
            else
            {
                Debug.LogWarning("HealingItem: 未找到效果资源！");
            }

            // 销毁当前Healing Item对象
            Debug.Log("HealingItem: 销毁Healing Item对象。");
            Destroy(gameObject);
        }
        else
        {
            // 如果不是Player对象，输出调试信息
            Debug.Log($"HealingItem: 与非Player对象 {other.gameObject.name} 碰撞。");
        }
    }
}
