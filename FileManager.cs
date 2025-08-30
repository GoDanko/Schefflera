using System.IO;
using System.Runtime.CompilerServices;
using WithUserCommunication;

namespace FileManager {
    class FileHandler {
        internal string Path;
        internal string FileName;
        public FileHandler(string path, string fileName) {
            Path = path;
            FileName = fileName;
        }

        internal static FileHandler? NewFile(string targetPath, bool forceDirectoryCreation = false) {

            if (EvaluatePathIntegrity(targetPath, forceDirectoryCreation) == false) { return null; }
            else {
                FileHandler createdFile = new FileHandler(targetPath, FindPathsName(targetPath));
                
                try {
                    using (File.Create(targetPath)) { };
                } catch {
                    Manifest.Call(new List<string> {$"Couldn't create File at: {targetPath}"});
                }

                return createdFile;
            }

        }

        static bool EvaluatePathIntegrity(string targetPath, bool fillMissingDirectories) {

            string directoryPath = "";
            for (int i = 0; i < targetPath.Length; i++) {

                if (targetPath[i] == '\\' || targetPath[i] == '/' && !Directory.Exists(directoryPath)) {

                    if (fillMissingDirectories) {
                        try {
                            Directory.CreateDirectory(directoryPath);
                        } catch {
                            Manifest.Call(new List<string> {$"Couldn't create Directory at: {directoryPath}"});
                            return false;
                        }

                    } else {
                        return false;
                    }
                }

                directoryPath += targetPath[i];
            }
            return true;
        }

        static string FindPathsName(string targetPath) {

            string reversedName = "";
            for (int i = targetPath.Length - 1; i >= 0; i--) {
                if (targetPath[i] != '\\' && targetPath[i] != '/') { reversedName += targetPath[i]; }
                else { break; }
            }

            string result = "";
            for (int i = reversedName.Length - 1; i >= 0; i--) { 
                result += reversedName[i];
            }

            return result;
        }

        internal bool Write(string content) {

            try {
                using (StreamWriter fileAccess = new StreamWriter(Path)) {
                    fileAccess.Write(content);
                }
                return true;

            } catch {
                Manifest.Call(new List<string> {$"File {FileName} couldn't be accessed"});
                return false;
            }
        }
    }
}