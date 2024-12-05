using UnityEngine;
using UnityEngine.UI;

public class BirdEyeCamera : MonoBehaviour
{
    public Camera birdEyeCamera;     // 鸟瞰图摄像头
    public Transform targetObject;   // 你要跟踪的目标对象
    public float height = 20f;       // 鸟瞰图摄像头的高度



    void Start()
    {
        birdEyeCamera = GetComponent<Camera>();
        //if (birdEyeCamera == null)
        //{
        //    birdEyeCamera = gameObject.AddComponent<Camera>();
        //}

        // 设置鸟瞰图显示在屏幕左上角
        birdEyeCamera.rect = new Rect(0f, 0.7f, 0.3f, 0.3f); // Viewport Rect

    }

    void Update()
    {
        if (targetObject != null)
        {
            // 获取目标对象的位置
            Vector3 targetPosition = targetObject.position;

            // 设置鸟瞰图摄像头的位置
            // 你可以根据需要调整摄像头的高度
            birdEyeCamera.transform.position = new Vector3(targetPosition.x, height, targetPosition.z);

            // 让鸟瞰图摄像头始终朝向目标对象
            birdEyeCamera.transform.LookAt(targetObject);
        }
    }
}
