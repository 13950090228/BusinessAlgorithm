using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace BusinessAlgorithm.IO {
    public class AbilityEffectCollect {
        private static string folderPath;
        private static string outputFilePath = @"C:\Users\zyq\Desktop\collected_AbilityEffect.txt";

        /// <summary>
        /// txt 信息映射收集类
        /// </summary>
        public static void CollectAETxt() {
            uint index = 0;
            string[] paths = { Directory.GetCurrentDirectory(), "Assets", "Config", "bin", "AbilityEffect" };
            folderPath = Path.Combine(paths);
            FileStream fs = new FileStream(outputFilePath, FileMode.Create);
            StreamWriter writer = new StreamWriter(fs);
            var aeFiles = CollectAEInDirectory(folderPath);
            foreach (var item in aeFiles) {
                writer.WriteLine($"{index}|{Path.GetFileNameWithoutExtension(item)}");
                index += 1;
            }
            writer.Close();
            Debug.Log("AbilityEffect 映射表收集完成");
        }

        static List<string> CollectAEInDirectory(string directoryPath) {
            var aeFiles = new List<string>();
            foreach (string subDirectoryPath in Directory.GetDirectories(directoryPath)) {
                aeFiles.AddRange(CollectAEInDirectory(subDirectoryPath));
            }
            aeFiles.AddRange(Directory.GetFiles(directoryPath, "*.txt"));
            return aeFiles;
        }

        /// <summary>
        /// AE映射表反向解析
        /// </summary>
        /// <param name="outputFilePath">txt 路径</param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static Dictionary<uint, string> CollectedAETxtKeyIsId(string outputFilePath, ref Dictionary<uint, string> dic) {
            FileStream fs = new FileStream(outputFilePath, FileMode.Open);
            StreamReader reader = new StreamReader(fs);
            string line;
            while ((line = reader.ReadLine()) != null) {
                string[] parts = line.Split('|');
                uint index = uint.Parse(parts[0]);
                string fileName = parts[1];
                dic[index] = fileName;
            }
            reader.Close();
            return dic;
        }
        
        /// <summary>
        /// AE映射表反向解析
        /// </summary>
        /// <param name="outputFilePath">txt 路径</param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static Dictionary<string, uint> CollectedAETxtKeyIsSign(string outputFilePath, ref Dictionary<string, uint> dic) {
            FileStream fs = new FileStream(outputFilePath, FileMode.Open);
            StreamReader reader = new StreamReader(fs);
            string line;
            while ((line = reader.ReadLine()) != null) {
                string[] parts = line.Split('|');
                uint index = uint.Parse(parts[0]);
                string fileName = parts[1];
                dic[fileName] = index;
            }
            reader.Close();
            return dic;
        }
    }
}