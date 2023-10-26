using System;
using Remouse.Core.Components;
using Remouse.Core.World;
using Remouse.MathLib;

namespace Remouse.Core.ECS.Systems
{
    public class MoveToDirectionSystem : IRunSystem, IInitSystem
    {
        private TilesSingleton _tiles;
        public void Init(IWorld world)
        {
            var query = world.Query().Inc<TilesSingleton>().End();
            
            TilesSingleton? tiles = null;
            foreach (var entity in query)
            {
                tiles = world.GetComponent<TilesSingleton>(entity);
            }

            if (!tiles.HasValue)
                throw new NullReferenceException("Tiles not found");

            _tiles = tiles.Value;
        }
        
        public void Run(IWorld world)
        {
            var query = world.Query().Inc<MoveInDirection>().Inc<MovementSpeed>().Inc<Transform>().End();

            foreach (var entity in query)
            {
                ref MoveInDirection direction = ref world.GetComponentRef<MoveInDirection>(entity);
                ref Transform transform = ref world.GetComponentRef<Transform>(entity);
                ref MovementSpeed speed = ref world.GetComponentRef<MovementSpeed>(entity);

                if (direction.movementDirection == Vec2.zero)
                    continue;

                Vec2 normalizedDirection = direction.movementDirection.Normalize();
                float remainingDistance = speed.speed * (1 / Simulation.TicksInSecond);

                while (remainingDistance > 0)
                {
                    float moveStep = Math.Min(remainingDistance, 1.0f);  // Размер тайла = 1.0f

                    Vec2 movementThisStep = normalizedDirection * moveStep;

                    // Проверка движения по X
                    Vec2 nextPositionX = transform.position + new Vec2(movementThisStep.x, 0);
                    if (!CanMoveTo(nextPositionX))
                    {
                        movementThisStep.x = 0;  // Если не можем двигаться по X, то обнуляем движение по этой оси
                    }

                    // Проверка движения по Y
                    Vec2 nextPositionY = transform.position + new Vec2(0, movementThisStep.y);
                    if (!CanMoveTo(nextPositionY))
                    {
                        movementThisStep.y = 0;  // Если не можем двигаться по Y, то обнуляем движение по этой оси
                    }

                    // Применяем движение
                    transform.position += movementThisStep;

                    // Уменьшаем оставшееся расстояние для движения
                    remainingDistance -= moveStep;
                }
            }
        }

        private bool CanMoveTo(Vec2 targetPosition)
        {
            int x = (int)targetPosition.x;
            int y = (int)targetPosition.y;

            if (x < 0 || x >= _tiles.tiles.Length || y < 0 || y >= _tiles.tiles[x].Length)
                return false;

            return _tiles.tiles[x][y].walkable;
        }
    }
}

