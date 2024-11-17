using System.Collections.Generic;
using UnityEngine;

public class SSActionManager : MonoBehaviour
{
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    private List<SSAction> waitingAdd = new List<SSAction>();
    private List<int> waitingDelete = new List<int>();

    // Update is called once per frame
    protected void Update()
    {
        // 添加等待添加的动作
        foreach (SSAction action in waitingAdd)
        {
            actions[action.GetInstanceID()] = action;
        }
        waitingAdd.Clear();

        SSDirector ssdDirector = SSDirector.getInstance();

        // 更新动作
        foreach (var kv in actions)
        {
            SSAction action = kv.Value;
            if (action.destory)
            {
                waitingDelete.Add(action.GetInstanceID());
            }
            else if (action.enable && !ssdDirector.gameEnd)
            {
                action.Update();
            }
        }

        // 删除标记为删除的动作
        ProcessDeletions();
    }

    /// <summary>
    /// 清理需要删除的动作，并进行特殊的UFO隐藏处理
    /// </summary>
    private void ProcessDeletions()
    {
        foreach (int key in waitingDelete)
        {
            if (actions.TryGetValue(key, out SSAction action))
            {
                HandleUFO(action);
                actions.Remove(key);
                Destroy(action);
            }
        }
        waitingDelete.Clear();
    }

    /// <summary>
    /// 处理 UFO 掉落和隐藏逻辑
    /// </summary>
    private void HandleUFO(SSAction action)
    {
        SSDirector ssdDirector = SSDirector.getInstance();

        // 输出调试信息：飞碟的 y 坐标
        Debug.Log($"[HandleUFO] UFO Y Position: {action.transform.position.y}");
        Debug.Log($"[HandleUFO] UFO Z Position: {action.transform.position.z}");
        if (action.transform.position.z <= 20)
        {
            Debug.Log("[HandleUFO] UFO reached Z <= 10. Setting game to end.");
            ssdDirector.SetGameEnd(true);
        }
        if (action.transform.position.y < -50)
        {
            Debug.Log("[HandleUFO] UFO dropped below -50. Hiding UFO.");
            HideAllUFOsMatching(action);
        }

    }

    /// <summary>
    /// 隐藏指定动作对应的UFO，并释放其资源
    /// </summary>
    public void HideAllUFOsMatching(SSAction action)
    {
        SSDirector ssdDirector = SSDirector.getInstance();

        Debug.Log($"[HideAllUFOsMatching] Hiding UFO with position: {action.transform.position}");

        foreach (var kv in actions)
        {
            SSAction otherAction = kv.Value;
            Debug.Log($"Key: {kv.Key}, GameObject: {kv.Value.gameobject.name}, Destroy: {kv.Value.destory}");
            //if (otherAction != action && otherAction.gameobject == action.gameobject)
            //{
                otherAction.destory = true;

                int index = ssdDirector.firstcontroller.getGameObjectIndex(action.transform.position);

                Debug.Log($"[HideAllUFOsMatching] Hiding UFO at index: {index}");
                ssdDirector.ccam.hideUFO(index);

                // 更新管理数组状态
                ssdDirector.firstcontroller.goList_usedAndFree[index] = 0;

                Debug.Log($"[HideAllUFOsMatching] UFO at index {index} is now hidden.");
            //}
        }
    }


    public void ClearAllList()
    {
        actions.Clear();
        waitingAdd.Clear();
        waitingDelete.Clear();
    }

    public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager)
    {
        action.gameobject = gameobject;
        action.transform = gameobject.transform;
        action.callback = manager;
        waitingAdd.Add(action);
        action.Start();
    }
}
