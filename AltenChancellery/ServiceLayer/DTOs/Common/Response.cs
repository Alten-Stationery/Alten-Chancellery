﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.DTOs.Common
{
    public class Response<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public T Data { get; set; }
        public string? Message { get; set; }
    }
}
