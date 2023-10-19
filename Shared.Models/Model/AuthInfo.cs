namespace Remouse.Shared.Models
{
    public struct AuthInfo
    {
        public ulong id;
        public byte[] authData;
        public RegisterInfo registerInfo;
        public string version;
    }
}