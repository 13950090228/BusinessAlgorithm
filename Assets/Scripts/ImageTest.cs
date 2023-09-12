using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Collections.Generic;

public struct PixelColorModel {
    public Color32 color32;
    public bool isContrast;
}

public class ImageEntity {
    public string sign;
    public int width;
    public int height;
    public List<PixelColorModel> models;
    public bool isContrast;

}

public class ImageTest : MonoBehaviour {
    [Header("文件夹A路径")]
    public string FolderPathA = @"C:\Users\pc\Desktop\ImageA";
    [Header("文件夹B路径")]
    public string FolderPathB = @"C:\Users\pc\Desktop\ImageB";
    [Header("导出图片的文件夹路径")]
    public string SameFolder = @"C:\Users\pc\Desktop\SameFolde";

    List<ImageEntity> AFolder = new List<ImageEntity>();
    List<ImageEntity> BFolder = new List<ImageEntity>();

    List<string> SameList = new List<string>();

    [Header("相似像素点位占比")]
    public float similarityThreshold = 0.2f;

    [Header("像素误差")]
    public int pixelDeviation = 0;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            GameStart();
        }
    }

    public void GameStart() {
        Debug.Log("[lyq]开始收集文件夹图片资源");
        AFolder = FolderImageLoad(FolderPathA);
        BFolder = FolderImageLoad(FolderPathB);

        Debug.Log("[lyq]开始计算是否有相似图片");
        for (int i = 0; i < AFolder.Count; i++) {
            for (int j = 0; j < BFolder.Count; j++) {
                if (AFolder[i].isContrast || BFolder[j].isContrast) {
                    continue;
                }
                PixelColorCheck(AFolder[i], BFolder[j], similarityThreshold);
            }
        }

        Debug.Log("[lyq]相似图片文件夹创建");
        SameImageFolderCreate(FolderPathA, SameFolder);
        AFolder.Clear();
        BFolder.Clear();
        SameList.Clear();
    }

    public void SameImageFolderCreate(string folderPathA, string sameFolder) {
        // 检查源文件夹是否存在
        if (Directory.Exists(folderPathA)) {
            // 如果目标文件夹不存在，则创建它
            if (!Directory.Exists(sameFolder)) {
                Directory.CreateDirectory(sameFolder);
            }

            // 获取源文件夹中的所有文件
            string[] files = Directory.GetFiles(folderPathA);

            // 遍历源文件夹中的文件
            foreach (string filePath in files) {
                // 获取文件名
                string fileName = Path.GetFileName(filePath);
                for (int i = 0; i < SameList.Count; i++) {
                    if (SameList[i] == fileName) {
                        // 构建目标文件的完整路径
                        string destinationPath = Path.Combine(sameFolder, fileName);
                        // 复制文件到目标文件夹
                        File.Copy(filePath, destinationPath, true); // 设置为true以允许覆盖同名文件
                        Debug.Log($"[lyq]图片：{fileName}已复制");
                    }
                }
            }

            Debug.Log("[lyq]相似图片文件夹创建成功");
        }
    }

    // 文件夹下图片收集
    public List<ImageEntity> FolderImageLoad(string path) {
        List<ImageEntity> imageEntitiyList = new List<ImageEntity>();

        // 检查文件夹路径是否存在
        if (Directory.Exists(path)) {
            // 获取文件夹中的所有文件
            string[] files = Directory.GetFiles(path);

            // 遍历文件列表
            foreach (string file in files) {
                // 检查文件是否是图片文件（你可以根据需要添加其他格式的检查）
                if (IsImageFile(file)) {
                    // 获取文件名并添加到列表中
                    string imageSign = Path.GetFileName(file);
                    string imagePath = Path.Combine(path, Path.GetFileName(file));
                    Texture2D texture2D = LoadTexture(imagePath);
                    ImageEntity imageEntity = GetTexture2DPixelInfo(texture2D, imageSign);
                    imageEntitiyList.Add(imageEntity);
                }
            }
        } else {
            Debug.LogError("输入的路径不存在: " + path);
        }

        return imageEntitiyList;
    }

    // 获取图片的像素点信息
    public ImageEntity GetTexture2DPixelInfo(Texture2D texture2D, string sign) {
        int width = texture2D.width;
        int height = texture2D.height;

        ImageEntity entity = new ImageEntity();
        entity.sign = sign;
        entity.width = width;
        entity.height = height;
        entity.models = new List<PixelColorModel>();
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                // 获取像素的颜色
                Color32 pixelColor = texture2D.GetPixel(x, y);
                entity.models.Add(new PixelColorModel() {
                    color32 = pixelColor,
                    isContrast = false
                });
            }
        }

        return entity;
    }

    bool IsImageFile(string filePath) {
        string extension = Path.GetExtension(filePath).ToLower();
        return extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".bmp" || extension == ".gif";
    }

    // 两张图片进行像素检测
    public void PixelColorCheck(ImageEntity entityA, ImageEntity entityB, float threshold) {
        if (entityA.width != entityB.width || entityA.height != entityB.height) {
            // SizeUnlikePixelColorCheck(entityA, entityB, threshold);
        } else {
            SizeLikePixelColorCheck(entityA, entityB, threshold);
        }
    }

    // 尺寸相同的图片检测
    public void SizeLikePixelColorCheck(ImageEntity entityA, ImageEntity entityB, float threshold) {
        int width = entityA.width;
        int height = entityA.height;
        float count = height * width;
        float sameCount = 0.0f;

        // 遍历图像的每个像素
        for (int i = 0; i < entityA.models.Count; i++) {
            Color32 colorA = entityA.models[i].color32;
            Color32 colorB = entityB.models[i].color32;

            bool rSame = Mathf.Abs(colorA.r - colorB.r) <= pixelDeviation;
            bool gSame = Mathf.Abs(colorA.g - colorB.g) <= pixelDeviation;
            bool bSame = Mathf.Abs(colorA.b - colorB.b) <= pixelDeviation;
            bool aSame = Mathf.Abs(colorA.a - colorB.a) <= pixelDeviation;
            if (rSame && gSame && bSame && aSame) {
                sameCount += 1;
            }
        }

        if (sameCount / count > similarityThreshold) {
            Debug.Log($"[lyq]这两张图片相似:{entityA.sign}  ,{entityB.sign}");
            entityA.isContrast = true;
            entityB.isContrast = true;
            SameList.Add(entityA.sign);
        }
    }

    public void SizeUnlikePixelColorCheck(ImageEntity entityA, ImageEntity entityB, float threshold) {
        int width = entityB.width;
        int height = entityB.height;
        float count = height * width;
        float sameCount = 0.0f;

        // 遍历图像的每个像素
        for (int i = 0; i < entityA.models.Count; i++) {
            for (int j = 0; j < entityB.models.Count; j++) {
                Color32 colorA = entityA.models[i].color32;
                Color32 colorB = entityB.models[j].color32;

                if (entityB.models[j].isContrast) {
                    continue;
                }

                bool rSame = Mathf.Abs(colorA.r - colorB.r) <= pixelDeviation;
                bool gSame = Mathf.Abs(colorA.g - colorB.g) <= pixelDeviation;
                bool bSame = Mathf.Abs(colorA.b - colorB.b) <= pixelDeviation;
                bool aSame = Mathf.Abs(colorA.a - colorB.a) <= pixelDeviation;
                if (rSame && gSame && bSame && aSame) {
                    sameCount += 1;
                    entityB.models[j] = new PixelColorModel(){
                        color32 = colorB,
                        isContrast = true
                    };
                }
            }
        }

        if (sameCount / count > 0.8f) {
            Debug.Log($"[lyq]这两张图片相似:{entityA.sign}  ,{entityB.sign}");
            entityA.isContrast = true;
            entityB.isContrast = true;
            SameList.Add(entityA.sign);
        }
    }

    private Texture2D LoadTexture(string imagePath) {
        byte[] bytes = File.ReadAllBytes(imagePath);
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(bytes);
        return texture;
    }
}