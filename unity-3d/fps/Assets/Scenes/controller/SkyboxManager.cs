using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    // 存储加载的天空盒材质
    private static Material[] skyboxMaterials;

    // 设定每隔多少秒切换一次天空盒
    private float switchInterval = 60f;  // 60秒（即1分钟）

    // 在启动时加载资源中的天空盒材质
    void Start()
    {
        LoadSkyboxes();
        SetRandomSkybox();  // 启动时随机设置天空盒

        // 每隔 switchInterval 秒调用一次切换天空盒的方法
        InvokeRepeating("SwitchRandomSkybox", switchInterval, switchInterval);
    }

    // 更新：检查按下 K 键来切换天空盒
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchRandomSkybox();  // 按下 K 键时切换天空盒
        }
    }

    // 加载 Resources 文件夹中的所有天空盒材质
    private void LoadSkyboxes()
    {
        // 加载指定路径下的所有材质文件
        skyboxMaterials = Resources.LoadAll<Material>("sky");

        if (skyboxMaterials.Length == 0)
        {
            Debug.LogWarning("没有找到任何天空盒材质！");
        }
    }

    // 随机设置天空盒
    private void SetRandomSkybox()
    {
        if (skyboxMaterials.Length > 0)
        {
            // 随机选择一个天空盒材质
            int randomIndex = Random.Range(0, skyboxMaterials.Length);
            RenderSettings.skybox = skyboxMaterials[randomIndex];
        }
    }

    // 提供外部接口随机切换天空盒
    public void SwitchRandomSkybox()
    {
        if (skyboxMaterials != null && skyboxMaterials.Length > 0)
        {
            // 随机选择并应用新的天空盒
            int randomIndex = Random.Range(0, skyboxMaterials.Length);
            RenderSettings.skybox = skyboxMaterials[randomIndex];
        }
        else
        {
            Debug.LogWarning("天空盒资源未加载或为空！");
        }
    }
}
