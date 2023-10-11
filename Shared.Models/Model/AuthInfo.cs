namespace Shared.Online.Models
{
    public struct AuthInfo
    {
        public ulong Id;
        public byte[] AuthData;
        public RegisterInfo registerInfo;
        public string version;
    }
}