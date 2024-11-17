using UnityEngine;

public class CCMoveToAction : SSAction
{
    public Vector3 target;
    public float speed;

    public static CCMoveToAction GetSSAction(Vector3 target, float speed)
    {
        var action = ScriptableObject.CreateInstance<CCMoveToAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }

    public override void Start()
    {
        // 可以为空或包含特定逻辑
    }

    public override void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, Time.deltaTime * speed);
        if (Vector3.Distance(this.transform.position, target) < 0.1f)
        {
            this.destory = true;
            this.callback.SSActionEvent(this);
        }
    }
}
