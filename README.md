![Nez](FAQs/images/nez-logo-black.png)

[![Build status](https://ci.appveyor.com/api/projects/status/github/prime31/Nez?branch=master&svg=true)](https://ci.appveyor.com/project/prime31/nez/branch/master)
[![NuGet version](https://img.shields.io/nuget/v/Nez.svg)](https://www.nuget.org/packages/Nez)
[![NuGet downloads](https://img.shields.io/nuget/dt/Nez.svg)](https://www.nuget.org/packages/Nez)
[![Join the chat at https://discord.gg/dsT3Cv](https://img.shields.io/badge/discord-join-7289DA.svg?logo=discord&longCache=true&style=flat)


Nez aims to be a lightweight 2D framework that sits on top of MonoGame/FNA. It provides a solid base for you to build a 2D game on. Some of the many features it includes are:

- Scene/Entity/Component system with Component render layer tracking and optional entity systems (an implementation that operates on a group of entities that share a specific set of components)
- SpatialHash for super fast broadphase physics lookups. You won't ever see it since it works behind the scenes but you'll love it nonetheless since it makes finding everything in your proximity crazy fast via raycasts or overlap checks.
- AABB, circle and polygon collision/trigger detection
- Farseer Physics (based on Box2D) integration for when you need a full physics simulation
- efficient coroutines for breaking up large tasks across multiple frames or animation timing (Core.startCoroutine)
- in-game debug console extendable by adding an attribute to any static method. Just press the tilde key like in the old days with Quake. Out of the box, it includes a visual physics debugging system, asset tracker, basic profiler and more. Just type 'help' to see all the commands or type 'help COMMAND' to see specific hints.
- Dear ImGui in-game debug panels with the ability to wire up your own ImGui windows
- in-game Component inspector. Open the debug console and use the command `inspect ENTITY_NAME` to display and edit fields/properties and call methods with a button click.
- Nez.Persistence Json and binary serialization. Json includes the ability to automatically resolve references and deal with polymorphic classes
- extensible rendering system. Add/remove renderers and post processors as needed. Renderables are sorted by render layer first then layer depth for maximum flexibility out of the box.
- pathfinding support via Astar and Breadth First Search
- deferred lighting engine with normal map support and both runtime and offline normal map generation
- tween system. Tween any int/float/Vector/quaternion/color/rectangle field or property.
- sprites with sprite animations, scrolling sprites, repeating sprites and sprite trails
- flexible line renderer with configurable end caps including super smooth rounded edges or lightning bolt-like sharp edges
- kick-ass particle system with added support for importing [Particle Designer](https://71squared.com/particledesigner) files
- optimized event emitter for core events that you can also add to any class of your own
- scheduler for delayed and repeating tasks (`Core.schedule` method)
- per-scene content managers. Load your scene-specific content then forget about it. Nez will unload it for you when you change scenes.
- customizable Scene transition system with several built in transitions
- Verlet physics bodies for super fun, constraint-to-particle squishy physics
- tons more stuff


Nez Systems
==========

- [Nez-Core](FAQs/Nez-Core.md)
- [Scene-Entity-Component](FAQs/Scene-Entity-Component.md)
- [Rendering](FAQs/Rendering.md)
- [Content Management](FAQs/ContentManagement.md)
- [Nez Physics/Collisions](FAQs/Physics.md)
- [Farseer Physics](FAQs/FarseerPhysics.md)
- [Scene Transitions](FAQs/SceneTransitions.md)
- [Pathfinding](FAQs/Pathfinding.md)
- [Runtime Inspector](FAQs/RuntimeInspector.md)
- [Verlet Physics](FAQs/Verlet.md)
- [Entity Processing Systems](FAQs/EntitySystems.md)
- [Nez.UI](FAQs/UI.md)
- [Nez.Persistence](Nez.Persistence/README.md)
- [Nez.ImGui](Nez.ImGui/README.md)
- [SVG Support](FAQs/SVG.md)
- [AI (FSM, Behavior Tree, GOAP, Utility AI)](FAQs/AI.md)
- [Deferred Lighting](FAQs/DeferredLighting.md)
- [Pipeline Importers](FAQs/PipelineImporters.md)
- [Samples](FAQs/Samples.md)



Setup
==========
### Install as a submodule:

- create a `Monogame Cross Platform Desktop Project`
- clone or download the Nez repository
- add the `Nez.Portable/Nez.csproj` project to your solution and add a reference to it in your main project
- make your main Game class (`Game1.cs` in a default project) subclass `Nez.Core`

If you intend to use any of the built in Effects or PostProcessors you should also copy or link the `DefaultContent/effects` folder into your projects `Content/nez/effects` folder and the `DefaultContent/textures` folder into `Content/nez/textures`. Be sure to set the Build Action to Content and enable the "Copy to output directory" property so they get copied into your compiled game.

Note: if you get compile errors referencing a missing `project.assets.json` file run `msbuild Nez.sln /t:restore` in the root Nez folder to restore them.


#### (optional) Pipeline Tool setup for access to the Nez Pipeline importers

- add the `Nez.PipelineImporter/Nez.PipelineImporter.csproj` project to your solution
- open the `Nez.PipelineImporter` references dialog and add a reference to the Nez project
- build the `Nez.PipelineImporter` project to generate the DLLs
- open the Pipeline Tool by double-clicking your `Content.mgcb` file, select `Content` and under Settings add `PipelineImporter.dll`, `Ionic.ZLib.dll`, `Newtonsoft.Json.dll` and `Nez.dll` to the References collection.

### Install through NuGet:

Add [Nez](https://www.nuget.org/packages/Nez/) and [Nez.PipelineImporter](https://www.nuget.org/packages/Nez.PipelineImporter/) to your project's NuGet packages.

The latter will not add any references to your projects, installing it this way fetches the necessary `dll` files that your `Content.mgcb` needs to reference. Given the version of Nez that you installed, edit `Content.mgcb` so it looks like this:

```bash
#----------------------------- Global Properties ---------------------------#

...

#-------------------------------- References -------------------------------#

/reference:../../packages/Nez.PipelineImporter.{VERSION}/tools/Nez.dll
/reference:../../packages/Nez.PipelineImporter.{VERSION}/tools/Nez.PipelineImporter.dll
/reference:../../packages/Nez.PipelineImporter.{VERSION}/tools/Newtonsoft.Json.dll
/reference:../../packages/Nez.PipelineImporter.{VERSION}/tools/Ionic.ZLib.dll

#---------------------------------- Content --------------------------------#

...
```

Installing through NuGet, the contents of the `DefaultContent` content folder are also included in the package. You will find them under `packages/Nez.{VERSION}/tools`.

Note that NuGet packages are compiled release DLLs! They will not contain any debug code such as the DebugConsole or debug visualization classes!

---

All Nez shaders are compiled for OpenGL so be sure to use the DesktopGL template, not DirectX! Nez only supports OpenGL out of the box to keep things compatible across Android/iOS/Mac/Linux/Windows.

If you are developing a mobile application you will need to enable touch input by calling `Input.touch.enableTouchSupport()`.


Pipeline Importers
==========
Nez comes stock with a decent bunch of Pipeline tool importers including:

- **Texture Atlas Generator**: give it a directory or a list of files and it will combine them all into a single atlas and provide easy access to the source images at runtime. Supports nine patch sprites as well in the [Android style](http://developer.android.com/tools/help/draw9patch.html) (single pixel border with black lines representing the patches). See also [this generator](https://romannurik.github.io/AndroidAssetStudio/nine-patches.html). The Texture Atlas Generator also includes a per-folder sprite animation generation. The atlas generator uses an XML file as input with an Asset Type of System.String[]. The string array should specify the folder or folders where the source images are located.
- **Tiled**: import [Tiled](http://www.mapeditor.org/) maps. Covers tile, image and object layers and rendering with full culling support built-in along with optimized collider generation.
- **Bitmap Fonts**: imports BMFont files (from programs like [Glyph Designer](https://71squared.com/glyphdesigner), [Littera](http://kvazars.com/littera/), etc). Outputs a single xnb file and includes SpriteBatch extension methods to display text the directly match the SpriteFont methods.
- **Particle Designer Importer**: imports [Particle Designer](https://71squared.com/particledesigner) particle systems for use with the Nez particle system
- **LibGdxAtlases**: imports libGDX texture atlases including nine patch support
- **Texture Packer**: imports a [TexturePacker](https://www.codeandweb.com/texturepacker) atlas and JSON file
- **UISkin Importer**: imports uiskin files (JSON format) that are converted to UISkins. See the [UI page](FAQs/UI.md) for an example and details of the JSON format.
- **Normal Map Generator**: generates normal maps from standard textures
- **XMLTemplateMaker**: this isn't so much an importer as a helper to make your own importer. Pass it a class and it spits out an XML template that you can use for your own custom XML-to-object importers.


Samples Repository
==========
You can find the samples repo [here](https://github.com/prime31/Nez-Samples). It contains a variety of sample scenes that demonstrate the basics of getting stuff done with Nez. [The wiki](https://github.com/prime31/Nez/wiki) also contains a few short examples. [This YouTube playlist](https://www.youtube.com/playlist?list=PLb8LPjN5zpx0ZerxdoVarLKlWJ1_-YD9M) also has a few relevant videos.



Using Nez with FNA
==========
Note that you have to install FNA the required FNA native libs per the [FNA documentation](https://github.com/FNA-XNA/FNA/wiki/1:-Download-and-Update-FNA). Here is what you need to do to get up and running with Nez + FNA:

- clone this repo recursively
- open the Nez solution (Nez/Nez.sln) and build it. This will cause the NuGet packages to refresh.
- download/clone FNA
- open your game's project and add a reference to the FNA and Nez.FNA
- (optinally) add references to Nez.FNA.ImGui or Nez.FNA.FarseerPhysics if you need them


The folder structure the cscproj files expect is something like this:

- TopLevelFolderHousingEverything
	- FNA
	- YourGameProject
	- Nez



### Acknowledgements/Attributions
Bits and pieces of Nez were cherry-picked from various places around the internet. If you see something in Nez that looks familiar open an issue with the details so that we can properly attribute the code.

I want to extend a special thanks to three people and their repos listed below. The Monocle Engine and MonoGame.Extended allowed me to get up and running with MonoGame nearly instantly when I was first evaluating if it would be a good alternative to use for making games. [libGDX](https://github.com/libgdx/libgdx) scene2D UI was ported over to Nez to get a jump start on a UI as well. Nez uses a bunch of concepts and code from all three of these repos.

Matt Thorson's fantastic [Monocle Engine](https://bitbucket.org/MattThorson/monocle-engine)

Dylan Wilson's excellent [MonoGame.Extended](https://github.com/craftworkgames/MonoGame.Extended) and his initial work on converted [Farseer Physics Engine](https://farseerphysics.codeplex.com/) to a Portable Class Library. Farseer is [Microsoft Permissive v1.1](https://farseerphysics.codeplex.com/license) licensed.

Nathan Sweet's libGDX Scene2D UI [libGDX](https://github.com/libgdx/libgdx). Nez UI is based on libGDX Scene2D which is [Apache licensed](UI_LICENSE).

