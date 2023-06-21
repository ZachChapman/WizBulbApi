# WiZ Light Bulb API

C# .NET 7 API for WiZ light bulbs.

A WinUI project styled to match the Windows 11 Action Centre is included. Starting this project will add an icon to the system tray.

![image](https://github.com/ZachChapman/WizBulbApi/assets/136128804/eb3f8d43-cd57-4411-9c27-64700c18255a)

## Usage
```csharp
var handles = new List<BulbHandle>();
var scanner = new BulbScanner(homeId: 123456);
await scanner.FindBulbHandlesAsync(handle => handles.Add(handle), timeout: 3000);

if(!handles.Any()) return;
var handle = handles[0];
var client = new BulbClient(handle);
var bulb = new Bulb(client);

await bulb.SetColour(Color.Blue);
await bulb.SetTemperature(7000);
await bulb.SetScene(Scene.Sunset, 35);
```

## Inspiration
This API was inspired by the following projects:

- [OpenWiz](https://github.com/UselessMnemonic/OpenWiz)
- [WizLib](https://github.com/nmoschkin/WizLib)

## Dependencies
[Framework](https://github.com/JelliChu/Framework)
