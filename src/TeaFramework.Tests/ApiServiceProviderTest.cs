using System;
using NUnit.Framework;
using TeaFramework.API.DependencyInjection;

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

            provider.InstallApi(one);
            provider.InstallApi(two);

            if (provider.GetApiService<TestApiServiceOne>() is null) throw new Exception("TestApiServiceOne not installed.");
            if (provider.GetApiService<TestApiServiceTwo>() is null) throw new Exception("TestApiServiceTwo not installed.");

            IQuizzical? quizzical = provider.GetServiceSingleton<IQuizzical>();
            ITesticle? testicle = provider.GetServiceSingleton<ITesticle>();

            if (quizzical is null) throw new Exception("IQuizzical singleton was not present.");
            if (testicle is null) throw new Exception("ITesticle singleton was not present.");

            Assert.AreEqual("Pass", quizzical.Quiz());
            Assert.AreEqual("Pass", testicle.Test());

            provider.UninstallApi<TestApiServiceOne>();
            provider.UninstallApi<TestApiServiceTwo>();

            quizzical = provider.GetServiceSingleton<IQuizzical>();
            testicle = provider.GetServiceSingleton<ITesticle>();

            if (quizzical is not null) throw new Exception("IQuizzical singleton was present.");
            if (testicle is not null) throw new Exception("ITesticle singleton was present.");
            if (provider.GetApiService<TestApiServiceOne>() is not null) throw new Exception("TestApiServiceOne not uninstalled.");
            if (provider.GetApiService<TestApiServiceTwo>() is not null) throw new Exception("TestApiServiceTwo not uninstalled.");
        }

        public interface IQuizzical
        {
            string Quiz();
        }

        public interface ITesticle
        {
            string Test();
        }

        public class TestApiServiceOne : IApiService
        {
            public string Name => nameof(TestApiServiceOne);

            public void Install(IApiServiceProvider apiServiceProvider) {
                apiServiceProvider.SetServiceSingleton<IQuizzical>(new Quizzical());
            }

            public void Uninstall(IApiServiceProvider apiServiceProvider) {
                apiServiceProvider.SetServiceSingleton<IQuizzical>(null);
            }

            private class Quizzical : IQuizzical
            {
                public string Quiz() {
                    return "Pass";
                }
            }
        }

        public class TestApiServiceTwo : IApiService
        {
            public string Name => nameof(TestApiServiceTwo);

            public void Install(IApiServiceProvider apiServiceProvider) {
                apiServiceProvider.SetServiceSingleton<ITesticle>(new Testicle());
            }

            public void Uninstall(IApiServiceProvider apiServiceProvider) {
                apiServiceProvider.SetServiceSingleton<ITesticle>(null);
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