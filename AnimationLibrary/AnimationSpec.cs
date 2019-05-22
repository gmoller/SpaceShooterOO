using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimationLibrary
{
    public class AnimationSpec
    {
        public string SpriteSheet { get; set; }
        public int Duration { get; set; } // in milliseconds
        public int NumberOfFrames { get; set; }
        public bool Repeating { get; set; }

        public Rectangle[] Frames { get; set; }
    }

    public static class AnimationSpecCreator
    {
        public static AnimationSpec Create(Texture2D spriteSheetTexture, int frameWidth, int frameHeight, int duration, bool isRepeating)
        {
            var spec = new AnimationSpec { SpriteSheet = spriteSheetTexture.Name, Duration = duration, Repeating = isRepeating };

            int cols = spriteSheetTexture.Width / frameWidth;
            int rows = spriteSheetTexture.Height / frameHeight;
            spec.NumberOfFrames = cols * rows;
            spec.Frames = new Rectangle[spec.NumberOfFrames];

            int x = 0;
            int y = 0;
            for (int i = 0; i < spec.NumberOfFrames; i++)
            {
                spec.Frames[i] = new Rectangle(x, y, frameWidth, frameHeight);
                x += frameWidth;
                if (x >= spriteSheetTexture.Width)
                {
                    x = 0;
                    y += frameHeight;
                }
            }

            return spec;
        }

        //public static AnimationSpec Create()
        //{
        //    var spec = new AnimationSpec { SpriteSheet = "MegaBlast03", Duration = 20, NumberOfFrames = 59, Repeating = false };
        //    spec.Frames = new Rectangle[spec.NumberOfFrames];

        //    spec.Frames[0] = new Rectangle(0, 0, 128, 128);
        //    spec.Frames[1] = new Rectangle(128, 0, 128, 128);
        //    spec.Frames[2] = new Rectangle(256, 0, 128, 128);
        //    spec.Frames[3] = new Rectangle(384, 0, 128, 128);
        //    spec.Frames[4] = new Rectangle(512, 0, 128, 128);
        //    spec.Frames[5] = new Rectangle(640, 0, 128, 128);
        //    spec.Frames[6] = new Rectangle(768, 0, 128, 128);
        //    spec.Frames[7] = new Rectangle(896, 0, 128, 128);

        //    spec.Frames[8] = new Rectangle(0, 128, 128, 128);
        //    spec.Frames[9] = new Rectangle(128, 128, 128, 128);
        //    spec.Frames[10] = new Rectangle(256, 128, 128, 128);
        //    spec.Frames[11] = new Rectangle(384, 128, 128, 128);
        //    spec.Frames[12] = new Rectangle(512, 128, 128, 128);
        //    spec.Frames[13] = new Rectangle(640, 128, 128, 128);
        //    spec.Frames[14] = new Rectangle(768, 128, 128, 128);
        //    spec.Frames[15] = new Rectangle(896, 128, 128, 128);

        //    spec.Frames[16] = new Rectangle(0, 256, 128, 128);
        //    spec.Frames[17] = new Rectangle(128, 256, 128, 128);
        //    spec.Frames[18] = new Rectangle(256, 256, 128, 128);
        //    spec.Frames[19] = new Rectangle(384, 256, 128, 128);
        //    spec.Frames[20] = new Rectangle(512, 256, 128, 128);
        //    spec.Frames[21] = new Rectangle(640, 256, 128, 128);
        //    spec.Frames[22] = new Rectangle(768, 256, 128, 128);
        //    spec.Frames[23] = new Rectangle(896, 256, 128, 128);

        //    spec.Frames[24] = new Rectangle(0, 384, 128, 128);
        //    spec.Frames[25] = new Rectangle(128, 384, 128, 128);
        //    spec.Frames[26] = new Rectangle(256, 384, 128, 128);
        //    spec.Frames[27] = new Rectangle(384, 384, 128, 128);
        //    spec.Frames[28] = new Rectangle(512, 384, 128, 128);
        //    spec.Frames[29] = new Rectangle(640, 384, 128, 128);
        //    spec.Frames[30] = new Rectangle(768, 384, 128, 128);
        //    spec.Frames[31] = new Rectangle(896, 384, 128, 128);

        //    spec.Frames[32] = new Rectangle(0, 512, 128, 128);
        //    spec.Frames[33] = new Rectangle(128, 512, 128, 128);
        //    spec.Frames[34] = new Rectangle(256, 512, 128, 128);
        //    spec.Frames[35] = new Rectangle(384, 512, 128, 128);
        //    spec.Frames[36] = new Rectangle(512, 512, 128, 128);
        //    spec.Frames[37] = new Rectangle(640, 512, 128, 128);
        //    spec.Frames[38] = new Rectangle(768, 512, 128, 128);
        //    spec.Frames[39] = new Rectangle(896, 512, 128, 128);

        //    spec.Frames[40] = new Rectangle(0, 640, 128, 128);
        //    spec.Frames[41] = new Rectangle(128, 640, 128, 128);
        //    spec.Frames[42] = new Rectangle(256, 640, 128, 128);
        //    spec.Frames[43] = new Rectangle(384, 640, 128, 128);
        //    spec.Frames[44] = new Rectangle(512, 640, 128, 128);
        //    spec.Frames[45] = new Rectangle(640, 640, 128, 128);
        //    spec.Frames[46] = new Rectangle(768, 640, 128, 128);
        //    spec.Frames[47] = new Rectangle(896, 640, 128, 128);

        //    spec.Frames[48] = new Rectangle(0, 768, 128, 128);
        //    spec.Frames[49] = new Rectangle(128, 768, 128, 128);
        //    spec.Frames[50] = new Rectangle(256, 768, 128, 128);
        //    spec.Frames[51] = new Rectangle(384, 768, 128, 128);
        //    spec.Frames[52] = new Rectangle(512, 768, 128, 128);
        //    spec.Frames[53] = new Rectangle(640, 768, 128, 128);
        //    spec.Frames[54] = new Rectangle(768, 768, 128, 128);
        //    spec.Frames[55] = new Rectangle(896, 768, 128, 128);

        //    spec.Frames[56] = new Rectangle(0, 896, 128, 128);
        //    spec.Frames[57] = new Rectangle(128, 896, 128, 128);
        //    spec.Frames[58] = new Rectangle(256, 896, 128, 128);

        //    return spec;
        //}
    }
}