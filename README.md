# TP-VoiceMeeter
VoiceMeeter plugin for Touch Portal

## Known issues
The level states that report dB in decimal form do not work with Touch Portal's less than and higher than conditions as those do not work with numbers with a decimal part. You can use duplicate level states that round off the decimal values to integers.

## Disclaimer
Another VoiceMeeter plugin already exists for Touch Portal. I created this plugin just because I couldn't wait for the developer of the other plugin to implement things that I needed. I currently have no intention of continuing development of this plugin once the other plugin becomes sufficient for what I need. That said, I do have other improvements planned including the possibility of leveraging new Touch Portal features as they are added.

## Credits
I am using VoiceMeeter C# wrapper code which is a modified version of code taken from https://github.com/tocklime/VoiceMeeterWrapper/
