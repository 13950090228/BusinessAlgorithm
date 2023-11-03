using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace BusinessAlgorithm.IO {
    /// <summary>
    /// 预制体收集类
    /// </summary>
    public static class PrefabCollector {
        private static string folderPath;
        private static string[] folderNames = { "Character", "Indicator", "Monster", "Summon" };    // 收集文件夹需自定义
        private static string outputFilePath = @"C:\Users\zyq\Desktop\collected_prefabs.txt";       // 映射表路径需自定义

        /// <summary>
        /// 预制体信息映射收集类
        /// </summary>
        public static void CollectPrefabs() {
            uint index = 0;
            string[] paths = { Directory.GetCurrentDirectory(), "Assets", "Art", "Product", "Prefab", "VFX" };  // 相对路径文件夹需自定义
            folderPath = Path.Combine(paths);
            FileStream fs = new FileStream(outputFilePath, FileMode.Create);
            StreamWriter writer = new StreamWriter(fs);
            foreach (string folderName in folderNames) {
                string folderPathWithName = Path.Combine(folderPath, folderName);
                var prefabFiles = CollectPrefabsInDirectory(folderPathWithName);
                foreach (var item in prefabFiles) {
                    writer.WriteLine($"{index}|{Path.GetFileNameWithoutExtension(item)}");
                    index += 1;
                }
            }
            writer.Close();
            Debug.Log("预制体映射表收集完成");
        }

        static List<string> CollectPrefabsInDirectory(string directoryPath) {
            var prefabFiles = new List<string>();
            foreach (string subDirectoryPath in Directory.GetDirectories(directoryPath)) {
                prefabFiles.AddRange(CollectPrefabsInDirectory(subDirectoryPath));
            }
            prefabFiles.AddRange(Directory.GetFiles(directoryPath, "*.prefab"));
            return prefabFiles;
        }

        /// <summary>
        /// 预制体映射表反向解析
        /// </summary>
        /// <param name="outputFilePath">txt 路径</param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static Dictionary<uint, string> CollectedPrefabsTxt(string outputFilePath, ref Dictionary<uint, string> dic) {
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
    }
}
