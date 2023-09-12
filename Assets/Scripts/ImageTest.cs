using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Collections.Generic;

public class ImageEntity {
    public string sign;
    public int width;
    public int height;
    public List<Color32> color32s;
    public bool isContrast;

}

public class ImageTest : MonoBehaviour {
    public string FolderPathA = @"C:\Users\pc\Desktop\ImageA";
    public string FolderPathB = @"C:\Users\pc\Desktop\ImageB";
    public string SameFolder = @"C:\Users\pc\Desktop\SameFolde";

    List<ImageEntity> AFolder = new List<ImageEntity>();
    List<ImageEntity> BFolder = new List<ImageEntity>();

    List<string> SameList = new List<string>();

    public float similarityThreshold = 0.2f;

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
                PixelColorReader(AFolder[i], BFolder[j], similarityThreshold);
            }
        }

        Debug.Log("[lyq]相似图片文件夹创建");
        SameImageFolderCreate(FolderPathA, SameFolder);
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
        entity.color32s = new List<Color32>();
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                // 获取像素的颜色
                Color32 pixelColor = texture2D.GetPixel(x, y);
                entity.color32s.Add(pixelColor);
            }
        }

        return entity;
    }

    bool IsImageFile(string filePath) {
        string extension = Path.GetExtension(filePath).ToLower();
        return extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".bmp" || extension == ".gif";
    }

    // 两张图片进行像素检测
    public void PixelColorReader(ImageEntity entityA, ImageEntity entityB, float threshold) {
        if (entityA.width != entityB.width || entityA.height != entityB.height) {
            return;
        }

        int width = entityA.width;
        int height = entityA.height;
        float count = height * width;
        float sameCount = 0.0f;

        // 遍历图像的每个像素
        for (int i = 0; i < entityA.color32s.Count; i++) {
            Color32 colorA = entityA.color32s[i];
            Color32 colorB = entityB.color32s[i];

            bool rSame = Mathf.Abs(colorA.r - colorB.r) < 5;
            bool gSame = Mathf.Abs(colorA.g - colorB.g) < 5;
            bool bSame = Mathf.Abs(colorA.b - colorB.b) < 5;
            bool aSame = Mathf.Abs(colorA.a - colorB.a) < 5;
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

    private Texture2D LoadTexture(string imagePath) {
        byte[] bytes = File.ReadAllBytes(imagePath);
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(bytes);
        return texture;
    }
}