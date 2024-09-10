using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RedBlueGames.Tools;
using Pixeye.Unity;

public interface ICatcher {

    public void Catch(GameObject o);
    public void Throw(Vector2 MoveValue);
    public void CatchAction(GameObject o);
    public void ThrowAction(Vector2 MoveValue);
    public void UpdateCatchableObject();
    public Vector3 GetHandPosition();

}

public class CatchUtility : MonoBehaviour {

    public static GameObject GetTargetClosestObject(Vector3 position, float radius, LayerMask mask, bool isInCamera = false)
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(position, radius, Vector2.zero, 100.0f, mask);

        if (hits.Count() > 0)
        {
            float min_target_distance = float.MaxValue;
            GameObject target = null;

            foreach (var hit in hits)
            {
                float target_distance = Vector3.Distance(position, hit.transform.position);

                if (target_distance < min_target_distance)
                {
                    min_target_distance = target_distance;
                    target = hit.transform.gameObject;
                }
            }
            return target;
        }
        else
        {
            return null;
        }
    }

    public static GameObject SearchCatchableObject(Vector3 centerPos, float searchRadius)
    {
        GameObject o = GetTargetClosestObject(
            centerPos,
            searchRadius,
            LayerMask.GetMask("Ball"),
            true
        );

        DebugUtility.DrawCircle(centerPos, searchRadius, Color.cyan, 8);

        return o;
    }
}