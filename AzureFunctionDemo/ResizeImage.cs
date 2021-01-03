using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace AzureFunctionDemo
{
    public static class ResizeImage
    {
        [FunctionName("ResizeImage")]
        public static void Run(
            [BlobTrigger("blob-demo/{name}")]Stream original,
            string name,
            [Blob("thumbnails/{name}", FileAccess.Write)] Stream resized,
            ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {original.Length} Bytes");

            int scale = 2;

            using (var image = Image.Load(original))
            {
                image.Mutate(x => x
                    .Resize(image.Width / scale, image.Height / scale)
                );
                image.Save(resized, new JpegEncoder());
            }
        }
    }
}
