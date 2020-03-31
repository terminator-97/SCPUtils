**SCPUtils Plugin**

Welcome to SCPUtils, this plugin has many features such as welcome messages, decontamination messages, autorestart when only 1 player is present in game and  punishements for the ones that quit / suicide as SCP and many others features <br />
Database will get created inside Exiled/SCPUtils folder.<br />
**You should add LiteDB.dll into Plugins/dependencies folder or plugin won't work**
**Required minimum Exiled version: 1.9.10**

### Configs:


| Config Name  | Type | Default Value | Description |
| ------------- | ------------- | ------------- | ------------- |
| scputils_enable_round_restart_check  | bool  | true | Enable / disable round restart check |
| scputils_enable_scp_suicide_autowarn | bool  | true | Enable / disable suicide autowarn (required for kick and warns) |
| scputils_auto_kick_scp_suicide  | bool  | true | Enable / disable autokick for scp suicides after a certain threshold |
| scputils_quit_equals_suicide  | bool  | true | Should quits be considered as suicide? |
| scputils_welcome_enabled  | bool  | true | Enable / Disabile welcome message when player join |
| scputils_decontamination_message_enabled  | bool  | false | Enable / disable a message when decontamination starts |
| scputils_enable_scp_suicide_auto_ban  | bool  | true |  Enable / disable autoban for scp suicides after a certain threshold |
| scputils_remove_overwatch_round_start  | bool  | false | Enable / disable overwatch removal for everyone when round starts |
| scputils_double_ban_duration_each_ban  | bool  | true | Multiply ban duration after each ban |
| scputils_welcome_message  | string  | Welcome to the server! | Welcome message, Change it! |
| scputils_ondecontamination_message  | string  | Please read server rules! | Decontamination message, Change it! |
| scputils_auto_restart_message  | string  | <color=red>Round Restart:</color>\n<color=yellow>Round will be restarted in {0} seconds due lack of players</color> | Autorestart message, {0} is the amount of seconds |
| scputils_suicide_warn_message  | string  | <color=red>WARN:\nAs per server rules SCP's suicide is an offence, doing it will result in a ban!</color> | Warn message |
| scputils_suicide_kick_message  | string  | Suicide as SCP | Kick message |
| scputils_auto_ban_message | string  | Exceeded SCP suicide limit Duration: {0} minutes | Ban message {0} is the ban duration |
| scputils_welcome_duration  | int  | 12 | Welcome message duration |
| scputils_decontamination_message_duration | int  | 10 | Decontamination message duration |
| scputils_auto_restart_time  | int  | 15 | After how many seconds round should be restarted if there is only 1 player? |
| scputils_autowarn_message_duration  | int  | 30 | Autowarn message duration |
| scputils_auto_ban_duration  | int | 15 | Autoban duration in minutes |
| scputils_auto_ban_tollerance  | int | 3 | Tollerance, if the player has commited less suicides than tollerance he won't get kicked or banned even if the percentage is outside of threshold |
| scputils_scp_079_tesla_event_wait  | int | 2 | If 079 trigger tesla for how many seconds player shouldn't get warned for suicide? |
| scputils_auto_ban_threshold | float | 30.5f | Percentage of suicides to trigger a ban (Suicides / Games played as SCP) * 100 |
| scputils_auto_kick_threshold  | float | 15.5f | Percentage of suicides to trigger a kick (Suicides / Games played as SCP) * 100 |

| Admin Commands  | Args | Permission | Description | 
| ------------- | ------------- | ------------- | ------------- |
| scp_utils_info  | none  | All Staff | Show plugin info (All staff) |
| scp_utils_player_info  | player  | scputils.playerinfo |Show player info (scputils.playerinfo perm) |
| scp_utils_player_reset  | player  | scputils.playerreset |Reset warns,suicides,bans,kick and games played stats (scputils.playerreset perm) |

Thanks to iopietro for his advices<br />










