using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnimalSpawner : MonoBehaviour
{
    // ��Դ·��
    private string crowPath = "birds/dexsoft-birds/crow";
    private string duckFPath = "birds/dexsoft-birds/duck_f";
    private string duckMPath = "birds/dexsoft-birds/duck_m";

    // ÿ�ֶ�����Ҫ���ɵ�����
    public int animalsPerType = 10;

    // �������ɵ�λ�÷�Χ
    public Vector3 spawnMin = new Vector3(-125, 0, -100);
    public Vector3 spawnMax = new Vector3(100, 0, 100);

    // �ƶ��ٶȣ����ݶ������Ϳ��Ե�����
    public float moveSpeed = 2f;

    // �洢���е���ѻ
    private List<GameObject> allCrows = new List<GameObject>();

    // ��Ϸ�ɹ�����ʾ
    public Text gameStatusText;

    // Start is called before the first frame update
    void Start()
    {
        // ʹ�� Resources.Load ���ض��� Prefabs
        GameObject crow = Resources.Load<GameObject>(crowPath);
        GameObject duck_f = Resources.Load<GameObject>(duckFPath);
        GameObject duck_m = Resources.Load<GameObject>(duckMPath);

        // ����Ƿ���سɹ�����ӡ������Ϣ
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

        // ����ÿ�ֶ���
        if (crow != null) SpawnAnimals(crow, "fly");
        if (duck_f != null) SpawnAnimals(duck_f, "walk");
        if (duck_m != null) SpawnAnimals(duck_m, "walk");
    }

    // ����ָ�����͵Ķ���
    void SpawnAnimals(GameObject animalPrefab, string animationName)
    {
        for (int i = 0; i < animalsPerType; i++)
        {
            // �������λ��
            Vector3 spawnPosition = new Vector3(
                Random.Range(spawnMin.x, spawnMax.x),
                animalPrefab.name.Contains("crow") ? Random.Range(10f, 20f) : 0f, // Crow �� Y ���귶Χ (10, 20)
                Random.Range(spawnMin.z, spawnMax.z)
            );
            // ʵ��������
            GameObject animal = Instantiate(animalPrefab, spawnPosition, Quaternion.identity);

            // ���ñ�ǩΪ 'Enemy'
            animal.tag = "Enemy";

            // ����������Բ�����������Ĭ�������
            if (animalPrefab.name.Contains("crow"))
            {
                Rigidbody rb = animal.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = animal.AddComponent<Rigidbody>(); // ��� Rigidbody ���
                }
                rb.useGravity = false;  // ����������������ѻ��������Ӱ

                // �������ѻ����ӵ���ѻ�б���
                allCrows.Add(animal);
                Enemy enemy = animal.AddComponent<Crow>();  // �����ѻ��Ϊ
            }
            else
            {
                Enemy enemy = animal.AddComponent<Duck>();  // ���Ѽ����Ϊ
            }

            // ���ö���Ķ�����
            Animation animation = animal.GetComponent<Animation>();
            if (animation != null)
            {
                if (animation[animationName] != null)
                {
                    animation[animationName].wrapMode = WrapMode.Loop; // ���ö���ѭ������
                    animation.Play(animationName); // ����ָ�����ƵĶ���
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

            // �����������ײ��
            Collider col = animal.GetComponent<Collider>();
            if (col == null)
            {
                col = animal.AddComponent<BoxCollider>(); // ��� Collider ���
            }
            BoxCollider boxCollider = col as BoxCollider;
            if (boxCollider != null)
            {
                boxCollider.size = new Vector3(3f, 3f, 3f);  // ���ô�С
                boxCollider.center = new Vector3(0f, 1f, 0f);  // �������ĵ�
            }
        }
    }

    // ����ѻ������ʱ���ô˷���
    public void OnCrowDestroyed(GameObject crow)
    {
        // ���б����Ƴ����ٵ���ѻ
        if (allCrows.Contains(crow))
        {
            allCrows.Remove(crow);
        }

        // ������е���ѻ�������٣���ʾ��Ϸ�ɹ���ʾ
        if (allCrows.Count == 0)
        {
            GameSuccess();
        }
    }

    // ��Ϸ�ɹ��Ĵ�����
    void GameSuccess()
    {
        if (gameStatusText != null)
        {
            gameStatusText.text = "Game Success!";
            Cursor.lockState = CursorLockMode.None;  // �������
            Cursor.visible = true;  // ��ʾ���
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;  // �������
            Cursor.visible = true;  // ��ʾ���
            SceneManager.LoadScene("SampleScene");
            Debug.Log("Game Success!");
        }
    }
}
