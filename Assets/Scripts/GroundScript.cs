using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GroundScript : MonoBehaviour
{
    public UnityEvent OnGroundHit;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Dragable dragable))
        {
            OnGroundHit.Invoke();
            GoalChecker.RestartGame(3);
        }
    }
}
