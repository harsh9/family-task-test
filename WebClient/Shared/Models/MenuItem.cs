using System;

public class MenuItem
{
    public bool IsActive { get; set; }
    public string IconColor { get; set; }
    public string Label { get; set; }
    public Guid ReferenceId { get; set; }

    protected virtual void OnClickCallback(object e)
    {
        EventHandler<object> handler = ClickCallback;
        if (handler != null)
        {
            handler(this, e);
        }
    }
    public event EventHandler<object> ClickCallback;
    public void InvokClickCallback(object e)
    {
        OnClickCallback(e);
    }
}
