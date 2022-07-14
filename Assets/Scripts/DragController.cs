using UnityEngine;
using UnityEngine.Events;

public class DragController : MonoBehaviour
{
    private static DragController _instance;
    public static DragController Instance => _instance;

    public bool CanDragNext { get; private set; } = true;

    private Dragable _draggingObject;

    public bool _touchMoved = false;

    public static void AllowDrag() => Instance.CanDragNext = true;

    private void Drag()
    {
        if (Input.touchCount != 1)
        {
            DragEnd();
            return;
        }

        Touch touch = Input.GetTouch(0);
        DragStart(touch);
        DragAction(touch);
    }

    private void DragStart(Touch touch)
    {
        if(touch.phase != TouchPhase.Began) return;

        if(!CanDragNext) return;

        if (!Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out RaycastHit hit)) return;

        if (!hit.collider.TryGetComponent(out Dragable dragable)) return;
            
        if(dragable.CanDrag == true)
        {
            _draggingObject = dragable;
        }
    }

    private void DragAction(Touch touch)
    {
        if (_draggingObject == null) return;

        if (touch.phase == TouchPhase.Moved && !_touchMoved) 
        {
            _touchMoved = true;
            _draggingObject.IsDraging = true;
            CanDragNext = false;
        }

        if (!_touchMoved) return;

        var position = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 7));
        position.z = 0;

        _draggingObject.transform.position = position;
    }

    private void DragEnd()
    {
        if (_draggingObject == null) return;

        if(!_touchMoved) _draggingObject.TouchAction();
        else
        {
            _draggingObject.IsDraging = false;
            if (_draggingObject.Colliding) _draggingObject.ResetObject();
        }

        _touchMoved = false;
        _draggingObject = null;
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        Drag();
    }
}
