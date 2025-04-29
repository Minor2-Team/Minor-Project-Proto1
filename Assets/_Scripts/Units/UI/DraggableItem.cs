using _Scripts.Scriptables;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


    public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Image _image;
        private CanvasGroup _group;
        private RectTransform _transform;
        public Transform parentAfterDrag;
        [SerializeField] private MachinePart machinePart;
        private Camera _cam;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _group = GetComponent<CanvasGroup>();
            _transform = transform as RectTransform;
            _cam = Camera.main;
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
                eventData.pointerDrag = null;
                HandleDroppedOutsideUI();
                
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
        }
        
        

        private void HandleDroppedOutsideUI()
        {
            
            Vector3 worldPosition = _cam.ScreenToWorldPoint(transform.position);
            worldPosition.z = 0;
            machinePart.Spawn(worldPosition);
            Destroy(gameObject);
        }
    }

