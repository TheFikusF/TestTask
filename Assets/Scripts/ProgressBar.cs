using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private static ProgressBar _instance;
    public static ProgressBar Instance => _instance;

    [SerializeField] private Slider _progressBar;
    private Dragable _dragable;

    public void SetDragable(Dragable dragable)
    {
        _progressBar.gameObject.SetActive(true);
        _progressBar.maxValue = dragable.TargetStillTime;
        _dragable = dragable;
    }

    public void Finish()
    {
        _progressBar.gameObject.SetActive(false);
        _dragable = null;
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        if( _dragable != null )
            _progressBar.value = _dragable.StandingStillTimer;
    }
}
