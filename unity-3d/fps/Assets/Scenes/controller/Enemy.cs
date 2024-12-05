using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Enemy : MonoBehaviour
{
    public int health = 100;

    // 接受伤害的方法
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"{gameObject.name} 受伤！剩余生命: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} 死亡！");

        // 在敌人死亡位置生成爆炸效果
        GameObject explosionEffect = Resources.Load<GameObject>("CFXR Prefabs/Eerie/CFXR2 WW Enemy Explosion");
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("未找到爆炸效果资源！");
        }
        GameObject bloodBag = Resources.Load<GameObject>("Healing Item");
        // 实例化对象
        GameObject instance = Instantiate(bloodBag, transform.position + new Vector3(0, 5f, 0), Quaternion.identity);
        // 设置实例化对象的 y 轴为 0
        instance.transform.position = new Vector3(instance.transform.position.x, 0, instance.transform.position.z);
        Destroy(gameObject);  // 销毁敌人对象
    }

    // 敌人的行为方法（例如移动等）
    public abstract IEnumerator Move();

    public abstract IEnumerator Rotate();
}
