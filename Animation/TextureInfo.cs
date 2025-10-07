using SFML.Graphics;
using SFML.System;

namespace Pacman;

public struct TextureInfo
{
    public IntRect TextureRect;

    public TextureInfo(IntRect textureRect)
    {
        TextureRect = textureRect;
    }
}