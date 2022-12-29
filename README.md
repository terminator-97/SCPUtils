

### SCPUtils Plugin:<br />

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
- **Playtime statistics:** You can see each user playtime day per day or total playtime using a simple command!
- **ASNs Bans:** You can ban specific ASNs to avoid ban evaders and cheaters, you can whitelist legit users to bypass the ASNs bans using a simple command, to add an ASN to blacklist add it inside server config setting.
- **Team protection:** Editing configs you can set protection to the teams you want against the teams you want on specific zones or entire map.
- **SCP-096 Target:** Players gets notified via hint when they become a SCP-096 Target.
- **Last Player:** Players gets notified via hint when they are the last player of the Team.
- **Custom Hints / Broadcast:** You can save hints / broadcasts in the database and use them easily by the ID.
- **Multi account detector:** Auto-detects multi accounts and based on plugin settings it informs the administrators, you can also set a webhook for reports about mute evaders (you can also remute them automatically by changing configs), you can also exclude specific asns or players from the detector
- **Handcuff ownership:** By simply updating a config only who cuff the player will be able to uncuff it.
- **SCP-Swap:** By setting a config you can decide the max allowed time for SCP-Swap requests and if the SCP has to be full health for swap to be allowed.

**Database will get created inside Exiled/SCPUtils folder.**<br /><br />
**Each server must have it's own database, you cannot use one database on multiple servers!**<br /><br />
**You must add LiteDB.dll and Newtonsoft.Json.dll into Plugins/dependencies folder or plugin won't work**<br /><br />
**Minimum requirements: Exiled version: 6.0.0 Dependencies: LiteDB 5.0.15 and Newtonsoft.Json 13.0.2**


### Configs:

You can see settings and edit them inside Exiled/port-config.yml file(example Exiled/7777-config.yml)

### Commands

**Admin commands and Game console commands**

