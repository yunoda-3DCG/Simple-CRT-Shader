Simple CRT Shader
==========

Simple CRT Shader is a post processing shader for Unity. This shader is made for the old TV effect and film. 
You can also use the Unity's Post Processing in the same time.

![sample1](https://user-images.githubusercontent.com/50200315/164515706-ab35170d-0fd0-4da9-bb0d-0aa7b40babc8.jpg)


System Requirements
-------------------

Unity 2021.2.9f1 or later versions. Built-in Render Pipeline only.
URP and HDRP are not supported.


How to use
----------
This shader needs to add the C# script to the camera in the scene. Please add "CRT Post Effecter" on the Main Camera and set the CRT material on the field of "Effect Material".
The parameter sample is here.

![property](https://user-images.githubusercontent.com/50200315/164526913-202682b6-2767-4438-a988-f81af9b64e2b.jpg)


The list of effects:
- White Noise
- Screen Jump
- Scanline
- Monochrome
- Flickering
- Slippage
- Chromatic Aberration
- Letter Box
- Decal Texture
- Film Dirt

License
-------

Copyright (C) 2021 yunoda

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
