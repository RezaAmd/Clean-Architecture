using Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Validation;

namespace Application.Extentions
{
    public static class DbValidationFilterExtentions
    {
        public static Error HandleDbValidateFilter(this DbEntityValidationException ex)
        {
            return new Error(0, ex.InnerException.Message);
        }

        public static Error HandleDbExtentionFilter(this DbUpdateException ex)
        {
            var errorType = (DbExceptionCode)ex.HResult;
            var result = new Error((int)DbExceptionCode.Other, ex.Message);

            if (errorType == DbExceptionCode.DuplicateName)
                result = DuplicateName();

            return result;
        }

        #region Describer
        public enum DbExceptionCode
        {
            DuplicateName = -2146233088,
            Other = 0
        }
        private static Error DuplicateName(string name = default) => new Error((int)DbExceptionCode.DuplicateName, $"نام {name} قبلا انتخاب شده.");
        #endregion
    }
}