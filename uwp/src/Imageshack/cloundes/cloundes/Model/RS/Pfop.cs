﻿// lindexi
// 16:34

using System;
using System.Text;
using System.Threading.Tasks;
using lindexi.uwp.ImageShack.Thirdqiniucs.Model.Auth;
using lindexi.uwp.ImageShack.Thirdqiniucs.Model.Conf;
using lindexi.uwp.ImageShack.Thirdqiniucs.Model.RPC;
using lindexi.uwp.ImageShack.Thirdqiniucs.Model.Util;
using Newtonsoft.Json;

namespace lindexi.uwp.ImageShack.Thirdqiniucs.Model.RS
{
    /// <summary>
    ///     Persistent identifier.
    /// </summary>
    internal class PersistentId
    {
        public string persistentId;
    }

    /// <summary>
    ///     Persitent error.
    /// </summary>
    internal class PersitentError
    {
        public int code;
        public string error;
    }

    /// <summary>
    ///     Persistent exception.
    /// </summary>
    internal class PersistentException : Exception
    {
        public PersistentException(PersitentError err)
        {
            this.error = err;
        }

        public PersitentError Error
        {
            get
            {
                return error;
            }
        }

        private PersitentError error;
    }

    /// <summary>
    ///     对已有资源手动触发持久化
    ///     POST /pfop/ HTTP/1.1
    ///     Host: api.qiniu.com
    ///     Content-Type: application/x-www-form-urlencoded
    ///     Authorization:
    ///     <AccessToken>
    ///         bucket=<bucket>&key=<key>&fops=<fop1>;<fop2>...<fopN>&notifyURL=<persistentNotifyUrl>
    /// </summary>
    internal class Pfop : QiniuAuthClient
    {
        /// <summary>
        ///     请求持久化
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="fops"></param>
        /// <param name="notifyURL"></param>
        /// <returns></returns>
        public async Task<string> Do(EntryPath entry, string[] fops, Uri notifyURL, string pipleline, int force = 0)
        {
            if ((fops.Length < 1) || string.IsNullOrEmpty(entry?.Bucket)
                || (notifyURL == null) || !notifyURL.IsAbsoluteUri)
            {
                throw new Exception("params error");
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(fops[0]);

            for (int i = 1; i < fops.Length; ++i)
            {
                sb.Append(";");
                sb.Append(fops[i]);
            }

            string body = string.Format("bucket={0}&key={1}&fops={2}&notifyURL={3}&force={4}&pipeline={5}", entry.Bucket,
                StringEx.ToUrlEncode(entry.Key), StringEx.ToUrlEncode(sb.ToString()), notifyURL.ToString(), force,
                pipleline);
            CallRet ret =
                await
                    CallWithBinary(Config.API_HOST + "/pfop/", "application/x-www-form-urlencoded",
                        StreamEx.ToStream(body), body.Length);

            if (ret.OK)
            {
                try
                {
                    PersistentId pid = JsonConvert.DeserializeObject<PersistentId>(ret.Response);
                    return pid.persistentId;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                throw new Exception(ret.Response);
            }
        }

        /// <summary>
        ///     Queries the pfop status.
        /// </summary>
        /// <returns>The pfop status.</returns>
        /// <param name="persistentId">Persistent identifier.</param>
        public async Task<string> QueryPfopStatus(string persistentId)
        {
            CallRet ret = await Call(string.Format("{0}/status/get/prefop?id={1}", Config.API_HOST, persistentId));
            if (ret.OK)
            {
                return ret.Response;
            }
            else
            {
                throw new Exception(ret.Response);
            }
        }
    }
}
