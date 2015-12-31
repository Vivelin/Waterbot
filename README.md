# Waterbot
Waterbot is a generic Twitch chat bot with no remarkable features as of yet. The bot will respond to greetings, announce when it is entering or leaving a channel, and responds to commands of the following forms:

- `!help`
- `help @username`
- `@username help`

where `username` is the name of the user the bot is connected as.

# Quick start
1. Download the latest release or get the source and build it with Visual Studio;
2. Start `WaterbotServer.exe` with the following arguments:
   - `--user=username`: The username of the Twitch account for the bot to run as;
   - `--key=oauth:xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx`: The OAuth key for the Twitch account which can be generated at <https://twitchapps.com/tmi/>;
   - The channel names to join. If no channels are specified, the bot will run in the channel of the account is it running as.

   `> WaterbotServer --user=username --key=oauth:xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx channel1 channel2 [...]`
3. Press Ctrl+C to leave all channels and disconnect from Twitch chat.
