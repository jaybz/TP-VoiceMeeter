# TP-VoiceMeeter
VoiceMeeter plugin for Touch Portal

## Disclaimer
Another VoiceMeeter plugin already exists for Touch Portal. I created this plugin just because I really wanted to ditch another piece of software similar to Touch Portal and the other plugin did not fit my needs entirely. I do not currently intend on continuing development of this plugin. This is intended as sample .Net code for other developers.

## Known issues
- The level states that report dB in decimal form do not work with Touch Portal's less than and higher than conditions as those do not work with numbers with a decimal part. You can use duplicate level states that round off the decimal values to integers. There aren't any duplicate states for other decimal states.
- Since TP-VoiceMeeter uses [TouchPortalAPI](https://github.com/tlewis17/TouchPortalAPI), any known issues with that also affect TP-VoiceMeeter.

## Credits
- The integration into Touch Portal uses [TouchPortalAPI](https://github.com/tlewis17/TouchPortalAPI)
- Generating entry.tp performs JSON serialization and deserialization using [Json.NET](https://github.com/JamesNK/Newtonsoft.Json)
- This app uses modified VoiceMeeter C# wrapper code taken from [VoiceMeeter Wrapper and simple client application to interface with nanokontrol 2](https://github.com/tocklime/VoiceMeeterWrapper/)
