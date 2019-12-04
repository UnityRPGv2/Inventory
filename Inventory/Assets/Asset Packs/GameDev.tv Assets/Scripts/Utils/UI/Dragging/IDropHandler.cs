namespace RPG.Core.UI.Dragging
{
    public interface IDiscardHandler<T>
    {
        bool DropItem(T item);
    }
}
