# TP-VoiceMeeter
VoiceMeeter plugin for Touch Portal

## Disclaimer
Another VoiceMeeter plugin already exists for Touch Portal. I created this plugin just because I really wanted to ditch another piece of software similar to Touch Portal and the other plugin did not fit my needs entirely. I do not currently intend on continuing development of this plugin if it becomes redundant. That said, I do have other improvements planned including the possibility using new Touch Portal features as they are added.

## Known issues
- The level states that report dB in decimal form do not work with Touch Portal's less than and higher than conditions as those do not work with numbers with a decimal part. You can use duplicate level states that round off the decimal values to integers. There aren't any duplicate states for other decimal states yet and because Touch Portal does not have sliders yet, I don't see any need for it at the moment. I am hesitant to add any more rounded off duplicates as these are just temporary workarounds. If you do have a compelling use case for those, open an issue here on GitHub so we can see if it is worth adding at this point.
- Since TP-VoiceMeeter uses [TouchPortalAPI](https://github.com/tlewis17/TouchPortalAPI), any known issues with that also affect TP-VoiceMeeter.

## Credits
- The integration into Touch Portal is implemented via [TouchPortalAPI](https://github.com/tlewis17/TouchPortalAPI)
- This app uses modified VoiceMeeter C# wrapper code taken from [VoiceMeeter Wrapper and simple client application to interface with nanokontrol 2](https://github.com/tocklime/VoiceMeeterWrapper/)
