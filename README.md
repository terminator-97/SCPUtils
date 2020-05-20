**SCPUtils Plugin**<br />

Welcome to SCPUtils, this plugin has many features such as welcome messages, decontamination messages, autorestart when only 1 player is present in game and  punishements for the ones that quit / suicide as SCP, temporarily badges, custom nicknames and colors, you can allow specific scps to specific roles to speak to humans using V and much more <br /><br />
Database will get created inside Exiled/SCPUtils folder.<br /><br />
**You must add LiteDB.dll into Plugins/dependencies folder or plugin won't work**<br /><br />
**Required minimum Exiled version: 1.9.10**

### Configs:


| Config Name  | Type | Default Value | Description |
| ------------- | ------------- | ------------- | ------------- |
| scputils_enabled | bool  | true | Enable / disable the entire plugin |
| scputils_enable_round_restart_check  | bool  | true | Enable / disable round restart check if there is only 1 player |
| scputils_enable_scp_suicide_autowarn | bool  | true | Enable / disable suicide autowarn (required for kick and warns) |
| scputils_auto_kick_scp_suicide  | bool  | true | Enable / disable autokick for scp suicides after a certain threshold |
| scputils_quit_equals_suicide  | bool  | true | Should quits be considered as suicide? |
| scputils_welcome_enabled  | bool  | true | Enable / Disabile welcome message when player join |
| scputils_decontamination_message_enabled  | bool  | false | Enable / disable a message when decontamination starts |
| scputils_enable_scp_suicide_auto_ban  | bool  | true |  Enable / disable autoban for scp suicides after a certain threshold |
| scputils_double_ban_duration_each_ban  | bool  | true | Multiply ban duration after each ban |
| scputils_auto_kick_banned_names  | bool  | true | Auto-kick invalid nicknames similiar to the ones specified in scputils_banned_names |
| scputils_welcome_message  | string  | Welcome to the server! | Welcome message, Change it! |
| scputils_decontamination_message  | string  | Decontamination has started! | Decontamination message, Change it! |
| scputils_auto_restart_message  | string  | <color=red>Round Restart:</color>\n<color=yellow>Round will be restarted in {0} seconds due lack of players</color> | Autorestart message, {0} is the amount of seconds |
| scputils_suicide_warn_message  | string  | <color=red>WARN:\nAs per server rules SCP's suicide is an offence, doing it will result in a ban!</color> | Warn message |
| scputils_suicide_kick_message  | string  | Suicide as SCP | Kick message |
| scputils_unauthorized_nickname_change | string  | You can't do that! | Missing permission message |
| scputils_unauthorized_color_change  | string  | You can't do that! | Missing permission message |
| scputils_invalid_nickname_text  | string  | This nickname has been restricted by server owner, please use another nickname! | Invalid nickname text when player changes their own name |
| scputils_database_name  | string  | SCPUtils | Change it only if you run multiple servers, LiteDb doesn't allow multiple server instances on same database |
| scputils_unauthorized_badge_change_visibility | string  | You need a higher administration level to use this command! | Missing permission message |
| scputils_auto_ban_message | string  | Exceeded SCP suicide limit Duration: {0} minutes | Ban message {0} is the ban duration |
| scputils_auto_kick_banned_name_message | string  | You're using a restricted nickname or too similar to a restricted one, please change it |
| scputils_welcome_duration  | int  | 12 | Welcome message duration |
| scputils_decontamination_message_duration | int  | 10 | Decontamination message duration |
| scputils_auto_restart_time  | int  | 15 | After how many seconds round should be restarted if there is only 1 player? |
| scputils_autowarn_message_duration  | int  | 30 | Autowarn message duration |
| scputils_auto_ban_duration  | int | 15 | Autoban duration in minutes |
| scputils_auto_ban_tollerance  | int | 5 | Tollerance, if the player has commited less suicides than tollerance he won't get kicked or banned even if the percentage is outside of threshold |
| scputils_scp_079_tesla_event_wait  | int | 2 | If 079 trigger tesla for how many seconds player shouldn't get warned for suicide? |
| scputils_auto_ban_threshold | float | 30.5f | Percentage of suicides to trigger a ban (Suicides / Games played as SCP) * 100 |
| scputils_auto_kick_threshold  | float | 15.5f | Percentage of suicides to trigger a kick (Suicides / Games played as SCP) * 100 |
| scputils_restricted_role_colors  | list | - | List of restricted colors in .scputils_change_color |
| scputils_banned_names  | list | - | list of banned nicknames |

