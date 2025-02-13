using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class ScrollableMenuPage : MonoBehaviour
{
    public UnityEvent OnOpen => onOpen;
    public UnityEvent OnClose => onClose;

    [SerializeField] private UnityEvent onOpen;
    [SerializeField] private UnityEvent onClose;
}
