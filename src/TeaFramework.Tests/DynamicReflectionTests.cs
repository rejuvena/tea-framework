using System;
using NUnit.Framework;
using TeaFramework.Features.Utility;

namespace TeaFramework.Tests
{
    public static class DynamicReflectionTests
    {
        private class TestClass
        {
            public string TestField1 = "Test Field One";
            public int TestField2 = 2;
            public static string TestField3 = "Test Field Three";
            public static int TestField4 = 4;

            public string TestProperty1 { get; set; } = "Test Property One";
            public int TestProperty2 { get; set; } = 2;
            public static string TestProperty3 { get; set; } = "Test Property 3";
            public static int TestProperty4 { get; set; } = 4;
        }
        
        [Test]
        public static void GenericTestFieldsProperties()
        {
            TestClass test = new();
            
            Console.WriteLine("Field getters:");
            Console.WriteLine(Reflection<TestClass>.InvokeFieldGetter("TestField1", test));
            Console.WriteLine(Reflection<TestClass>.InvokeFieldGetter("TestField2", test));
            Console.WriteLine(Reflection<TestClass>.InvokeFieldGetter("TestField3", null));
            Console.WriteLine(Reflection<TestClass>.InvokeFieldGetter("TestField4", null));
            
            Console.WriteLine("Field setters:");
            Reflection<TestClass>.InvokeFieldSetter("TestField1", test, "Modified 1");
            Console.WriteLine(test.TestField1);
            Reflection<TestClass>.InvokeFieldSetter("TestField2", test, -2);
            Console.WriteLine(test.TestField2);
            Reflection<TestClass>.InvokeFieldSetter("TestField3", null, "Modified 3");
            Console.WriteLine(TestClass.TestField3);
            Reflection<TestClass>.InvokeFieldSetter("TestField4", null, -4);
            Console.WriteLine(TestClass.TestField4);
            
            Console.WriteLine("Property getters:");
            Console.WriteLine(Reflection<TestClass>.InvokePropertyGetter("TestProperty1", test));
            Console.WriteLine(Reflection<TestClass>.InvokePropertyGetter("TestProperty2", test));
            Console.WriteLine(Reflection<TestClass>.InvokePropertyGetter("TestProperty3", null));
            Console.WriteLine(Reflection<TestClass>.InvokePropertyGetter("TestProperty4", null));
            
            Console.WriteLine("Property setters:");
            Reflection<TestClass>.InvokePropertySetter("TestProperty1", test, "Modified 1");
            Console.WriteLine(test.TestProperty1);
            Reflection<TestClass>.InvokePropertySetter("TestProperty2", test, -2);
            Console.WriteLine(test.TestProperty2);
            Reflection<TestClass>.InvokePropertySetter("TestProperty3", null, "Modified 3");
            Console.WriteLine(TestClass.TestProperty3);
            Reflection<TestClass>.InvokePropertySetter("TestProperty4", null, -4);
            Console.WriteLine(TestClass.TestProperty4);
        }
    }
}
