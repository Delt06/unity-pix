# Unity PIX

A [PIX on Windows](https://devblogs.microsoft.com/pix/) integration for Unity, allowing to make GPU captures right from the Unity Editor.

> 📝 The plugin works only under Windows 10+ and DirectX 12.

## Installation

### 1. Download and install PIX on Windows

PIX on Windows downloads page is available [here](https://devblogs.microsoft.com/pix/download/).

### 2. Add the package to the Unity project

#### Option 1
- Open Package Manager through `Window/Package Manager`
- Click "+" and choose "Add package from git URL..."
- Insert the URL:

```
https://github.com/Delt06/unity-pix.git?path=Packages/com.deltation.unity-pix
```

#### Option 2
Add the following line to `Packages/manifest.json`:
```
"com.deltation.unity-pix": "https://github.com/Delt06/unity-pix.git?path=Packages/com.deltation.unity-pix",
```

## How to use

For usage instructions please follow the [Wiki](https://github.com/Delt06/unity-pix/wiki/Usage) page.
