using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TeaFramework.API.CustomLoading;
using TeaFramework.Impl.CustomLoading;

namespace TeaFramework.Tests
{
    public static class LoadStepCollectionTests
    {
        [Test]
        public static void TestOrdering()
        {
            // Verify that a randomly ordered LoadStepCollection is, when iterated over, iterated in ascending order of weight.

            const int iterations = 6;

            List<string> names = new();
            for (int i = 0; i < iterations; i++)
                names.Add(Guid.NewGuid().ToString());

            List<float> weights = new();
            for (int i = 1; i < iterations + 1; i++)
                weights.Add(i);

            Random rand = new();
            Dictionary<string, float> steps = weights.ToDictionary(x => names[(int)x - 1]);
            Dictionary<string, float> randomSequence = steps.OrderBy(x => rand.NextDouble())
                .ToDictionary(x => x.Key, x => x.Value);

            LoadStepCollection collection = new();
            for (int i = 0; i < iterations; i++)
                collection.Add(new LoadStep(
                    randomSequence.Keys.ElementAt(i),
                    randomSequence.Values.ElementAt(i),
                    x => { },
                    x => { }
                ));

            float counter = 1f;
            foreach (ILoadStep step in collection)
                Assert.AreEqual(counter++, step.Weight);
        }
    }
}
