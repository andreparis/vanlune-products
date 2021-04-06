using MediatR;
using Microsoft.AspNetCore.Http;
using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Application.Application.MediatR.Commands.UploadImages
{
    public class UploadImagesCommand : IRequest<Response>
    {
        public string FileName { get; set; }
        public string File { get; set; }
    }
}
