using ReDI;
using Remouse.Network.Client;
using Remouse.Network.Models;

namespace Remouse.GameClient
{
    public class GameClientIdController
    {
        public int PlayerGameId { get => _playerGameId; }
        private int _playerGameId;
        
        [Inject]
        private void SubscribeOnEvents(IClientMessageBus messageBus)
        {
            messageBus.SubscribeToMessage<GameIdMessage>(HandleGameIdMessage);
        }

        private void HandleGameIdMessage(GameIdMessage message)
        {
            _playerGameId = message.gameId;
        }
    }
}