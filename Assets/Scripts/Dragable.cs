using UnityEngine;
using UnityEngine.Events;

public class Dragable : MonoBehaviour
{
    private bool _colliding = false;
    public bool Colliding => _colliding;

    private bool _standingStill = false;
    private bool _canDrag = true;
    public bool CanDrag { 
        get => _canDrag; 
        private set
        {
            if (value == false) OnDrop.Invoke();
            _canDrag = value;
        }
    }

    private bool _isDraging = true;
    public bool IsDraging
    {
        get => _isDraging;
        set
        {
            if (value == false)
            {
                CanDrag = false;
            }
            else
                OnDrag.Invoke();
            _isDraging = value;
        }
    }

    private Vector3 _previousPosition;

    private float _standStillTimer;
    public float StandingStillTimer => _standStillTimer;
    [SerializeField] private float _targetStillTime;
    public float TargetStillTime => _targetStillTime;

    private Vector3 _startPosition;
    [SerializeField]private Vector3 _rotationVector;

    public UnityEvent OnDrag;
    public UnityEvent OnDrop;
    public UnityEvent OnBecomeSolid;
    public UnityEvent OnReset;

    private void Start()
    {
        _startPosition = transform.position;
        GoalChecker.Instance.AddDragable(this);
        OnBecomeSolid.AddListener(DragController.AllowDrag);
        OnBecomeSolid.AddListener(ProgressBar.Instance.Finish);
        OnBecomeSolid.AddListener(() => GoalChecker.Instance.ExcludeDragable(this));
        OnDrop.AddListener(() => ProgressBar.Instance.SetDragable(this));
    }

    private void OnTriggerStay(Collider other)
    {
        _colliding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _colliding = false;
    }

    private void Update()
    {
        if (!_standingStill)
            StandStillCounter();
    }

    private void Rotate()
    {
        transform.Rotate(_rotationVector);
    }

    public void TouchAction()
    {
        Rotate();
    }

    public void ResetObject()
    {
        transform.position = _startPosition;
        IsDraging = false;
        CanDrag = true;
        _standingStill = false;
        _standStillTimer = 0;
        OnReset.Invoke();
        ProgressBar.Instance.Finish();
        DragController.AllowDrag();
        //OnBecomeSolid.Invoke();
    }

    private void StandStillCounter()
    {
        if (_canDrag) return;

        if (transform.position != _previousPosition)
        {
            _previousPosition = transform.position;
            _standStillTimer = 0;
        }
        else
        {
            _standStillTimer += Time.deltaTime;
        }

        if (_standStillTimer > _targetStillTime)
        {
            _standingStill = true;
            OnBecomeSolid.Invoke();
        }
    }
}
