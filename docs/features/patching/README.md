# Patching Framework

Users are provided with both the raw tools to create their own patching framework, as well as a default implementation. Patches are described using an `IPatch` object. `TeaMod` implements `IPatchRepository`, but `ITeaMod` does not.

## Patch Autoloading

`IPatch` inherits `ILoadable`, and thus any objects implementing `IPatch` will become auto-loadable classes. In the default implementation, this patches will apply themselves and register themselves to an assocaited `Mod` if it implements `IPatchRepository.` If you wish for your `IPatchRepository` to not be associated with your `Mod`, you will have to create your own implementation class.

## `IPatchRepository`

`IPatchRepository` keeps track of every registered patch in an assembly. It only has a `List<T>` of `IMonoModPatch`es.

## `IMonoModPatch`, `ILPatch`, and `DetourPatch`

`ILPatch` and `DetourPatch` both implement `IMonoModPatch`, which contains `void Apply()` and `void Unapply()`. `IMonoModPatch`es are what gets registered to a `IPatchRepository` once a parent `IPatch` gets loaded. A unique `IMonoModPatch` instance should be created by all `IPatch`es.

`ILPatch` and `DetourPatch` are the two default implementations of `IMonoModPatch`, and they are the two types that handle applying IL edits and detours by the default patching system.
