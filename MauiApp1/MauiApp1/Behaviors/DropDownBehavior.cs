namespace MauiApp1.Behaviors;

public class DropDownBehavior : Behavior<Label>
{
    
    
    public DropDownBehavior(BindableProperty property)
    {
        
    }
    
    protected override void OnAttachedTo(Label bindable)
    {
        base.OnAttachedTo(bindable);
    }

    protected override void OnDetachingFrom(Label bindable)
    {
        base.OnDetachingFrom(bindable);
    }
    
    private void OnDropDownTapped(object sender, EventArgs e)
    {
        if (sender is Label label)
        {
            var custom = label.BindingContext as Custom;
            if (custom != null)
            {
                custom.DropDownTapped();
            }
        }
    }
}