namespace Bakery
{
    public interface IDeviceManager
    {
        EnumInputScheme InputScheme { get; }
        EnumGamepadType GamepadType { get; }
    }
}
