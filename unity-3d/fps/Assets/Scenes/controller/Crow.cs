using UnityEngine;
using System.Collections;

// 子类：乌鸦
public class Crow : Enemy
{
    private Rigidbody rb;
    private GameObject target;  // 目标（Player）

    public float speed = 5f;  // 飞行速度
    public float rotationSpeed = 5f;  // 旋转平滑度
    public float attackRange = 3f;  // 攻击范围
    public float attackDistance = 1f;  // 进入攻击状态的最大距离
    public float attackCooldown = 2f;  // 攻击冷却时间

    private bool isAttacking = false;  // 是否正在攻击
    private Animation anim;  // 动画组件

    void Start()
    {
        // 获取 Rigidbody 组件
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody 组件未找到！");
        }

        // 获取目标对象（PlayerCapsule）
        target = GameObject.Find("PlayerCapsule");

        if (target != null)
        {
            Debug.Log("目标对象 'PlayerCapsule' 已找到。");
        }
        else
        {
            Debug.LogWarning("未找到目标对象 'PlayerCapsule'！");
        }

        // 获取 Animation 组件
        anim = GetComponent<Animation>();

        if (anim == null)
        {
            Debug.LogError("Animation 组件未找到！");
        }

        // 启动移动协程
        StartCoroutine(Move());
        StartCoroutine(Rotate());
    }

    // 平滑飞行
    public override IEnumerator Move()
    {
        while (true)
        {
            if (target != null)
            {
                // 计算飞向目标的方向
                Vector3 direction = (target.transform.position - transform.position).normalized;

                // 平滑移动：使用 Lerp 插值平滑过渡到目标位置
                Vector3 targetPosition = transform.position + direction * speed * Time.deltaTime;

                // 应用平滑位置
                rb.MovePosition(targetPosition);

                // 检查与目标的距离
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                //Debug.Log($"当前距离目标：{distanceToTarget}");

                // 如果接近目标并且没有在攻击，播放攻击动画
                if (distanceToTarget < attackRange && !isAttacking)
                {
                    StartCoroutine(Attack());
                }
                // 如果目标离开攻击范围，播放飞行动画
                else if (distanceToTarget >= attackRange && isAttacking)
                {
                    StopCoroutine(Attack());
                    anim.Play("fly");  // 播放飞行动画
                    isAttacking = false;
                }
            }
            else
            {
                Debug.LogWarning("目标对象为空，无法设置目标位置！");
            }

            yield return null;  // 每帧更新一次
        }
    }

    // 攻击动画
    private IEnumerator Attack()
    {
        isAttacking = true;

        // 播放攻击1动画
        anim.Play("attack1");

        // 等待攻击动画播放完毕
        yield return new WaitForSeconds(anim["attack1"].length);  // 等待动画播放完成

        // 调用 TakeDamage 方法，传入攻击1伤害 5f
        if (target != null)
        {
            Player playerScript = target.GetComponent<Player>();  // 获取目标（玩家）的脚本
            if (playerScript != null)
            {
                playerScript.TakeDamage(5f);  // 使用攻击1的伤害值 5
            }
        }

        // 播放攻击2动画
        anim.Play("attack2");
        yield return new WaitForSeconds(anim["attack2"].length);  // 等待动画播放完成

        // 调用 TakeDamage 方法，传入攻击2伤害 10f
        if (target != null)
        {
            Player playerScript = target.GetComponent<Player>();  // 获取目标（玩家）的脚本
            if (playerScript != null)
            {
                playerScript.TakeDamage(10f);  // 使用攻击2的伤害值 10
            }
        }

        // 攻击动画播放完成后，恢复飞行状态
        anim.Play("fly");
        isAttacking = false;
    }


    // 每秒随机增加或减少旋转 x, y, z 轴（适用于crow），并限制旋转范围
    public override IEnumerator Rotate()
    {
        while (true)
        {
            if (target != null)
            {
                // 计算飞向目标的方向
                Vector3 direction = (target.transform.position - transform.position).normalized;

                // 旋转朝向目标：使用 Slerp 插值平滑过渡
                // 旋转朝向目标：使用 Slerp 插值平滑过渡
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // 调整旋转速度，让旋转更快
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed * 4);  // 这里乘以2来加速旋转

            }

            yield return new WaitForSeconds(0.3f);  // 每 0.3 秒调整一次
        }
    }

    protected override void Die()
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
        GameObject.FindObjectOfType<AnimalSpawner>().OnCrowDestroyed(gameObject);
    }
}
