using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using UnityEngine;
using Menace.States;

[BepInPlugin("menace.blackmarkettradeboost", "Black Market Trade Boost", "1.0.0")]
public class Plugin : BasePlugin
{
    internal static ManualLogSource L;
    internal static ConfigEntry<float> TradeMult;
    internal static ConfigEntry<bool> RestockAfterMissions;
    internal static ConfigEntry<bool> CheapRestock;
    internal static ConfigEntry<float> SupplyMult;
    internal static ConfigEntry<int> LootBonus;

    public override void Load()
    {
        L = Log;
        TradeMult = Config.Bind("BlackMarket", "PlayerItemsTradeValueMult", 100f,
            "Multiplier on the trade value of YOUR items at the black market. 1 = normal.");
        RestockAfterMissions = Config.Bind("BlackMarket", "RestockBlackMarketAfterMissions", true,
            "Restock the black market after every mission.");
        CheapRestock = Config.Bind("BlackMarket", "CheapBlackMarketRestock", true,
            "Make black market restocks cheaper.");
        SupplyMult = Config.Bind("Economy", "PlayerSupplyMult", 0f,
            "Supply income multiplier. 0 = leave the game's value unchanged.");
        LootBonus = Config.Bind("Economy", "LootDropChanceBonus", -1,
            "Loot drop chance bonus. -1 = leave the game's value unchanged.");

        AddComponent<TweakApplier>();
        L.LogInfo("Black Market Trade Boost loaded - waiting for a campaign.");
    }
}

public class TweakApplier : MonoBehaviour
{
    public TweakApplier(IntPtr ptr) : base(ptr) { }

    private float _nextCheck;

    private void Update()
    {
        if (Time.unscaledTime < _nextCheck) return;
        _nextCheck = Time.unscaledTime + 2f;

        try
        {
            var state = StrategyState.Get();
            if (state == null) return;
            var d = state.m_GlobalDifficulty;
            if (d == null) return;

            bool changed = false;

            if (!Mathf.Approximately(d.PlayerItemsTradeValueMult, Plugin.TradeMult.Value))
            { d.PlayerItemsTradeValueMult = Plugin.TradeMult.Value; changed = true; }

            if (d.RestockBlackMarketAfterMissions != Plugin.RestockAfterMissions.Value)
            { d.RestockBlackMarketAfterMissions = Plugin.RestockAfterMissions.Value; changed = true; }

            if (d.CheapBlackMarketRestock != Plugin.CheapRestock.Value)
            { d.CheapBlackMarketRestock = Plugin.CheapRestock.Value; changed = true; }

            if (Plugin.SupplyMult.Value > 0f && !Mathf.Approximately(d.PlayerSupplyMult, Plugin.SupplyMult.Value))
            { d.PlayerSupplyMult = Plugin.SupplyMult.Value; changed = true; }

            if (Plugin.LootBonus.Value >= 0 && d.LootDropChanceBonus != Plugin.LootBonus.Value)
            { d.LootDropChanceBonus = Plugin.LootBonus.Value; changed = true; }

            if (changed) Plugin.L.LogInfo("Difficulty tweaks applied to current campaign.");
        }
        catch (Exception)
        {
            // Not in a campaign yet (menus, loading) - try again in 2 seconds.
        }
    }
}