| Admin Commands  | Args | Permission | Description | 
| ------------- | ------------- | ------------- | ------------- |
| scputils_help  | none  | scputils.help | Show plugin info |
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
| scputils_staff_list | - | scputils.stafflist | Show both local staff and global staff present in game |
| scputils_enable_suicide_warns | - | scputils.warnmanagement | Enabled previously disabled suicide / quits warns |
| scputils_disable_suicide_warns | - | scputils.warnmanagement | Disable suicides / quits warns for the rest of the round |
| scputils_global_edit | <Total  SCP Games to remove> <Suicides/Quits to remove> <Kicks to remove> <Bans to remove> | scputils.globaledit | Globally edits player stats (removes total scp games/suicides/kicks/bans) |
| scputils_player_edit | <id / userid> <Total  SCP Games> <Suicides/Quits> <Kicks> <Bans>| scputils.playeredit | Edits player stats (total scp games/suicides/kicks/bans) by setting them to specified amount |
| scputils_player_delete | <userid / id> | scputils.playerdelete | Deletes a player from db, action is irreversible, do this when player is not in server. |
| scputils_preference_persist | <userid / id> | scputils.keep | If disabled by config players that lose the permission to change name,color,hide badge will have that setting resetted, by using this command you allow the player to use their preference even without permissions |
| scputils_player_restrict | <userid / id> | scputils.moderatecommands | <duration in minutes (0=permanent) <reason> | You can a specific player from change name and color feature |
| scputils_player_unrestrict | <userid / id> | scputils.moderatecommands | Unban a previously command banned player |
| scputils_show_command_bans | <userid / id> | scputils.moderatecommands | Show command ban history of a specific player |
| scputils_remove_previous_badge | <userid / id> | scputils.handlebadges | Removes previous badge from database for that player |
| scputils_round_info | <userid / id> | See bellow | Show round info |
| scputils_online_list | <userid / id> | See bellow | Show online player list |
| scputils_player_dnt | <userid / id> | scputils.dnt | Ignore DNT requests from a certain player |
| scputils_player_warnings | <userid / id> | scputils.showwarns | Show all scputils warnings of a specific player |
| scputils_player_warning | <userid / id> | scputils.showwarns | Show last scputils warning of a specific player |
| scputils_player_unwarn | <userid / id> <warn id> | scputils.unwarn | Removes a specific warning from a user |
| scputils_player_broadcast | <userid / id> <broadcast/hint> <id> <duration (optional> | scputils.broadcast | Send an hint or broadcast to a specific player |
| scputils_broadcast | <broadcast/hint> <id> <duration (optional>  | scputils.broadcast | Send an hint or broadcast to all players |
| scputils_set_round_ban | <id / userid> <amount> | scputils.roundban  | Sets the number of round ban to one player |
| scputils_dupeip | < id / userid > | scputils.dupeip | Check if player has another account on same IP |
| scputils_broadcast_create | <id> <duration> <text> | scputils.broadcastcreate | Create a custom broadcast |
| scputils_broadcast_delete | <id> | scputils.broadcastdelete | Delete a custom broadcast |
| scputils_broadcast_list | none | scputils.broadcastlist | List all created broadcast |
| scputils_multiaccount_whitelist | <userid / id> | scputils.whitelistma | Whitelists / unwhitelist an account from multiaccount detector |
| scputils_badge_playtime | <badge> <days> | scputils.playtime | Show playtime for a specific badge |

**Console commands**

| Player Commands  | Args | Permission | Description | 
| ------------- | ------------- | ------------- | ------------- |
| scputils_help  | none  | none | Show plugin commands |
| scputils_info  | none  | none | Show plugin info |
| scputils_permissions_view  | none  | none | Show your plugin permissions |
| scputils_change_nickname  | Nickname / None | scputils.changenickname | Change your nickname, changes take effects next round/rejoin |
| scputils_change_color | Color / None | scputils.changecolor | Change your badge color |
| scputils_show_badge  | none  | scputils.badgevisibility | Permanently show your badge |
| scputils_hide_badge  | none | scputils.badgevisibility | Permanently hide your badge |
| scputils_my_info  | none | none | Show your preferences and temporarily badges info |
| scputils_play_time | none | scputils.ownplaytime | Show your own playtime with a max range of 120 days |
| scputils_swap_request (configurable) | <player> | none | Sends a SCP swap request
| scputils_swap_request_accept (configurable) | none | none | Accept a SCP swap request
| scputils_swap_request_deny (configurable) | none | none | Deny a SCP swap request
| scputils_swap_request_cancel (configurable) | none | none | Cancel a SCP swap request
| scputils_scp_list  | none | Requires to be scp and swap module enabled | Show SCP list

**Generic permissions**

| Permisssion  | Description | 
| ------------- | ------------- | 
| scputils.bypassnickrestriction | Allows to bypass nickname restrictions |
| scputils.help | Show also admin command list in scputils_help command, without this permission only user commands are shown |

Pro tip: use scputils_speak.* to allow someone to speak with all the SCPs, set permission to default role to allow everyone to speak with that scp.

Console commands must be executed like .scputils_help in game console (press ò to open it)

**Round Info**

This command allows to show advanced round info and users are able to use it by default to see informations about their own team (using user console), if you don't like that just edit configs.
It has also the following permissions (those bypass server config):

| Permisssion  | Description | 
| ------------- | ------------- | 
| scputils.roundinfo.execute | Needed to simply executing the command, doesnt show any info |
| scputils.roundinfo.roundtime | Show roundtime |
| scputils.roundinfo.tickets | Show tickets of all teams |
| scputils.roundinfo.nextrespawnteam | Show which team respawn next and when |
| scputils.roundinfo.respawncount | Show how many times MTF/Chaos respawned |
| scputils.roundinfo.lastrespawn | Show when MTF/Chaos respawned |

**Online List**

This command show online player list, it's also usable by users on normal console, it uses advanced permissions like past command.


| Permisssion  | Description | 
| ------------- | ------------- | 
| scputils.onlinelist.basic | Allows to execute the command, see total players online and player nicknames |
| scputils.onlinelist.userid | Show UserIDs |
| scputils.onlinelist.badge | Show badges (hidden or not) |
| scputils.onlinelist.role | Show roles |
| scputils.onlinelist.health | Show players health |
| scputils.onlinelist.flags | Show player flags (God, DNT, Muted etc...) |


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
        - scputils.*
		- scputils_speak.*
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

**Team protection**

ClassD = Class-D  <br />
ChaosInsurgency = Chaos Insurgency <br />
FoundationForces = MTF Team (included guards) <br />
Dead = Spectators <br />
Scientists = Scientist <br />
SCPs = SCP <br />
Tutorial = Tutorial <br />

*Sample Config for team protection:*<br />

In the following example we will protect Class-D against MTF team and Scientist, since they are listed on cuffed_protected_teams they require to be handcuffed to be protected. They also must be in Entrance or Surface to get protection. <br />

```
  # You have to add the team you want to protect from the target as key and enemy teams on the list as value, on github documentation you can see all the teams.
  cuffed_immunity_players:
    CDP:
    - MTF
    - RSC
  # Indicates if the protected teams should be cuffed to get the protection, if you don't add a team it will get protection regardless
  cuffed_protected_teams:
  - CDP
  # Indicates in which zones the protected team is protected, Zone list: Surface, Entrance, HeavyContainment, LightContainment, Unspecified }
  cuffed_safe_zones:
    CDP:
    - Entrance
    - Surface
```

<br />Using scputils.* grants every permission that starts with prefix scputils
Using '*' grants every possible permission on the server<br />
To verify if you yml file is valid paste it into this website: http://www.yamllint.com/<br />
Note: you must add every group in permissionsm don't forget default one<br /><br />

Data stored on database is intended only for internal use, sharing it is a violation of SCP:SL EULA and will cause your server delist.<br /><br />

Thanks to iopietro and Exiled community for the advices<br />
