using Modding;
using System;
using System.Collections.Generic;
using System.Reflection;
using Ancinia.Consts;
using UnityEngine;
using UObject = UnityEngine.Object;
using SFCore.Generics;
using SFCore.Utils;
using LanguageStrings = Ancinia.Consts.LanguageStrings;

namespace Ancinia;

public class Ancinia : FullSettingsMod<AcSaveSettings, AcGlobalSettings>
{
    internal static Ancinia Instance;

    public LanguageStrings LangStrings { get; private set; }
    public TextureStrings SpriteDict { get; private set; }
    public AudioStrings AudioDict { get; private set; }
    public SceneChanger SceneChanger { get; private set; }

#if DEBUG_CHARMS
    // DEBUG
    public CharmHelper charmHelper { get; private set; }
#endif

    public static AudioClip GetAudio(string name) => Instance.AudioDict.Get(name);

    public static Sprite GetSprite(string name) => Instance.SpriteDict.Get(name);

    public override string GetVersion() => Util.GetVersion(Assembly.GetExecutingAssembly());

    public override List<ValueTuple<string, string>> GetPreloadNames()
    {
        return new List<ValueTuple<string, string>>
        {
            new ValueTuple<string, string>("White_Palace_18", "White Palace Fly"),
            new ValueTuple<string, string>("White_Palace_18", "saw_collection/wp_saw"),
            new ValueTuple<string, string>("White_Palace_18", "saw_collection/wp_saw (2)"),
            new ValueTuple<string, string>("White_Palace_18", "Soul Totem white_Infinte"),
            new ValueTuple<string, string>("White_Palace_18", "Area Title Controller"),
            new ValueTuple<string, string>("White_Palace_18", "glow response lore 1/Glow Response Object (11)"),
            new ValueTuple<string, string>("White_Palace_18", "_SceneManager"),
            new ValueTuple<string, string>("White_Palace_18", "Inspect Region"),
            new ValueTuple<string, string>("White_Palace_18", "_Managers/PlayMaker Unity 2D"),
            new ValueTuple<string, string>("White_Palace_18", "Music Region (1)"),
            new ValueTuple<string, string>("White_Palace_17", "WP Lever"),
            new ValueTuple<string, string>("White_Palace_17", "White_ Spikes"),
            new ValueTuple<string, string>("White_Palace_17", "Cave Spikes Invis"),
            new ValueTuple<string, string>("White_Palace_09", "Quake Floor"),
            new ValueTuple<string, string>("Grimm_Divine", "Charm Holder"),
            new ValueTuple<string, string>("White_Palace_03_hub", "WhiteBench"),
            new ValueTuple<string, string>("Crossroads_07", "Breakable Wall_Silhouette"),
            new ValueTuple<string, string>("Deepnest_East_Hornet_boss", "Hornet Outskirts Battle Encounter"),
            new ValueTuple<string, string>("Deepnest_East_Hornet_boss", "Hornet Boss 2"),
            new ValueTuple<string, string>("White_Palace_03_hub", "door1"),
            new ValueTuple<string, string>("White_Palace_03_hub", "Dream Entry")
        };
    }

    public Ancinia() : base("Ancinia")
    {
        Instance = this;

        LangStrings = new LanguageStrings();
        SpriteDict = new TextureStrings();

        //AchievementHelper.AddAchievement(AchievementStrings.DefeatedWeaverPrincessKey, GetSprite(TextureStrings.AchievementWeaverPrincessKey), LanguageStrings.AchievementDefeatedWeaverPrincessTitleKey, LanguageStrings.AchievementDefeatedWeaverPrincessTextKey, true);

        InitInventory();

#if DEBUG_CHARMS
        charmHelper = new CharmHelper();
        charmHelper.customCharms = 4;
        charmHelper.customSprites = new Sprite[] { GetSprite(TextureStrings.YKey), GetSprite(TextureStrings.EKey), GetSprite(TextureStrings.EKey), GetSprite(TextureStrings.TKey) };
#endif

        InitCallbacks();
    }

    public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
    {
        Log("Initializing");
        Instance = this;

        SceneChanger = new SceneChanger(preloadedObjects);
        AudioDict = new AudioStrings(SceneChanger);

        //GameManager.instance.StartCoroutine(Register2BossModCore());
        //Platform.Current.EncryptedSharedData.SetInt(AchievementStrings.DefeatedWeaverPrincessKey, 0); // DEBUG

        Log("Initialized");
    }

