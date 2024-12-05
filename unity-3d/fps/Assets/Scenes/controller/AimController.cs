using UnityEngine;
using Cinemachine;

public class AimController : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera; // 用于引用 Cinemachine Virtual Camera
    private float normalFOV;                        // 存储初始的 FOV 值
    public float aimFOV = 20f;                      // 瞄准镜的 FOV
    public float transitionDuration = 0.1f;         // FOV 平滑过渡的时间（0.5秒）
    //2
    private float targetFOV;                        // 目标 FOV，用于过渡
    private float currentFOV;                       // 当前 FOV 值

    void Start()
    {
        // 获取场景中第一个 CinemachineVirtualCamera 对象
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        if (virtualCamera != null)
        {
            // 获取初始的 FOV 值
            normalFOV = virtualCamera.m_Lens.FieldOfView;
            currentFOV = normalFOV;  // 初始化时的当前 FOV 为正常视角
            targetFOV = normalFOV;   // 初始化时的目标 FOV 为正常视角
        }
        else
        {
            Debug.LogError("CinemachineVirtualCamera not found in the scene!");
        }
    }

    void Update()
    {
        // 检测瞄准按键（右键）
        if (Input.GetMouseButtonDown(1)) // 右键按下时，开始瞄准
        {
            targetFOV = aimFOV;  // 设置目标 FOV 为瞄准镜 FOV
        }
        else if (Input.GetMouseButtonUp(1)) // 右键松开时，恢复普通视角
        {
            targetFOV = normalFOV;  // 设置目标 FOV 为正常 FOV
        }

        // 平滑过渡 FOV 值
        if (virtualCamera != null)
        {
            currentFOV = Mathf.MoveTowards(currentFOV, targetFOV, (Mathf.Abs(targetFOV - currentFOV) / transitionDuration) * Time.deltaTime);
            virtualCamera.m_Lens.FieldOfView = currentFOV;
        }
    }
}
