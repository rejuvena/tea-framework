# Referencing Tea Framework

In order to use Tea Framework, you'll have to strongly-reference it inside your [`build.txt`](https://github.com/tModLoader/tModLoader/wiki/build.txt) file.

```toml
modReferences=TeaFramework
```

If you already have other mods in your `modReferences` list, you may add `TeaFramework` by appending `,TeaFramework` (mind the comma).

Once you have done this, you have a few approaches for importing the assembly into your mod to reference in compilation and with an IDE.

### tModLoader Mod Dependencies

The first approach is obtaining the mod's DLL and referencing it in the `lib/` folder. You may find a full overview [here](../installation/mod\_deps.md).

### NuGet Package Dependency

{% hint style="danger" %}
This has not yet been tried, but will be investigated.
{% endhint %}

The second approach is through referencing a [NuGet package](../LINK-TODO/) containing the Tea Framework dependency. You may find a full overview [here](../installation/nuget\_package.md).
