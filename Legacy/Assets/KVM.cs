using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Klak.Ndi
{
    public class KVM : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        // Start is called before the first frame update
        [SerializeField] private Canvas parentCanvas = null;        // the parent canvas of this UI - only needed to determine if we need the camera  
        [SerializeField] private RectTransform rect = null;         // the recttransform of the UI object
        [SerializeField] private GameObject reciever = null;

        // you can serialize this as well - do NOT assign it if the canvas render mode is overlay though
        private Camera UICamera = null;                             // the camera that is rendering this UI

        Interop.KVMdata kvmdata = new Interop.KVMdata();


        private void Start()
        {
            

            Debug.Log("KVM Enabled");
            if (rect == null)
                rect = GetComponent<RectTransform>();

            if (parentCanvas == null)
                parentCanvas = GetComponentInParent<Canvas>();

            if (UICamera == null && parentCanvas.renderMode == RenderMode.ScreenSpaceCamera)
                UICamera = parentCanvas.worldCamera;
        }

        public void Update()
        {
            NdiReceiver _recv = reciever.GetComponent<Klak.Ndi.NdiReceiver>();
            
        
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, UICamera, out Vector2 localPos);
            kvmdata.MouseX = (float)localPos.x / rect.rect.width;
            kvmdata.MouseY = (float)(localPos.y / rect.rect.height) / -1;
            _recv.KVM = kvmdata;
            
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            kvmdata.TouchD = true;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            kvmdata.TouchD = false;
        }
    }
}