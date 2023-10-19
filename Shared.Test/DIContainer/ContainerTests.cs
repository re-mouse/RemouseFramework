using NUnit.Framework;
using Remouse.DIContainer;
using System;

namespace Remouse.DIContainer.Tests
{
    [TestFixture]
    public class ContainerTests
    {
        private interface IService {}
        private class Service : IService, IDisposable
        {
            public bool IsDisposed { get; private set; }
            public void Dispose() => IsDisposed = true;
        }

        [Test]
        public void TestRegisterAndResolve()
        {
            var builder = new ContainerBuilder();
            var typeManager = new TypeManager();
            typeManager.RegisterType<IService>().AsType<Service>().WithSingletonLifetime();
            var container = builder.Build();
            var service = container.Resolve<IService>();
            Assert.IsNotNull(service);
            Assert.IsInstanceOf<Service>(service);
        }

        [Test]
        public void TestResolveNotRegisteredReturnsNull()
        {
            var builder = new ContainerBuilder();
            var container = builder.Build();
            var service = container.Resolve<IService>();
            Assert.IsNull(service);
        }

        [Test]
        public void TestContainerDisposeDisposesService()
        {
            var builder = new ContainerBuilder();
            var typeManager = new TypeManager();
            typeManager.RegisterType<IService>().AsType<Service>().WithSingletonLifetime();
            var container = builder.Build();
            var service = container.Resolve<IService>() as Service;
            Assert.IsFalse(service.IsDisposed);
            container.Dispose();
            Assert.IsTrue(service.IsDisposed);
        }

        [Test]
        public void TestDoubleDisposeThrowsException()
        {
            var builder = new ContainerBuilder();
            var container = builder.Build();
            container.Dispose();
            Assert.Throws<ObjectDisposedException>(() => container.Dispose());
        }
    }
}