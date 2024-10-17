using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakBox : MonoBehaviour
{
    [SerializeField] private FMODUnity.EventReference m_BallHitSound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CatchableBall ball = collision.gameObject.GetComponent<CatchableBall>();

        if (ball.m_State == BallState.Throwed || ball.m_State == BallState.Bound) {
            FMODUnity.RuntimeManager.PlayOneShot(m_BallHitSound, transform.position);
            if (ball != null) Destroy(this.gameObject);
        }
    }
}
