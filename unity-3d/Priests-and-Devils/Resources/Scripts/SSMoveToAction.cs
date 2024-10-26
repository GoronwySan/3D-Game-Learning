using UnityEngine;

namespace DevilBoatGame
{
    public class SSMoveToAction : SSAction
    {
        public Vector3 target;
        public float speed;

        private SSMoveToAction() { }
        public static SSMoveToAction GetSSAction(Vector3 _target, float _speed)
        {
            SSMoveToAction action = ScriptableObject.CreateInstance<SSMoveToAction>();
            action.target = _target;
            action.speed = _speed;
            return action;
        }

        public override void Update()
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
            if (this.transform.position == target)
            {
                this.destroy = true;
                this.callback.SSActionEvent(this);
            }
        }

        public override void Start() { }
    }
}
