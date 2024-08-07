<div align="center">

<img height="100px" src="./res/icon.png" />

## Cultbaus Battle Text

<img height="400px" src="./res/record.gif" />

An ImGui replacement for FlyText in Final Fantasy XIV.

</div>

# Quick Start

I'll preface this section saying that, at the time of writing, this is still very much in development, and all releases should be considered unstable. There *will* be breaking changes that will not have migration support. You have been warned!

Add the repository to your custom repos list in Dalamud -> Settings -> Experimental: `https://raw.githubusercontent.com/cultbaus/MyDalamudPlugins/main/repo.json`, then install the plugin. Use `/cbt` to configure.

# Prior Art

Thank you to the creators & maintainers of [FlyTextFilter][1] and [DamageInfo][2] for providing inspiration and an in-code reference implementation for where to begin. It would literally not have been possible for me to begin work on this without their efforts.

# Attribution

Additionally, thank you to the creators & maintainers of [DelvUI][4] and [XIVComboExpanded][3] for providing additional reference material and implementation details that were lost on me from the official documentation sources. Similar to the aforementioned prior art, these plugins were instrumental in providing me a basis for which I could begin.

# Help Wanted
* Localization support
* [FontManager](./CBT/Helpers/FontManager.cs) could use improvement - perhaps using the Dalamud global picker to load all the fonts available to a user.

[1]: https://github.com/Aireil/FlyTextFilter
[2]: https://github.com/lmcintyre/DamageInfoPlugin
[3]: https://github.com/MKhayle/XIVComboExpanded
[4]: https://github.com/DelvUI/DelvUI