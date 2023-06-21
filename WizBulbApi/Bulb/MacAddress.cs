using System.Net.NetworkInformation;

namespace WizBulbApi;

public class MacAddress
{
    private readonly byte[] _macAddress;

    public MacAddress(byte[] macAddress)
    {
        _macAddress = macAddress;
    }

    public MacAddress(string macAddress)
    {
        _macAddress = PhysicalAddress.Parse(macAddress).GetAddressBytes();
    }

    public byte[] ToBytes()
    {
        return _macAddress;
    }

    public override string ToString()
    {
        return BitConverter.ToString(_macAddress);
    }

    public override bool Equals(object? obj)
    {
        if(obj is not MacAddress other) return false;

        if(ReferenceEquals(this, other)) return true;

        return Enumerable.SequenceEqual(this._macAddress, other._macAddress);
    }

    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }

    public static bool operator ==(MacAddress objA, MacAddress objB) => Equals(objA, objB);
    public static bool operator !=(MacAddress objA, MacAddress objB) => !(objA == objB);
}