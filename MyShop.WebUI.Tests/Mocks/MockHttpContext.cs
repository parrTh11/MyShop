﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.WebUI.Tests.Mocks
{
    public class MockHttpContext : HttpContextBase
    {
        private MockRequest request;
        private MockResponce responce;
        private HttpCookieCollection cookies;

        public MockHttpContext()
        {
            cookies = new HttpCookieCollection();
            this.request = new MockRequest(cookies);
            this.responce = new MockResponce(cookies);
        }

        public override HttpRequestBase Request
        {
            get
            {
                return request;
            }
        }

        public override HttpResponseBase Response
        {
            get
            {
                return responce;
            }
        }
    }

    public class MockResponce : HttpResponseBase
    {
        private readonly HttpCookieCollection cookies;

        public MockResponce(HttpCookieCollection cookies)
        {
            this.cookies = cookies;
        }

        public override HttpCookieCollection Cookies
        {
            get
            {
                return cookies;
            }
        }
    }

    public class MockRequest : HttpRequestBase
    {
        private readonly HttpCookieCollection cookies;

        public MockRequest(HttpCookieCollection cookies)
        {
            this.cookies = cookies;
        }

        public override HttpCookieCollection Cookies
        {
            get
            {
                return cookies;
            }
        }
    }
}
