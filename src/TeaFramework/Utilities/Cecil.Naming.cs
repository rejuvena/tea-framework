using System;
using System.Reflection;
using System.Text;

namespace TeaFramework.Utilities
{
    partial class Cecil
    {
        public static string GetMemberFullName(object obj) {
            static string Combine(string? declaringName, string name) {
                if (declaringName is null) return name;

                return declaringName + "::" + name;
            }

            return obj switch
            {
                MemberInfo memberInfo => Combine(memberInfo.DeclaringType?.FullName, memberInfo.Name),
                _ => $"Object of type \"{obj}\" does not derive from \"{typeof(MemberInfo).FullName}\"!"
            };
        }

        public static string GetFullName(object obj) {
            string memberFullName = GetMemberFullName(obj);

            return obj switch
            {
                EventInfo eventInfo => eventInfo.EventHandlerType!.FullName + " " + memberFullName,
                MethodInfo methodInfo => GetMethodName(methodInfo, memberFullName),
                FieldInfo fieldInfo => fieldInfo.DeclaringType!.FullName + " " + memberFullName,
                PropertyInfo propertyInfo => GetPropertyName(propertyInfo, memberFullName),
                Type type => GetTypeName(type),
                _ => $"Object of type \"{obj}\" does not have a FullName!"
            };
        }

        public static string GetMethodName(MethodInfo methodInfo, string memberFullName) {
            StringBuilder builder = new StringBuilder()
                                   .Append(methodInfo.ReturnType.FullName)
                                   .Append(' ')
                                   .Append(memberFullName);

            bool varArgs = (methodInfo.CallingConvention & CallingConventions.VarArgs) != 0;
            ParameterInfo[] parameters = methodInfo.GetParameters();

            builder.Append('(');

            if (parameters.Length != 0)
                for (int i = 0; i < parameters.Length; i++) {
                    ParameterInfo param = parameters[i];

                    if (i > 0) builder.Append(',');

                    builder.Append(param.ParameterType.FullName);
                }

            if (varArgs) {
                if (parameters.Length != 0) builder.Append(',');

                builder.Append("...");
            }

            builder.Append(')');

            return builder.ToString();
        }

        public static string GetPropertyName(PropertyInfo propertyInfo, string memberFullName) {
            StringBuilder builder = new();

            builder.Append(propertyInfo.PropertyType.Name)
                   .Append(' ')
                   .Append(memberFullName)
                   .Append('(');

            ParameterInfo[]? parameters = propertyInfo.GetMethod?.GetParameters();

            if (parameters is not null && parameters.Length != 0)
                for (int i = 0; i < parameters.Length; i++) {
                    if (i > 0) builder.Append(',');

                    builder.Append(parameters[i].ParameterType.FullName);
                }

            builder.Append(')');

            return builder.ToString();
        }

        public static string GetTypeName(Type? type) {
            // Not consistent w/ Mono.Cecil.
            if (type is null) return "<null type>";

            // if nested: use shorthand name, else: use qualified name. if no qualified name, use "<unnamed>"
            string fullName = type.IsNested ? type.Name : type.FullName ?? "<unnamed>";

            if (type.IsNested) fullName = GetTypeName(type.DeclaringType!) + '/' + fullName;

            return fullName;
        }
    }
}