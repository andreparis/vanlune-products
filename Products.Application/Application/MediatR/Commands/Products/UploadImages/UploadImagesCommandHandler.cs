using Products.Application.MediatR.Base;
using Products.Domain.DataAccess.S3;
using Products.Infraestructure.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Image = System.Drawing;

namespace Products.Application.Application.MediatR.Commands.UploadImages
{
    public class UploadImagesCommandHandler : AbstractRequestHandler<UploadImagesCommand>
    {
        private readonly IProductsS3 _productsS3;
        private readonly ILogger _logger;

        public UploadImagesCommandHandler(IProductsS3 productsS3,
            ILogger logger)
        {
            _productsS3 = productsS3;
            _logger = logger;
        }

        internal override HandleResponse HandleIt(UploadImagesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(request.File);

                _logger.Info("byte getted");

                MemoryStream ms = new MemoryStream(imageBytes);

                _logger.Info("ms getted");

                var result = _productsS3.UploadImage(ms, request.FileName).Result;

                _logger.Info($"result is {result}");

                return new HandleResponse()
                {
                    Content = result
                };
            }
            catch (Exception e)
            {
                _logger.Error($"Error {e.Message} at {e.StackTrace}");

                return new HandleResponse()
                {
                    Error = "Error to save image"
                };
            }

            
        }
    }
}
