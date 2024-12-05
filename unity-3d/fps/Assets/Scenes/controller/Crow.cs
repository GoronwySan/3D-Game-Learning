using UnityEngine;
using System.Collections;

// ���ࣺ��ѻ
public class Crow : Enemy
{
    private Rigidbody rb;
    private GameObject target;  // Ŀ�꣨Player��

    public float speed = 5f;  // �����ٶ�
    public float rotationSpeed = 5f;  // ��תƽ����
    public float attackRange = 3f;  // ������Χ
    public float attackDistance = 1f;  // ���빥��״̬��������
    public float attackCooldown = 2f;  // ������ȴʱ��

    private bool isAttacking = false;  // �Ƿ����ڹ���
    private Animation anim;  // �������

    void Start()
    {
        // ��ȡ Rigidbody ���
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody ���δ�ҵ���");
        }

        // ��ȡĿ�����PlayerCapsule��
        target = GameObject.Find("PlayerCapsule");

        if (target != null)
        {
            Debug.Log("Ŀ����� 'PlayerCapsule' ���ҵ���");
        }
        else
        {
            Debug.LogWarning("δ�ҵ�Ŀ����� 'PlayerCapsule'��");
        }

        // ��ȡ Animation ���
        anim = GetComponent<Animation>();

        if (anim == null)
        {
            Debug.LogError("Animation ���δ�ҵ���");
        }

        // �����ƶ�Э��
        StartCoroutine(Move());
        StartCoroutine(Rotate());
    }

    // ƽ������
    public override IEnumerator Move()
    {
        while (true)
        {
            if (target != null)
            {
                // �������Ŀ��ķ���
                Vector3 direction = (target.transform.position - transform.position).normalized;

                // ƽ���ƶ���ʹ�� Lerp ��ֵƽ�����ɵ�Ŀ��λ��
                Vector3 targetPosition = transform.position + direction * speed * Time.deltaTime;

                // Ӧ��ƽ��λ��
                rb.MovePosition(targetPosition);

                // �����Ŀ��ľ���
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                //Debug.Log($"��ǰ����Ŀ�꣺{distanceToTarget}");

                // ����ӽ�Ŀ�겢��û���ڹ��������Ź�������
                if (distanceToTarget < attackRange && !isAttacking)
                {
                    StartCoroutine(Attack());
                }
                // ���Ŀ���뿪������Χ�����ŷ��ж���
                else if (distanceToTarget >= attackRange && isAttacking)
                {
                    StopCoroutine(Attack());
                    anim.Play("fly");  // ���ŷ��ж���
                    isAttacking = false;
                }
            }
            else
            {
                Debug.LogWarning("Ŀ�����Ϊ�գ��޷�����Ŀ��λ�ã�");
            }

            yield return null;  // ÿ֡����һ��
        }
    }

    // ��������
    private IEnumerator Attack()
    {
        isAttacking = true;

        // ���Ź���1����
        anim.Play("attack1");

        // �ȴ����������������
        yield return new WaitForSeconds(anim["attack1"].length);  // �ȴ������������

        // ���� TakeDamage ���������빥��1�˺� 5f
        if (target != null)
        {
            Player playerScript = target.GetComponent<Player>();  // ��ȡĿ�꣨��ң��Ľű�
            if (playerScript != null)
            {
                playerScript.TakeDamage(5f);  // ʹ�ù���1���˺�ֵ 5
            }
        }

        // ���Ź���2����
        anim.Play("attack2");
        yield return new WaitForSeconds(anim["attack2"].length);  // �ȴ������������

        // ���� TakeDamage ���������빥��2�˺� 10f
        if (target != null)
        {
            Player playerScript = target.GetComponent<Player>();  // ��ȡĿ�꣨��ң��Ľű�
            if (playerScript != null)
            {
                playerScript.TakeDamage(10f);  // ʹ�ù���2���˺�ֵ 10
            }
        }

        // ��������������ɺ󣬻ָ�����״̬
        anim.Play("fly");
        isAttacking = false;
    }


    // ÿ��������ӻ������ת x, y, z �ᣨ������crow������������ת��Χ
    public override IEnumerator Rotate()
    {
        while (true)
        {
            if (target != null)
            {
                // �������Ŀ��ķ���
                Vector3 direction = (target.transform.position - transform.position).normalized;

                // ��ת����Ŀ�꣺ʹ�� Slerp ��ֵƽ������
                // ��ת����Ŀ�꣺ʹ�� Slerp ��ֵƽ������
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // ������ת�ٶȣ�����ת����
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed * 4);  // �������2��������ת

            }

            yield return new WaitForSeconds(0.3f);  // ÿ 0.3 �����һ��
        }
    }

    protected override void Die()
    {
        Debug.Log($"{gameObject.name} ������");

        // �ڵ�������λ�����ɱ�ըЧ��
        GameObject explosionEffect = Resources.Load<GameObject>("CFXR Prefabs/Eerie/CFXR2 WW Enemy Explosion");
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("δ�ҵ���ըЧ����Դ��");
        }
        GameObject bloodBag = Resources.Load<GameObject>("Healing Item");
        // ʵ��������
        GameObject instance = Instantiate(bloodBag, transform.position + new Vector3(0, 5f, 0), Quaternion.identity);
        // ����ʵ��������� y ��Ϊ 0
        instance.transform.position = new Vector3(instance.transform.position.x, 0, instance.transform.position.z);
        Destroy(gameObject);  // ���ٵ��˶���
        GameObject.FindObjectOfType<AnimalSpawner>().OnCrowDestroyed(gameObject);
    }
}
