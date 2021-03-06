﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using Team.Infrastructure.IRepositories;
using Team.Model;

namespace Team.Infrastructure.Repositories
{
    public class ImagesResource : ControllerBase,IImagesResource
    {
        /// <summary>
        /// 图片格式
        /// </summary>
        public static string[] LimitPictureType =
            {".PNG", ".JPG", ".JPEG", ".BMP", ".ICO"};

        /// <summary>
        /// 加载图片
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public FileContentResult LoadingPhoto(string path,int name)
        {
            path = Directory.GetCurrentDirectory() + path + name + ".jpeg";
            FileInfo fi=new FileInfo(path);
            if (!fi.Exists)
            {
                return null;
            }
            FileStream fs = fi.OpenRead();
            byte[] buffer=new byte[fi.Length];
            //读取图片字节流
            fs.Read(buffer, 0, Convert.ToInt32(fi.Length));

            var resource =  File(buffer, "image/jpeg");
            fs.Close();
            return resource;
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="path">路劲</param>
        /// <param name="name">图片名字</param>
        /// <returns></returns>
        public CustomStatusCode UpLoadPhoto(IFormFile formFile, string path, object name)
        {
            CustomStatusCode code;
            var currentPictureExtension = Path.GetExtension(formFile.FileName).ToUpper();
            var imagePath = Directory.GetCurrentDirectory() + path;
            
            if (LimitPictureType.Contains(currentPictureExtension))
            {
                if (Directory.Exists(imagePath) == false)
                {
                    Directory.CreateDirectory(imagePath);
                }

                name = name + ".jpeg";
                imagePath +=name;
                using (var fs = System.IO.File.Create(imagePath))
                {
                    formFile.CopyTo(fs);
                    fs.Flush();
                }
                //_logger.LogInformation($"图片 {name.ToString()} 上传成功");
                code = new CustomStatusCode
                {
                    Status = "200",
                    Message = $"图片 {name.ToString()} 上传成功"
                };
                return code;
            }
            //_logger.LogInformation($"用户 {user.Id} 上传头像失败，格式错误");
            code = new CustomStatusCode
            {
                Status = "400",
                Message = $"图片 {name.ToString()} 上传失败，格式错误"
            };
            return code;
        }


    }
}
