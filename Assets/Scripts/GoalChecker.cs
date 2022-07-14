using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GoalChecker : MonoBehaviour
{
    private static GoalChecker _instance;
    public static GoalChecker Instance => _instance;

    private List<Dragable> _dragables = new List<Dragable>();
    public UnityEvent OnFinish;

    private static IEnumerator RestartGameAction(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void RestartGame(float time = 0)
    {
        Instance.StartCoroutine(RestartGameAction(time));
    }

    public void AddDragable(Dragable dragable) => _dragables.Add(dragable);
    
    public void ExcludeDragable(Dragable dragable)
    {
        _dragables.Remove(dragable);
        CheckCompletion();
    }

    private void CheckCompletion()
    {
        if (_dragables.Count == 0)
        {
            OnFinish.Invoke();
            RestartGame(3);
        }
    }

    private void Awake()
    {
        _instance = this;
    }
}
