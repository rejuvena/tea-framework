# Referencing With DLLs

Since Tea Framework is a mod for tModLoader, you can obtain the Tea Framework `.dll` file by extracting it in-game. Alternatively, `.dll` releases will always be provided on the [GitHub repository](https://github.com/rejuvena/tea-framework) whenever an update is released.

Additionally, due to Tea Framework's mod status, the `.dll` you reference in your project is not what will be used at runtime. Instead, the actually mod assembly loaded by tModLoader will be used. This ensures Tea Framework is always up-to-date.

## Obtaining a `.dll`

As outlined above, you have several options.

### In-Game Extraction

Tea Framework purposefully allows users to extract all of its files. You may open tModLoader, navigate to your mods list, click on the "More Info" button, and click the "Extract" button to obtain a copy of `TeaFramework.dll`.

### GitHub Releases

We manually attach the latest `.dll` on our [GitHub releases page](https://github.com/rejuvena/tea-framework/releases) as well.

## Using the `.dll`

Now that you have obtained a `.dll`, you have to reference it in your project. This guide assumes you have a firm understanding of your IDE and C#. To stay in line with tModLoader, you should throw your `.dll` into a `lib` folder in your mod's root.

With the `.dll` now referenced, you should be given access to all members stored in the assembly. You may return to the installation guide's homepage for further instructions.
