# SCPUtils: NorthwoodPluginAPI EDITION<br>

## Command Base Permissions:
 - scputils base command: no permission required.
 - scputils announce: Broadcasting
 - scputils asn: KickingAndShortTermBanning
 - scputils badge: PlayersManagement
 - scputils list: GameplayData
 - scputils player: GameplayData
 - scputils playtime: GameplayData

## Configuration:
With new PluginAPI system, our configs have been splitted in 5 new files: "Command Translation", "Config", "Database", "Permissions" and "Translations".

## Commands: Remote Admin and Server Console
| Command | Arguments | Remote Admin Permission | Aliases |
| --- | --- | --- | --- |
| scputils | [Command] | No permission | scpu, su |
|  |  |  |  |
| scputils announce | [Subcommand: create, delete, list or send] | Broadcasting | a |
|  |  |  |  |
| scputils asn | [Subcommand: whitelist or unwhitelist] | KickingAndShortTermBanning | No aliases. |
| scputils asn whitelist | [PlayerID] | KickingAndShortTermBanning | w |
| scputils asn unwhitelist | [PlayerID] | KickingAndShortTermBanning | uw |
|  |  |  |  |
| scputils badge | [Subcommand: set, revoke or playtime (this subcommand is in two command)] | PlayersManagement | b, group |
| scputils badge set | [PlayerID], [GroupName], [Time] | PlayersManagement | s |
| scputils badge revoke | [PlayerID] | PlayersManagement | r, remove |
| scputils badge playtime | [GroupName], [Days] | PlayersManagement | pt |
|  |  |  |  |
| scputils list | [Subcommand: staff, player] | GameplayData | l |
| scputils list staff | No argument. | GameplayData | s |
| scputils list player | No argument. | GameplayData | p, online, o |
|  |  |  |  |
| scputils player | [Subcommand: broadcast, delete, Dnt, Edit, Info, List, Reset, Restriction, Unrestriction] | GameplayData | p, pl |
| scputils player broadcast | [PlayerID], [Type], [ID], [Seconds] | GameplayData | b |
| scputils player delete | [PlayerID] | ServerConfigs | remove, cancel, c, d |
| scputils player dnt | [PlayerID] | GameplayData | No aliases. |
| scputils player edit | [PlayerID], [TotalGames], [TotalQuitOrSuicide], [TotalKicks], [TotalBans] | ServerConfigs | e |
| scputils player info | [PlayerID] | GameplayData | i |
| scputils player list | [MinimunPercentage] | ServerConfigs | l |
| scputils player reset | [PlayerID], [Preference or All] | PermissionsManagement | r |
| scputils player restriction | [PlayerID], [Time], [Reason] | PermissionsManagement | rs |
| scputils player unrestriction | [PlayerID] | PermissionsManagement | un, ur |
|  |  |  |  |
| scputils playtime | [Subcommand: user, group] | GameplayData | pt, play |
| scputils playtime user | [PlayerID], [Days] | GameplayData | u, member, staff |
| scputils playtime badge | [BadgeName], [Days] | SetGroup | b, group, g |

## Commands: Client Console
| Command | Arguments | Aliases |
| --- | --- | --- |
| scputils | [Command] | scpu, su |
|  |  |  |
| scputils playtime | No arguments | pt |
|  |  |  |
