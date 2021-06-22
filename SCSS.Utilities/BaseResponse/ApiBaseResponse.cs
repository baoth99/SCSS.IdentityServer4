using SCSS.Utilities.Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.BaseResponse
{
    public class ApiBaseResponse
    {
        #region BaseApiResponseModel-Ok

        /// <summary>
        /// Oks this instance.
        /// </summary>
        /// <returns></returns>
        public static ApiResponseModel OK()
        {
            return new ApiResponseModel()
            {
                IsSuccess = BooleanConstants.TRUE,
                StatusCode = HttpStatusCodes.Ok
            };
        }

        /// <summary>
        /// Oks the specified resource data.
        /// </summary>
        /// <param name="resData">The resource data.</param>
        /// <returns></returns>
        public static ApiResponseModel OK(object resData)
        {
            int? totalRecord = null;
            if (resData is ICollection col)
            {
                totalRecord = col.Count;
            }
            return new ApiResponseModel()
            {
                IsSuccess = BooleanConstants.TRUE,
                StatusCode = HttpStatusCodes.Ok,
                Data = resData,
                Total = totalRecord
            };
        }

        /// <summary>
        /// Oks the specified resource data.
        /// </summary>
        /// <param name="resData">The resource data.</param>
        /// <param name="totalRecord">The total record.</param>
        /// <returns></returns>
        public static ApiResponseModel OK(object resData, int totalRecord)
        {
            return new ApiResponseModel()
            {
                IsSuccess = BooleanConstants.TRUE,
                StatusCode = HttpStatusCodes.Ok,
                Data = resData,
                Total = totalRecord
            };
        }
        #endregion

        #region BaseApiResponseModel-Error

        /// <summary>
        /// Errors this instance.
        /// </summary>
        /// <returns></returns>
        public static ApiResponseModel Error()
        {
            return new ApiResponseModel()
            {
                IsSuccess = BooleanConstants.FALSE,
                StatusCode = HttpStatusCodes.BadRequest
            };
        }

        /// <summary>
        /// Errors the specified MSG code.
        /// </summary>
        /// <param name="msgCode">The MSG code.</param>
        /// <returns></returns>
        public static ApiResponseModel Error(string msgCode)
        {
            return new ApiResponseModel()
            {
                IsSuccess = BooleanConstants.FALSE,
                StatusCode = HttpStatusCodes.BadRequest,
                MessageCode = msgCode
            };
        }


        /// <summary>
        /// Errors the specified MSG code.
        /// </summary>
        /// <param name="msgCode">The MSG code.</param>
        /// <param name="msgDetail">The MSG detail.</param>
        /// <param name="resData">The resource data.</param>
        /// <returns></returns>
        public static ApiResponseModel Error(string msgCode, string msgDetail, object resData)
        {
            int? totalRecord = null;
            if (resData is ICollection col)
            {
                totalRecord = col.Count;
            }
            return new ApiResponseModel()
            {
                IsSuccess = BooleanConstants.FALSE,
                StatusCode = HttpStatusCodes.BadRequest,
                MessageCode = msgCode,
                MessageDetail = msgDetail,
                Data = resData,
                Total = totalRecord
            };
        }

        #endregion

        #region BaseApiResponseModel-NotFound

        /// <summary>
        /// Nots the found.
        /// </summary>
        /// <returns></returns>
        public static ApiResponseModel NotFound()
        {
            return new ApiResponseModel()
            {
                IsSuccess = BooleanConstants.FALSE,
                StatusCode = HttpStatusCodes.NotFound
            };
        }

        /// <summary>
        /// Nots the found.
        /// </summary>
        /// <param name="msgCode">The MSG code.</param>
        /// <returns></returns>
        public static ApiResponseModel NotFound(string msgCode)
        {
            return new ApiResponseModel()
            {
                IsSuccess = BooleanConstants.FALSE,
                StatusCode = HttpStatusCodes.NotFound,
                MessageCode = msgCode
            };
        }

        #endregion
    }
}
