#if !UNITY_5_3_OR_NEWER

using Remouse.GameServer;
using Remouse.Shared.Infrastructure;
using Remouse.Shared.Utils.Log;

ProjectConfiguration configuration = new ProjectConfiguration
{
#if DEBUG
    buildType = BuildType.Debug
#elif RELEASE
    buildType = BuildType.Release
#endif
};

ServerConfig config = new ServerConfig
{
    ticksPerSecond = 60,
    mapId = "Start",
};
Logger.SetLogger(new ConsoleLogger());

ServerBootstrap bootstrap = new ServerBootstrap(config);
bootstrap.Start();

while (true)
{
    bootstrap.Update();
}

#endif