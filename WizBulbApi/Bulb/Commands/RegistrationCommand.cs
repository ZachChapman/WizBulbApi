using System.Net;

namespace WizBulbApi.Commands;

public class RegistrationCommand : BulbCommand
{
    private readonly int _homeId;
    private readonly IPAddress _ipAddress;
    private readonly MacAddress _macAddress;

    public RegistrationCommand(int homeId, IPAddress ipAddress, MacAddress macAddress)
    {
        _homeId = homeId;
        _ipAddress = ipAddress;
        _macAddress = macAddress;
    }

    public override StateMethod Method => StateMethod.Registration;
    public int HomeId => _homeId;
    public string PhoneIp => _ipAddress.ToString();
    public string PhoneMac => _macAddress.ToString().Replace("-", "").ToLower();
    public bool Register => true;
}
