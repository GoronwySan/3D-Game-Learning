using UnityEngine;

namespace DevilBoatGame
{
    public class SSAction : ScriptableObject
    {
        public bool enable = true;
        public bool destroy = false;
        public GameObject gameobject;
        public Transform transform;
        public ISSActionCallback callback;

        protected SSAction() { }

        public virtual void Start()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}
