
**SCPUtils Plugin**<br />

Welcome to SCPUtils, overtime i implemented many features so i decided to rework the documentation.

This is the list of SCPUtils features with a brief description, i recomend to read configs:

- **Welcome Message:** You can set in server config a welcome broadcast that every player join will see.
- **Decontamination Message:** A broadcast that all players will see when decontamination starts
- **Auto-Restart:** Having only one player in server may cause round stuck forever, with this plugin you can avoid it!
- **Advanced SCP Suicide / Quit punishements:** If a SCP suicide or leave the server you can punish him, with warns, kicks or bans, depending on settings you use and DC/Suicide percentage player has (configurable also for tutorial role)
- **Temporarily badges:** Sometimes may be useful to give a player a temporarily role for event winners and donators (or anything else you want), with this plugin it's easy, see commands list.
- **Custom nicknames:** Admins with permission can set any nickname to any player in server using admin console, users with permission can set their own nickname using a simple command on User's console.
- **Nickname Blacklist:** You can prevent players joining with blacklisted nicknames and preventing them to change nicknames to restricted ones, users with bypassnickname permission bypass this limit!
- **Badge colors:** Admins can assign a color to every person, users with permissions can assign any color to themselves if it's not in restricted list!
- **Permanently show/hide badges:** Users with permission can permanently show or hide badge using a simple command (in user console)
- **SCPSpeak features:** Playing with permissions you can decide which badge (even default one) can speak with that SCP like 939 using V!
- **Playtime statistics:** You can see each user playtime day per day or total playtime using a simple command!
- **ASNs Bans:** You can ban specific ASNs to avoid ban evaders and cheaters, you can whitelist legit users to bypass the ASNs bans using a simple command, to add an ASN to blacklist add it inside server config setting.

**Database will get created inside Exiled/SCPUtils folder.**<br /><br />
**You must add LiteDB.dll into Plugins/dependencies folder or plugin won't work**<br /><br />
**Minimum requirements: Exiled version: 2.0.7 nad LiteDB 5.0.8**
**Currently plugin is in beta phase, documentation is not complete**

### Configs:

You can see settings and edit them inside Exiled/port-config.yml file(example Exiled/7777-config.yml)

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
| scputils_whitelist_asn | <id / userid> | scputils.whitelist | Add player to ASN whitelist |
| scputils_unwhitelist_asn | <id / userid> | scputils.whitelist | Removes player to ASN whitelist |

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
| scputils.help | Show also admin command list in scputils_help command, without this permission only user commands are shown |

Pro tip: use scputils_speak.* to allow someone to speak with all the SCPs, set permission to default role to allow everyone to speak with that scp.

Console commands must be executed like .scputils_help in game console (press ò to open it)

**Config Example**

To edit your configs you must go into EXILED folder and edit port-config.yml file (example 7777-config.yml), and edit them<br />


<br />To edit permissions you must go into Plugins/Exiled Permissions folder and edit permissions.yml file, bellow you can see a sample config<br />

```
    user:
    inheritance: []
        default: true
        permissions:
        - scputils_speak.scp049
    owner:
        permissions:
        - '*'
    admin:
    inheritance: []
        permissions:       
        - scputils.playerinfo
        - scputils.playerlist
        - scputils.changecolor
        - scputils.changenickname
        - scputils.badgevisibility
        - scputils.playerresetpreferences
        - scputils_speak.*      
    vip:
    inheritance: []
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
