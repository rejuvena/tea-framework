# Method Generators

Tea Framework provides simple utilities for retrieving and setting the values of fields and properties by generic methods are runtime to perform operations, which is significanly faster than the normal `GetX` and `SetX` methods if used more than once.

## Examples

### Getting the Value of a Field

```cs
typeof(MyType).InvokeFieldGetter("MyField", myInstance); // returns the value of MyType.MyField
```

### Setting the Value of a Field

```cs
typeof(MyType).InvokeFieldSetter("MyField", myInstance, newInstance); // set the value of MyType.MyField
```

### Getting the Value of a Property

```cs
typeof(MyType).InvokePropertyGetter("MyProperty", myInstance); // returns the value of MyType.MyProperty
```

### Setting the Value of a Property

```cs
typeof(MyType).InvokePropertySetter("MyProperty", myInstance, newInstance); // set the value of MyType.MyProperty
```
