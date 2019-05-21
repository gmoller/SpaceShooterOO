using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteFontPlus;

namespace SpaceShooterUtilities
{
    public sealed class AssetsManager
    {
        private static readonly Lazy<AssetsManager> Lazy = new Lazy<AssetsManager>(() => new AssetsManager());

        private readonly Dictionary<string, Texture2D> _textures;
        private readonly Dictionary<string, SoundEffect> _sounds;
        private readonly Dictionary<string, SpriteFont> _spriteFonts;
        private readonly Dictionary<string, DynamicSpriteFont> _dynamicSpriteFonts;

        public static AssetsManager Instance => Lazy.Value;

        public ContentManager ContentManager { get; set; }

        private AssetsManager()
        {
            _textures = new Dictionary<string, Texture2D>();
            _sounds = new Dictionary<string, SoundEffect>();
            _spriteFonts = new Dictionary<string, SpriteFont>();
            _dynamicSpriteFonts = new Dictionary<string, DynamicSpriteFont>();
        }

        public void AddTexture(string key, string assetName)
        {
            var texture = ContentManager.Load<Texture2D>(assetName);
            _textures.Add(key, texture);
        }

        public void AddTextures(List<string> textures)
        {
            foreach (string texture in textures)
            {
                AddTexture(texture, texture);
            }
        }

        public void AddTextures(params string[] textures)
        {
            foreach (string texture in textures)
            {
                AddTexture(texture, texture);
            }
        }

        public Texture2D GetTexture(string key)
        {
            return _textures[key];
        }
 
        public void AddSound(string key, string assetName)
        {
            var sound = ContentManager.Load<SoundEffect>(assetName);
            _sounds.Add(key, sound);
        }

        public void AddSounds(List<string> sounds)
        {
            foreach (string sound in sounds)
            {
                AddSound(sound, sound);
            }
        }

        public void AddSounds(params string[] sounds)
        {
            foreach (string sound in sounds)
            {
                AddSound(sound, sound);
            }
        }

        public SoundEffect GetSound(string key)
        {
            return _sounds[key];
        }

        public void AddSpriteFonts(List<string> fontNames)
        {
            foreach (string fontName in fontNames)
            {
                AddSpriteFont(fontName, fontName);
            }
        }

        public void AddSpriteFonts(params string[] fontNames)
        {
            foreach (string fontName in fontNames)
            {
                AddSpriteFont(fontName, fontName);
            }
        }

        public void AddSpriteFont(string key, string assetName)
        {
            var spriteFont = ContentManager.Load<SpriteFont>(assetName);
            _spriteFonts.Add(key, spriteFont);
        }

        public void AddDynamicSpriteFont(string fontName)
        {
            var dynamicSpriteFont = DynamicSpriteFont.FromTtf(File.ReadAllBytes($@"Content\{fontName}.ttf"), 50);
            _dynamicSpriteFonts.Add(fontName, dynamicSpriteFont);
        }

        public SpriteFont GetSpriteFont(string key)
        {
            return _spriteFonts[key];
        }

        public DynamicSpriteFont GetDynamicSpriteFont(string key)
        {
            return _dynamicSpriteFonts[key];
        }
    }
}