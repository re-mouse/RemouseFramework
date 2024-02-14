// ----------------------------------------------------------------------------
// The Proprietary or MIT-Red License
// Copyright (c) 2012-2023 Leopotam <leopotam@yandex.ru>
// ----------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Text;
#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace Shared.EcsLib.LeoEcsLite {
    public struct EcsPackedEntity {
        internal int id;
        internal int gen;
    }

    public struct EcsPackedEntityWithWorld {
        internal int id;
        internal int gen;
        internal EcsWorld world;
#if DEBUG
        // For using in IDE debugger.
        internal object[] DebugComponentsView {
            get {
                object[] list = null;
                if (world != null && world.IsAlive () && world.IsEntityAliveInternal (id) && world.GetEntityGen (id) == gen) {
                    world.GetComponents (id, ref list);
                }
                return list;
            }
        }
        // For using in IDE debugger.
        internal int DebugComponentsCount {
            get {
                if (world != null && world.IsAlive () && world.IsEntityAliveInternal (id) && world.GetEntityGen (id) == gen) {
                    return world.GetComponentsCount (id);
                }
                return 0;
            }
        }

        // For using in IDE debugger.
        public override string ToString () {
            if (id == 0 && gen == 0) { return "Entity-Null"; }
            if (world == null || !world.IsAlive () || !world.IsEntityAliveInternal (id) || world.GetEntityGen (id) != gen) { return "Entity-NonAlive"; }
            Type[] types = null;
            var count = world.GetComponentTypes (id, ref types);
            StringBuilder sb = null;
            if (count > 0) {
                sb = new StringBuilder (512);
                for (var i = 0; i < count; i++) {
                    if (sb.Length > 0) { sb.Append (","); }
                    sb.Append (types[i].Name);
                }
            }
            return $"Entity-{id}:{gen} [{sb}]";
        }
#endif
    }

#if ENABLE_IL2CPP
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
    internal static class EcsEntityExtensions {
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static EcsPackedEntity PackEntity (this EcsWorld world, int entity) {
            EcsPackedEntity packed;
            packed.id = entity;
            packed.gen = world.GetEntityGen (entity);
            return packed;
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static bool Unpack (this in EcsPackedEntity packed, EcsWorld world, out int entity) {
            entity = packed.id;
            return
                world != null
                && world.IsAlive ()
                && world.IsEntityAliveInternal (packed.id)
                && world.GetEntityGen (packed.id) == packed.gen;
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static bool EqualsTo (this in EcsPackedEntity a, in EcsPackedEntity b) {
            return a.id == b.id && a.gen == b.gen;
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static EcsPackedEntityWithWorld PackEntityWithWorld (this EcsWorld world, int entity) {
            EcsPackedEntityWithWorld packedEntity;
            packedEntity.world = world;
            packedEntity.id = entity;
            packedEntity.gen = world.GetEntityGen (entity);
            return packedEntity;
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static bool Unpack (this in EcsPackedEntityWithWorld packedEntity, out EcsWorld world, out int entity) {
            world = packedEntity.world;
            entity = packedEntity.id;
            return
                world != null
                && world.IsAlive ()
                && world.IsEntityAliveInternal (packedEntity.id)
                && world.GetEntityGen (packedEntity.id) == packedEntity.gen;
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static bool EqualsTo (this in EcsPackedEntityWithWorld a, in EcsPackedEntityWithWorld b) {
            return a.id == b.id && a.gen == b.gen && a.world == b.world;
        }
    }
}
