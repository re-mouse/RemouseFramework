using NUnit.Framework;
using System;

namespace Remouse.DI.Tests
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

        public class TestModule : Module
        {
            public override void BindDependencies(TypeManager typeBinder)
            {
                typeBinder.AddSingleton<IService, Service>().AsDisposable();
            }

            public override void BindModuleDependencies(ModuleManager moduleBinder)
            {
            }
        }
        
        public class DuplicatedTestModule : Module
        {
            public override void BindDependencies(TypeManager typeBinder)
            {
                typeBinder.AddSingleton<IService, Service>().AsDisposable();
                typeBinder.AddTransient<IService, Service>().AsDisposable();
            }

            public override void BindModuleDependencies(ModuleManager moduleBinder)
            {
                
            }
        }

        public class TestModuleWithDependencies : Module
        {
            public override void BindDependencies(TypeManager typeBinder)
            {
                
            }

            public override void BindModuleDependencies(ModuleManager moduleBinder)
            {
                moduleBinder.RegisterModule<TestModule>();
            }
        }
        
        public class TestModuleWithDuplicatedDependencies : Module
        {
            public override void BindDependencies(TypeManager typeBinder)
            {
                
            }

            public override void BindModuleDependencies(ModuleManager moduleBinder)
            {
                moduleBinder.RegisterModule<TestModule>();
                moduleBinder.RegisterModule<TestModule>();
            }
        }

        [Test]
        public void TestRegisterAndResolve()
        {
            var builder = new ContainerBuilder();
            builder.AddModule<TestModule>();
            var container = builder.Build();
            var service = container.Resolve<IService>();
            Assert.IsNotNull(service);
            Assert.IsInstanceOf<Service>(service);
        }
        
        [Test]
        public void TestModuleDependencies()
        {
            var builder = new ContainerBuilder();
            builder.AddModule<TestModuleWithDependencies>();
            var container = builder.Build();
            var service = container.Resolve<IService>();
            Assert.IsNotNull(service);
            Assert.IsInstanceOf<Service>(service);
        }
        
        [Test]
        public void TestModuleWidthDuplicatedDependencies()
        {
            var builder = new ContainerBuilder();
            builder.AddModule<TestModuleWithDuplicatedDependencies>();
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
            builder.AddModule<TestModule>();
            var container = builder.Build();
            var service = container.Resolve<IService>() as Service;
            Assert.IsFalse(service.IsDisposed);
            container.Dispose();
            Assert.IsTrue(service.IsDisposed);
        }
        
        [Test]
        public void TestModuleDuplicate()
        {
            var builder = new ContainerBuilder();
            builder.AddModule<TestModule>();
            builder.AddModule<TestModule>();
            var container = builder.Build();
            var service = container.Resolve<IService>() as Service;
            var service2 = container.Resolve<IService>() as Service;
            Assert.IsFalse(service.IsDisposed);
            container.Dispose();
            Assert.IsTrue(service.IsDisposed);
            
            Assert.AreEqual(service, service2);
        }
        
        [Test]
        public void TestModuleWithDuplicateType()
        {
            var builder = new ContainerBuilder();
            builder.AddModule<DuplicatedTestModule>();
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