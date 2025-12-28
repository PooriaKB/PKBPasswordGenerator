# PKB Password Generator

![App Preview](overview.gif) <!-- We'll make a GIF from your video -->

A **secure, offline random password generator** built with WPF (C#) featuring strong encryption, modern UI, and local vault storage.

## 🎬 Overview Video

Watch the app in action:

https://github.com/user-attachments/assets/4f8398dc-d638-4c71-a741-b256b14f18b1


*(Click to play the demo)*

## ⭐ Features

- Strong random password generation (custom length, characters)
- AES-GCM encrypted vault with PBKDF2 key derivation
- Master password protection
- Secure password reveal/copy (auto-clear clipboard)
- Custom dark theme with draggable title bar
- Vault protected against accidental deletion/modification

## ⬇️ Download

**Latest Version (Windows 10/11)**  
[![Download Latest](https://img.shields.io/badge/Download%20Latest-v1.0.1-blue?style=for-the-badge)](https://github.com/PooriaKB/PKBPasswordGenerator/releases/download/v1.0.1/PKBPasswordGen1.0.1.msi)

> Installer includes desktop shortcut <br/>
> Please uninstall previous versions before installing the new one

## v1.0.1 fixes :
- App closing & opening loading speed
- General optimization

## ✅ Installation

1. Download the `.msi` from the link above
2. Run the installer
3. Launch from Desktop
4. Register your master password on first use

## 🛠️ Building from Source

- Requires Visual Studio 2022/2025 with .NET desktop development workload
- Open `PasswordGenerator.sln`
- Build and run

## 🔐 Security

- Vault stored in `%AppData%\Roaming\PKBPasswordGenerator`
- Encrypted with industry-standard AES-GCM
- No data sent online — 100% offline

## ⚠️ Read With Caution

- Remember your master password! No recovery option.
- Backup your `PKBPasswordGenerator` folder regularly.(Preferably to a cloud storage like Google Drive)
- Don't modify or delete `PKBPasswordGenerator` folder in `%AppData&` manually.
- Since your vault is encrypted no one can access your passwords from anywhere, except from this app by having your master password.AGAIN DON'T FORGET IT!
- You can install the app on multiple devices and use the same vault file by copying it to the same location on other devices but you must have the vault master password.

## 📝 Additional note
- This is my first WPF project & I didn't learn many concepts like MVVM or Data binding before this project so the code structure might not be optimal.
- I tried to keep the dependencies minimal so I didn't use any third party libraries for encryption or UI components.
- The app is tested on Windows 10 and 11 but it should work on older versions of Windows that support WPF.
- There will be improvements and new features in future updates based on user feedback.
- Feel free to open issues or contribute on GitHub!
## License

MIT License — feel free to use and modify.

---

Made with ❤️ by Pooria Kordbacheh
