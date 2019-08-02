using UnityEngine;
using UnityEngine.EventSystems;
public class CharactorRote : MonoBehaviour, IDragHandler, IBeginDragHandler
{

    //上次拖拽位置
    private float _lastPos;
    private float _lastPosY;
    public GameObject CurrentCharactor;

    private void Start()
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        float directionx = (eventData.position.x - _lastPos) / 5;
        float directiony = (eventData.position.y - _lastPosY) / 5;
        
        _lastPos = eventData.position.x;
        _lastPosY = eventData.position.y;
        
        CurrentChapterRote(Vector3.down * directionx);
        CurrentChapterRote(Vector3.left * directiony);
    }

    public void CurrentChapterRote(Vector3 rote)
    {
        CurrentCharactor.transform.Rotate(rote);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _lastPos = eventData.position.x;
        _lastPosY = eventData.position.y;
    }
}