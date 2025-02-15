using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace plugins.draganddrop_unity3d_main.draganddrop_unity3d_main.Scripts
{
    public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Image _image;
        private CanvasGroup _group;
        public Transform parentAfterDrag;
 
        private void Start()
        {
            _image = GetComponent<Image>();
            _group = GetComponent<CanvasGroup>();
        }
 
        public void OnBeginDrag(PointerEventData eventData)
        {
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.parent.parent.parent);
            transform.SetAsLastSibling();
            transform.position = Input.mousePosition;

            _group.alpha = .5f;
            _image.raycastTarget = false;
        }
 
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }
 
        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(parentAfterDrag);
 
            _group.alpha = 1f;
            _image.raycastTarget = true;
        }
    }
}