    private void InitCallbacks()
    {
        // Hooks
        ModHooks.GetPlayerBoolHook += OnGetPlayerBoolHook;
        ModHooks.SetPlayerBoolHook += OnSetPlayerBoolHook;
        ModHooks.GetPlayerIntHook += OnGetPlayerIntHook;
        ModHooks.SetPlayerIntHook += OnSetPlayerIntHook;
        ModHooks.ApplicationQuitHook += SaveAcGlobalSettings;
        ModHooks.LanguageGetHook += OnLanguageGetHook;
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void InitInventory()
    {
        //ItemHelper.AddNormalItem(GetSprite(TextureStrings.InvHornetKey), nameof(SaveSettings.SFGrenadeTestOfTeamworkHornetCompanion), LanguageStrings.HornetInvNameKey, LanguageStrings.HornetInvDescKey);
    }

    private void OnSceneChanged(UnityEngine.SceneManagement.Scene from, UnityEngine.SceneManagement.Scene to)
    {
        string scene = to.name;

        if (scene == TransitionGateNames.Town)
        {
            // Town, debug entrance on the right
            SceneChanger.CR_Change_Town(to);
        }
        else if (scene == TransitionGateNames.AC01)
        {
            GameManager.instance.RefreshTilemapInfo(scene);
        }
        else if (scene == TransitionGateNames.AC02)
        {
            GameManager.instance.RefreshTilemapInfo(scene);
        }
    }

    private void SaveAcGlobalSettings()
    {
        SaveGlobalSettings();
    }

#region Get/Set Hooks

    private string OnLanguageGetHook(string key, string sheet, string orig)
    {
#if DEBUG_CHARMS
        // There probably is a better way to do this, but for now take this
#region Custom Charms
        if (key.StartsWith("CHARM_NAME_"))
        {
            int charmNum = int.Parse(key.Split('_')[2]);
            if (charmHelper.charmIDs.Contains(charmNum))
            {
                return "CHARM NAME";
            }
        }
        if (key.StartsWith("CHARM_DESC_"))
        {
            int charmNum = int.Parse(key.Split('_')[2]);
            if (charmHelper.charmIDs.Contains(charmNum))
            {
                return "CHARM DESC";
            }
        }
#endregion
#endif
        if (LangStrings.ContainsKey(key, sheet))
        {
            return LangStrings.Get(key, sheet);
        }
        return orig;
    }

    private bool OnGetPlayerBoolHook(string target, bool orig)
    {
#if DEBUG_CHARMS
        if (target.StartsWith("gotCharm_"))
        {
            int charmNum = int.Parse(target.Split('_')[1]);
            if (charmHelper.charmIDs.Contains(charmNum))
            {
                return Settings.gotCustomCharms[charmHelper.charmIDs.IndexOf(charmNum)];
            }
        }
        if (target.StartsWith("newCharm_"))
        {
            int charmNum = int.Parse(target.Split('_')[1]);
            if (charmHelper.charmIDs.Contains(charmNum))
            {
                return Settings.newCustomCharms[charmHelper.charmIDs.IndexOf(charmNum)];
            }
        }
        if (target.StartsWith("equippedCharm_"))
        {
            int charmNum = int.Parse(target.Split('_')[1]);
            if (charmHelper.charmIDs.Contains(charmNum))
            {
                return Settings.equippedCustomCharms[charmHelper.charmIDs.IndexOf(charmNum)];
            }
        }
#endif
        FieldInfo tmpField = ReflectionHelper.GetFieldInfo(typeof(AcSaveSettings), target);
        if (tmpField != null)
        {
            return (bool) tmpField.GetValue(SaveSettings);
        }
        if (target == "alwaysFalse")
        {
            return false;
        }
        return orig;
    }

    private bool OnSetPlayerBoolHook(string target, bool orig)
    {
#if DEBUG_CHARMS
        if (target.StartsWith("gotCharm_"))
        {
            int charmNum = int.Parse(target.Split('_')[1]);
            if (charmHelper.charmIDs.Contains(charmNum))
            {
                Settings.gotCustomCharms[charmHelper.charmIDs.IndexOf(charmNum)] = val;
            }
        }
        if (target.StartsWith("newCharm_"))
        {
            int charmNum = int.Parse(target.Split('_')[1]);
            if (charmHelper.charmIDs.Contains(charmNum))
            {
                Settings.newCustomCharms[charmHelper.charmIDs.IndexOf(charmNum)] = val;
            }
        }
        if (target.StartsWith("equippedCharm_"))
        {
            int charmNum = int.Parse(target.Split('_')[1]);
            if (charmHelper.charmIDs.Contains(charmNum))
            {
                Settings.equippedCustomCharms[charmHelper.charmIDs.IndexOf(charmNum)] = val;
            }
        }
#endif
        FieldInfo tmpField = ReflectionHelper.GetFieldInfo(typeof(AcSaveSettings), target);
        if (tmpField != null)
        {
            tmpField.SetValue(SaveSettings, orig);
        }
        return orig;
    }

    private int OnGetPlayerIntHook(string target, int orig)
    {
#if DEBUG_CHARMS
        if (target.StartsWith("charmCost_"))
        {
            int charmNum = int.Parse(target.Split('_')[1]);
            if (charmHelper.charmIDs.Contains(charmNum))
            {
                return Settings.customCharmCosts[charmHelper.charmIDs.IndexOf(charmNum)];
            }
        }
#endif
        FieldInfo tmpField = ReflectionHelper.GetFieldInfo(typeof(AcSaveSettings), target);
        if (tmpField != null)
        {
            return (int) tmpField.GetValue(SaveSettings);
        }
        return orig;
    }

    private int OnSetPlayerIntHook(string target, int orig)
    {
        FieldInfo tmpField = ReflectionHelper.GetFieldInfo(typeof(AcSaveSettings), target);
        if (tmpField != null)
        {
            tmpField.SetValue(SaveSettings, orig);
        }
        return orig;
    }

#endregion Get/Set Hooks
}