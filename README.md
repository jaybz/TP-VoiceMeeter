# TP-VoiceMeeter
VoiceMeeter plugin for Touch Portal

## Known issues
The level states that report dB in decimal form do not work with Touch Portal's less than and higher than conditions as those do not work with numbers with a decimal part. You can use duplicate level states that round off the decimal values to integers.

## Disclaimer
Another VoiceMeeter plugin already exists for Touch Portal. I created this plugin just because I really wanted to ditch another piece of software similar to Touch Portal and the other plugin did not fit my needs entirely. I do not currently intend on continuing development of this plugin if it becomes redundant. That said, I do have other improvements planned including the possibility using new Touch Portal features as they are added.

## Credits
- The integration into Touch Portal is implemented via https://github.com/tlewis17/TouchPortalAPI
- This app uses modified VoiceMeeter C# wrapper code taken from https://github.com/tocklime/VoiceMeeterWrapper/
