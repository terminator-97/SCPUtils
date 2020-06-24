**SCPUtils Plugin**<br />

Welcome to SCPUtils, this plugin has many features such as welcome messages, decontamination messages, autorestart when only 1 player is present in game and  punishements for the ones that quit / suicide as SCP, temporarily badges, custom nicknames and colors, you can allow specific scps to specific roles to speak to humans using V and much more <br /><br />
Database will get created inside Exiled/SCPUtils folder.<br /><br />
**You must add LiteDB.dll into Plugins/dependencies folder or plugin won't work**<br /><br />
**Required minimum Exiled version: 2.0.0**

**Currently plugin is in beta phase, documentation is not complete**

### Configs:

Just check configs inside the the file.

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
| scputils_play_time  | <player / id / userid> <range days> | scputils.playtime | Show recent player activity withing the specified days |

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