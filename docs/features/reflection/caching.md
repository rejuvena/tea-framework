# Reflection Caching

Reflection caching reduces code boilerplate and allows us to provide additional reflection utilities.

## `GetCachedX` and `GetCachedXNullable`

As an alternative to the normal `GetX` reflection methods, we provide `GetCachedX` and `GetCachedXNullable` methods. These are extension methods that are usable in place of the normal `GetX` methods.

All `GetCachedX` methods rely on their `GetCachedXNullable` equivalents, and work under the assumption the returned value is guaranteed to not be `null`. These methods do not have any `BindingFlag` parameters, as we instead search for all members regardless of access modifiers.

### `GetCachedMethod`

`GetCachedMethod` is unique in that takes not only a `String name`, but also a `Type[]? signature` and `int genericCount`. These are respectully `null` and `0` by default, but are used in order to allow users to access overloads. `signature` describes the non-generic parameters, and `genericCount` denotes the amount of generic parameters a method has.

### `GetCachedConstructor`

`GetCachedConstructor` has an additional `Type[] signature` parameter, which _must_ be provided in order to retrieve a constructor.

## Utilities

Described below are the various utility methods provided by Tea Framework.

### `InvokeUnderlyingMethod`

Invokes a method defined in a `FieldInfo` or `PropertyInfo`'s `FieldType`/`PropertyType`. `Type[]? signature` and `int genericCount` are the same as `GetCachedMethod`.

### `GetFieldValue`

Returns the value of the described field.

### `GetPropertyValue`

Returns the value of the described property.

### `SetFieldValue`

Sets the value of the described field.

### `SetPropertyValue`

Sets the value of the described property.

### `SetNewInstance`

Allows you to either directly set the value of a `FieldInfo`/`PropertyInfo` or use one created from `Activator.CreateInstance`.
