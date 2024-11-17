using System.Collections.Generic;
using UnityEngine;

public class CCSequenceAction : SSAction, ISSActionCallback
{
    public List<SSAction> sequence;
    private int sequenceCount;
    public int repeat = -1;  // Repeat indefinitely if set to -1
    public int start = 0;

    public static CCSequenceAction GetSSAction(int repeat, int start, List<SSAction> sequence)
    {
        var action = ScriptableObject.CreateInstance<CCSequenceAction>();
        action.repeat = repeat;
        action.sequence = sequence;
        action.start = start;
        action.sequenceCount = sequence.Count;
        return action;
    }

    public override void Start()
    {
        if (sequence == null || sequence.Count == 0) return;

        // Initialize only the first action in the sequence
        var currentAction = sequence[start];
        currentAction.gameobject = this.gameobject;
        currentAction.transform = this.transform;
        currentAction.callback = this;
        currentAction.Start();
    }

    public override void Update()
    {
        if (sequence == null || sequenceCount == 0) return;

        if (start < sequenceCount)
        {
            sequence[start].Update();
        }
    }

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed, int intParam = 0, string strParam = null, Object objectParam = null)
    {
        source.destory = false;
        start++;

        if (start >= sequenceCount)
        {
            start = 0;
            if (repeat > 0) repeat--;
            if (repeat == 0)
            {
                this.destory = true;
                this.callback?.SSActionEvent(this);
                return;
            }
        }

        // Initialize the next action in the sequence
        if (start < sequenceCount)
        {
            var nextAction = sequence[start];
            nextAction.gameobject = this.gameobject;
            nextAction.transform = this.transform;
            nextAction.callback = this;
            nextAction.Start();
        }
    }
}
