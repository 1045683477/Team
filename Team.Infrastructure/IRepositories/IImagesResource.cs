using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Team.Model;

namespace Team.Infrastructure.IRepositories
{
    /// <summary>
    /// 图片处理
    /// </summary>
    public interface IImagesResource
    {
        /// <summary>
        /// 加载图片
        /// </summary>
        /// <param name="path">地址</param>
        /// <param name="name">图片名</param>
        /// <returns></returns>
        FileContentResult LoadingPhoto(string path, int name);

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="path">路劲</param>
        /// <param name="name">图片名字</param>
        /// <returns></returns>
        CustomStatusCode UpLoadPhoto(IFormFile formFile,string path, object name);
    }
}
