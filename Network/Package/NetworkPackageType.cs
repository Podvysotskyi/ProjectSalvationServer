namespace Game.Network.Package;

public enum NetworkPackageType : ushort
{
    Empty = 0,
    
    CLogin = 100,
    SAuth = 101,
    
    SPlayerStatus = 1000,
    SPlayerPosition = 1001,
}