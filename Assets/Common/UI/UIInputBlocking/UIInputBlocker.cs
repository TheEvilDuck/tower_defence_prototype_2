using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.UI.InputBlocking
{
    public class UIInputBlocker : MonoBehaviour
    {
        [SerializeReference]private UIInputBlockerElement[] _preregisteredElemets;

        public event Action blockStarted;
        public event Action blockEnded;

        private bool _block = false;

        private bool Block 
        {
            set
            {
                if (_block!=value)
                {
                    if (value)
                        blockStarted?.Invoke();
                    else   
                        blockEnded?.Invoke();

                    _block = value;
                }
            }
        }
        private List<UIInputBlockerElement>_registeredElements;
        private List<UIInputBlockerElement>_hoveredElements;

        private void Awake() 
        {
            _registeredElements = new List<UIInputBlockerElement>();
            _hoveredElements = new List<UIInputBlockerElement>();

            foreach(UIInputBlockerElement element in _preregisteredElemets)
                RegisterNewElement(element);
        }

        private void OnDestroy() 
        {
            if (_registeredElements.Count==0)
                return;

            for (int i = _registeredElements.Count-1; i>0;i--)
                UnregisterElement(_registeredElements[i]);
        }

        public void RegisterNewElement(UIInputBlockerElement element)
        {
            if (_registeredElements.Contains(element))
                return;

            _registeredElements.Add(element);

            element.pointerEntered+=OnBlockerElementHoverStarted;
            element.poinerExited+=OnBlockerElementHoverEnd;
        }

        public void UnregisterElement(UIInputBlockerElement element)
        {
            if (!_registeredElements.Contains(element))
                return;

            _registeredElements.Remove(element);

            if (_hoveredElements.Contains(element))
                _hoveredElements.Remove(element);

            if (_hoveredElements.Count==0)
                Block = false;

            element.pointerEntered-=OnBlockerElementHoverStarted;
            element.poinerExited-=OnBlockerElementHoverEnd;
            
        }

        private void OnBlockerElementHoverStarted(UIInputBlockerElement element)
        {
            if (_hoveredElements.Contains(element))
                return;

            _hoveredElements.Add(element);

            Block = true;
        }

        private void OnBlockerElementHoverEnd(UIInputBlockerElement element)
        {
            if (!_hoveredElements.Contains(element))
                return;

            _hoveredElements.Remove(element);

            if (_hoveredElements.Count==0)
                Block = false;
        }
    }
}
