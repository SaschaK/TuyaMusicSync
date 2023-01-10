# TuyaMusicSync

ONLY FOR PRIVATE USE! No commercial use allowed!
Music Sync WPF Application with Un4seen BASS.dll

This application uses code fragments of the following article from CodeProject.com written by webmaster442 (pieces of analyzer.cs).
https://www.codeproject.com/Articles/797537/Making-an-Audio-Spectrum-analyzer-with-Bass-dll-Cs

![image](https://user-images.githubusercontent.com/4045393/211415698-042d3086-7f9e-49b4-9c4c-8791b24f09b5.png)
I'm using some Nuget-Packages:
  TuyaNet:        https://github.com/ClusterM/tuyanet/
  MahApps.Metro:  https://github.com/MahApps/MahApps.Metro
  
You have to set up a developer account at iot.tuya.com (How-To by ClusterM (TuyaNet):
https://github.com/ClusterM/tuyanet/#how-to-obtain-local-key

Currently it's not possible to use the application w/o a cloud connection, because the local key is missing in the Broadcast provided by the lights.
Please take care that your computer has to be in the same Wifi as the lights are, otherwise the broadcast can't be retrieved.

After you created the developer account and added your SmartLife account (if needed), set up the Tuya Account Region, Access ID, Access secret and any of your device IDs on the left side in the application. You have to restart the application after you type in your credentials.

Choose one of the available Sound devices (output devices). If you want to use a microphone input, change the microphone settings as mentioned on the following link:
https://www.addictivetips.com/windows-tips/output-mic-sound-to-speakers-windows-10

Afterwards you have to generate a color pallet with the count of colors you want (min. 5, max. 40). I think you'll get the best result if you use 20.

Reducing the brightness, generating colors and playing with the delay is also possible if the light show was already started.

On the "Devices" Tab you have to select lights via double click from the available "cloud lights / local lights" box.
It will appear in the "Selected lights" box. If you selected lights in the wrong order, use the up arrow, down arrow or the delete button to correct the order of your selected lights.

![image](https://user-images.githubusercontent.com/4045393/211425014-501d761b-da84-4674-83e6-ccbefbc48f2f.png)

If devices will stay offline, please switch them off, wait a few sec. and turn them on again. They are also offline if you are in the wrong Wifi!
When this not helps, restart the app.

Demo: https://www.youtube.com/watch?v=R1E-YyNcE8s

I use it in combination with an Echo Dot, a Line-In Cable and my laptop's microphone jack .
The Echo Dot is in a Multi-Room Music group.
