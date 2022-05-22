# Plugging In

Once you have referenced Tea Framework in your mod, you'll need to communicate with the API. There are two ways to do this.

### Extending `TeaMod`

This first approach is what we recommend you do when possible. `TeaMod` provides default implementations for `ITeaMod`, including vital components such as [load steps](../features/load-steps.md) and content loaders.

All you have to do is make your main `Mod` class extend `TeaMod` instead of `Mod`. If your `Mod` class extends another library's class, see [_Implementing `ITeaMod`_](plugging-in.md#implementing-iteamod)_._

### Implementing `ITeaMod`

In the event that you either:

* Want to use your own custom implementations of Tea Framework features.
* Cannot extend `TeaMod` because another library already has their own `Mod` class (i.e. `MyExampleMod : OtherLibraryMod`).

You may directly implement `ITeaMod` (`MyExampleMod : OtherLibraryMod, ITeaMod`) instead. **If you want to use our patching framework, please implement `IPatchRepository` as well.**

See how to best implement `ITeaMod` by [referencing `TeaMod`](../../src/TeaFramework/TeaMod.cs).
