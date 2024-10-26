using UnityEngine;

namespace DevilBoatGame
{
    public class LandModel
    {
        GameObject land;
        public int land_mark;
        RoleModel[] roles = new RoleModel[6];
        Vector3[] role_positions;

        public LandModel(int sign)
        {
            land_mark = sign;
            land = Object.Instantiate(Resources.Load("Prefabs/Land", typeof(GameObject)), new Vector3(8.5F * land_mark, 0.5F, 0), Quaternion.identity) as GameObject;
            role_positions = new Vector3[] { new Vector3(4.6F * land_mark, 1.8F, 0), new Vector3(5.8F * land_mark, 1.8F, 0), new Vector3(7.0F * land_mark, 1.8F, 0), new Vector3(8.2F * land_mark, 1.8F, 0), new Vector3(9.4F * land_mark, 1.8F, 0), new Vector3(10.6F * land_mark, 1.8F, 0) };
        }

        public int GetLandMark() => land_mark;

        public Vector3 GetEmptyPosition()
        {
            for (int i = 0; i < 6; i++)
            {
                if (roles[i] == null) return role_positions[i];
            }
            return Vector3.zero;
        }

        public void AddRole(RoleModel role)
        {
            for (int i = 0; i < 6; i++)
            {
                if (roles[i] == null)
                {
                    roles[i] = role;
                    break;
                }
            }
        }

        public RoleModel RemoveRole(string name)
        {
            for (int i = 0; i < 6; i++)
            {
                if (roles[i] != null && roles[i].GetName() == name)
                {
                    roles[i] = null;
                    return roles[i];
                }
            }
            return null;
        }

        public int GetTotal(int id)
        {
            int sum = 0;
            foreach (RoleModel role in roles)
            {
                if (role != null && role.GetSign() == id) sum++;
            }
            return sum;
        }

        public void Reset() => roles = new RoleModel[6];
    }
}

