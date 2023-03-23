namespace AceInTheHole
{
    public interface IInteractable
    {
        public bool ShouldJumpOff
        {
            get;
        }
        public void Interact();
    }
}