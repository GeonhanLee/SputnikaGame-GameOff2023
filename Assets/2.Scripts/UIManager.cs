using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private RectTransform _mask;
    [SerializeField] private RectTransform _tutText;
    [SerializeField] private float _tutTextoffset = 2f;
    [SerializeField] private Transform _tutFollowTarget;
    [Space]
    [SerializeField] private RectTransform _evoRing;
    [SerializeField] private GravityField _evoRingTarget;

    public void SetUIPos()
    {
        Camera cam = Camera.main;
        
        _mask.position = _tutFollowTarget.position;
        _tutText.position = _tutFollowTarget.position + cam.transform.TransformDirection(Vector3.down) * _tutTextoffset;

        _evoRing.position = _evoRingTarget.transform.position;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)_canvas.transform, Camera.main.WorldToScreenPoint(_evoRingTarget.transform.position), cam, out var center);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)_canvas.transform, Camera.main.WorldToScreenPoint(_evoRingTarget.transform.position + Vector3.right * _evoRingTarget.Radius), cam, out var centerPlusRadius);

        var width = Vector2.Distance(center, centerPlusRadius) * 2;
        _evoRing.sizeDelta = new Vector2(width, width);

        //_evoRing.anchoredPosition = cam.WorldToViewportPoint(_evoRingTarget.position);

        //Vector2 viewportPosition = cam.WorldToViewportPoint(_evoRingTarget.position);
        //_evoRing.position = new Vector2(viewportPosition.x * _evoRing.sizeDelta.x, viewportPosition.y * _evoRing.sizeDelta.y);

        //_evoRing.localScale = _evoRingTarget.localScale;
        // apply scale

    }
}
