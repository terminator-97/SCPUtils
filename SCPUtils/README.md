**SCPUtils Plugin**

Welcome to SCPUtils, this plugin has many features such as welcome messages, decontamination messages, autorestart when only 1 player is present in game and  punishements for the ones that quit / suicide as SCP

Configs:

| Config Name  | Type | Default Value |
| ------------- | ------------- | ------------- |
| scputils_enable_round_restart_check  | bool  | true |
| scputils_enable_scp_suicide_autowarn | bool  | true |
| scputils_auto_kick_scp_suicide  | bool  | true |
| scputils_quit_equals_suicide  | bool  | true |
| scputils_welcome_enabled  | bool  | true |
| scputils_decontamination_message_enabled  | bool  | false |
| scputils_enable_scp_suicide_auto_ban  | bool  | true |
| scputils_remove_overwatch_round_start  | bool  | false |
| scputils_double_ban_duration_each_ban  | bool  | true |
| scputils_welcome_message  | string  | Welcome to the server! |
| scputils_ondecontamination_message  | string  | Please read server rules! |
| scputils_auto_restart_message  | string  | <color=red>Round Restart:</color>\n<color=yellow>Round will be restarted in " + autoRestartTime + " seconds due lack of players</color> |
| scputils_suicide_warn_message  | string  | <color=red>WARN:\nAs per server rules SCP's suicide is an offence, doing it will result in a ban!</color> |
| scputils_suicide_kick_message  | string  | \n[Kicked(SCPUtils - Suicide as SCP)] |
| scputils_auto_ban_message | string  | \n[Banned(SCPUtils - Exceeded SCP suicide warn limit)] \n Duration: {duration} |
| scputils_welcome_duration  | int  | 12 |
| scputils_decontamination_message_duration | int  | 10 |
| scputils_auto_restart_time  | int  | 15 |
| scputils_autowarn_message_duration  | int  | 30 |
| scputils_auto_ban_duration  | int | 360 |
| scputils_auto_ban_tollerance  | int | 3 |
| scputils_scp_079_tesla_event_wait  | int | 2 |
| scputils_auto_ban_threshold | float | 15.5f |
| scputils_auto_kick_threshold  | float | 5.5f |

| Admin Commands  | Arg | Description | 
| ------------- | ------------- | ------------- |
| scp_utils_info  | none  | Show plugin info (All staff) |
| scp_utils_player_info  | player  | Show player info (All staff) |
| scp_utils_player_reset  | player  | Reset warns,suicides,bans,kick and games played stats (ServerCommands perm) |


**Version: 1.0.0**









