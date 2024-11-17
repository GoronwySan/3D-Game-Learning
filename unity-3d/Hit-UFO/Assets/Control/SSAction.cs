using UnityEngine;

/// <summary>
/// SSAction 是一个抽象基类，定义了动作的基本结构和回调接口。
/// </summary>
public abstract class SSAction : ScriptableObject
{
    public bool enable { get; set; } = true;
    public bool destory { get; set; } = false;

    public GameObject gameobject { get; set; }
    public Transform transform { get; set; }
    public ISSActionCallback callback { get; set; }

    // 构造函数
    protected SSAction()
    {
        enable = true;
        destory = false;
    }

    /// <summary>
    /// 动作启动时的初始化方法，派生类必须实现。
    /// </summary>
    public abstract void Start();

    /// <summary>
    /// 动作每帧的更新方法，派生类必须实现。
    /// </summary>
    public abstract void Update();
}
