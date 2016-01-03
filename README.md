# Waterbot
Waterbot is a generic Twitch chat bot with no remarkable features as of yet. The bot will respond to greetings, announce when it is entering or leaving a channel, and responds to commands of the following forms:

- `!help`
- `help @username`
- `@username help`

where `username` is the name of the user the bot is connected as.

# Quick start
1. Download the latest release or get the source and build it with Visual Studio;
2. Start `WaterbotServer.exe` from a command prompt. This should tell you that a default configuration has been created. Open this file in a text editor and modify at least the following values:

   - `UserName`: The username of the Twitch account for the bot to run as;
   - `OAuthToken`: The OAuth token for the Twitch account which can be generated at <https://twitchapps.com/tmi/>;
   - `DefaultChannels`: The channels to join upon starting Waterbot. Waterbot will always join the its own channel (specified by `UserName`) and any channels specified on the command line.

3. Start `WaterbotServer.exe` again. It should connect to Twitch and join the channels you specified;
4. Press Ctrl+C to leave all channels and disconnect from Twitch chat.
