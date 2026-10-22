# Black Market Trade Boost

A BepInEx 6 (IL2CPP) plugin for **MENACE** that boosts the trade value of your
items at the Black Market, restocks the market after every mission and enables
the game's cheaper restock option. Settings are applied automatically a few
seconds after a campaign loads.

## How it works

Every 2 seconds the plugin calls `Menace.States.StrategyState.Get()`. Once a
campaign is loaded, it reads `m_GlobalDifficulty` and sets these fields to the
values from the config file:

| Field | Default |
| --- | --- |
| `PlayerItemsTradeValueMult` | 100 |
| `RestockBlackMarketAfterMissions` | true |
| `CheapBlackMarketRestock` | true |
| `PlayerSupplyMult` | 0 (unchanged) |
| `LootDropChanceBonus` | -1 (unchanged) |

Config file: `BepInEx/config/menace.blackmarkettradeboost.cfg`

## Building

Requirements:

- .NET SDK 6.0 or newer
- MENACE with BepInEx 6 (IL2CPP, bleeding-edge build) installed, launched once
  so that `BepInEx/interop` exists

```
dotnet build -c Release -p:GameDir="C:\Program Files (x86)\Steam\steamapps\common\Menace"
```

On Linux, pass your Steam library path instead, e.g.
`-p:GameDir="$HOME/.steam/steam/steamapps/common/Menace"`.

The build output `MenaceTweaks.dll` is copied to
`BepInEx/plugins/MenaceTweaks/` automatically.

## Linux / Steam Deck

Add this Steam launch option so Proton loads BepInEx:

```
WINEDLLOVERRIDES="winhttp=n,b" %command%
```
