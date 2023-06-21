namespace WizBulbApi;

public class StateParams
{
    /// <summary> The state of the device. </summary>
    /// <remarks> True = On. False = Off. </remarks>
    public bool? State { get; set; }

    /// <summary> The ID of the current scene. </summary>
    public int? SceneId { get; set; }

    /// <summary> The speed of dynamic light modes. </summary>
    /// <remarks> Range: 20 - 200 </remarks>
    public int? Speed { get; set; }

    /// <summary> Whether a scene is playing. </summary>
    public bool? Play { get; set; }

    /// <summary> Red track value. </summary>
    /// <remarks> Range: 0 - 255 </remarks>
    public int? R { get; set; }

    /// <summary> Green track value. </summary>
    /// <remarks> Range: 0 - 255 </remarks>
    public int? G { get; set; }

    /// <summary> Blue track value. </summary>
    /// <remarks> Range: 0 - 255 </remarks>
    public int? B { get; set; }

    /// <summary> Cool White track value. </summary>
    /// <remarks> Range: 0 - 255 </remarks>
    public int? C { get; set; }

    /// <summary> Warm White track value. </summary>
    /// <remarks> Range: 0 - 255 </remarks>
    public int? W { get; set; }

    /// <summary> CCT value, measured in Kelvins. </summary> 
    /// <remarks> Range depends on the product. Common range is 2700K - 6500K </remarks>
    public int? Temp { get; set; }

    /// <summary> Brightness value. </summary>
    /// <remarks> Range: 10 - 100 </remarks>
    public int? Dimming { get; set; }

    /// <summary> The speed of the pulse command. </summary>
    public int? Delta { get; set; }

    /// <summary> The duration of the speed command. </summary>
    /// <remarks> Milliseconds </remarks>
    public int? Duration { get; set; }


    /// <summary> The IP of the host machine. </summary>
    public string PhoneIp { get; set; }

    /// <summary> The MAC address of the host machine. </summary>
    public string PhoneMac { get; set; }

    /// <summary> Whether a registration request is successful. </summary>
    public bool? Register { get; set; }

    /// <summary> </summary>
    public int? Id { get; set; }

    
    /// <summary> Name of the remote light. </summary>
    public string ModuleName { get; set; }

    /// <summary> MAC address of the device. </summary>
    public string Mac { get; set; }


    /// <summary> The Home ID of the remote light. </summary>
    public int? HomeId { get; set; }

    /// <summary> The Group ID of the remote light. </summary>
    public int? GroupId { get; set; }

    /// <summary> The Room ID of the remote light. </summary>
    public int? RoomId { get; set; }

    /// <summary> </summary>
    public bool? HomeLock { get; set; }

    /// <summary> </summary>
    public bool? PairingLock { get; set; }

    /// <summary> </summary>
    public string FwVersion { get; set; }

    /// <summary> </summary>
    public int? FadeIn { get; set; }

    /// <summary> </summary>
    public int? FadeOut { get; set; }

    /// <summary> </summary>
    public bool? FadeNight { get; set; }

    /// <summary> </summary>
    public int? DftDim { get; set; }

    /// <summary> </summary>
    public int[] PwmRange { get; set; }

    /// <summary> </summary>
    public int[] DrvConf { get; set; }

    /// <summary> </summary>
    public int[] WhiteRange { get; set; }

    /// <summary> </summary>
    public int[] ExtRange { get; set; }

    /// <summary> </summary>
    public bool? Po { get; set; }
}
