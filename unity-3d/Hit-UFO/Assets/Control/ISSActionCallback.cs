using UnityEngine;

/// <summary>
/// 表示动作事件的类型
/// </summary>
public enum SSActionEventType : int
{
    Started,    // 动作开始
    Completed   // 动作完成
}

/// <summary>
/// 动作回调接口，定义动作事件的回调方法
/// </summary>
public interface ISSActionCallback
{
    /// <summary>
    /// 在动作发生特定事件（开始、完成等）时回调
    /// </summary>
    /// <param name="source">触发事件的动作对象</param>
    /// <param name="events">事件类型（开始、完成）</param>
    /// <param name="intParam">整数参数，用于传递附加信息</param>
    /// <param name="strParam">字符串参数，用于传递附加信息</param>
    /// <param name="objectParam">对象参数，用于传递附加信息</param>
    void SSActionEvent(
        SSAction source,
        SSActionEventType events = SSActionEventType.Completed,
        int intParam = 0,
        string strParam = null,
        Object objectParam = null
    );
}
