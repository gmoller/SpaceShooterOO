﻿using System;
using System.Collections.Generic;
using AnimationLibrary;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooterUtilities
{
    public sealed class AssetsManager
    {
        private static readonly Lazy<AssetsManager> Lazy = new Lazy<AssetsManager>(() => new AssetsManager());

        private readonly Dictionary<string, Texture2D> _textures;
        private readonly Dictionary<string, AnimationSpec> _animations;
        private readonly Dictionary<string, SoundEffect> _sounds;
        private readonly Dictionary<string, SpriteFont> _spriteFonts;

        public static AssetsManager Instance => Lazy.Value;

        public ContentManager ContentManager { get; set; }

        private AssetsManager()
        {
            _textures = new Dictionary<string, Texture2D>();
            _animations = new Dictionary<string, AnimationSpec>();
            _sounds = new Dictionary<string, SoundEffect>();
            _spriteFonts = new Dictionary<string, SpriteFont>();
        }

        #region Textures
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

        public void AddTexture(string assetName)
        {
            AddTexture(assetName, assetName);
        }

        public void AddTexture(string key, Texture2D texture)
        {
            _textures.Add(key, texture);
        }

        public void AddTexture(string key, string assetName)
        {
            var texture = ContentManager.Load<Texture2D>(assetName);
            _textures.Add(key, texture);
        }

        public Texture2D GetTexture(string key)
        {
            return _textures[key];
        }
        #endregion

        #region Animations
        public void AddAnimation(string key, AnimationSpec spec)
        {
            _animations.Add(key, spec);
        }

        public AnimationSpec GetAnimations(string key)
        {
            return _animations[key];
        }
        #endregion

        #region Sounds
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

        public void AddSound(string assetName)
        {
            AddSound(assetName, assetName);
        }

        public void AddSound(string key, string assetName)
        {
            var sound = ContentManager.Load<SoundEffect>(assetName);
            _sounds.Add(key, sound);
        }

        public SoundEffect GetSound(string key)
        {
            return _sounds[key];
        }
        #endregion

        #region SpriteFonts
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

        public void AddSpriteFont(string assetName)
        {
            AddSpriteFont(assetName, assetName);
        }

        public void AddSpriteFont(string key, string assetName)
        {
            var spriteFont = ContentManager.Load<SpriteFont>(assetName);
            _spriteFonts.Add(key, spriteFont);
        }

        public void AddSpriteFont(string key, SpriteFont font)
        {
            _spriteFonts.Add(key, font);
        }

        public SpriteFont GetSpriteFont(string key)
        {
            return _spriteFonts[key];
        }
        #endregion
    }
}