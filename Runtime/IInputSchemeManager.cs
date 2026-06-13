namespace Bakery
{
    public interface IInputSchemeManager
    {
        bool IsPlayerMovementDisabled { get; }
        void ToggleControls(bool isOn);
    }
}
