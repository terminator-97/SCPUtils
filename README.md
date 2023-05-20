# SCPUtils: NorthwoodPluginAPI EDITION<br>

## Command Base Permissions:
 - scputils base command: no permission required.
 - scputils announce: Broadcasting
 - scputils asn: KickingAndShortTermBanning
 - scputils badge: PlayersManagement
 - scputils playtime: GameplayData

## Configuration:
With new PluginAPI system, our configs have been splitted in 3 new files: "Command Translation", "Config", "Database" and "Permissions".

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

## Commands: Client Console
| Command | Arguments | Aliases |
| | | |
| scputils | [Command] | scpu, su |
| | | |
| scputils playtime | No arguments | pt |
