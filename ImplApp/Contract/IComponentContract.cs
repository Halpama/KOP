namespace Contract
{
    public interface IComponentContract
    {
        string Id { get; }
        string Title { get; }
        string Category { get; }
        UserControl GetControl();
    }
}