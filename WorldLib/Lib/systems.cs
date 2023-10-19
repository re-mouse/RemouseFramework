// ----------------------------------------------------------------------------
// The Proprietary or MIT-Red License
// Copyright (c) 2012-2023 Leopotam <leopotam@yandex.ru>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace Remouse.Shared.EcsLib.LeoEcsLite {
    internal interface IEcsSystem { }

    internal interface IEcsPreInitSystem : IEcsSystem {
        void PreInit (IEcsSystems systems);
    }

    internal interface IEcsInitSystem : IEcsSystem {
        void Init (IEcsSystems systems);
    }

    internal interface IEcsRunSystem : IEcsSystem {
        void Run (IEcsSystems systems);
    }

    internal interface IEcsPostRunSystem : IEcsSystem {
        void PostRun (IEcsSystems systems);
    }

    internal interface IEcsDestroySystem : IEcsSystem {
        void Destroy (IEcsSystems systems);
    }

    internal interface IEcsPostDestroySystem : IEcsSystem {
        void PostDestroy (IEcsSystems systems);
    }

    internal interface IEcsSystems {
        T GetShared<T> () where T : class;
        IEcsSystems AddWorld (EcsWorld world, string name);
        EcsWorld GetWorld (string name = null);
        Dictionary<string, EcsWorld> GetAllNamedWorlds ();
        IEcsSystems Add (IEcsSystem system);
        List<IEcsSystem> GetAllSystems ();
        void Init ();
        void Run ();
        void Destroy ();
    }

