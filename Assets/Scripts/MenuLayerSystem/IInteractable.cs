public interface IInteractable
{
    /// <summary>
    /// This function will be called when the interaction of the object needs to be disabled
    /// </summary>
    public void DisableInteraction();

    /// <summary>
    /// This function will be called when the interaction of the object needs to be enabled
    /// </summary>
    public void EnableInteraction();

}
