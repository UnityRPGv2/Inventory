using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core.UI.Dragging
{
    public interface IDragContainer<T> where T : class
    {
        bool CanAcceptItem(T item);
        T ReplaceItem(T item);
    }
}