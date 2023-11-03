using System.Collections.Generic;
using System.Xml;
using System;
using UnityEngine;

namespace BusinessAlgorithm.IO {
    /// <summary>
    /// SoundbanksInfo 读取类
    /// </summary>
    public static class SoundbanksInfoXmlHelper {
        static XmlDocument xmlDoc;
        static string xmlPath;

        // 外部读取路径（仅在 PC 上用）
        // string[] paths = { Directory.GetCurrentDirectory(), "Assets", "Sound", "Audio", "GeneratedSoundBanks", "Windows", "SoundbanksInfo.xml" };
        // string fullpath = Path.Combine(paths);

        /// <summary>
        /// 设置文件路径
        /// </summary>
        /// <param name="path"></param>
        public static void SetXmlPath(string path) {
            xmlPath = path;
        }

        /// <summary>
        /// 读取 Xml 文件进内存
        /// </summary>
        public static void ReadXml() {
            if (!CheckPathIsNull()) {
                xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlPath);
            }
        }

        static bool CheckPathIsNull() {
            if (xmlPath == null) {
                Debug.LogError("请先设置文件路径: SetXmlPath");
                return true;
            }

            return false;
        }


        /// <summary>
        /// 加载全局基础 bnk 信息，key 为 id
        /// </summary>
        /// <param name="bnks">bnk 字符串数组</param>
        /// <param name="dic">外部字典引用</param>
        public static void LoadBaseBnkInfoKeyIsId(string[] bnks, ref Dictionary<uint, string> dic) {
            if (CheckPathIsNull()) {
                return;
            }

            XmlNode xmlRoot = xmlDoc.DocumentElement;
            foreach (XmlNode node1 in xmlRoot.SelectNodes("SoundBanks/SoundBank")) {
                string bnkSign = node1.SelectSingleNode("Path").InnerText.Replace(".bnk", "");
                if (Array.IndexOf(bnks, bnkSign) != -1) {
                    foreach (XmlNode node2 in node1.SelectNodes("IncludedEvents/Event")) {
                        dic[Convert.ToUInt32(node2.Attributes["Id"].Value)] = node2.Attributes["Name"].Value;
                    }
                }
            }
        }

        /// <summary>
        /// 加载全局基础 bnk 信息，key 为 sign
        /// </summary>
        /// <param name="bnks">bnk 字符串数组</param>
        /// <param name="dic">外部字典引用</param>
        public static void LoadBaseBnkInfoKeyIsSign(string[] bnks, ref Dictionary<string, uint> dic) {
            if (CheckPathIsNull()) {
                return;
            }

            XmlNode xmlRoot = xmlDoc.DocumentElement;
            foreach (XmlNode node1 in xmlRoot.SelectNodes("SoundBanks/SoundBank")) {
                string bnkSign = node1.SelectSingleNode("Path").InnerText.Replace(".bnk", "");
                if (Array.IndexOf(bnks, bnkSign) != -1) {
                    foreach (XmlNode node2 in node1.SelectNodes("IncludedEvents/Event")) {
                        dic[node2.Attributes["Name"].Value] = Convert.ToUInt32(node2.Attributes["Id"].Value);
                    }
                }
            }
        }

        /// <summary>
        /// 全量加载所有 bnk 信息，key 为 id
        /// </summary>
        /// <param name="dic">外部字典引用</param>
        public static void LoadAllBnkInfoKeyIsId(ref Dictionary<uint, string> dic) {
            if (CheckPathIsNull()) {
                return;
            }

            XmlNode xmlRoot = xmlDoc.DocumentElement;
            foreach (XmlNode node1 in xmlRoot.SelectNodes("SoundBanks/SoundBank/IncludedEvents")) {
                foreach (XmlNode node2 in node1.SelectNodes("Event")) {
                    dic[Convert.ToUInt32(node2.Attributes["Id"].Value)] = node2.Attributes["Name"].Value;
                }
            }
        }

        /// <summary>
        /// 全量加载所有 bnk 信息，key 为 sign
        /// </summary>
        /// <param name="dic">外部字典引用</param>
        public static void LoadAllBnkInfoKeyIsSign(ref Dictionary<string, uint> dic) {
            if (CheckPathIsNull()) {
                return;
            }

            XmlNode xmlRoot = xmlDoc.DocumentElement;
            foreach (XmlNode node1 in xmlRoot.SelectNodes("SoundBanks/SoundBank/IncludedEvents")) {
                foreach (XmlNode node2 in node1.SelectNodes("Event")) {
                    dic[node2.Attributes["Name"].Value] = Convert.ToUInt32(node2.Attributes["Id"].Value);
                }
            }
        }
    }
}