#if ENABLE_IL2CPP
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
    internal class EcsSystems : IEcsSystems {
        readonly EcsWorld _defaultWorld;
        readonly Dictionary<string, EcsWorld> _worlds;
        readonly List<IEcsSystem> _allSystems;
        readonly List<IEcsRunSystem> _runSystems;
        readonly List<IEcsPostRunSystem> _postRunSystems;
        readonly object _shared;
#if DEBUG
        protected bool inited;
#endif

        public EcsSystems (EcsWorld defaultWorld, object shared = null) {
            _defaultWorld = defaultWorld;
            _shared = shared;
            _worlds = new Dictionary<string, EcsWorld> (8);
            _allSystems = new List<IEcsSystem> (128);
            _runSystems = new List<IEcsRunSystem> (128);
            _postRunSystems = new List<IEcsPostRunSystem> (128);
        }

        public virtual T GetShared<T> () where T : class {
            return _shared as T;
        }

        public virtual IEcsSystems AddWorld (EcsWorld world, string name) {
#if DEBUG && !LEOECSLITE_NO_SANITIZE_CHECKS
            if (inited) { throw new Exception ("Cant add world after initialization."); }
            if (world == null) { throw new Exception ("World cant be null."); }
            if (string.IsNullOrEmpty (name)) { throw new Exception ("World name cant be null or empty."); }
            if (_worlds.ContainsKey (name)) { throw new Exception ($"World with name \"{name}\" already added."); }
#endif
            _worlds[name] = world;
            return this;
        }

        public virtual EcsWorld GetWorld (string name = null) {
            if (name == null) {
                return _defaultWorld;
            }
            _worlds.TryGetValue (name, out var world);
            return world;
        }

        public virtual Dictionary<string, EcsWorld> GetAllNamedWorlds () {
            return _worlds;
        }

        public virtual IEcsSystems Add (IEcsSystem system) {
#if DEBUG && !LEOECSLITE_NO_SANITIZE_CHECKS
            if (inited) { throw new Exception ("Cant add system after initialization."); }
#endif
            _allSystems.Add (system);
            if (system is IEcsRunSystem runSystem) {
                _runSystems.Add (runSystem);
            }
            if (system is IEcsPostRunSystem postRunSystem) {
                _postRunSystems.Add (postRunSystem);
            }
            return this;
        }

        public virtual List<IEcsSystem> GetAllSystems () {
            return _allSystems;
        }

        public virtual void Init () {
#if DEBUG && !LEOECSLITE_NO_SANITIZE_CHECKS
            if (inited) { throw new Exception ("Already initialized."); }
#endif
            foreach (var system in _allSystems) {
                if (system is IEcsPreInitSystem initSystem) {
                    initSystem.PreInit (this);
#if DEBUG && !LEOECSLITE_NO_SANITIZE_CHECKS
                    var worldName = CheckForLeakedEntities (this);
                    if (worldName != null) { throw new Exception ($"Empty entity detected in world \"{worldName}\" after {initSystem.GetType ().Name}.PreInit()."); }
#endif
                }
            }
            foreach (var system in _allSystems) {
                if (system is IEcsInitSystem initSystem) {
                    initSystem.Init (this);
#if DEBUG && !LEOECSLITE_NO_SANITIZE_CHECKS
                    var worldName = CheckForLeakedEntities (this);
                    if (worldName != null) { throw new Exception ($"Empty entity detected in world \"{worldName}\" after {initSystem.GetType ().Name}.Init()."); }
#endif
                }
            }
#if DEBUG
            inited = true;
#endif
        }

        public virtual void Run () {
#if DEBUG && !LEOECSLITE_NO_SANITIZE_CHECKS
            if (!inited) { throw new Exception ("Cant run without initialization."); }
#endif
            for (int i = 0, iMax = _runSystems.Count; i < iMax; i++) {
                _runSystems[i].Run (this);
#if DEBUG && !LEOECSLITE_NO_SANITIZE_CHECKS
                var worldName = CheckForLeakedEntities (this);
                if (worldName != null) { throw new Exception ($"Empty entity detected in world \"{worldName}\" after {_runSystems[i].GetType ().Name}.Run()."); }
#endif
            }
            for (int i = 0, iMax = _postRunSystems.Count; i < iMax; i++) {
                _postRunSystems[i].PostRun (this);
#if DEBUG && !LEOECSLITE_NO_SANITIZE_CHECKS
                var worldName = CheckForLeakedEntities (this);
                if (worldName != null) { throw new Exception ($"Empty entity detected in world \"{worldName}\" after {_postRunSystems[i].GetType ().Name}.PostRun()."); }
#endif
            }
        }

        public virtual void Destroy () {
            for (var i = _allSystems.Count - 1; i >= 0; i--) {
                if (_allSystems[i] is IEcsDestroySystem destroySystem) {
                    destroySystem.Destroy (this);
#if DEBUG && !LEOECSLITE_NO_SANITIZE_CHECKS
                    var worldName = CheckForLeakedEntities (this);
                    if (worldName != null) { throw new Exception ($"Empty entity detected in world \"{worldName}\" after {destroySystem.GetType ().Name}.Destroy()."); }
#endif
                }
            }
            for (var i = _allSystems.Count - 1; i >= 0; i--) {
                if (_allSystems[i] is IEcsPostDestroySystem postDestroySystem) {
                    postDestroySystem.PostDestroy (this);
#if DEBUG && !LEOECSLITE_NO_SANITIZE_CHECKS
                    var worldName = CheckForLeakedEntities (this);
                    if (worldName != null) { throw new Exception ($"Empty entity detected in world \"{worldName}\" after {postDestroySystem.GetType ().Name}.PostDestroy()."); }
#endif
                }
            }
            _worlds.Clear ();
            _allSystems.Clear ();
            _runSystems.Clear ();
            _postRunSystems.Clear ();
#if DEBUG
            inited = false;
#endif
        }

#if DEBUG && !LEOECSLITE_NO_SANITIZE_CHECKS
        public static string CheckForLeakedEntities (IEcsSystems systems) {
            if (systems.GetWorld ().CheckForLeakedEntities ()) { return "default"; }
            foreach (var pair in systems.GetAllNamedWorlds ()) {
                if (pair.Value.CheckForLeakedEntities ()) {
                    return pair.Key;
                }
            }
            return null;
        }
#endif
    }
}
