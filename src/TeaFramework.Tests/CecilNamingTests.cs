using System;
using System.Reflection;
using NUnit.Framework;
using TeaFramework.Features.Utility;

namespace TeaFramework.Tests
{
    public class InstanceClass
    {
        public class NestedInstanceClass
        {
            public class InceptionInstanceClass
            {
            }
        }
    }

    public static class TestClass
    {
        public static int TestField1;
        public static InstanceClass TestField2;

        public static InstanceClass TestProperty1 { get; }

        public static int TestProperty2 { private get; set; }

        public static int TestMethod1(int a)
        {
            return -1;
        }

        public static void TestMethod2(bool a, __arglist)
        {
        }
    }

    public static class CecilNamingTests
    {
        [Test]
        public static void PrintTest()
        {
            static FieldInfo GetField(string name) =>
                typeof(TestClass).GetField(name, Reflection.UniversalFlags)!;

            static PropertyInfo GetProperty(string name) =>
                typeof(TestClass).GetProperty(name, Reflection.UniversalFlags)!;

            static MethodInfo GetMethod(string name) =>
                typeof(TestClass).GetMethod(name, Reflection.UniversalFlags)!;

            Console.WriteLine(Cecil.GetFullName(GetField(nameof(TestClass.TestField1))));
            Console.WriteLine(Cecil.GetFullName(GetField(nameof(TestClass.TestField2))));
            Console.WriteLine(Cecil.GetFullName(GetProperty(nameof(TestClass.TestProperty1))));
            Console.WriteLine(Cecil.GetFullName(GetProperty(nameof(TestClass.TestProperty2))));
            Console.WriteLine(Cecil.GetFullName(GetMethod(nameof(TestClass.TestMethod1))));
            Console.WriteLine(Cecil.GetFullName(GetMethod(nameof(TestClass.TestMethod2))));
            Console.WriteLine(Cecil.GetFullName(typeof(InstanceClass)));
            Console.WriteLine(Cecil.GetFullName(typeof(InstanceClass.NestedInstanceClass)));
            Console.WriteLine(Cecil.GetFullName(typeof(InstanceClass.NestedInstanceClass.InceptionInstanceClass)));
            Console.WriteLine(Cecil.GetFullName(typeof(TestClass)));
        }
    }
}
