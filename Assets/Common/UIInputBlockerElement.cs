using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Common.UI
{
    public class UIInputBlockerElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<UIInputBlockerElement> pointerEntered;
        public event Action<UIInputBlockerElement> poinerExited;

        public void OnPointerEnter(PointerEventData eventData) => pointerEntered?.Invoke(this);

        public void OnPointerExit(PointerEventData eventData) => poinerExited?.Invoke(this);
    }
}
