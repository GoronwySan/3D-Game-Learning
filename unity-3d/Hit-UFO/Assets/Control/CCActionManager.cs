using System.Collections.Generic;
using UnityEngine;

public class CCActionManager : SSActionManager, ISSActionCallback
{
    private const int InitialUFOCount = 20;
    private static readonly Vector3 HiddenPosition = new Vector3(0, 0, -5);

    public FirstController sceneController;
    public void Init()
    {
        // 在此处添加延迟初始化逻辑（如需要）
        Debug.Log("CCActionManager initialized");
    }

    protected void Start()
    {
        sceneController = (FirstController)SSDirector.getInstance().firstcontroller;
        SSDirector.getInstance().ccam = this;

        for (int i = 0; i < InitialUFOCount; ++i)
        {
            UFOToDestination(i);
        }
    }

    protected new void Update()
    {
        base.Update();
    }

    #region ISSActionCallback implementation
    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed, int intParam = 0, string strParam = null, Object objectParam = null)
    {
        // Handle action events if necessary
    }
    #endregion

    public void UFOToDestination(int num)
    {
        if (sceneController == null || sceneController.goList == null || sceneController.goList.Count <= num)
        {
            Debug.Log("Scene controller or goList is not properly initialized.");
            //return;
        }

        GameObject obj = sceneController.goList[num];
        if (obj != null)
        {
            obj.transform.position = GetRandomPosition(100);

            //var shakeActions = new List<SSAction>
            //{
            //    shakeTheDestination(50),
            //    shakeTheDestination(25),
            //    shakeTheDestination(10),
            //    shakeTheDestination(5)
            //};

            //CCSequenceAction css = CCSequenceAction.GetSSAction(1, 0, shakeActions);
            sceneController.goList_usedAndFree[num] = 1;
            //this.RunAction(obj, css, this);
        }
        else
        {
            Debug.LogError("Object at specified index is null.");
        }
    }

    private Vector3 GetRandomPosition(int range)
    {
        return new Vector3(Random.Range(-range / 2, range / 2), Random.Range(0, range / 2), range);
    }

    public static CCMoveToAction shakeTheDestination(int positionZ)
    {
        var destination = new Vector3(Random.Range(-positionZ / 2, positionZ / 2), Random.Range(-positionZ / 2, positionZ / 2), positionZ);
        float speed = SSDirector.getInstance().GetCreditCount() / 300 + 10;
        return CCMoveToAction.GetSSAction(destination, speed);
    }

    public void UFODrop(int num)
    {
        GameObject obj = sceneController.goList[num];
        CCMoveToAction ccma = CCMoveToAction.GetSSAction(new Vector3(obj.transform.position.x, -100, obj.transform.position.z), 50);
        this.RunAction(obj, ccma, this);
    }

    public void hideUFO(int num)
    {
        GameObject go = sceneController.goList[num];
        go.transform.position = HiddenPosition;
    }
}
