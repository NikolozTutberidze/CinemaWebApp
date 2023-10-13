using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Domain.CustomResponse
{
    public class ServiceResponse<T>
    {
        public bool? Success { get; set; } = false;
        public bool? ErrorOccured { get; set; } = null;
        public string? ErrorMessage { get; set; } = null;

        public T? Data { get; set; } = default(T);


        public static ServiceResponse<T> SuccessResult(T? data)
        {
            return new ServiceResponse<T>()
            {
                Success = true,
                ErrorOccured = false,
                Data = data
            };
        }

        public static ServiceResponse<T> ErrorResult(string? error)
        {
            return new ServiceResponse<T>()
            {
                Success = false,
                ErrorOccured = true,
                ErrorMessage = error ?? "Unexpected error occured"
            };
        }
    }
}
