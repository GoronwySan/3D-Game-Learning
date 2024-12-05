using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    // �洢���ص���պв���
    private static Material[] skyboxMaterials;

    // �趨ÿ���������л�һ����պ�
    private float switchInterval = 60f;  // 60�루��1���ӣ�

    // ������ʱ������Դ�е���պв���
    void Start()
    {
        LoadSkyboxes();
        SetRandomSkybox();  // ����ʱ���������պ�

        // ÿ�� switchInterval �����һ���л���պеķ���
        InvokeRepeating("SwitchRandomSkybox", switchInterval, switchInterval);
    }

    // ���£���鰴�� K �����л���պ�
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchRandomSkybox();  // ���� K ��ʱ�л���պ�
        }
    }

    // ���� Resources �ļ����е�������պв���
    private void LoadSkyboxes()
    {
        // ����ָ��·���µ����в����ļ�
        skyboxMaterials = Resources.LoadAll<Material>("sky");

        if (skyboxMaterials.Length == 0)
        {
            Debug.LogWarning("û���ҵ��κ���պв��ʣ�");
        }
    }

    // ���������պ�
    private void SetRandomSkybox()
    {
        if (skyboxMaterials.Length > 0)
        {
            // ���ѡ��һ����պв���
            int randomIndex = Random.Range(0, skyboxMaterials.Length);
            RenderSettings.skybox = skyboxMaterials[randomIndex];
        }
    }

    // �ṩ�ⲿ�ӿ�����л���պ�
    public void SwitchRandomSkybox()
    {
        if (skyboxMaterials != null && skyboxMaterials.Length > 0)
        {
            // ���ѡ��Ӧ���µ���պ�
            int randomIndex = Random.Range(0, skyboxMaterials.Length);
            RenderSettings.skybox = skyboxMaterials[randomIndex];
        }
        else
        {
            Debug.LogWarning("��պ���Դδ���ػ�Ϊ�գ�");
        }
    }
}
