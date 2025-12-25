using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Runtime.Versioning;

namespace DataAccess.Models
{
    public class Vault
    {
        public byte[] derivedKey;
        public byte[] salt;

        public PasswordDataAccess currentVaultPasswordsAccess = new PasswordDataAccess();


        #region VaultRelatedMethods

        public void CreateVault(string masterPassword, string vaultFilePath)
        {

            this.salt = GenerateRandomSalt(32);
            this.derivedKey = DeriveKey(masterPassword, salt, 100000, 32);
            SaveEmptyVault(derivedKey, currentVaultPasswordsAccess.Passwords, vaultFilePath);




        }
        public void SaveEmptyVault(byte[] derivedKey, List<Password> passwords, string vaultFilePath)
        {

            byte[] jsonSerializedPasswords = SerializePasswordsToByte(passwords);

            byte[] encryptedBlob = EncryptPasswords(jsonSerializedPasswords, derivedKey);
            // Prepares final file buffer
            byte[] fileContent = new byte[salt.Length + encryptedBlob.Length];
            // Combine salt with encryptedBlob
            Buffer.BlockCopy(salt, 0, fileContent, 0, salt.Length);
            Buffer.BlockCopy(encryptedBlob, 0, fileContent, salt.Length, encryptedBlob.Length);

            File.WriteAllBytesAsync(vaultFilePath, fileContent);

            // Apply ACL protections 
            ApplyFileProtections(vaultFilePath);

        }
        public void SaveVault(byte[] derivedKey, List<Password> passwords, string vaultFilePath)
        {
            // Temporarily remove read-only for writing
            FileAttributes originalAttributes = File.GetAttributes(vaultFilePath);
            File.SetAttributes(vaultFilePath, originalAttributes & ~FileAttributes.ReadOnly);

            byte[] jsonSerializedPasswords = SerializePasswordsToByte(passwords);

            byte[] encryptedBlob = EncryptPasswords(jsonSerializedPasswords, derivedKey);
            // Prepares final file buffer
            byte[] fileContent = new byte[salt.Length + encryptedBlob.Length];
            // Combine salt with encryptedBlob
            Buffer.BlockCopy(salt, 0, fileContent, 0, salt.Length);
            Buffer.BlockCopy(encryptedBlob, 0, fileContent, salt.Length, encryptedBlob.Length);

            File.WriteAllBytesAsync(vaultFilePath, fileContent);

            // Re-apply ACL protections (in case they changed)
            ApplyFileProtections(vaultFilePath);




        }
        public List<Password> UnlockVault(string masterPassword, string vaultFilePath, out byte[] derivedKey)
        {
            // Check if file exists
            if (!File.Exists(vaultFilePath))
                throw new FileNotFoundException("Vault file not found.", vaultFilePath);

            // Read the entire vault file
            byte[] fileContent = File.ReadAllBytes(vaultFilePath);

            // Extract the salt (first 32 bytes)
            if (fileContent.Length < 32)
                throw new CryptographicException("Vault file is corrupted or too small.");

            byte[] salt = new byte[32];
            Buffer.BlockCopy(fileContent, 0, salt, 0, 32);

            // Derive the key using the user's master password + stored salt
            this.derivedKey = DeriveKey(masterPassword, salt, 100000, 32);  // same parameters as CreateVault
            derivedKey = this.derivedKey;

            // Extract the encrypted blob (everything after the salt)
            byte[] encryptedBlob = new byte[fileContent.Length - 32];
            Buffer.BlockCopy(fileContent, 32, encryptedBlob, 0, encryptedBlob.Length);

            try
            {
                // Decrypt the blob
                byte[] decryptedJsonBytes = DecryptPasswords(encryptedBlob, derivedKey);

                // Deserialize back to real password objects
                List<Password> passwords = DeserializePasswords(decryptedJsonBytes);

                // Store salt in the instance for future saves
                this.salt = salt;

                return passwords;
            }
            catch (CryptographicException)
            {
                // This catches wrong password OR tampered file
                throw new CryptographicException("Wrong master password or vault file is corrupted.");
            }
        }
        #endregion


        #region DeriveKey&SaltRelatedMethods

        public byte[] GenerateRandomSalt(int size)
        {
            byte[] salt = new byte[size];
            RandomNumberGenerator.Fill(salt); // Cryptographical secure random bytes
            return salt;
        }
        public byte[] DeriveKey(string masterPassword, byte[] salt, int iterations, int keySize)
        {
            // Use the static Pbkdf2 method to avoid SYSLIB0060
            return Rfc2898DeriveBytes.Pbkdf2(
                password: masterPassword,
                salt: salt,
                iterations: iterations,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: keySize
            );
        }
        #endregion

