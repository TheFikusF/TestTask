using System.Collections.Generic;
using UnityEngine;

public class RandomLayout : MonoBehaviour
{
    [SerializeField] List<Transform> _layouts;
    void Start()
    {
        _layouts.ForEach(layout => layout.gameObject.SetActive(false));
        _layouts[Random.Range(0, _layouts.Count)].gameObject.SetActive(true);
    }
}
