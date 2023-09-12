using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System.IO;

public class ImageTest : MonoBehaviour {
    public string imagePathA = @"D:\Users\PC3070\Desktop\Image\imageA.png";
    public string imagePathB = @"D:\Users\PC3070\Desktop\Image\imageB.png";

    public float similarityThreshold = 0.2f;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("开始计算");

            Texture2D textureA = LoadTexture(imagePathA);
            Texture2D textureB = LoadTexture(imagePathB);

            // bool areImagesSimilar = AreImagesSimilar(textureA, textureB, similarityThreshold);
            PixelColorReader(textureA, textureB, similarityThreshold);
            Debug.Log("结束计算");

        }
    }

    private void PixelColorReader(Texture2D imageA, Texture2D imageB, float threshold) {
        if (imageA.width != imageB.width || imageA.height != imageB.height) {
            Debug.LogError("图像尺寸不同");
        }

        int width = imageA.width;
        int height = imageA.height;
        int count = height * width;
        int sameCount = 0;
        // 遍历图像的每个像素
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                // 获取像素的颜色
                Color32 pixelColorA = imageA.GetPixel(x, y);
                Color32 pixelColorB = imageB.GetPixel(x, y);
                // Debug.Log($"pixelColorA:{pixelColorA},pixelColorB:{pixelColorB},x:{x}.y:{y}");

                if (pixelColorA.Equals(pixelColorB)) {
                    sameCount += 1;
                }
            }
        }
        Debug.Log($"sameCount:{sameCount}");
        if (sameCount / count > similarityThreshold) {
            Debug.Log("这两张图片相似");
        } else {
            Debug.Log("这两张图片不相似");
        }
    }

    private Texture2D LoadTexture(string imagePath) {
        byte[] bytes = File.ReadAllBytes(imagePath);
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(bytes);
        return texture;
    }

    private bool AreImagesSimilar(Texture2D imageA, Texture2D imageB, float threshold) {
        if (imageA.width != imageB.width || imageA.height != imageB.height) {
            return false; // 图像尺寸不同，不相似
        }

        Color[] pixelsA = imageA.GetPixels();
        Color[] pixelsB = imageB.GetPixels();

        int differentPixelCount = 0;

        for (int i = 0; i < pixelsA.Length; i++) {
            // 仅比较阿尔法值
            float alphaA = pixelsA[i].a;
            float alphaB = pixelsB[i].a;
            Debug.Log("pixelsA[i]:" + pixelsA[i]);
            Debug.Log("pixelsB[i]:" + pixelsB[i]);

            if (Mathf.Abs(alphaA - alphaB) > threshold) {
                differentPixelCount++;
            }
        }

        // 计算不同像素的百分比
        float differencePercentage = (float)differentPixelCount / pixelsA.Length;

        // 如果不同像素的百分比低于阈值，则图像相似
        return differencePercentage <= threshold;
    }
}