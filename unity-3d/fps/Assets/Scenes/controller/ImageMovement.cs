using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ImageMovement : MonoBehaviour
{
    public Canvas canvas;  // 需要检查的Canvas
    private bool isMoving; // 用来标识是否在移动
    public Transform targetObject;   // 你要跟踪的目标对象
    private Vector3 targetPosition; // 目标位置
    private Vector3 old;

    // 用于管理图像显示状态
    //private bool imagesVisible = true;

    private List<GameObject> group1Images = new List<GameObject>(); // 存储 1, 2, 3, 4 组的图像
    private List<GameObject> group2Images = new List<GameObject>(); // 存储 5, 6, 7, 8 组的图像


    void Start()
    {
        // 初始化并获取Canvas下的所有Image对象
        Image[] images = canvas.GetComponentsInChildren<Image>();

        foreach (Image img in images)
        {
            // 初始化每个Image的原始位置
            img.gameObject.AddComponent<MovementData>();
            MovementData data = img.GetComponent<MovementData>();
            data.originalPosition = img.rectTransform.localPosition;

            if (img.name == "1" || img.name == "2" || img.name == "3" || img.name == "4")
            {
                group1Images.Add(img.gameObject);
            }
            else if (img.name == "5" || img.name == "6" || img.name == "7" || img.name == "8")
            {
                group2Images.Add(img.gameObject);
                img.gameObject.SetActive(false);
            }

            // 输出调试信息
            Debug.Log("Image " + img.name + " initialized at position: " + data.originalPosition);
        }
    }

    void Update()
    {
        // 记录之前的位置并判断是否在移动
        Vector3 neww = targetObject.position;

        // 如果目标位置变化，则标记为正在移动
        if (neww != old)
        {
            isMoving = true;
            old = neww;
        }
        else
        {
            isMoving = false;
        }
        move();

    }
    
    void move()
    {
        // 获取Canvas下所有的Image对象
        Image[] images = canvas.GetComponentsInChildren<Image>();

        foreach (Image img in images)
        {
            MovementData data = img.GetComponent<MovementData>();



            // 根据Image名字进行不同的位移处理
            if (isMoving)
            {
                if (img.name == "1")
                {
                    // 图像1，位置增加 (0, 30, 0)
                    targetPosition = data.originalPosition + new Vector3(0, 30, 0);
                    Debug.Log("Image " + img.name + " moving up. Target position: " + targetPosition);
                }
                else if (img.name == "2")
                {
                    // 图像2，位置减少 (0, 30, 0)
                    targetPosition = data.originalPosition - new Vector3(0, 30, 0);
                    Debug.Log("Image " + img.name + " moving down. Target position: " + targetPosition);
                }
                else if (img.name == "3")
                {
                    // 图像3，位置增加 (30, 0, 0)
                    targetPosition = data.originalPosition + new Vector3(30, 0, 0);
                    Debug.Log("Image " + img.name + " moving right. Target position: " + targetPosition);
                }
                else if (img.name == "4")
                {
                    // 图像4，位置减少 (30, 0, 0)
                    targetPosition = data.originalPosition - new Vector3(30, 0, 0);
                    Debug.Log("Image " + img.name + " moving left. Target position: " + targetPosition);
                }
                else if (img.name == "5")
                {
                    // 图像1，位置增加 (0, 30, 0)
                    targetPosition = data.originalPosition + new Vector3(-15, 15, 0);
                    Debug.Log("Image " + img.name + " moving up. Target position: " + targetPosition);
                }
                else if (img.name == "6")
                {
                    // 图像2，位置减少 (0, 30, 0)
                    targetPosition = data.originalPosition - new Vector3(15, 15, 0);
                    Debug.Log("Image " + img.name + " moving down. Target position: " + targetPosition);
                }
                else if (img.name == "7")
                {
                    // 图像3，位置增加 (30, 0, 0)
                    targetPosition = data.originalPosition + new Vector3(15, 15, 0);
                    Debug.Log("Image " + img.name + " moving right. Target position: " + targetPosition);
                }
                else if (img.name == "8")
                {
                    // 图像4，位置减少 (30, 0, 0)
                    targetPosition = data.originalPosition - new Vector3(-15, 15, 0);
                    Debug.Log("Image " + img.name + " moving left. Target position: " + targetPosition);
                }
                else
                {
                    continue;
                }

                // 使用Lerp来平滑过渡
                img.rectTransform.localPosition = Vector3.Lerp(img.rectTransform.localPosition, targetPosition, 0.5f); // 可以调整速度系数
            }
            else
            {
                //// 如果没有移动，则复原
                img.rectTransform.localPosition = Vector3.Lerp(img.rectTransform.localPosition, data.originalPosition, 0.5f);
            }
        }
    }

    // 公共函数：切换图像显示状态
    // 切换图像显示状态
    private bool isbool = true;
    public void SwitchImages()
    {
        // 切换 1, 2, 3, 4 和 5, 6, 7, 8 的显示状态
        foreach (GameObject img in group1Images)
        {
            img.SetActive(!isbool); // 如果 5, 6, 7, 8 可见，则隐藏 1, 2, 3, 4
        }

        foreach (GameObject img in group2Images)
        {
            img.SetActive(isbool); // 如果 1, 2, 3, 4 可见，则隐藏 5, 6, 7, 8
        }
        isbool = !isbool;
    }
}


// 用于存储每个Image对象的原始位置
public class MovementData : MonoBehaviour
{
    public Vector3 originalPosition;
}
