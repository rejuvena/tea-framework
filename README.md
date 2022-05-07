# Tea Framework

_Tea Framework_ (hereby _Tea_) is a progressive modding framework for [tModLoader](https://github.com/tModLoader).

Tea provides numerous utilities for modders to use, and heavily reduces boilerplate.

## Why the Name?

Stemming from a joke about the naming convention of Terraria-related projects (see: **t**Shock, **t**ModLoader, **t**Config, **t**API), Tea was originally envisioned as several projects. When I was first learning assembly loading, I decided to "port" a super-old version of tModLoader to 1.4 (rest assured, it never got far). After this, I later contemplated using the name for a sort of "competitive mod loader" (think Forge vs. Fabric (and Quilt as well..., I guess)). We came to the conclusion that Tea, as a concept, worked better as a tModLoader mod that offered an expanded featureset.

Tea works off of two libraries, [TomatoLib](https://github.com/Steviegt6/TomatoLib) by [myself](https://github.com/Steviegt6) and [pboneLib](https://github.com/Pbone3/PboneLib) by [pbone](https://github.com/Pbone3).

## Features

Tea is still under heavy development. We have designed Tea in such a way that allows us to create base interfaces for all features of the library. This allows users to freely implement their own dependencies instead of using our default ones. We have purposefully designed Tea to center around the `Terraria.ModLoader.Mod` class (we expect you to use `TeaFramework.TeaMod`, which extends `Terraria.ModLoader.Mod` and implements `ITeaMod` and `IPatchRepository`), though you may freely implement every interface aside from `TeaMod` elsewhere.

Outlined below is a list of currently-implemented features, as well as our goals moving forward:

- A fully-featured reflection library built in.
  - Features caching (likely made redundant by the modern CLR...) as well as easy access to members through the elimenation of `BindingFlag` boilerplate.
  - Dynamic method generation for quickly accessing private fields and properties (a modified and expanded system developed by [absoluteAquarian](https://github.com/absoluteAquarian)).
- An expanded patching API for easily modifying compiled IL at runtime.
  - This is a sort of "wrapper" around [MonoMod](https://github.com/MonoMod/MonoMod), which is already supplied by tModLoader.
  - Patches are delegate-based and use autoloading for convenience.
  - Isolated to an `IPatchRepository`, mostly decoupling it from `Terraria.ModLoader.Mod`.
- **(PLANNED)** A redone loading system, working with steps (represented as `ILoadStep`).
  - Eliminates `Terraria.ModLoader.Mod.Load()`.

## Documentation
Tea has summaries for most important types and members. Additionally, we have a [Read the Docs](readthedocs.io) 
