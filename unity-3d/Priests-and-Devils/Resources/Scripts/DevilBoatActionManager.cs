using UnityEngine;
using System.Collections.Generic;

namespace DevilBoatGame
{
    public class DevilBoatActionManager : SSActionManager, ISSActionCallback
    {
        private SSAction boatMove;
        private SequenceAction roleMove;

        protected new void Update()
        {
            base.Update();
        }

        public void moveBoat(GameObject boat, Vector3 end, float speed)
        {
            boatMove = SSMoveToAction.GetSSAction(end, speed);
            this.RunAction(boat, boatMove, this);
        }

        public void moveRole(GameObject role, Vector3 middle, Vector3 end, float speed)
        {
            SSAction action1 = SSMoveToAction.GetSSAction(middle, speed);
            SSAction action2 = SSMoveToAction.GetSSAction(end, speed);
            roleMove = SequenceAction.GetSSAcition(1, 0, new List<SSAction> { action1, action2 });
            this.RunAction(role, roleMove, this);
        }

        public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted, int intParam = 0, string strParam = null, Object objectParam = null)
        {
        }
    }
}
