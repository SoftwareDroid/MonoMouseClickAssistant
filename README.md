# MonoMouseClickAssistant
Prototyp for an mouse click assistant in mono. If you need an animation use: https://github.com/SoftwareDroid/Mouse_Assistant_QT


The goal of this project was to rebuild the click assistant of the Gnome desktop (see Settings/Accessibility) in a platform independent way.  After looking at the corresponding source code, it could not be ported trivially (gtk with C implementation in the desktop).  For this reason I rewrote it.

 For this I chose three different programming languages to decide which of the prototypes is best:

https://github.com/SoftwareDroid/MonoMouseClickAssistant/
https://github.com/SoftwareDroid/Mouse_Assistant_QT
https://github.com/SoftwareDroid/RustMouseClickAssistant


Rust had the disadvantage that a GUI animation was not so easy.  With Mono the GUI animation did not behave the same under Windows and Linux.  Only the implementation with Python and QT met all my requirements, so now the click-assistant feels like a copy.
