using UnityEngine;

public class HealingItem : MonoBehaviour
{
    // ��ײ���
    // void OnCollisionEnter(Collision collision)
    // {
    //     // ����Ƿ��������Ϊ "Player" �Ķ���
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         // ��ȡPlayer�ű�������HealHealth����
    //         Player playerScript = collision.gameObject.GetComponent<Player>();
    //         if (playerScript != null)
    //         {
    //             playerScript.HealHealth(10);
    //         }
    //     }
    // }

    // ���ʹ�ô������Ļ�������ʹ�� OnTriggerEnter
    void OnTriggerEnter(Collider other)
    {
        // ���������Ϣ������Ƿ��봥�����Ӵ�
        Debug.Log($"HealingItem OnTriggerEnter: ��⵽����� {other.gameObject.name} ����ײ��");

        // ����Ƿ��������Ϊ "Player" �Ķ���
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("HealingItem: ����Player���󣬿�ʼ�ָ�����ֵ��");

            // ��ȡPlayer�ű�������HealHealth����
            Player playerScript = other.gameObject.GetComponent<Player>();
            if (playerScript != null)
            {
                Debug.Log("HealingItem: ����Player�ű���HealHealth������");
                playerScript.HealHealth(10); // ����ָ� 10 ������ֵ
            }
            else
            {
                Debug.LogWarning("HealingItem: �Ҳ���Player�ű���");
            }

            // ���ز���ʾ��ըЧ��
            GameObject explosionEffect = Resources.Load<GameObject>("CFXR Prefabs/Magic Misc/CFXR3 Magic Aura A (Runic)");
            if (explosionEffect != null)
            {
                Debug.Log("HealingItem: ʵ������ըЧ����");
                GameObject effectInstance = Instantiate(explosionEffect, transform.position, Quaternion.identity);
                // ��ʱ3������ٱ�ըЧ��
                Destroy(effectInstance, 3f);
            }
            else
            {
                Debug.LogWarning("HealingItem: δ�ҵ�Ч����Դ��");
            }

            // ���ٵ�ǰHealing Item����
            Debug.Log("HealingItem: ����Healing Item����");
            Destroy(gameObject);
        }
        else
        {
            // �������Player�������������Ϣ
            Debug.Log($"HealingItem: ���Player���� {other.gameObject.name} ��ײ��");
        }
    }
}
