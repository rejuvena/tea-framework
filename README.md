# Tea Framework
_Tea_ is a modding framework that functions as a mod, and is designed to
be used as a dependency by other mods for _tModLoader_.

It utilizes tModLoader's assembly/mod dependency functionality to provide
an additional layer of abstraction between numerous base tML calls as well
as providing numerous niche utilities.

_Tea_ is developed for the 1.4 _tModLoader_ alpha.

## What is Tea?
Stemming from a joke about the naming convention of Terraria-related
projects (see: **t**Shock, **t**ModLoader, **t**Config, **t**API), Tea
was originally envisioned as a sort of "competitor mod loader" (think
Forge vs Fabric (and I guess Quilt...) for Minecraft).

_Tea_ works off the base of an older library mod developed by
[myself](https://github.com/Steviegt6) called _TomatoLib_, recycling much
of the code originally used, though with considerably better quality and
stripping away many redundancies.

## Features
* An extra layer of abstraction, inherit from `TeaFramework.TeaMod` instead of `Terraria.ModLoader.Mod`!
* `FastNoiseLite`, all located in `TeaFramework.Common.Utilities.FastNoiseLite`.
* Advanced reflection utilities, speed up your programming without having to mess with the nitty-gritty!
* Enumerable comparison system framework (compare one item against an enumerable collection to find a match).
* `ItemCollection` system for `Item` enumeration.
* A variety of helpful extension methods to aid you in various ways.

## Planned Features
* Markdown-formattable descriptions.
* Localizable descriptions and mod names, though this could be considered a little... redundant.
* Hooks for modifying the drawing of your mod's UI panel in the mod list.
* A "Service" system.
* Better AI system with a more component-based focus.
* Abstraction of many TML services to provide safety even after updates. Helps with maintainability, though I would not recommend relying on this.
* Manipulation of button drawing for your mod's UI panel.
* Better logging services.
* and more, I guess :)