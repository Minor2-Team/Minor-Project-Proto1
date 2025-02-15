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
            if (!IsPointerOverUIObject())
            {
                Debug.Log("Dropped outside UI!");
                HandleDroppedOutsideUI();
                eventData.pointerDrag = null;
                Destroy(gameObject);
            }

            return;

            bool IsPointerOverUIObject()
            {
                return EventSystem.current.IsPointerOverGameObject();
            }
        }
 
        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(parentAfterDrag);
 
            _group.alpha = 1f;
            _image.raycastTarget = true;
            print("enddrag");
        }
        
        

        private void HandleDroppedOutsideUI()
        {
            //spawn the correct machine part at correct position and dragging it
            
            // Add logic here to handle when item is dropped outside UI
            // Example: Reset position, delete item, etc.
            transform.position = parentAfterDrag.position; // Reset position
        }
    }
}
