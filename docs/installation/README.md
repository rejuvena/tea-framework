# Installation

As _Tea Framework_ is an external assembly and mod, you will need to install it in some way in order to utilize it.

## Strong Referencing

In order to use Tea Framework, you'll have to strongly-reference it inside of your [`build.txt`](https://github.com/tModLoader/tModLoader/wiki/build.txt) file.

<!-- I use TOML here for some primitive highlighting.
Is there a better way? -->

```toml
modReferences=TeaFramework
```

If you already have other mods in your `modReferences` list, you may add `TeaFramework` by appending `,TeaFramework` (mind the comma).

Once you have done this, you have a few approaches for importing the assembly into your mod to reference in compilation and with an IDE.

### tModLoader Mod Dependencies

The first approach is obtaining the mod's DLL and referencing it in the `lib/` folder. You may find a full overview [here](installation/mod_deps.md).

### NuGet Package Dependency

---

**_I haven't actually confirmed whether this works or not, but it is planned._**

---

The second approach is through referencing a [NuGet package](LINK-TODO) containing the Tea Framework dependency. You may find a full overview [here](installation/nuget_package.md).

## `ITeaMod`/`TeaMod` Loading

Once you have referenced Tea Framework in your mod, you'll need to communicate with the API. There are two ways to do this.

### Extending `TeaMod`

The first approach is what we recommend you should do when possible. The `TeaMod` class which extends `Mod` and implements `ITeaMod`. This is the easiest way to directly communicate with Tea Framework's API. This class also provides a plentiful amount of default implementations for you to work with, heavily reducing boilerplate.

### Implementing `ITeaMod`

In the event that you either:

- Want to use your own custom implementations of Tea Framework features.
- Cannot extend `TeaMod` because another library already has their own `Mod` class (i.e. `MyExampleMod : OtherLibraryMod`).

You may directly implement `ITeaMod` (`MyExampleMod : OtherLibraryMod, ITeaMod`) instead. **If you want to use our patching framework, please implement `IPatchRepository` as well.**
