#region Namespaces

using System;
using System.Configuration;
using Troy.Utilities.Resources;

#endregion

namespace Troy.Utilities.CrossCutting
{
    public static class ConfigurationHandler
    {
        #region Methods

        /// <summary>
        ///   Get Applications settings values
        /// </summary>
        /// <param name="key">key value</param>
        /// <returns></returns>
        public static string GetAppSettingsValue(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogException(ex);
                throw new ConfigurationErrorsException(string.Format(TroyResource.KeyNotFound, key));
            }
        }

        #endregion
    }
}