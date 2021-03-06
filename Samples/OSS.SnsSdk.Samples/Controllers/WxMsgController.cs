﻿using System.IO;
using Microsoft.AspNetCore.Mvc;
using OSS.Clients.Chat.WX;
using OSS.Clients.Chat.WX.Mos;
using OSS.Clients.Samples.Controllers.Codes;
using OSS.Common.BasicMos.Resp;
using OSS.Tools.Log;

namespace OSS.Clients.SNS.Samples.Controllers
{
    public class WXChatController : Controller
    {
        private static readonly WXChatConfig config = new WXChatConfig()
        {
            AppId = "wx835dddda838bb558",
            SecurityType = WXSecurityType.Safe,
            Token = "2DMEMYU9Zrv8C4ddddzvTghlUf2Z60s3",
            EncodingAesKey = "b2kcgLAsxwMjWmi6tCixPTZQ1MbY76VLXgZKHgfLOSf",
        };

        // 【一】 直接在初始化中指定配置信息
        private static readonly WXCustomMsgHandler _msgService = new WXCustomMsgHandler(config);

        // 【二】 在构造函数中动态设置配置信息
        public WXChatController()
        {
            _msgService.SetContextConfig(config);
           //WXChatConfigProvider.SetContextConfig(config);
        }

        #region  【A】 高级自定义方法实现

        static WXChatController()
        {
            //  用户可以自定义消息处理委托，
            //   也可以通过 RegisterEventMsgHandler 自定义事件处理委托
            WXChatConfigProvider.RegisteProcessor<WXTextRecMsg>("test_msg", ProcessTestMsg);
        }
        private static WXBaseReplyMsg ProcessTestMsg(WXTextRecMsg msg)
        {
            return new WXTextReplyMsg() { Content = " test_msg 类型消息返回 " };
        }

        #endregion

        #region   微信消息接口模块

        /// <summary>
        ///   在微信端配置的地址（包含微信第一次Get验证
        /// </summary>
        /// <param name="appid"> 如果是平台提供者，此参数为 授权的公众号AppId</param>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="echostr"></param>
        /// <returns></returns>
        public ContentResult Msg(string appid,string signature, string timestamp, string nonce, string echostr)
        {
            // 直接传入Stream也是可以的
            // 这里为了记录传入加密前的日志，所以先获取再传入
            // var res = _msgService.Process(Request.Body, signature, timestamp, nonce, echostr);

            string contentXml;
            using (var reader = new StreamReader(Request.Body))
            {
                contentXml = reader.ReadToEnd();
                LogHelper.Info(contentXml);
            }

            var res = _msgService.Process(contentXml, signature, timestamp, nonce, echostr);
            if (res.IsSuccess())
                return Content(res.data);

            LogHelper.Info($" 当前请求处理失败，原因：{res.msg}");
            return Content( "success");
        }

        #endregion

    }

    

}