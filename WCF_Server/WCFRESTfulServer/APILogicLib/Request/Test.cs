﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APILogicLib.Request
{
    public static class TestEcho
    {
        public static RES_DEV_ECHO Process(REQ_DEV_ECHO requestPacket)
        {
            var responseResult = new RES_DEV_ECHO();
            responseResult.Result = true;
            responseResult.ResData = string.Format("WaitSec: {0}, ReqData: {1}", requestPacket.WaitSec, requestPacket.ReqData);
            return responseResult;
        }
    }


    //public static class TestEcho2
    //{
    //    public static async Task<RES_DEV_ECHO> Process(REQ_DEV_ECHO requestPacket)
    //    {
    //        var responseResult = new RES_DEV_ECHO();
    //        return responseResult;
    //    }
    //}

    public static class TestEcho3
    {
        public static async Task<RES_DEV_ECHO> Process(REQ_DEV_ECHO requestPacket)
        {
            var responseResult = new RES_DEV_ECHO();
            return responseResult;
        }
    }
}
