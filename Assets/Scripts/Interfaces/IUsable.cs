public interface IUsable
{
    public InventorySlot slot { get; set; }
    public void Use();
    public void UnUse();
    public void AltUse();
    public void AltUnUse();
}