**Admin commands**

| Admin Commands  | Args | Permission | Description | 
| ------------- | ------------- | ------------- | ------------- |
| scputils_help  | none  | none | Show plugin info |
| scputils_player_info  | player / id / userid  | scputils.playerinfo | Show player info |
| scputils_player_list  | minimun percentage  | scputils.playerlist | List all players with a percetage equal or higher of quits/suicides |
| scputils_player_reset  | player / id / userid  | scputils.playerreset  | Reset warns,suicides,bans,kick and games played stats |
| scputils_player_reset_preferences  | player / id / userid  | scputils.playerresetpreferences  | Reset nickname,badge color,show badge preference |
| scputils_set_color  | <player / id / userid> <color/None>   | scputils.playersetcolor  | Change player color |
| scputils_set_name  | <player / id / userid> <name/None>   | scputils.playersetname  | Change player name, changes take effects next round/rejoin |
| scputils_set_badge  | <player / id / userid> <badge name> <duration in minutes> | scputils.handlebadges | Add a temp player badge |
| scputils_revoke_badge  | <player / id / userid> | scputils.handlebadges | Revoke a badge given to a player |

**Console commands**

| Player Commands  | Args | Permission | Description | 
| ------------- | ------------- | ------------- | ------------- |
| scputils_help  | none  | none | Show plugin commands |
| scputils_info  | none  | none | Show plugin info |
| scputils_change_nickname  | Nickname / None | scputils.changenickname | Change your nickname, changes take effects next round/rejoin |
| scputils_change_color | Color / None | scputils.changecolor | Change your badge color |
| scputils_show_badge  | none  | scputils.badgevisibility | Permanently show your badge |
| scputils_hide_badge  | none | scputils.badgevisibility | Permanently hide your badge |
| scputils_my_info  | none | none | Show your preferences and temporarily badges info |

**Speak permissions**

| Permisssion  | Description | 
| ------------- | ------------- | 
| scputils_speak.scp049  | Allows to speak with V using this scp |
| scputils_speak.scp0492  | Allows to speak with V using this scp |
| scputils_speak.scp079 | Allows to speak with V using this scp |
| scputils_speak.scp096 | Allows to speak with V using this scp |
| scputils_speak.scp106  | Allows to speak with V using this scp |
| scputils_speak.scp173  | Allows to speak with V using this scp |


**Generic permissions**

| Permisssion  | Description | 
| ------------- | ------------- | 
| scputils.bypassnickrestriction | Allows to bypass nickname restrictions |

Pro tip: use scputils_speak.* to allow someone to speak with all the SCPs, set permission to default role to allow everyone to speak with that scp.

Console commands must be executed like .scputils_help in game console (press ò to open it)

**Config Example**

To edit your configs you must go into EXILED folder and edit port-config.yml file (example 7777-config.yml), bellow you can see a sample config<br />

```
scputils_welcome_message: WELCOME TO MY SERVER!
scputils_scp_079_tesla_event_wait: 3
scputils_auto_ban_threshold: 35
scputils_restricted_role_colors: 
 - red
 - magenta
 - pink 
 - cyan
 - lime
 - deep_pink
 - crimson
 - carmine
scputils_banned_names:
 - Admin
 - Nickname1
 - Nickname2
 - Nickname3
```

<br />To edit permissions you must go into Plugins/Exiled Permissions folder and edit permissions.yml file, bellow you can see a sample config<br />

```
groups:
    user:
        default: true
        permissions:
        - scputils_speak.scp049
    owner:
        permissions:
        - '*'
    admin:
        permissions:       
        - scputils.playerinfo
        - scputils.playerlist
        - scputils.changecolor
        - scputils.changenickname
        - scputils.badgevisibility
        - scputils.playerresetpreferences
        - scputils_speak.*      
    vip:
        permissions:        
        - scputils.changecolor
        - scputils.changenickname     
        - scputils_speak.*	  
```		
		
<br />Using scputils.* grants every permission that starts with prefix scputils
Using '*' grants every possible permission on the server<br />
To verify if you yml file is valid paste it into this website: http://www.yamllint.com/<br />
Note: you must add every group in permissionsm don't forget default one<br /><br />

Data stored on database is intended only for internal use, sharing it is a violation of SCP:SL EULA and will cause your server delist.<br /><br />

Thanks to iopietro for his advices<br />