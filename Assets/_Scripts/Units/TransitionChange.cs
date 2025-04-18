using System;
using _Scripts.Units;
using UnityEngine;

public class TransitionChange : MonoBehaviour
{
    public event Action<State,bool> OnAnyCollision;
    public event Action<bool> OnMouseClick;
    public event Action OnMouseMove;
    private State currState;
    private bool flag;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out State state))
        {
            OnAnyCollision?.Invoke(state,true);
        }
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out State state))
        {
            OnAnyCollision?.Invoke(state,false);
        }  
    }
    private void OnMouseDown()
    {
        OnMouseClick?.Invoke(true);
    }

    private void OnMouseDrag()
    {
        OnMouseMove?.Invoke();
    }

    private void OnMouseUp()
    {
        OnMouseClick?.Invoke(false);
        
    }
}
