# 2-Player Mode Character Selector 

Always wanted to create one of those cool character selection screen in fighting games. Well here is my attempt!. 😊

![ScreeShot](Design/screenshots/ss-04.png)

## How its done!
For this project, I wanted to try Unity's UIToolkit for the selection grid. One UXML represents all the swatch elements for maximum reusability, while a second UXML acts as the container. The swatch UXML instances are arranged in three sets of 2x4 grids, with styling done using Unity's Style Sheets.

Input handling is done using Unity's new Input System. Upon running, the first input device (keyboard on PC) is detected as the controller for P1. For the second character, the system automatically detects when a second input device (i.e., an Xbox controller) is connected, and the selection swatch appears for P2.

The character 3D models are rigged using Mixamo and imported into Blender to mix the animation clips using the NLA Editor. The models are exported from Blender as FBX files, with three unique idle fighting pose clips. A basic Mecanim setup is done in Unity, along with a controller component to handle model changes. Also few lines to ensure that no two nearby swatches play the same animation when switching.

The background elements are simple SpriteRenderers, with their positions updated each frame through a custom class component, which has speed and range as attributes.

Finally, two audio clips are managed by an AudioManager class to handle player join sounds and player selection change sounds. A background music track would also be a nice addition todo! ✌

## How to setup the project ?

### Prerequisites
- Created with Unity 2022.3.44f1
- From Unity Asset Store page of [DOTween](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676), click 'Add To My Assets' to to link this package to your account if not already owned.


### Fixing DOTween missing issue.

The project uses DOTween Unity Asset Store package which is not included in the repo and can be manually imported using the following guide.

- Upon opening the project you may be shown with a an error dialog, choose 'Enter SafeMode'. This is because the DOTween module is missing and yet to be installed.

<p align="center" width="100%">
    <img alt="setup-01" src="Design/screenshots/setup-01.png"> 
</p>

- Goto `Window -> Package Manager`. Under `Packages:My Assets`, select 'DOTween (HOTween v2)', download and import.

<p align="center" width="100%">
    <img alt="setup-02" width="75%" src="Design/screenshots/setup-02.png"> 
</p>

- After importing, click `Open DOTween Utility Panel` button to setup DOTween. (Alternativley you can always open using `Tools -> Demigiant -> DOTween Utility Panel` )

<p align="center" width="100%">
    <img alt="setup-03" src="Design/screenshots/setup-03.png"> 
</p>

- From DOTween Utility Panel, ensure `UI` option is enabled. You can enable other modules for DOTween if you plan on improving.

<p align="center" width="100%">
    <img alt="setup-04" src="Design/screenshots/setup-04.png"> 
</p>

- Click `Apply` and wait for the new assemblies to finish compiling.