using System;
using NUnit.Framework;
using TeaFramework.API.DependencyInjection;
using TeaFramework.Features;

namespace TeaFramework.Tests
{
    public static class ApiServiceProviderTest
    {
        [Test]
        public static void TestBasicApiProvider() {
            // Test objective: Ensure APIs can install and uninstall possible.

            IApiServiceProvider provider = new ApiServiceProvider(null!);
            TestApiServiceOne one = new();
            TestApiServiceTwo two = new();

            provider.AddApi(one);
            provider.AddApi(two);

            if (provider.GetApi<TestApiServiceOne>() is null) throw new Exception("TestApiServiceOne not installed.");
            if (provider.GetApi<TestApiServiceTwo>() is null) throw new Exception("TestApiServiceTwo not installed.");

            IQuizzical? quizzical = provider.GetService<IQuizzical>();
            ITesticle? testicle = provider.GetService<ITesticle>();

            if (quizzical is null) throw new Exception("IQuizzical singleton was not present.");
            if (testicle is null) throw new Exception("ITesticle singleton was not present.");

            Assert.AreEqual("Pass", quizzical.Quiz());
            Assert.AreEqual("Pass", testicle.Test());

            provider.RemoveAll();

            quizzical = provider.GetService<IQuizzical>();
            testicle = provider.GetService<ITesticle>();

            if (quizzical is not null) throw new Exception("IQuizzical singleton was present.");
            if (testicle is not null) throw new Exception("ITesticle singleton was present.");
            if (provider.GetApi<TestApiServiceOne>() is not null) throw new Exception("TestApiServiceOne not uninstalled.");
            if (provider.GetApi<TestApiServiceTwo>() is not null) throw new Exception("TestApiServiceTwo not uninstalled.");
        }

        public interface IQuizzical : IService
        {
            string Quiz();
        }

        public interface ITesticle : IService
        {
            string Test();
        }

        public class TestApiServiceOne : IApi
        {
            public string Name => nameof(TestApiServiceOne);

            public void AddTo(IApiServiceProvider apiServiceProvider) {
                apiServiceProvider.SetService<IQuizzical>(new Quizzical());
            }
            
            private class Quizzical : IQuizzical
            {
                public string Quiz() {
                    return "Pass";
                }
            }
        }

        public class TestApiServiceTwo : IApi
        {
            public string Name => nameof(TestApiServiceTwo);

            public void AddTo(IApiServiceProvider apiServiceProvider) {
                apiServiceProvider.SetService<ITesticle>(new Testicle());
            }

            private class Testicle : ITesticle
            {
                public string Test() {
                    return "Pass";
                }
            }
        }
    }
}