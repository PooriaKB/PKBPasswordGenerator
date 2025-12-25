# PKB Password Generator

![App Preview](overview.gif) <!-- We'll make a GIF from your video -->

A **secure, offline password manager** built with WPF (C#) featuring strong encryption, modern UI, and local vault storage.

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
[![Download Latest](https://img.shields.io/badge/Download%20Latest-v1.0.0-blue?style=for-the-badge)](https://github.com/PooriaKB/PKBPasswordGenerator/releases/latest/download/PKBPasswordGenerator.Setup.msi)

> Installer includes desktop shortcut

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


## License

MIT License — feel free to use and modify.

---

Made with ❤️ by Pooria Kordbacheh