        #region PasswordEncryption&DecryptionRelatedMethods

        public byte[] SerializePasswordsToByte(List<Password> passwords)
        {
            var jsonPasswordsString = JsonSerializer.Serialize(passwords);
            var jsonPasswordsBytes = System.Text.Encoding.UTF8.GetBytes(jsonPasswordsString);
            return jsonPasswordsBytes;

        }
        public byte[] EncryptPasswords(byte[] plainData, byte[] key)
        {
            // generate a fresh radom 12 byte nonce "Number Used Once"
            byte[] nonce = new byte[12];
            RandomNumberGenerator.Fill(nonce);

            // Ciphertext will be same legnth as plaintext
            byte[] ciphertext = new byte[plainData.Length];

            // Authentication tag is always 16 bytes in AES-GCM
            byte[] tag = new byte[16];

            // Perform the encryption using AES-GCM
            using (var aes = new AesGcm(key))
            {
                aes.Encrypt(nonce, plainData, ciphertext, tag);
            }

            // Combine everything into one byte array to save
            byte[] encryptedResult = new byte[nonce.Length + ciphertext.Length + tag.Length];
            // Copy nonce then ciphertext then tag in order
            Buffer.BlockCopy(nonce, 0, encryptedResult, 0, nonce.Length);
            Buffer.BlockCopy(ciphertext, 0, encryptedResult, nonce.Length, ciphertext.Length);
            Buffer.BlockCopy(tag, 0, encryptedResult, nonce.Length + ciphertext.Length, tag.Length);

            return encryptedResult;



        }
        public byte[] DecryptPasswords(byte[] encryptedDataWithNonceAnTag, byte[] key)
        {
            if (encryptedDataWithNonceAnTag == null || encryptedDataWithNonceAnTag.Length <= 28)
            {
                throw new CryptographicException("Ecnrypted data is too shor or cropted");
            }

            // Extract the 12-byte nonce from the beginning
            byte[] nonce = new byte[12];
            Buffer.BlockCopy(encryptedDataWithNonceAnTag, 0, nonce, 0, 12);
            // Extract the authentication tag from last 16 bytes
            byte[] tag = new byte[16];
            Buffer.BlockCopy(encryptedDataWithNonceAnTag, encryptedDataWithNonceAnTag.Length - 16, tag, 0, 16);
            // Extract the actual ciphertext 
            int cipherTextLength = encryptedDataWithNonceAnTag.Length - tag.Length - nonce.Length;
            byte[] cipherText = new byte[cipherTextLength];
            Buffer.BlockCopy(encryptedDataWithNonceAnTag, nonce.Length, cipherText, 0, cipherTextLength);
            // The output buffer
            byte[] plainData = new byte[cipherTextLength];
            // The decryption
            using (var aes = new AesGcm(key))
            {
                aes.Decrypt(nonce, cipherText, tag, plainData);
            }

            return plainData;
        }
        public List<Password> DeserializePasswords(byte[] passwords)
        {
            string jsonPasswordsString = Encoding.UTF8.GetString(passwords);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            return JsonSerializer.Deserialize<List<Password>>(jsonPasswordsString, options)!;
        }
        #endregion

        #region FileProtectionsRelatedMethods
        [SupportedOSPlatform("windows")]
        private void ApplyFileProtections(string vaultFilePath)
        {
            // Set read-only attribute (prevents accidental modification)
            File.SetAttributes(vaultFilePath, FileAttributes.ReadOnly);

            // Set ACL to deny delete for current user (prevents accidental deletion)
            var fileInfo = new FileInfo(vaultFilePath);
            var security = fileInfo.GetAccessControl();

            // Change ownership to Administrators (requires app to run elevated or use a helper)
            var adminsSid = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);
            security.SetOwner(adminsSid);

            // Remove existing user permissions
            var currentUserSid = WindowsIdentity.GetCurrent().User;
            security.PurgeAccessRules(currentUserSid);

            // Grant current user read/write (for app to save), but NOT delete
            security.AddAccessRule(new FileSystemAccessRule(
                currentUserSid,
                FileSystemRights.Read | FileSystemRights.Write,
                AccessControlType.Allow));

            // Deny delete explicitly for current user
            security.AddAccessRule(new FileSystemAccessRule(
                currentUserSid,
                FileSystemRights.Delete,
                AccessControlType.Deny));

            // Allow full control for Administrators group (so admins can delete if needed)
            var admins = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);
            security.AddAccessRule(new FileSystemAccessRule(
                admins,
                FileSystemRights.FullControl,
                AccessControlType.Allow));

            fileInfo.SetAccessControl(security);
        }
        #endregion
    }
}
