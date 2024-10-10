using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Ancinia.Consts;

public class TextureStrings
{
    //#region Inventory Items
    //public const string InvHornetKey = "Inv_Hornet";
    //private const string InvHornetFile = "Ancinia.Resources.Inv_Hornet.png";
    //#endregion Inventory Items
    //#region Bosses
    //public const string WeavernPrincessKey = "WeavernPrincess";
    //private const string WeavernPrincessFile = "Ancinia.Resources.WeavernPrincess.png";
    //#endregion Bosses
    //#region Achievements
    //public const string AchievementItemKey = "Achievement_Item";
    //private const string AchievementItemFile = "Ancinia.Resources.Achievement_Item.png";
    //public const string AchievementBossKey = "Achievement_Boss";
    //private const string AchievementBossFile = "Ancinia.Resources.Achievement_Boss.png";
    //public const string AchievementWeaverPrincessKey = "Achievement_WeaverPrincess";
    //private const string AchievementWeaverPrincessFile = "Ancinia.Resources.Achievement_WeaverPrincess.png";
    //#endregion Achievements
    //#region Misc
    //#endregion Misc

    private readonly Dictionary<string, Sprite> _dict;

    public TextureStrings()
    {
        Assembly asm = Assembly.GetExecutingAssembly();
        _dict = new Dictionary<string, Sprite>();
        Dictionary<string, string> tmpTextures = new Dictionary<string, string>();
        //tmpTextures.Add(InvHornetKey, InvHornetFile);
        //tmpTextures.Add(AchievementItemKey, AchievementItemFile);
        //tmpTextures.Add(AchievementBossKey, AchievementBossFile);
        //tmpTextures.Add(AchievementWeaverPrincessKey, AchievementWeaverPrincessFile);

        foreach (KeyValuePair<string, string> pair in tmpTextures)
        {
            using (Stream s = asm.GetManifestResourceStream(pair.Value))
            {
                if (s != null)
                {
                    byte[] buffer = new byte[s.Length];
                    s.Read(buffer, 0, buffer.Length);
                    s.Dispose();

                    //Create texture from bytes
                    Texture2D tex = new Texture2D(2, 2);

                    tex.LoadImage(buffer, true);

                    // Create sprite from texture
                    // Split is to cut off the DreamKing.Resources. and the .png
                    _dict.Add(pair.Key, Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f)));
                }
            }
        }
    }

    public Sprite Get(string key)
    {
        return _dict[key];
    }
}