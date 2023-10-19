using System.Collections.Generic;

namespace GameClient
{
    interface IInputCommandsProvider
    {
        void GetCommands(List<PlayerNetworkMessage> result);
    }

    class InputCommandsProvider : IInputCommandsProvider
    {
        public void GetCommands(List<PlayerNetworkMessage> result)
        {
        }
    }
}