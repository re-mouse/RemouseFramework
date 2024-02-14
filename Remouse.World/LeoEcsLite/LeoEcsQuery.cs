using System.Runtime.CompilerServices;
using Shared.EcsLib.LeoEcsLite;

namespace Remouse.World
{

    public class LeoEcsQueryBuilder : IQueryBuilder
    {
        private readonly EcsWorld _world;
        private EcsWorld.Mask _mask;

        internal LeoEcsQueryBuilder(EcsWorld world)
        {
            _world = world;
            _mask = new EcsWorld.Mask(_world);
        }

        public IQueryBuilder Inc<T>() where T : struct, IComponent
        {
            _mask.Inc<T>();
            return this;
        }

        public IQueryBuilder Exc<T>() where T : struct, IComponent
        {
            _mask.Exc<T>();
            return this;
        }

        public IQuery End()
        {
            EcsFilter filter = _mask.End();
            return new LeoEcsQuery(filter);
        }
    }

    public class LeoEcsQuery : IQuery
    {
        private readonly EcsFilter _filter;

        internal LeoEcsQuery(EcsFilter filter)
        {
            _filter = filter;
        }

        public IQuery.IEnumerator GetEnumerator()
        {
            return new Enumerator(_filter.GetEnumerator());
        }

        public struct Enumerator : IQuery.IEnumerator
        {
            private EcsFilter.Enumerator _enumerator;

            internal Enumerator(EcsFilter.Enumerator enumerator)
            {
                _enumerator = enumerator; 
            }
            
            public int Current {
                [MethodImpl (MethodImplOptions.AggressiveInlining)]
                get => _enumerator.Current;
            }

            [MethodImpl (MethodImplOptions.AggressiveInlining)]
            public bool MoveNext () {
                return _enumerator.MoveNext();
            }

            [MethodImpl (MethodImplOptions.AggressiveInlining)]
            public void Dispose()
            {
                _enumerator.Dispose();
            }
        }
    }

}