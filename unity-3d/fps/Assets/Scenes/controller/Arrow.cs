using UnityEngine;
using UnityEngine.SceneManagement;

public class Arrow : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // ��ȡ��ײ����ı�ǩ������
        string tag = collision.gameObject.tag;

        // ������ײ����ı�ǩ�����ʹ���ͬ�ķ�Ӧ
        switch (tag)
        {
            case "Enemy":
                // ����������ˣ������˺�����������Ӧ
                HandleEnemyCollision(collision);
                break;
            case "Wood":
                // �������ľ�ģ����ܻῨס���������΢����
                HandleWoodCollision(collision);
                break;
            case "Ground":
                // ����������棬�����ܻ�ͣ����
                HandleGroundCollision(collision);
                break;
            case "Target":
                // �������Ŀ�ִ꣬������߼�
                HandleTargetCollision(collision);
                break;
            case "Training":
                // �������Ŀ�ִ꣬������߼�
                HandleTrainingCollision(collision);
                break;
            case "quit":
                // �������Ŀ�ִ꣬������߼�
                HandleQuitCollision(collision);
                break;
            case "switch":
                // �������Ŀ�ִ꣬������߼�
                ImageMovement i;
                // ��ȡ TargetController �ű�����
                i = FindObjectOfType<ImageMovement>();
                i.SwitchImages();
                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero; // ֹͣ�����˶�
                }
                break;
            case "return":
                // �������Ŀ�ִ꣬������߼�
                Cursor.lockState = CursorLockMode.None;  // �������
                Cursor.visible = true;  // ��ʾ���
                SceneManager.LoadScene("SampleScene");
                break;
            default:
                // ���������͵��������Ĭ�ϴ���
                HandleDefaultCollision(collision);
                break;
        }
    }

    // ��������˵���ײ
    private void HandleEnemyCollision(Collision collision)
    {
        ScoreManager.Instance.AddScore(10);  // ���� 10 ��
                                             // ��ȡ���˶���Animal��
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        // ������˴��ڣ����� TakeDamage ����
        if (enemy != null)
        {
            enemy.TakeDamage(100);  // ֪ͨ�����ܵ��˺�
        }

        Debug.Log("�����е��ˣ�");
        // ���ټ�������������
        gameObject.SetActive(false);  // ͣ�ü�
    }

    // ������ľ�ĵ���ײ
    private void HandleWoodCollision(Collision collision)
    {
        Debug.Log("����ס��ľ�ģ�");
        // ֹͣ�����˶������ܲ��ٻس�
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero; // ֹͣ�����˶�
        }
    }

    // ������������ײ
    private void HandleGroundCollision(Collision collision)
    {
        Debug.Log("����أ�");
        // ֹͣ����ģ�⣬ʹ����ֹ
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;  // ֹͣ����ģ��
        }
    }

    // ������Ŀ�����ײ
    private void HandleTargetCollision(Collision collision)
    {
        ScoreManager.Instance.AddScore(50);  // ���� 50 ��
        Debug.Log("������Ŀ�꣡");
        //Rigidbody rb = GetComponent<Rigidbody>();
        //if (rb != null)
        //{
        //    rb.isKinematic = true;  // ֹͣ����ģ��
        //}
        // �����������������Ĵ���������Ŀ����ʧ�򴥷�����Ч��
        //collision.gameObject.SetActive(false);  // ����Ŀ���ڱ����к���ʧ
        //gameObject.SetActive(false);  // ͣ�ü�
                                      // �ڵ�������λ�����ɱ�ըЧ��
        GameObject explosionEffect = Resources.Load<GameObject>("CFXR Prefabs/Explosions/CFXR4 Firework HDR Shoot Single (Random Color)");
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, collision.transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("δ�ҵ���ըЧ����Դ��");
        }
        // ������ײ��Ŀ�����
        Destroy(collision.gameObject);  // ����Ŀ�����
        TargetController targetController;
        // ��ȡ TargetController �ű�����
        targetController = FindObjectOfType<TargetController>();
        targetController.OnTargetDestroyed();
        // ����Ҫ�������£���������Ŀ�궼�����٣������� TargetController �ű�
        if (targetController != null && targetController.totalTargetsDestroyed >= targetController.targetCount + 10)
        {
            targetController.DestroyController();  // ͨ���ⲿ�ű����� TargetController �ű�
            Invoke(nameof(HandleQuitCollision), 10f);
            HandleQuitCollision(collision);
        }
    }

    private void HandleTrainingCollision(Collision collision)
    {

        GameObject a = GameObject.Find("GameObject");
        GameObject newTarget = Instantiate(a, new Vector3(0, 0, 0), Quaternion.identity);

            // Ϊ�¶������TargetController�ű�
        newTarget.AddComponent<TargetController>();

        GameObject c = GameObject.Find("PlayerCapsule");
        CharacterController characterController;
        // ��ȡ��ɫ���������
        characterController = c.GetComponent<CharacterController>();
        characterController.enabled = false; // ���ÿ�����
        c.transform.position = new Vector3(150, 0, -185); // ֱ������λ��
        characterController.enabled = true; // ���ÿ�����

    }

    private void HandleQuitCollision(Collision collision)
    {
        GameObject c = GameObject.Find("PlayerCapsule");
        CharacterController characterController;
        // ��ȡ��ɫ���������
        characterController = c.GetComponent<CharacterController>();
        characterController.enabled = false; // ���ÿ�����
        c.transform.position = new Vector3(0, 0, -10); // ֱ������λ��
        characterController.enabled = true; // ���ÿ�����
        TargetController targetController;
        // ��ȡ TargetController �ű�����
        targetController = FindObjectOfType<TargetController>();
        targetController.DestroyAllTargets();

    }

    // Ĭ����ײ����
    private void HandleDefaultCollision(Collision collision)
    {
        Debug.Log("����" + collision.gameObject.name + "��������ײ");
        // ֹͣ����ģ�⣬ʹ����ֹ
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;  // ֹͣ����ģ��
        }
        // ����ѡ�����ټ�������������
        //gameObject.SetActive(false);  // ͣ�ü�
    }
}
