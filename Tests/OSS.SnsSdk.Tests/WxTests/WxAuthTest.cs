﻿using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSS.Clients.Oauth.WX;
using OSS.Common.BasicMos;
using OSS.Common.Extention;
using OSS.Tools.Http.Extention;
using OSS.Tools.Http.Mos;

namespace OSS.Social.Tests.WXTests
{
    [TestClass]
    public class WXAuthTest
    {
        private static AppConfig m_Config = new AppConfig()
        {
            AppId = "wxaa9e6cb3f03afa97",
            AppSecret = "0fc0c6f735a90fda1df5fc840e010144"
        };

        private static WXOauthApi m_AuthApi = new WXOauthApi(m_Config);
        [TestMethod]
        public void AuthTest()
        {
            var tokecRes = m_AuthApi.GetOauthAccessTokenAsync("ssss").WaitResult();
            string token = tokecRes.access_token;
        }



        [TestMethod]
        public void GetTest()
        {
            var req = new OssHttpRequest
            {
                AddressUrl =
                    "https://api.weixin.qq.com/sns/oauth2/access_token?appid=wxaa9e6cb3f03afa97&secret=0fc0c6f735a90fda1df5fc840e010144&code=ssss&grant_type=authorization_code"
            };
            req.HttpMethod = HttpMethod.Get;
            var result = req.RestSend().WaitResult();
            var resp = result.Content.ReadAsStringAsync().WaitResult();
            Assert.IsTrue(!string.IsNullOrEmpty(resp));
        }
    }
}
