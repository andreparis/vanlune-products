﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Application.MediatR.Base
{
    public class HandleResponse
    {
        public object Content { get; set; }
        public long ContentIdentification { get; set; }
        public string Error { get; set; }
    }
}
